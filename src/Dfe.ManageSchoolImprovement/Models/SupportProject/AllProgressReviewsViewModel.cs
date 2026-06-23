using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class AllProgressReviewsViewModel
    {
        public Guid Id { get; set; }
        public int ReadableId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Reviewer { get; set; } = string.Empty;
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ProgressStatusClass { get; set; } = string.Empty;
        public string ProgressStatus { get; set; } = string.Empty;
        public DateTime? NextReviewDate { get; private set; }
        
        public string Type { get; set; } = string.Empty;

        public static AllProgressReviewsViewModel Create(ProgressReviewViewModel model, string progressStatusClass, string progressStatus)
        {
            return new AllProgressReviewsViewModel
            {
                Id = model.Id,
                ReadableId = model.ReadableId,
                ReviewDate = model.ReviewDate,
                Reviewer = model.Reviewer,
                Order = model.Order,
                Title = model.Title,
                ProgressStatusClass = progressStatusClass,
                ProgressStatus = progressStatus,
                NextReviewDate = model.NextReviewDate,
                Type = "Review progress"
            };
        }

        public static AllProgressReviewsViewModel Create(ImprovementPlanReviewViewModel model, string progressStatusClass, string progressStatus)
        {
            return new AllProgressReviewsViewModel
            {
                Id = model.Id,
                ReadableId = model.ReadableId,
                ReviewDate = model.ReviewDate,
                Reviewer = model.Reviewer,
                Order = model.Order,
                Title = model.Title,
                ProgressStatusClass = progressStatusClass,
                ProgressStatus = progressStatus,
                NextReviewDate = model.NextReviewDate,
                Type = "Matched"
            };
        }
    }
}