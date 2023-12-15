using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HyperDimension.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Infrastructure.Database.Configuration;

public static class CommonConfigure
{
    [SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields")]
    public static void ApplyCommonConfigurations(this ModelBuilder modelBuilder)
    {
        var baseEntityConfigureMethod = typeof(CommonConfigure)
            .GetMethod(nameof(ConfigureBaseEntity), BindingFlags.NonPublic | BindingFlags.Static);
        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(x => x.ClrType.IsSubclassOf(typeof(BaseEntity)));

        foreach (var clrType in entityTypes.Select(x => x.ClrType))
        {
            var genericMethod = baseEntityConfigureMethod!.MakeGenericMethod(clrType);
            var parameters = new object[] { modelBuilder };
            genericMethod.Invoke(null, parameters);
        }
    }

    private static void ConfigureBaseEntity<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>(builder =>
            {
                builder.Property(p => p.EntityId).ValueGeneratedNever();
                builder.Property(p => p.ConcurrencyStamp).ValueGeneratedNever();
            })
            .ConfigureEnumStringConverter<TEntity>();
    }

    private static void ConfigureEnumStringConverter<TEntity>(this ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        var properties = typeof(TEntity)
            .GetProperties()
            .Where(p => p.PropertyType.IsEnum);
        foreach (var property in properties)
        {
            modelBuilder.Entity<TEntity>().Property(property.Name).HasConversion<string>();
        }
    }
}
