using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ImprovementPlanViewModel
    {
        public Guid Id { get; set; }
        public int ReadableId { get; set; }
        public int SupportProjectId { get; set; }
        public bool? ObjectivesSectionComplete { get; set; }
        public List<ImprovementPlanObjectiveViewModel> ImprovementPlanObjectives { get; set; } = new();
        public List<ImprovementPlanReviewViewModel> ImprovementPlanReviews { get; set; } = new();

        public static ImprovementPlanViewModel Create(ImprovementPlanDto dto)
        {
            return new ImprovementPlanViewModel
            {
                Id = dto.id,
                ReadableId = dto.readableId,
                SupportProjectId = dto.supportProjectId,
                ObjectivesSectionComplete = dto.objectivesSectionComplete,
                ImprovementPlanObjectives = dto.ImprovementPlanObjectives?
                    .Select(ImprovementPlanObjectiveViewModel.Create)
                    .ToList() ?? new List<ImprovementPlanObjectiveViewModel>(),
                ImprovementPlanReviews = dto.ImprovementPlanReviews?
                    .Select(x => ImprovementPlanReviewViewModel.Create(x, dto))
                    .ToList() ?? new List<ImprovementPlanReviewViewModel>()
            };
        }
    }
}