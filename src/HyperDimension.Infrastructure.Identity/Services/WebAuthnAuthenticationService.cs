﻿using System.Text;
using Fido2NetLib;
using Fido2NetLib.Objects;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Common;
using HyperDimension.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace HyperDimension.Infrastructure.Identity.Services;

public class WebAuthnAuthenticationService
    : IWebAuthnAuthenticationService
{
    private readonly IFido2 _fido2;
    private readonly IDistributedCache _cache;
    private readonly IHyperDimensionDbContext _dbContext;

    public WebAuthnAuthenticationService(
        IFido2 fido2,
        IDistributedCache cache,
        IHyperDimensionDbContext dbContext)
    {
        _fido2 = fido2;
        _cache = cache;
        _dbContext = dbContext;
    }

    public async Task<CredentialCreateOptions> CreateRegistrationOptionsAsync(
        Fido2User user,
        IEnumerable<byte[]> existingCredentialIds)
    {
        var existingDescriptor = existingCredentialIds
            .Select(x => new PublicKeyCredentialDescriptor(x))
            .ToList();

        var options = _fido2.RequestNewCredential(user, existingDescriptor);
        var challenge = Base64Url.Encode(options.Challenge);
        await _cache.SetStringAsync(GetCacheKey(challenge), options.ToJson());

        return options;
    }

    public async Task<Result<AttestationVerificationSuccess>> VerifyRegistrationAssertionAsync(
        string challenge,
        AuthenticatorAttestationRawResponse attestationResponse)
    {
        var options = await _cache.GetStringAsync(GetCacheKey(challenge));
        var fidoOptions = CredentialCreateOptions.FromJson(options);

        var fidoCredentials = await _fido2.MakeNewCredentialAsync(
            attestationResponse, fidoOptions, (p, _) =>
            {
                var user = _dbContext.Users
                    .Include(x => x.WebAuthnDevices)
                    .FirstOrDefault(x => x.Username == p.User.Name);

                if (user is null)
                {
                    return Task.FromResult(false);
                }

                var existing = user.WebAuthnDevices.Select(x => x.CredentialId)
                    .Any(x => x.SequenceEqual(p.CredentialId));

                return Task.FromResult(existing is false);
            });

        if (fidoCredentials.Status != "ok")
        {
            return fidoCredentials.ErrorMessage;
        }

        return fidoCredentials.Result.ExpectNotNull();
    }

    public async Task<AssertionOptions> CreateAuthenticationAssertionOptionsAsync(IEnumerable<byte[]> existingCredentialIds)
    {
        var descriptors = existingCredentialIds
            .Select(x => new PublicKeyCredentialDescriptor(x));

        var options = _fido2.GetAssertionOptions(
            descriptors,
            UserVerificationRequirement.Discouraged);

        var challenge = Base64Url.Encode(options.Challenge);
        await _cache.SetStringAsync(GetCacheKey(challenge), options.ToJson());

        return options;
    }

    public async Task<Result<Guid>> VerifyAuthenticationAssertionAsync(
        string challenge,
        AuthenticatorAssertionRawResponse assertionResponse)
    {
        var options = await _cache.GetStringAsync(GetCacheKey(challenge));
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

    private static string GetCacheKey(string challenge)
    {
        return $"webauthn:challenge:{challenge}";
    }
}
