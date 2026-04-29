using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;

namespace Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories
{
    public interface IWatchlistRepository : IRepository<Watchlist>
    {
        Task<IEnumerable<int>> GetAllSchoolsForUser(string user, CancellationToken cancellationToken);
    }
}

