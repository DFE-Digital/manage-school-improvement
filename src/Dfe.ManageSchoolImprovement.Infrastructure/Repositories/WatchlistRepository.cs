using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageSchoolImprovement.Infrastructure.Repositories
{
    public class WatchlistRepository(RegionalImprovementForStandardsAndExcellenceContext dbContext)
        : Repository<Watchlist, RegionalImprovementForStandardsAndExcellenceContext>(dbContext), IWatchlistRepository
    {
        public async Task<IEnumerable<Watchlist>> GetAllSchoolsForUser(string user, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(user))
            {
                return Array.Empty<Watchlist>();
            }

            return await DbSet()
                .AsNoTracking()
                .Where(w => w.User == user)
                .Select(w => new Watchlist(w.Id,
                    w.SupportProjectId,
                    w.User!,
                    w.ReadableId))
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Watchlist>> GetAllWatchlistsForSchool(SupportProjectId supportProjectId,
            CancellationToken cancellationToken)
        {
            return await DbSet()
                .AsNoTracking()
                .Where(w => w.SupportProjectId == supportProjectId)
                .Select(w => new Watchlist(w.Id,
                    w.SupportProjectId,
                    w.User!,
                    w.ReadableId))
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
