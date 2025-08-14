using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class ImprovementPlan : IEntity<ImprovementPlanId>
    {
        public ImprovementPlan(ImprovementPlanId id, SupportProjectId supportProjectId)
        {
            Id = id;
            SupportProjectId = supportProjectId;
        }

        public ImprovementPlanId Id { get; private set; }
        public int ReadableId { get; }
        public SupportProjectId SupportProjectId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool? ObjectivesSectionComplete { get; set; }

        public IEnumerable<ImprovementPlanObjective> ImprovementPlanObjectives => _improvementPlanObjectives.AsReadOnly();
        private readonly List<ImprovementPlanObjective> _improvementPlanObjectives = new();

        public IEnumerable<ImprovementPlanReview> ImprovementPlanReviews => _improvementPlanReviews.AsReadOnly();
        private readonly List<ImprovementPlanReview> _improvementPlanReviews = new();

        public void AddObjective(ImprovementPlanObjectiveId improvementPlanObjectiveId, ImprovementPlanId improvementPlanId, string areaOfImprovement, string details, int order)
        {
            _improvementPlanObjectives.Add(new ImprovementPlanObjective(improvementPlanObjectiveId, improvementPlanId, areaOfImprovement, details, order));
        }

        public void SetObjectivesComplete(bool objectivesSectionComplete)
        {
            ObjectivesSectionComplete = objectivesSectionComplete;
        }

        public void SetObjectiveDetails(ImprovementPlanObjectiveId improvementPlanObjectiveId, string details)
        {
            var objective = _improvementPlanObjectives.FirstOrDefault(o => o.Id == improvementPlanObjectiveId);

            if (objective == null)
            {
                throw new KeyNotFoundException($"Improvement plan objective with id {improvementPlanObjectiveId} not found");
            }

            objective.SetDetails(details);
        }

        public void AddReview(ImprovementPlanReviewId improvementPlanReviewId, string reviewer, DateTime reviewDate)
        {
            var order = _improvementPlanReviews.Count + 1;
            var title = $"{order.ToOrdinalWord()} Review";

            _improvementPlanReviews.Add(new ImprovementPlanReview(improvementPlanReviewId, Id, reviewDate, reviewer, title, order));
        }

        public void AddImprovementPlanObjectiveProgress(ImprovementPlanReviewId improvementPlanReviewId, ImprovementPlanObjectiveProgressId improvementPlanObjectiveProgressId, ImprovementPlanObjectiveId improvementPlanObjectiveId, string progressStatus, string progressDetails)
        {
            var review = _improvementPlanReviews.SingleOrDefault(x => x.Id == improvementPlanReviewId);

            if (review == null)
            {
                throw new KeyNotFoundException($"Improvement plan review with id {improvementPlanReviewId} not found");
            }

            review.AddObjectiveProgress(improvementPlanObjectiveProgressId,
                                        improvementPlanObjectiveId,
                                        improvementPlanReviewId,
                                        progressStatus,
                                        progressDetails);
        }

        public void SetImprovementPlanObjectiveProgressDetails(ImprovementPlanReviewId improvementPlanReviewId, ImprovementPlanObjectiveProgressId improvementPlanObjectiveProgressId, string progressStatus, string progressDetails)
        {
            var review = _improvementPlanReviews.SingleOrDefault(x => x.Id == improvementPlanReviewId);

            if (review == null)
            {
                throw new KeyNotFoundException($"Improvement plan review with id {improvementPlanReviewId} not found");
            }

            review.SetImprovementPlanObjectiveProgressDetails(
                improvementPlanObjectiveProgressId,
                progressStatus,
                progressDetails);
        }

        public void SetNextReviewDate(ImprovementPlanReviewId improvementPlanReviewId, DateTime? nextReviewDate)
        {
            var review = _improvementPlanReviews.SingleOrDefault(x => x.Id == improvementPlanReviewId);

            if (review == null)
            {
                throw new KeyNotFoundException($"Improvement plan review with id {improvementPlanReviewId} not found");
            }

            review.SetNextReviewDate(nextReviewDate);
        }

        public void SetImprovementPlanReviewDetails(ImprovementPlanReviewId improvementPlanReviewId, string reviewer, DateTime reviewDate)
        {
            var review = _improvementPlanReviews.SingleOrDefault(x => x.Id == improvementPlanReviewId);

            if (review == null)
            {
                throw new KeyNotFoundException($"Improvement plan review with id {improvementPlanReviewId} not found");
            }

            review.SetDetails(reviewer, reviewDate);
        }
    }
}
