using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries
{
    public interface IWatchlistQueryService
    {
        Task<Result<IEnumerable<Watchlist>>> GetAllSchoolsForUser(string user, CancellationToken cancellationToken);
        Task<Result<IEnumerable<Watchlist>>> GetAllWatchlistsForSchool(SupportProjectId supportProjectId, CancellationToken cancellationToken);
    }
}
