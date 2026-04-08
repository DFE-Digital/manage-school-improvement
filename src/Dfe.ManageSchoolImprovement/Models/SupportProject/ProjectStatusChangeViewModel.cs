using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ProjectStatusChangeViewModel
    {
        public ProjectStatusValue ProjectStatus { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime? ChangedDateOfDecision { get; set; }
        public string? ChangedDetails { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime ValidFrom { get; set; }
        public SupportProjectEligibilityStatus? Eligibility { get; set; }
        public DateTime? DateSupportIsDueToEnd { get; set; }
    }
}
