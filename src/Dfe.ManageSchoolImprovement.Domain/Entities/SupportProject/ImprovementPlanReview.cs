using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class ImprovementPlanReview : IEntity<ImprovementPlanReviewId>
    {
        public ImprovementPlanReview(
            ImprovementPlanReviewId id,
            ImprovementPlanId improvementPlanId,
            DateTime reviewDate,
            string reviewer,
            string title,
            int order)
        {
            Id = id;
            ImprovementPlanId = improvementPlanId;
            ReviewDate = reviewDate;
            Reviewer = reviewer;
            Title = title;
            Order = order;
        }

        public ImprovementPlanReviewId Id { get; private set; }
        public int ReadableId { get; }
        public ImprovementPlanId ImprovementPlanId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string Title { get; set; }
        public int Order { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Reviewer { get; set; }
        public string? HowIsTheSchoolProgressingOverall { get; set; }
        public string? OverallProgressDetails { get; set; }

        public IEnumerable<ImprovementPlanObjectiveProgress> ImprovementPlanObjectiveProgresses => _ImprovementPlanObjectiveProgresses.AsReadOnly();
        private readonly List<ImprovementPlanObjectiveProgress> _ImprovementPlanObjectiveProgresses = new();

        public void AddObjectiveProgress(ImprovementPlanObjectiveProgressId improvementPlanObjectiveProgressId,
                                         ImprovementPlanObjectiveId improvementPlanObjectiveId,
                                         ImprovementPlanReviewId improvementPlanReviewId,
                                         string progressStatus,
                                         string progressDetails)
        {
            _ImprovementPlanObjectiveProgresses.Add(new ImprovementPlanObjectiveProgress(
                improvementPlanObjectiveProgressId,
                improvementPlanObjectiveId,
                Id,
                progressStatus,
                progressDetails));
        }

        public void SetImprovementPlanObjectiveProgressDetails(ImprovementPlanObjectiveProgressId improvementPlanObjectiveProgressId, string progressStatus, string progressDetails)
        {
            var progress = _ImprovementPlanObjectiveProgresses.SingleOrDefault(x => x.Id == improvementPlanObjectiveProgressId);

            if (progress == null)
            {
                throw new KeyNotFoundException($"Improvement plan review objective progress with id {improvementPlanObjectiveProgressId} not found");
            }

            progress.SetProgress(progressStatus, progressDetails);

        }

        public void SetNextReviewDate(DateTime? nextReviewDate)
        {
            NextReviewDate = nextReviewDate;
        }

        public void SetDetails(string reviewer, DateTime reviewDate)
        {
            Reviewer = reviewer;
            ReviewDate = reviewDate;
        }

        public void SetOverallProgress(string howIsTheSchoolProgressingOverall, string overallProgressDetails)
        {
            HowIsTheSchoolProgressingOverall = howIsTheSchoolProgressingOverall;
            OverallProgressDetails = overallProgressDetails;
        }
    }
}
