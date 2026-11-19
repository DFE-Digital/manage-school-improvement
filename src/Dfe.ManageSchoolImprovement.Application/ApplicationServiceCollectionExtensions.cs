using Dfe.ManageSchoolImprovement.Application.Common.Behaviours;
using Dfe.ManageSchoolImprovement.Application.MappingProfiles;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using FluentValidation;
using Mapster;
using MapsterMapper;
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

            var mappingConfig = new TypeAdapterConfig();
            mappingConfig.Default.MaxDepth(64);
            mappingConfig.Default.MapToConstructor(true);
            new RiseProfile().Register(mappingConfig);

            services.AddSingleton(mappingConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddBackgroundService();

            services.AddScoped<ISupportProjectQueryService, SupportProjectQueryService>();
            services.AddScoped<ISupportProjectAuditQueryService, SupportProjectAuditQueryService>();
            services.AddScoped<IWatchlistQueryService, WatchlistQueryService>();

            return services;
        }
    }
}
