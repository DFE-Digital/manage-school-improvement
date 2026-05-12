using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries
{
    public interface IWatchlistQueryService
    {
        Task<Result<IEnumerable<Watchlist>>> GetAllSchoolsForUser(string user, CancellationToken cancellationToken);
    }
}
