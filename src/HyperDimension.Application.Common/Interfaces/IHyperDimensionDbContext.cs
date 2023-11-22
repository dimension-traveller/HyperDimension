namespace HyperDimension.Application.Common.Interfaces;

public interface IHyperDimensionDbContext
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
