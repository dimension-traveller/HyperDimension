using HyperDimension.Common;
using HyperDimension.Domain.Entities.Security;
using HyperDimension.Domain.Enums;

namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface ISecurityTokenService
{
    public Task<Result<Token>> CreateTokenAsync(
        Guid userId, TokenUsage usage, DateTimeOffset currentTime,
        CancellationToken cancellationToken = default);
}
