using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class ImprovementPlanReview : IEntity<ImprovementPlanReviewId>
    {
        public ImprovementPlanReview(ImprovementPlanReviewId id, ImprovementPlanId improvementPlanId, DateTime reviewDate, string reviewer)
        {
            Id = id;
            ImprovementPlanId = improvementPlanId;
            ReviewDate = reviewDate;
            Reviewer = reviewer;
        }

        public ImprovementPlanReviewId? Id { get; private set; }
        public int ReadableId { get; }
        public ImprovementPlanId ImprovementPlanId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Reviewer { get; set; }

        public IEnumerable<ImprovementPlanObjectiveProgress> ImprovementPlanObjectiveProgresses => _ImprovementPlanObjectiveProgresses.AsReadOnly();
        private readonly List<ImprovementPlanObjectiveProgress> _ImprovementPlanObjectiveProgresses = new();

        public void AddObjectiveProgress(ImprovementPlanObjectiveId improvementPlanObjectiveId, ImprovementPlanId improvementPlanId, string areaOfImprovement, string details, int order)
        {
            // _ImprovementPlanObjectiveProgresses.Add(new ImprovementPlanObjectiveProgress(improvementPlanObjectiveId, improvementPlanId, areaOfImprovement, details, order));
        }
    }
}
