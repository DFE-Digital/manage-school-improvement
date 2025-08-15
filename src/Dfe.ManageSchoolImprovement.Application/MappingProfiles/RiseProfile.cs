using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.MappingProfiles
{
    public class RiseProfile : Profile
    {
        public RiseProfile()
        {
            ConfigureValueObjectMappings();
            ConfigureEntityMappings();
        }

        private void ConfigureValueObjectMappings()
        {
            // SupportProjectId mappings
            CreateValueObjectMapping<SupportProjectId, int>();

            // FundingHistoryId mappings  
            CreateValueObjectMapping<FundingHistoryId, Guid>();
        }

        private void ConfigureEntityMappings()
        {
            // SupportProject mapping (special case with int ID)
            CreateMap<Domain.Entities.SupportProject.SupportProject, SupportProjectDto>()
                .ForCtorParam("id", opt => opt.MapFrom(src => ExtractNullableValue<SupportProjectId, int>(src.Id)))
                .ReverseMap();

            // Entities with Guid-based IDs
            CreateEntityMapping<Domain.Entities.SupportProject.FundingHistory, FundingHistoryDto, FundingHistoryId>();

            CreateEntityMapping<Domain.Entities.SupportProject.ImprovementPlan, ImprovementPlanDto, ImprovementPlanId>();

            CreateEntityMappingWithParent<Domain.Entities.SupportProject.ImprovementPlanObjective, ImprovementPlanObjectiveDto, ImprovementPlanObjectiveId>(
                "improvementPlanId", src => ExtractNullableValue<ImprovementPlanId, Guid>(src.ImprovementPlanId));

            CreateEntityMappingWithParent<Domain.Entities.SupportProject.ImprovementPlanReview, ImprovementPlanReviewDto, ImprovementPlanReviewId>(
                "improvementPlanId", src => ExtractNullableValue<ImprovementPlanId, Guid>(src.ImprovementPlanId));

            CreateEntityMappingWithMultipleParents<Domain.Entities.SupportProject.ImprovementPlanObjectiveProgress, ImprovementPlanObjectiveProgressDto, ImprovementPlanObjectiveProgressId>();
        }

        private void CreateValueObjectMapping<TValueObject, TPrimitive>()
            where TValueObject : class
        {
            CreateMap<TValueObject, TPrimitive>()
                .ConvertUsing(src => (TPrimitive)src.GetType().GetProperty("Value")!.GetValue(src)!);

            CreateMap<TPrimitive, TValueObject>()
                .ConvertUsing(value => (TValueObject)Activator.CreateInstance(typeof(TValueObject), value)!);
        }

        private void CreateEntityMapping<TEntity, TDto, TId>(string? additionalParamName = null, Func<TEntity, object?>? additionalParamMapping = null)
            where TEntity : class
            where TDto : class
        {
            var mapping = CreateMap<TEntity, TDto>()
                .ForCtorParam("id", opt => opt.MapFrom(src => ExtractNullableValue<TId, Guid>(GetEntityId<TEntity, TId>(src))));

            if (additionalParamName != null && additionalParamMapping != null)
            {
                mapping.ForCtorParam(additionalParamName, opt => opt.MapFrom(additionalParamMapping));
            }

            mapping.ReverseMap();
        }

        private void CreateEntityMappingWithParent<TEntity, TDto, TId>(string parentParamName, Func<TEntity, object?> parentMapping)
            where TEntity : class
            where TDto : class
        {
            CreateMap<TEntity, TDto>()
                .ForCtorParam("id", opt => opt.MapFrom(src => ExtractNullableValue<TId, Guid>(GetEntityId<TEntity, TId>(src))))
                .ForCtorParam(parentParamName, opt => opt.MapFrom(parentMapping))
                .ReverseMap();
        }

        private void CreateEntityMappingWithMultipleParents<TEntity, TDto, TId>()
            where TEntity : class
            where TDto : class
        {
            CreateMap<TEntity, TDto>()
                .ForCtorParam("id", opt => opt.MapFrom(src => ExtractNullableValue<TId, Guid>(GetEntityId<TEntity, TId>(src))))
                .ForCtorParam("improvementPlanReviewId", opt => opt.MapFrom(src => ExtractNullableValue<ImprovementPlanReviewId, Guid>(GetPropertyValue<TEntity, ImprovementPlanReviewId>(src, "ImprovementPlanReviewId"))))
                .ForCtorParam("improvementPlanObjectiveId", opt => opt.MapFrom(src => ExtractNullableValue<ImprovementPlanObjectiveId, Guid>(GetPropertyValue<TEntity, ImprovementPlanObjectiveId>(src, "ImprovementPlanObjectiveId"))))
                .ReverseMap();
        }

        private static TId? GetEntityId<TEntity, TId>(TEntity entity) where TEntity : class
        {
            return GetPropertyValue<TEntity, TId>(entity, "Id");
        }

        private static TProperty? GetPropertyValue<TEntity, TProperty>(TEntity entity, string propertyName) where TEntity : class
        {
            var property = typeof(TEntity).GetProperty(propertyName);
            return property != null ? (TProperty?)property.GetValue(entity) : default;
        }

        private static TPrimitive? ExtractNullableValue<TValueObject, TPrimitive>(TValueObject? valueObject)
            where TPrimitive : struct
        {
            if (valueObject == null) return null;

            var valueProperty = typeof(TValueObject).GetProperty("Value");
            return valueProperty != null ? (TPrimitive?)valueProperty.GetValue(valueObject) : null;
        }
    }
}
