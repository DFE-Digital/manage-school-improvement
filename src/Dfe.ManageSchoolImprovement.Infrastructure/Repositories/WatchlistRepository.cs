using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageSchoolImprovement.Infrastructure.Repositories
{
    public class WatchlistRepository(RegionalImprovementForStandardsAndExcellenceContext dbContext)
        : Repository<Watchlist, RegionalImprovementForStandardsAndExcellenceContext>(dbContext), IWatchlistRepository
    {
        public async Task<IEnumerable<int>> GetAllSchoolsForUser(string user, CancellationToken cancellationToken)
        {
            return await DbSet()
                .AsNoTracking()
                .Where(w => w.User == user)
                .Select(w => w.SupportProjectId.Value)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}