using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ImprovementPlanObjectiveProgressViewModel
    {
        public Guid Id { get; set; }
        public int ReadableId { get; set; }
        public Guid ImprovementPlanReviewId { get; set; }
        public Guid ImprovementPlanObjectiveId { get; set; }
        public string HowIsSchoolProgressing { get; set; } = string.Empty;
        public string ProgressDetails { get; set; } = string.Empty;

        public static ImprovementPlanObjectiveProgressViewModel Create(ImprovementPlanObjectiveProgressDto dto)
        {
            return new ImprovementPlanObjectiveProgressViewModel
            {
                Id = dto.id,
                ReadableId = dto.readableId,
                ImprovementPlanReviewId = dto.improvementPlanReviewId,
                ImprovementPlanObjectiveId = dto.improvementPlanObjectiveId,
                HowIsSchoolProgressing = dto.howIsSchoolProgressing,
                ProgressDetails = dto.progressDetails
            };
        }
    }
}