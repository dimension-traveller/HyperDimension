using System.Globalization;
using HyperDimension.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HyperDimension.Infrastructure.Database.Interceptors;

public class ConcurrentStampInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var entries = eventData.Context?.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        if (entries is null)
        {
            return result;
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture);

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                entity.ConcurrencyStamp = now;
            }
        }

        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = eventData.Context?.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        if (entries is null)
        {
            return new ValueTask<InterceptionResult<int>>(result);
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture);

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                entity.ConcurrencyStamp = now;
            }
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }
}
