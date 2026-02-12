using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ProjectStatusChangeViewModel
    {
        public ProjectStatusValue ProjectStatus { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string ChangedDetails { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}