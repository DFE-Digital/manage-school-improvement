using Dfe.ManageSchoolImprovement.Application.Common.Models;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries
{
    public interface IWatchlistQueryService
    {
        Task<Result<IEnumerable<int>>> GetAllSchoolsForUser(string user, CancellationToken cancellationToken);
    }
}
