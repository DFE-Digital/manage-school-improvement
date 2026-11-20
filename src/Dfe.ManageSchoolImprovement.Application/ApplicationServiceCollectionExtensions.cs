using Dfe.ManageSchoolImprovement.Application.Common.Behaviours;
using Dfe.ManageSchoolImprovement.Application.Common.Mediation;
using Dfe.ManageSchoolImprovement.Application.MappingProfiles;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using AutoMapper.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);

            services.AddMediator(options =>
            {
                options.Assemblies = [typeof(ApplicationServiceCollectionExtensions)];
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.PipelineBehaviors = [
                    typeof(UnhandledExceptionBehaviour<,>),
                    typeof(ValidationBehaviour<,>),
                    typeof(PerformanceBehaviour<,>)
                ];
            });

            services.AddScoped<IAppMediator, MediatorAppMediator>();

            services.AddAutoMapper(cfg =>
            {
                cfg.Internal().ForAllMaps((_, mapping) =>
                {
                    mapping.MaxDepth(64);
                });
            }, typeof(RiseProfile));

            services.AddBackgroundService();

            services.AddScoped<ISupportProjectQueryService, SupportProjectQueryService>();
            services.AddScoped<ISupportProjectAuditQueryService, SupportProjectAuditQueryService>();
            services.AddScoped<IWatchlistQueryService, WatchlistQueryService>();

            return services;
        }
    }
}
