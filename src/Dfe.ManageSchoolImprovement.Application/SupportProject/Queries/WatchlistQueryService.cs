using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries
{
    public class WatchlistQueryService(IWatchlistRepository watchlistRepository)
        : IWatchlistQueryService
    {

        public async Task<Result<IEnumerable<int>>> GetAllSchoolsForUser(string user,
            CancellationToken cancellationToken)
        {
            var supportProjectIds = await watchlistRepository.GetAllSchoolsForUser(user, cancellationToken);

            return Result<IEnumerable<int>>.Success(supportProjectIds);
        }

    }
}
