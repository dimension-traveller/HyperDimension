using HyperDimension.Domain.Entities.Common;

namespace HyperDimension.Application.Common.Interfaces.Database;

public interface IDatabaseMetadataService
{
    public Task<string> GetTableName<TEntity>() where TEntity : BaseEntity;

    public Task<string> GetColumnName<TEntity>(string propertyName) where TEntity : BaseEntity;
}
