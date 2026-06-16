using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ProgressReviewViewModel
    {
        internal const string ProgressStatusNotRecorded = "Progress not recorded";
        internal const string ProgressStatusRecorded = "Progress recorded";
        
        public Guid Id { get; set; }
        public int ReadableId { get; set; }
        public int SupportProjectId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Reviewer { get; set; } = string.Empty;
        public DateTime? NextReviewDate { get; private set; }
        public string? NextSteps { get; set; }
        public string? AdditionalDetails { get; set; }
        public string ProgressStatusClass { get; set; } = string.Empty;
        public string ProgressStatus { get; set; } = string.Empty;
        public int Order { get; set; }
        public string Title { get; set; }

        public static ProgressReviewViewModel Create(ProgressReviewDto dto)
        {
            var progressStatus = ProgressStatusNotRecorded;
            var progressStatusCssClass = "govuk-tag--blue";

            if (dto.nextSteps != null)
            {
                progressStatus = ProgressStatusRecorded;
                progressStatusCssClass = "govuk-tag--green";
            }
            
            return new ProgressReviewViewModel
            {
                Id = dto.id,
                ReadableId = dto.readableId,
                SupportProjectId = dto.supportProjectId,
                ReviewDate = dto.reviewDate,
                Reviewer = dto.reviewer,
                NextReviewDate = dto.nextReviewDate,
                NextSteps = dto.nextSteps,
                AdditionalDetails = dto.additionalDetails,
                ProgressStatusClass = progressStatusCssClass,
                ProgressStatus = progressStatus,
                Order = dto.order,
                Title = dto.title,
            };
        }
    }
}