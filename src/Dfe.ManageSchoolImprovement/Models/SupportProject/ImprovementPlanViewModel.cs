namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ImprovementPlanViewModel
    {
        public Guid Id { get; set; }
        public int SupportProjectId { get; set; }
        public List<ImprovementPlanObjectiveViewModel> ImprovementPlanObjectives { get; set; } = new();
        public int ReadableId { get; internal set; }
    }
}