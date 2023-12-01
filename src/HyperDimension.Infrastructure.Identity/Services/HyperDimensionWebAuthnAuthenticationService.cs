using System.Text;
using Fido2NetLib;
using Fido2NetLib.Objects;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common;
using HyperDimension.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace HyperDimension.Infrastructure.Identity.Services;

public class HyperDimensionWebAuthnAuthenticationService
    : IHyperDimensionWebAuthnAuthenticationService
{
    private readonly IFido2 _fido2;
    private readonly IDistributedCache _cache;
    private readonly IHyperDimensionDbContext _dbContext;

    public HyperDimensionWebAuthnAuthenticationService(
        IFido2 fido2,
        IDistributedCache cache,
        IHyperDimensionDbContext dbContext)
    {
        _fido2 = fido2;
        _cache = cache;
        _dbContext = dbContext;
    }

    public async Task<CredentialCreateOptions> CreateWebAuthnRegistrationOptionsAsync(Fido2User user)
    {
        var existingUser = _dbContext.Users
            .Include(x => x.WebAuthnDevices)
            .FirstOrDefault(x => x.Username == user.Name);

        var existingDescriptor = existingUser?
            .WebAuthnDevices
            .Select(x => x.CredentialId)
            .Select(x => new PublicKeyCredentialDescriptor(x))
            .ToList() ?? [];

        var options = _fido2.RequestNewCredential(user, existingDescriptor);
        var challenge = Base64Url.Encode(options.Challenge);
        await _cache.SetStringAsync(challenge, options.ToJson());

        return options;
    }

    public async Task<Result<Guid>> VerifyWebAuthnRegistrationAsync(string cacheKey, AuthenticatorAttestationRawResponse attestationResponse)
    {
        var options = await _cache.GetStringAsync(cacheKey);
        var fidoOptions = CredentialCreateOptions.FromJson(options);

        var fidoCredentials = await _fido2.MakeNewCredentialAsync(
            attestationResponse, fidoOptions, (p, _) =>
            {
                var existingUser = _dbContext.Users
                    .Include(x => x.WebAuthnDevices)
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Username == p.User.Name);

                if (existingUser is null)
                {
                    return Task.FromResult(true);
                }

                var existingCredentialId = existingUser.WebAuthnDevices
                    .Find(x => x.CredentialId.SequenceEqual(p.CredentialId));

                return Task.FromResult(existingCredentialId is null);
            });

        if (fidoCredentials.Status != "ok")
        {
            return fidoCredentials.ErrorMessage;
        }

        var existingUser = _dbContext.Users
            .Include(x => x.WebAuthnDevices)
            .FirstOrDefault(x => x.Username == fidoCredentials.Result!.User.Name);

        var device = new WebAuthn
        {
            CredentialId = fidoCredentials.Result!.CredentialId,
            PublicKey = fidoCredentials.Result!.PublicKey,
            UserHandle = fidoCredentials.Result!.User.Id,
            SignatureCounter = fidoCredentials.Result!.Counter,
            CredType = fidoCredentials.Result!.CredType,
            RegDate = DateTimeOffset.UtcNow,
            AaGuid = fidoCredentials.Result.Aaguid
        };

        Guid userGuid;

        if (existingUser is not null)
        {
            existingUser.WebAuthnDevices.Add(device);
            userGuid = existingUser.EntityId;
        }
        else
        {
            var user = new User
            {
                Username = fidoCredentials.Result.User.Name,
                DisplayName = fidoCredentials.Result.User.DisplayName,
                Email = Encoding.UTF8.GetString(fidoCredentials.Result!.User.Id),
                PasswordHash = string.Empty,
                Salt = string.Empty,
                Roles = [],
                WebAuthnDevices = [device]
            };

            userGuid = user.EntityId;

            await _dbContext.Users.AddAsync(user);
        }

        await _dbContext.SaveChangesAsync(CancellationToken.None);
        return userGuid;
    }

    public async Task<Result<AssertionOptions>> CreateWebAuthnAssertionOptionsAsync(string userIdentifier)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Username == userIdentifier || x.Email == userIdentifier);

        if (user is null)
        {
            return "User does not exist";
        }

        var descriptors = user.WebAuthnDevices
            .Select(x => x.CredentialId)
            .Select(x => new PublicKeyCredentialDescriptor(x));

        var options = _fido2.GetAssertionOptions(
            descriptors,
            UserVerificationRequirement.Discouraged);

        var challenge = Base64Url.Encode(options.Challenge);
        await _cache.SetStringAsync(challenge, options.ToJson());

        return options;
    }

    public async Task<Result<Guid>> VerifyWebAuthnAssertionAsync(string cacheKey, AuthenticatorAssertionRawResponse assertionResponse)
    {
        var options = await _cache.GetStringAsync(cacheKey);
        var fidoOptions = AssertionOptions.FromJson(options);

        var device = await _dbContext.WebAuthnDevices
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.CredentialId.SequenceEqual(assertionResponse.Id));

        if (device is null)
        {
            return "Device does not exist";
        }

        var result = await _fido2.MakeAssertionAsync(
            assertionResponse,
            fidoOptions,
            device.PublicKey,
            device.SignatureCounter,
            (args, _)
                => Task.FromResult(
                    device.UserHandle.SequenceEqual(args.UserHandle)));

        if (result.Status != "ok")
        {
            return result.ErrorMessage;
        }

        return device.User.EntityId;
    }
}
