namespace HyperDimension.Infrastructure.Identity.Abstract;

public interface IHyperDimensionIdentityService
{
    public bool CheckAccess(string userId, string permission);
}
