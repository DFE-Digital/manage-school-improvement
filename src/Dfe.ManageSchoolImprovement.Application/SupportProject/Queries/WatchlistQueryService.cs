using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries
{
    public class WatchlistQueryService(IWatchlistRepository watchlistRepository)
        : IWatchlistQueryService
    {

        public async Task<Result<IEnumerable<Watchlist>>> GetAllSchoolsForUser(string user,
            CancellationToken cancellationToken)
        {
            var supportProjects = await watchlistRepository.GetAllSchoolsForUser(user, cancellationToken);

            return Result<IEnumerable<Watchlist>>.Success(supportProjects);
        }

        public async Task<Result<IEnumerable<Watchlist>>> GetAllWatchlistsForSchool(SupportProjectId supportProjectId, CancellationToken cancellationToken)
        {
            var watchlists = await watchlistRepository.GetAllWatchlistsForSchool(supportProjectId, cancellationToken);
            
            return Result<IEnumerable<Watchlist>>.Success(watchlists);
        }

    }
}
