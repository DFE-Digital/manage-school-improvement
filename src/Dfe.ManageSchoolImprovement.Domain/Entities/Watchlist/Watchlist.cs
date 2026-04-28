using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class Watchlist : BaseAggregateRoot, IEntity<WatchlistId>
    {
        private Watchlist()
        {
            Id = default!;
            SupportProjectId = default!;
        }

        public Watchlist(WatchlistId id, SupportProjectId supportProjectId, string user)
        {
            Id = id;
            SupportProjectId = supportProjectId;
            User = user;
        }

        public WatchlistId Id { get; set; }
        public int ReadableId { get; }
        public SupportProjectId SupportProjectId { get; set; }
        public string? User { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}