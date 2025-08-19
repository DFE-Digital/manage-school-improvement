using Dfe.Academisation.ExtensionMethods;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ImprovementPlanReviewViewModel
    {
        internal const string ProgressStatusNotRecorded = "Progress not recorded";
        internal const string ProgressStatusPartlyRecorded = "Progress partly recorded";
        internal const string ProgressStatusRecorded = "Progress recorded";

        public Guid Id { get; set; }
        public int ReadableId { get; set; }
        public Guid ImprovementPlanId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Reviewer { get; set; } = string.Empty;
        public string ProgressStatusClass { get; set; } = string.Empty;
        public string ProgressStatus { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }


        public List<ImprovementPlanObjectiveProgressViewModel> ImprovementPlanObjectiveProgresses { get; set; } = new();
        public DateTime? NextReviewDate { get; private set; }

        public static ImprovementPlanReviewViewModel Create(ImprovementPlanReviewDto dto, ImprovementPlanDto improvementPlanDto)
        {
            var progressStatus = ProgressStatusNotRecorded;
            var progressStatusCssClass = "govuk-tag--blue";

            if (dto.ImprovementPlanObjectiveProgresses != null && dto.ImprovementPlanObjectiveProgresses.Any())
            {
                var progresses = dto.ImprovementPlanObjectiveProgresses.Count();
                var objectives = improvementPlanDto.ImprovementPlanObjectives.Count();

                if (progresses == 0)
                {
                    progressStatus = ProgressStatusNotRecorded;
                    progressStatusCssClass = "govuk-tag--blue";
                }
                else if (progresses < objectives)
                {
                    progressStatus = ProgressStatusPartlyRecorded;
                    progressStatusCssClass = "govuk-tag--orange";
                }
                else
                {
                    progressStatus = ProgressStatusRecorded;
                    progressStatusCssClass = "govuk-tag--green";
                }
            }
            return new ImprovementPlanReviewViewModel
            {
                Id = dto.id,
                ReadableId = dto.readableId,
                ImprovementPlanId = dto.improvementPlanId,
                ReviewDate = dto.reviewDate,
                NextReviewDate = dto.nextReviewDate,
                Reviewer = dto.reviewer,
                ImprovementPlanObjectiveProgresses = dto.ImprovementPlanObjectiveProgresses?
                    .Select(ImprovementPlanObjectiveProgressViewModel.Create)
                    .ToList() ?? new List<ImprovementPlanObjectiveProgressViewModel>(),
                ProgressStatus = progressStatus,
                ProgressStatusClass = progressStatusCssClass,
                Title = dto.title.ToSentenceCase(),
                Order = dto.order
            };
        }
    }
}