using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class ImprovementPlanObjectiveProgress : IEntity<ImprovementPlanObjectiveProgressId>
    {
        public ImprovementPlanObjectiveProgress(ImprovementPlanObjectiveProgressId id, ImprovementPlanObjectiveId improvementPlanObjectiveId, ImprovementPlanReviewId improvementPlanReviewId, string progressDetails)
        {
            Id = id;
            ImprovementPlanObjectiveId = improvementPlanObjectiveId;
            ImprovementPlanReviewId = improvementPlanReviewId;
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
        public string ProgressDetails { get; set; }
        public string ProgressStatus { get; set; }

        public void SetProgress(string progress)
        {
            ProgressDetails = progress;
        }
    }
}
