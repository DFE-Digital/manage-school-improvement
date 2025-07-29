namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record ImprovementPlanObjectiveDto(Guid id,
            int readableId,
            Guid improvementPlanId,
            int order,
            string areaOfImprovement,
            string details
            )
    {
    }
}
