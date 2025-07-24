namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ImprovementPlanObjectiveViewModel
    {
        public Guid Id { get; set; }
        public Guid ImprovementPlanId { get; set; }
        public string AreaOfImprovement { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}