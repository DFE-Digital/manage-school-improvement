using Dfe.ManageSchoolImprovement.Application.Common.Behaviours;
using Dfe.ManageSchoolImprovement.Application.MappingProfiles;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            var performanceLoggingEnabled = config.GetValue<bool>("Features:PerformanceLoggingEnabled");

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

                if (performanceLoggingEnabled)
                {
                    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
                }
            });

            services.AddAutoMapper(typeof(RiseProfile));

            services.AddBackgroundService();

            services.AddScoped<ISupportProjectQueryService, SupportProjectQueryService>();
            services.AddScoped<ISupportProjectAuditQueryService, SupportProjectAuditQueryService>();

            return services;
        }
    }
}
