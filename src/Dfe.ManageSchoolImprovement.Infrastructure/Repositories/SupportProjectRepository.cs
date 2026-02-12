using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Dfe.ManageSchoolImprovement.Utils;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Dfe.ManageSchoolImprovement.Infrastructure.Repositories
{
    public class SupportProjectRepository(RegionalImprovementForStandardsAndExcellenceContext dbContext)
        : Repository<SupportProject, RegionalImprovementForStandardsAndExcellenceContext>(dbContext), ISupportProjectRepository
    {
        public async Task<(IEnumerable<SupportProject> projects, int totalCount)> SearchForSupportProjects(
            SupportProjectSearchCriteria searchCriteria,
            int page,
            int count,
            CancellationToken cancellationToken)
        {
            IQueryable<SupportProject> queryable = DbSet();

            queryable = FilterByRegion(searchCriteria.Regions, queryable);
            queryable = FilterByKeyword(searchCriteria.Title, queryable);
            queryable = FilterByAssignedUsers(searchCriteria.AssignedUsers, queryable);
            queryable = FilterByAssignedAdvisers(searchCriteria.AssignedAdvisers, queryable);
            queryable = FilterByLocalAuthority(searchCriteria.LocalAuthorities, queryable);
            queryable = FilterByTrusts(searchCriteria.Trusts, queryable);
            queryable = FilterByDate(searchCriteria.Dates, queryable);
            queryable = FilterByState(searchCriteria.States, queryable);

            var totalProjects = await queryable.CountAsync(cancellationToken);
            var projects = await queryable
                .OrderByDescending(acp => acp.CreatedOn)
                .Skip((page - 1) * count)
                .Take(count).ToListAsync(cancellationToken);

            return (projects, totalProjects);
        }

        private static IQueryable<SupportProject> FilterByState(IEnumerable<string>? states, IQueryable<SupportProject> queryable)
        {
            if (states != null && states.Any())
            {
                // Convert state strings to enum values (case-insensitive)
                var enumValues = states
                    .Select(s => Enum.TryParse<ProjectStatusValue>(s, true, out var val) ? (ProjectStatusValue?)val : null)
                    .Where(e => e.HasValue)
                    .Select(e => e.Value)
                    .ToList();

                var stateSet = new HashSet<string>(states, StringComparer.OrdinalIgnoreCase);
                queryable = queryable.Where(p => enumValues.Contains(p.ProjectStatus));
            }

            return queryable;
        }

        private static IQueryable<SupportProject> FilterByTrusts(IEnumerable<string>? trusts, IQueryable<SupportProject> queryable)
        {
            if (trusts != null && trusts.Any())
            {
                var lowerCaseRegions = trusts.Select(trust => trust.ToLower());
                queryable = queryable.Where(p =>
                    !string.IsNullOrEmpty(p.TrustName) && lowerCaseRegions.Contains(p.TrustName.ToLower()));
            }

            return queryable;
        }

        private static IQueryable<SupportProject> FilterByAssignedUsers(IEnumerable<string>? assignedUsers, IQueryable<SupportProject> queryable)
        {
            if (assignedUsers != null && assignedUsers.Any())
            {
                var lowerCaseDeliveryOfficers = assignedUsers.Select(officer => officer.ToLower());

                if (lowerCaseDeliveryOfficers.Contains("not assigned"))
                {
                    // Query by unassigned or assigned delivery officer
                    queryable = queryable.Where(p =>
                                (!string.IsNullOrEmpty(p.AssignedDeliveryOfficerFullName) && lowerCaseDeliveryOfficers.Contains(p.AssignedDeliveryOfficerFullName.ToLower()))
                                || string.IsNullOrEmpty(p.AssignedDeliveryOfficerFullName));
                }
                else
                {
                    // Query by assigned delivery officer only
                    queryable = queryable.Where(p =>
                        !string.IsNullOrEmpty(p.AssignedDeliveryOfficerFullName) && lowerCaseDeliveryOfficers.Contains(p.AssignedDeliveryOfficerFullName.ToLower()));
                }
            }

            return queryable;
        }

        private static IQueryable<SupportProject> FilterByAssignedAdvisers(IEnumerable<string>? assignedAdvisers, IQueryable<SupportProject> queryable)
        {
            if (assignedAdvisers != null && assignedAdvisers.Any())
            {
                var lowerCaseAdvisers = assignedAdvisers.Select(adviser => adviser.ToLower());

                if (lowerCaseAdvisers.Contains("not assigned"))
                {
                    // Query by unassigned or assigned adviser
                    queryable = queryable.Where(p =>
                        (!string.IsNullOrEmpty(p.AdviserFullName) && lowerCaseAdvisers.Contains(p.AdviserFullName.ToLower()))
                        || string.IsNullOrEmpty(p.AdviserFullName));
                }
                else
                {
                    // Query by assigned adviser only
                    queryable = queryable.Where(p =>
                        !string.IsNullOrEmpty(p.AdviserFullName) && lowerCaseAdvisers.Contains(p.AdviserFullName.ToLower()));
                }
            }

            return queryable;
        }

        private static IQueryable<SupportProject> FilterByRegion(IEnumerable<string>? regions, IQueryable<SupportProject> queryable)
        {

            if (regions != null && regions.Any())
            {
                var lowerCaseRegions = regions.Select(region => region.ToLower());
                queryable = queryable.Where(p =>
                    !string.IsNullOrEmpty(p.Region) && lowerCaseRegions.Contains(p.Region.ToLower()));
            }

            return queryable;
        }

        private static IQueryable<SupportProject> FilterByKeyword(string? title, IQueryable<SupportProject> queryable)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                queryable = queryable.Where(p => EF.Functions.Like(p.SchoolName!, $"%{title}%") ||
                                                 EF.Functions.Like(p.SchoolUrn, $"%{title}%"));
            }

            return queryable;
        }

        private static IQueryable<SupportProject> FilterByLocalAuthority(IEnumerable<string>? localAuthorities, IQueryable<SupportProject> queryable)
        {
            if (localAuthorities != null && localAuthorities.Any())
            {
                var lowerCaseRegions = localAuthorities.Select(la => la.ToLower());
                queryable = queryable.Where(p =>
                    !string.IsNullOrEmpty(p.LocalAuthority) &&
                    lowerCaseRegions.Contains(p.LocalAuthority.ToLower()));
            }

            return queryable;
        }

        private static IQueryable<SupportProject> FilterByDate(IEnumerable<string>? dates,
            IQueryable<SupportProject> queryable)
        {
            if (dates != null && dates.Any())
            {
                var enGb = new CultureInfo("en-GB");
                var yearMonthKeys = dates
                    .Select(date => DateTime.ParseExact(date, "yyyy MMMM", enGb))
                    .Select(d => d.Year * 100 + d.Month)
                    .ToList();

                queryable = queryable.Where(p =>
                    yearMonthKeys.Contains(p.CreatedOn.Year * 100 + p.CreatedOn.Month));
            }

            return queryable;
        }

        public async Task<IEnumerable<string>> GetAllProjectRegions(CancellationToken cancellationToken)
        {
            return await DbSet().OrderByDescending(p => p.Region)
                    .AsNoTracking()
                    .Select(p => p.Region)
                    .Where(p => !string.IsNullOrEmpty(p))
                    .Distinct()
                    .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetAllProjectLocalAuthorities(CancellationToken cancellationToken)
        {
            return await DbSet().OrderByDescending(p => p.LocalAuthority)
                    .AsNoTracking()
                    .Select(p => p.LocalAuthority)
                    .Where(p => !string.IsNullOrEmpty(p))
                    .Distinct()
                    .ToListAsync(cancellationToken);

        }

        public async Task<SupportProject?> GetSupportProjectById(SupportProjectId id, CancellationToken cancellationToken)
        {
            return await DefaultIncludes().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<SupportProject?> GetSupportProjectByUrn(string urn, CancellationToken cancellationToken)
        {
            return await DefaultIncludes()
                .SingleOrDefaultAsync(x => x.SchoolUrn == urn, cancellationToken);
        }

        private IQueryable<SupportProject> DefaultIncludes()
        {
            return DbSet()
                .Include(x => x.Notes)
                .Include(x => x.Contacts)
                .Include(x => x.FundingHistories)
                .Include(x => x.ImprovementPlans)
                .ThenInclude(x => x.ImprovementPlanObjectives)
                .Include(x => x.ImprovementPlans)
                .ThenInclude(x => x.ImprovementPlanReviews)
                .ThenInclude(x => x.ImprovementPlanObjectiveProgresses)
                .Include(x => x.EngagementConcerns)
                .AsQueryable();
        }

        public async Task<IEnumerable<string>> GetAllProjectAssignedUsers(CancellationToken cancellationToken)
        {
            return await DbSet().OrderByDescending(p => p.AssignedDeliveryOfficerFullName)
                .AsNoTracking()
                .Select(p => p.AssignedDeliveryOfficerFullName!)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetAllProjectAssignedAdvisers(CancellationToken cancellationToken)
        {
            return await DbSet().OrderByDescending(p => p.AdviserFullName)
                .AsNoTracking()
                .Select(p => p.AdviserFullName!)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetAllProjectTrusts(CancellationToken cancellationToken)
        {
            return await DbSet().OrderByDescending(p => p.TrustName)
                .AsNoTracking()
                .Select(p => p.TrustName!)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetAllProjectYears(CancellationToken cancellationToken)
        {
            var years = await DbSet()
                .AsNoTracking()
                .Select(p => p.CreatedOn.Year)
                .Distinct()
                .OrderByDescending(year => year)
                .Select(year => year.ToString())
                .ToListAsync(cancellationToken);
            return years;
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllProjectStatuses(CancellationToken cancellationToken)
        {
            var statuses = await DbSet()
                .AsNoTracking()
                .Select(p => new KeyValuePair<string, string>(p.ProjectStatus.ToString(), p.ProjectStatus.GetDisplayName()))
                .Distinct()
                .ToListAsync(cancellationToken);

            return statuses;
        }
    }
}
