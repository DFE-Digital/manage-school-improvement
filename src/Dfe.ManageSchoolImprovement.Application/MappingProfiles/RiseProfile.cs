using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.MappingProfiles
{
    public class RiseProfile : Profile
    {
        public RiseProfile()
        {
            ConfigureValueObjects();

            ConfigureEntities();
        }

        private void ConfigureValueObjects()
        {
            // Explicit mapping from SupportProjectId to int and vice versa
            CreateMap<SupportProjectId, int>()
                .ConvertUsing(src => src.Value);

            CreateMap<int, SupportProjectId>()
                .ConvertUsing(value => new SupportProjectId(value));

            CreateMap<FundingHistoryId, Guid>()
                .ConvertUsing(src => src.Value);

            CreateMap<Guid, FundingHistoryId>()
                .ConvertUsing(value => new FundingHistoryId(value));
        }

        private void ConfigureEntities()
        {
            ConfigureSupportProject();
            ConfigureImprovementPlans();
            ConfigureProgressReviews();
            ConfigureEngagementConcerns();
        }

        private void ConfigureEngagementConcerns()
        {
            CreateMap<Domain.Entities.SupportProject.EngagementConcern, EngagementConcernDto>()
                .ForCtorParam("id", opt => opt.MapFrom(src => src.Id != null ? src.Id.Value : (Guid?)null))
                .ReverseMap();
        }
        private void ConfigureProgressReviews()
        {
            ConfigureImprovementPlanReviewMapping();
            ConfigureImprovementPlanObjectiveProgressMapping();
            ConfigureProgressReviewMapping();
        }

        private void ConfigureImprovementPlanReviewMapping()
        {
            CreateMap<Domain.Entities.SupportProject.ImprovementPlanReview, ImprovementPlanReviewDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => ToNullableGuid(src.Id)))
               .ForCtorParam("improvementPlanId", opt => opt.MapFrom(src => ToNullableGuid(src.ImprovementPlanId)))
               .ReverseMap();
        }

        private void ConfigureImprovementPlanObjectiveProgressMapping()
        {
            CreateMap<Domain.Entities.SupportProject.ImprovementPlanObjectiveProgress, ImprovementPlanObjectiveProgressDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => ToNullableGuid(src.Id)))
               .ForCtorParam("improvementPlanReviewId", opt => opt.MapFrom(src => ToNullableGuid(src.ImprovementPlanReviewId)))
               .ForCtorParam("improvementPlanObjectiveId", opt => opt.MapFrom(src => ToNullableGuid(src.ImprovementPlanObjectiveId)))
               .ReverseMap();
        }

        private void ConfigureProgressReviewMapping()
        {
            CreateMap<Domain.Entities.SupportProject.ProgressReview, ProgressReviewDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => ToNullableGuid(src.Id)))
               .ReverseMap();
        }

        private static Guid? ToNullableGuid(ImprovementPlanReviewId? id) => id is null ? null : id.Value;

        private static Guid? ToNullableGuid(ImprovementPlanId? id) => id is null ? null : id.Value;

        private static Guid? ToNullableGuid(ImprovementPlanObjectiveProgressId? id) => id is null ? null : id.Value;

        private static Guid? ToNullableGuid(ImprovementPlanObjectiveId? id) => id is null ? null : id.Value;

        private static Guid? ToNullableGuid(ProgressReviewId? id) => id is null ? null : id.Value;

        private void ConfigureImprovementPlans()
        {
            CreateMap<Domain.Entities.SupportProject.ImprovementPlan, ImprovementPlanDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => src.Id != null ? src.Id.Value : (Guid?)null))
               .ReverseMap();

            CreateMap<Domain.Entities.SupportProject.ImprovementPlanObjective, ImprovementPlanObjectiveDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => src.Id != null ? src.Id.Value : (Guid?)null))
               .ForCtorParam("improvementPlanId", opt => opt.MapFrom(src => src.ImprovementPlanId != null ? src.ImprovementPlanId.Value : (Guid?)null))
               .ReverseMap();
        }

    

        private void ConfigureSupportProject()
        {
            CreateMap<Domain.Entities.SupportProject.SupportProject, SupportProjectDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => src.Id != null ? src.Id.Value : (int?)null)) // Map Id only if not null
                .ReverseMap();
            
        }
    }
}
