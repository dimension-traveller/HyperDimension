using HyperDimension.Common;

namespace HyperDimension.Application.Common.Interfaces;

public interface IHyperDimensionIdentityService
{
    public Task<Result<Guid>> VerifyLoginTokenAsync(string token);
    public Task<string> CreateLoginTokenAsync(Guid userId);
}
