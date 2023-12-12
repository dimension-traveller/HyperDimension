namespace HyperDimension.Application.Common.Interfaces.Identity;

public interface IPermissionService
{
    public Task<bool> AllowAccess(string permission, Guid userId);
}
