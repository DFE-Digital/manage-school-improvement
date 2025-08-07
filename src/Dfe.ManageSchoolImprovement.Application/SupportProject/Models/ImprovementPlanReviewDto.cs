namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record ImprovementPlanReviewDto(
            Guid id,
            int readableId,
            Guid improvementPlanId,
            DateTime reviewDate,
            string reviewer,
            IEnumerable<ImprovementPlanObjectiveProgressDto> ImprovementPlanObjectiveProgresses = null!
            )
    {
    }
}
