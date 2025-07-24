namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record ImprovementPlanDto(Guid id,
            int readableId,
            int supportProjectId,
            IEnumerable<ImprovementPlanObjectiveDto> ImprovementPlanObjectives = null!
            )
    {
    }
}
