namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record ImprovementPlanObjectiveProgressDto(
            Guid id,
            int readableId,
            Guid improvementPlanReviewId,
            Guid improvementPlanObjectiveId,
            string howIsSchoolProgressing,
            string progressDetails
            )
    {
    }
}
