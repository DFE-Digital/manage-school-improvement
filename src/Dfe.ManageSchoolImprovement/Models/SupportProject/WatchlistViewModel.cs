using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class WatchlistViewModel
    {
        public int? SupportProjectId { get; set; }
        public string? User { get; set; }
        public string? SchoolName { get; set; }
        public DateTime? DateAdded { get; set; }
        public string? AssignedTo { get; set; }
        public string? SupportingOrganisationName { get; set; }
        public ProjectStatusValue? Status { get; set; }
    }
}