using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ProgressReviewViewModel
    {
        public Guid Id { get; set; }
        public int ReadableId { get; set; }
        public int SupportProjectId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Reviewer { get; set; } = string.Empty;
        public DateTime? NextReviewDate { get; private set; }
        public int Order { get; set; }
        public string Title { get; set; }

        public static ProgressReviewViewModel Create(ProgressReviewDto dto)
        {
            return new ProgressReviewViewModel
            {
                Id = dto.id,
                ReadableId = dto.readableId,
                SupportProjectId = dto.supportProjectId,
                ReviewDate = dto.reviewDate,
                Reviewer = dto.reviewer,
                NextReviewDate = dto.nextReviewDate,
                Order = dto.order,
                Title = dto.title,
            };
        }
    }
}