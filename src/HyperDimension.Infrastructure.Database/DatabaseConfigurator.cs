using System.Security.Cryptography.X509Certificates;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Infrastructure.Database.Options;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace HyperDimension.Infrastructure.Database;

public static class DatabaseConfigurator
{
    public static void AddHyperDimensionDatabase(this IServiceCollection services)
    {
        services.AddDbContext<IHyperDimensionDbContext, HyperDimensionDbContext>();

        // Add data protection
        var builder = services.AddDataProtection()
            .SetApplicationName("HyperDimension")
            .PersistKeysToDbContext<HyperDimensionDbContext>();

        var dataProtectionOptions = HyperDimensionConfiguration.Instance
            .GetOption<DatabaseDataProtectionOptions>();

        if (dataProtectionOptions.EnableCertificate is false)
        {
            return;
        }

        builder.ProtectKeysWithCertificate(new X509Certificate2(
            dataProtectionOptions.Certificate.Path,
            dataProtectionOptions.Certificate.Password));
        builder.UnprotectKeysWithAnyCertificate(
            dataProtectionOptions.RotatedCertificates
                .Select(x => new X509Certificate2(x.Path, x.Password))
                .ToArray());
    }
}
