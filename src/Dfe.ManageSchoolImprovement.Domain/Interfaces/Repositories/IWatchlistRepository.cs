using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories
{
    public interface IWatchlistRepository : IRepository<Watchlist>
    {
        Task<IEnumerable<Watchlist>> GetAllSchoolsForUser(string user, CancellationToken cancellationToken);
        Task<IEnumerable<Watchlist>> GetAllWatchlistsForSchool(SupportProjectId supportProjectId, CancellationToken cancellationToken);
    }
}

