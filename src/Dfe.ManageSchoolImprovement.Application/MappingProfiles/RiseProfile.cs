using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Mapster;

using EngagementConcern = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.EngagementConcern;
using ImprovementPlan = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.ImprovementPlan;
using ImprovementPlanObjective = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.ImprovementPlanObjective;
using ImprovementPlanObjectiveProgress = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.ImprovementPlanObjectiveProgress;
using ImprovementPlanReview = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.ImprovementPlanReview;
using ProgressReview = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.ProgressReview;
using SupportProjectContact = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProjectContact;
using SupportProjectEntity = global::Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProject;
using SupportProjectNote = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProjectNote;

namespace Dfe.ManageSchoolImprovement.Application.MappingProfiles
{
    public sealed class RiseProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            ConfigureValueObjects(config);
            ConfigureEntities(config);
        }

        private static void ConfigureValueObjects(TypeAdapterConfig config)
        {
            config.NewConfig<SupportProjectId, int>().MapWith(source => source.Value);
            config.NewConfig<int, SupportProjectId>().MapWith(value => new SupportProjectId(value));

            RegisterGuidValueObject<FundingHistoryId>(config, value => value.Value, value => new FundingHistoryId(value));
            RegisterGuidValueObject<ImprovementPlanId>(config, value => value.Value, value => new ImprovementPlanId(value));
            RegisterGuidValueObject<ImprovementPlanReviewId>(config, value => value.Value, value => new ImprovementPlanReviewId(value));
            RegisterGuidValueObject<ImprovementPlanObjectiveProgressId>(config, value => value.Value, value => new ImprovementPlanObjectiveProgressId(value));
            RegisterGuidValueObject<ImprovementPlanObjectiveId>(config, value => value.Value, value => new ImprovementPlanObjectiveId(value));
            RegisterGuidValueObject<ProgressReviewId>(config, value => value.Value, value => new ProgressReviewId(value));
            RegisterGuidValueObject<EngagementConcernId>(config, value => value.Value, value => new EngagementConcernId(value));
        }

        private static void RegisterGuidValueObject<TValueObject>(
            TypeAdapterConfig config,
            System.Linq.Expressions.Expression<Func<TValueObject, Guid>> toGuid,
            System.Linq.Expressions.Expression<Func<Guid, TValueObject>> fromGuid)
        {
            config.NewConfig<TValueObject, Guid>().MapWith(toGuid);
            config.NewConfig<Guid, TValueObject>().MapWith(fromGuid);
        }

        private static void ConfigureEntities(TypeAdapterConfig config)
        {
            config.NewConfig<SupportProjectContact, SupportProjectContact>().MapWith(source => source);
            config.NewConfig<SupportProjectNote, SupportProjectNote>().MapWith(source => source);

            ConfigureSupportProject(config);
            ConfigureImprovementPlans(config);
            ConfigureProgressReviews(config);
            ConfigureEngagementConcerns(config);
        }

        private static void ConfigureEngagementConcerns(TypeAdapterConfig config)
        {
            config.NewConfig<EngagementConcern, EngagementConcernDto>()
                .Map(destination => destination.Id, source => source.Id.Value)
                .TwoWays();

            config.ForType<EngagementConcernDto, EngagementConcern>()
                .ConstructUsing(source => new EngagementConcern(
                    new EngagementConcernId(source.Id),
                    new SupportProjectId(source.SupportProjectId),
                    new EngagementConcernDetails
                    {
                        Details = source.EngagementConcernDetails,
                        Summary = source.EngagementConcernSummary,
                        RaisedDate = source.EngagementConcernRaisedDate,
                        Resolved = source.EngagementConcernResolved,
                        ResolvedDetails = source.EngagementConcernResolvedDetails,
                        ResolvedDate = source.EngagementConcernResolvedDate
                    }));
        }

        private static void ConfigureProgressReviews(TypeAdapterConfig config)
        {
            ConfigureImprovementPlanReviewMapping(config);
            ConfigureImprovementPlanObjectiveProgressMapping(config);
            ConfigureProgressReviewMapping(config);
        }

        private static void ConfigureImprovementPlanReviewMapping(TypeAdapterConfig config)
        {
            config.NewConfig<ImprovementPlanReview, ImprovementPlanReviewDto>()
                .Map(destination => destination.id, source => source.Id.Value)
                .Map(destination => destination.improvementPlanId, source => source.ImprovementPlanId.Value)
                .TwoWays();

            config.ForType<ImprovementPlanReviewDto, ImprovementPlanReview>()
                .ConstructUsing(source => new ImprovementPlanReview(
                    new ImprovementPlanReviewId(source.id),
                    new ImprovementPlanId(source.improvementPlanId),
                    source.reviewDate,
                    source.reviewer,
                    source.title,
                    source.order));
        }

        private static void ConfigureImprovementPlanObjectiveProgressMapping(TypeAdapterConfig config)
        {
            config.NewConfig<ImprovementPlanObjectiveProgress, ImprovementPlanObjectiveProgressDto>()
                .Map(destination => destination.id, source => source.Id == null ? Guid.Empty : source.Id.Value)
                .Map(destination => destination.improvementPlanReviewId, source => source.ImprovementPlanReviewId.Value)
                .Map(destination => destination.improvementPlanObjectiveId, source => source.ImprovementPlanObjectiveId.Value)
                .TwoWays();

            config.ForType<ImprovementPlanObjectiveProgressDto, ImprovementPlanObjectiveProgress>()
                .ConstructUsing(source => new ImprovementPlanObjectiveProgress(
                    new ImprovementPlanObjectiveProgressId(source.id),
                    new ImprovementPlanObjectiveId(source.improvementPlanObjectiveId),
                    new ImprovementPlanReviewId(source.improvementPlanReviewId),
                    source.howIsSchoolProgressing,
                    source.progressDetails));
        }

        private static void ConfigureProgressReviewMapping(TypeAdapterConfig config)
        {
            config.NewConfig<ProgressReview, ProgressReviewDto>()
                .Map(destination => destination.id, source => source.Id.Value)
                .TwoWays();

            config.ForType<ProgressReviewDto, ProgressReview>()
                .ConstructUsing(source => new ProgressReview(
                    new ProgressReviewId(source.id),
                    new SupportProjectId(source.supportProjectId),
                    source.reviewDate,
                    source.reviewer,
                    source.order,
                    source.title));
        }

        private static void ConfigureImprovementPlans(TypeAdapterConfig config)
        {
            config.NewConfig<ImprovementPlan, ImprovementPlanDto>()
                .Map(destination => destination.id, source => source.Id.Value)
                .TwoWays();

            config.ForType<ImprovementPlanDto, ImprovementPlan>()
                .ConstructUsing(source => new ImprovementPlan(
                    new ImprovementPlanId(source.id),
                    new SupportProjectId(source.supportProjectId)));

            config.NewConfig<ImprovementPlanObjective, ImprovementPlanObjectiveDto>()
                .Map(destination => destination.id, source => source.Id.Value)
                .Map(destination => destination.improvementPlanId, source => source.ImprovementPlanId.Value)
                .TwoWays();

            config.ForType<ImprovementPlanObjectiveDto, ImprovementPlanObjective>()
                .ConstructUsing(source => new ImprovementPlanObjective(
                    new ImprovementPlanObjectiveId(source.id),
                    new ImprovementPlanId(source.improvementPlanId),
                    source.areaOfImprovement,
                    source.details,
                    source.order));
        }

        private static void ConfigureSupportProject(TypeAdapterConfig config)
        {
            config.NewConfig<SupportProjectEntity, SupportProjectDto>()
                .Map(destination => destination.Id, source => source.Id.Value)
                .TwoWays();

            config.ForType<SupportProjectDto, SupportProjectEntity>()
                .ConstructUsing(source => new SupportProjectEntity(
                    new SupportProjectId(source.Id),
                    source.SchoolName,
                    source.SchoolUrn,
                    source.LocalAuthority,
                    source.Region,
                    source.TrustName == null && source.TrustReferenceNumber == null
                        ? null
                        : new TrustDetails
                        {
                            TrustName = source.TrustName,
                            TrustReferenceNumber = source.TrustReferenceNumber
                        },
                    source.Address));
        }
    }
}
