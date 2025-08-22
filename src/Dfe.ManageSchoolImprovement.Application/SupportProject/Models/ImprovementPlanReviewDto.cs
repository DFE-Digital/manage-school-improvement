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
            string howIsTheSchoolProgressingOverall,
            string overallProgressDetails,
            IEnumerable<ImprovementPlanObjectiveProgressDto> ImprovementPlanObjectiveProgresses = null!
            )
    {
    }
}
