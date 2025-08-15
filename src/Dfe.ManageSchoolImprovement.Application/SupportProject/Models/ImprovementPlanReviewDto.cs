namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record ImprovementPlanReviewDto(
            Guid id,
            int readableId,
            Guid improvementPlanId,
            DateTime reviewDate,
            DateTime? nextReviewDate,
            string reviewer,
            string title,
            int order,
            IEnumerable<ImprovementPlanObjectiveProgressDto> ImprovementPlanObjectiveProgresses = null!
            )
    {
    }
}
