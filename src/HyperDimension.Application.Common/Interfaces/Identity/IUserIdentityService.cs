using HyperDimension.Common;

namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface IUserIdentityService
{
    public Task<Result<Guid>> VerifyLoginTokenAsync(string token);
    public Task<string> CreateLoginTokenAsync(Guid userId);
}
