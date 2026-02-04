using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Dfe.ManageSchoolImprovement.Infrastructure.Repositories;
using Dfe.ManageSchoolImprovement.Infrastructure.Security;
using Dfe.ManageSchoolImprovement.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            //Repos
            services.AddScoped<ISupportProjectRepository, SupportProjectRepository>();
            services.AddScoped<ISupportProjectAuditRepository, SupportProjectAuditRepository>();

            //Cache service
            services.AddServiceCaching(config);

            //Db
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<RegionalImprovementForStandardsAndExcellenceContext>(options =>
                options.UseSqlServer(connectionString));

            // Utils
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserContextService, UserContextService>();

            // Health check
            AddInfrastructureHealthCheck(services);

            return services;
        }

        public static void AddInfrastructureHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<RegionalImprovementForStandardsAndExcellenceContext>("RISE Database");
        }
    }
}
