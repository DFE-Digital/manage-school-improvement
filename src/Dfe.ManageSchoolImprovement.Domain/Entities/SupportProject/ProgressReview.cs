using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class ProgressReview : IEntity<ProgressReviewId>
    {
        public ProgressReview(ProgressReviewId id,
            SupportProjectId supportProjectId,
            DateTime reviewDate,
            string reviewer,
            int order,
            string title)
        {
            Id = id;
            SupportProjectId = supportProjectId;
            ReviewDate = reviewDate;
            Reviewer = reviewer;
            Order = order;
            Title = title;
        }
        
        public ProgressReviewId Id { get; private set; }
        public SupportProjectId SupportProjectId { get; private set; }
        public int ReadableId { get; }
        public DateTime ReviewDate { get; set; }
        public string Reviewer { get; set; } = string.Empty;
        public DateTime? NextReviewDate { get; set; }
        public string? NextSteps { get; set; }
        public string? AdditionalDetails { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }

        public void SetNextReviewDate(DateTime? nextReviewDate)
        {
            NextReviewDate = nextReviewDate;
        }
        
        public void SetDetails(string nextSteps, string? additionalDetails)
        {
            NextSteps = nextSteps;
            AdditionalDetails = additionalDetails;
        }
    }
}

