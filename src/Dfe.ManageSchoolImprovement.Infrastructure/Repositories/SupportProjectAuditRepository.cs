using Dfe.ManageSchoolImprovement.Domain.Entities.Audit;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class SupportProjectAuditRepository(RegionalImprovementForStandardsAndExcellenceContext dbContext)
        : Repository<SupportProject, RegionalImprovementForStandardsAndExcellenceContext>(dbContext), ISupportProjectAuditRepository
    {
        private const string PeriodStart = "PeriodStart";
        private const string PeriodEnd = "PeriodEnd";

        /// <summary>
        /// Gets all historical versions of a SupportProject by its ID
        /// </summary>
        /// <param name="supportProjectId">The support project ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of all historical versions of the support project</returns>
        public async Task<ICollection<SupportProject>> GetSupportProjectHistoryAsync(
            SupportProjectId supportProjectId,
            CancellationToken cancellationToken = default)
        {
            return await DbContext.SupportProjects
                .TemporalAll()
                .Where(sp => sp.Id == supportProjectId)
                .OrderBy(sp => EF.Property<DateTime>(sp, PeriodStart))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the SupportProject as it was at a specific point in time
        /// </summary>
        /// <param name="supportProjectId">The support project ID</param>
        /// <param name="asOfDate">The date/time to query for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The support project as it existed at the specified date, or null if not found</returns>
        public async Task<SupportProject?> GetSupportProjectAsOfAsync(
            SupportProjectId supportProjectId,
            DateTime asOfDate,
            CancellationToken cancellationToken = default)
        {
            return await DbContext.SupportProjects
                .TemporalAsOf(asOfDate)
                .FirstOrDefaultAsync(sp => sp.Id == supportProjectId, cancellationToken);
        }

        /// <summary>
        /// Gets all versions of a SupportProject that existed between two dates
        /// </summary>
        /// <param name="supportProjectId">The support project ID</param>
        /// <param name="fromDate">Start date (inclusive)</param>
        /// <param name="toDate">End date (inclusive)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of support project versions that existed in the date range</returns>
        public async Task<ICollection<SupportProject>> GetSupportProjectBetweenDatesAsync(
            SupportProjectId supportProjectId,
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken = default)
        {
            return await DbContext.SupportProjects
                .TemporalBetween(fromDate, toDate)
                .Where(sp => sp.Id == supportProjectId)
                .OrderBy(sp => EF.Property<DateTime>(sp, PeriodStart))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets all versions of a SupportProject from a specific date onwards
        /// </summary>
        /// <param name="supportProjectId">The support project ID</param>
        /// <param name="fromDate">Start date (inclusive)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of support project versions from the specified date</returns>
        public async Task<ICollection<SupportProject>> GetSupportProjectFromDateAsync(
            SupportProjectId supportProjectId,
            DateTime fromDate,
            CancellationToken cancellationToken = default)
        {
            return await DbContext.SupportProjects
                .TemporalFromTo(fromDate, DateTime.UtcNow)
                .Where(sp => sp.Id == supportProjectId)
                .OrderBy(sp => EF.Property<DateTime>(sp, PeriodStart))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets all SupportProjects that were active/existed at a specific point in time
        /// </summary>
        /// <param name="asOfDate">The date/time to query for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of all support projects that existed at the specified date</returns>
        public async Task<ICollection<SupportProject>> GetAllSupportProjectsAsOfAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default)
        {
            return await DbContext.SupportProjects
                .TemporalAsOf(asOfDate)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the audit trail for a specific field of a SupportProject - only shows records where the field actually changed
        /// </summary>
        /// <param name="supportProjectId">The support project ID</param>
        /// <param name="fieldSelector">Expression to select the field to audit</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of field changes over time with timestamps - only when the field value changed</returns>
        public async Task<ICollection<SupportProjectFieldAudit<T>>> GetFieldAuditTrailAsync<T>(
            SupportProjectId supportProjectId,
            Expression<Func<SupportProject, T>> fieldSelector,
            CancellationToken cancellationToken = default)
        {
            var fieldName = GetMemberName(fieldSelector);
            var compiledSelector = fieldSelector.Compile();

            // Get all historical records with temporal properties
            var historicalData = await DbContext.SupportProjects
                .TemporalAll()
                .Where(sp => sp.Id == supportProjectId)
                .Select(sp => new
                {
                    SupportProject = sp,
                    ValidFrom = EF.Property<DateTime>(sp, PeriodStart),
                    ValidTo = EF.Property<DateTime>(sp, PeriodEnd)
                })
                .OrderBy(x => x.ValidFrom)
                .ToListAsync(cancellationToken);

            // Filter to only records where the field value changed
            var changesOnly = historicalData
                .Aggregate(
                    new List<SupportProjectFieldAudit<T>>(),
                    (acc, current) =>
                    {
                        var currentValue = compiledSelector(current.SupportProject);
                        var audit = new SupportProjectFieldAudit<T>
                        {
                            SupportProjectId = current.SupportProject.Id,
                            FieldName = fieldName,
                            FieldValue = currentValue,
                            ValidFrom = current.ValidFrom,
                            ValidTo = current.ValidTo,
                            LastModifiedBy = current.SupportProject.LastModifiedBy,
                            LastModifiedOn = current.SupportProject.LastModifiedOn
                        };

                        // Include if it's the first record or if the value changed
                        if (acc.Count == 0 || !EqualityComparer<T>.Default.Equals(currentValue, acc[acc.Count - 1].FieldValue))
                        {
                            acc.Add(audit);
                        }

                        return acc;
                    });

            return changesOnly;
        }

        private static string GetMemberName<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            throw new ArgumentException("Expression must be a member expression");
        }
    }
}
