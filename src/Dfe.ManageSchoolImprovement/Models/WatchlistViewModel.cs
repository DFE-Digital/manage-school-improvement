using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class WatchlistModel
    {
        public int? SupportProjectId { get; set; }
        public WatchlistId? WatchlistId { get; set; }
        public string? User { get; set; }
    }
}