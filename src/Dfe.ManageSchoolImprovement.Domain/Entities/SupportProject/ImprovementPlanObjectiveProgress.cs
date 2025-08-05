using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class ImprovementPlanObjectiveProgress : IEntity<ImprovementPlanObjectiveProgressId>
    {
        public ImprovementPlanObjectiveProgress(
            ImprovementPlanObjectiveProgressId id,
            ImprovementPlanObjectiveId improvementPlanObjectiveId,
            ImprovementPlanReviewId improvementPlanReviewId,
            string howIsSchoolProgressing,
            string progressDetails)
        {
            Id = id;
            ImprovementPlanObjectiveId = improvementPlanObjectiveId;
            ImprovementPlanReviewId = improvementPlanReviewId;
            HowIsSchoolProgressing = howIsSchoolProgressing;
            ProgressDetails = progressDetails;
        }

        public ImprovementPlanObjectiveProgressId? Id { get; private set; }
        public int ReadableId { get; }
        public ImprovementPlanReviewId ImprovementPlanReviewId { get; private set; }
        public ImprovementPlanObjectiveId ImprovementPlanObjectiveId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public string ProgressDetails { get; private set; }
        public string HowIsSchoolProgressing { get; private set; }

        public void SetProgress(string howIsSchoolProgressing, string progress)
        {
            HowIsSchoolProgressing = howIsSchoolProgressing;
            ProgressDetails = progress;
        }
    }
}
