using Azure.Identity;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Dfe.ManageSchoolImprovement.Infrastructure.Repositories;
using Dfe.ManageSchoolImprovement.Infrastructure.Security;
using Dfe.ManageSchoolImprovement.Infrastructure.Security.Authorization;
using Dfe.ManageSchoolImprovement.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.DataProtection;
using System.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            //Repos
            services.AddScoped<ISupportProjectRepository, SupportProjectRepository>();

            //Cache service
            services.AddServiceCaching(config);

            //Db
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<RegionalImprovementForStandardsAndExcellenceContext>(options =>
                options.UseSqlServer(connectionString));

            // Utils
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserContextService, UserContextService>();

            // Data Protection
            services.AddDataProtectionServices(config);

            // Health check
            AddInfrastructureHealthCheck(services);

            return services;
        }

        public static void AddInfrastructureHealthCheck(this IServiceCollection services) {
            services.AddHealthChecks()
                .AddDbContextCheck<RegionalImprovementForStandardsAndExcellenceContext>("RISE Database");
        }

        public static void AddDataProtectionServices(this IServiceCollection services, IConfiguration config)
        {
            // Setup basic Data Protection and persist keys.xml to local file system
            var dp = services.AddDataProtection();

            // If a Key Vault Key URI is defined, expect to encrypt the keys.xml
            var kvProtectionKeyUri = config.GetValue<string>("DataProtection:KeyVaultKey");
            if (!string.IsNullOrWhiteSpace(kvProtectionKeyUri))
            {
                var kvProtectionPath = config.GetValue<string>("DataProtection:Path");

                if (string.IsNullOrWhiteSpace(kvProtectionPath))
                {
                    throw new InvalidOperationException("DataProtection:Path is undefined or empty");
                }

                var kvProtectionPathDir = new DirectoryInfo(kvProtectionPath);
                if (!kvProtectionPathDir.Exists || kvProtectionPathDir.Attributes.HasFlag(FileAttributes.ReadOnly))
                {
                    throw new ReadOnlyException($"DataProtection path '{kvProtectionPath}' cannot be written to");
                }

                dp.PersistKeysToFileSystem(kvProtectionPathDir);
                dp.ProtectKeysWithAzureKeyVault(new Uri(kvProtectionKeyUri), new DefaultAzureCredential());
            }
        }
    }
}
