using Dfe.ManageSchoolImprovement.Domain.Entities.Audit;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories
{
    public interface ISupportProjectAuditRepository : IRepository<SupportProject>
    {
        /// <summary>
        /// Gets all historical versions of a SupportProject by its ID
        /// </summary>
        Task<ICollection<SupportProject>> GetSupportProjectHistoryAsync(
            SupportProjectId supportProjectId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the SupportProject as it was at a specific point in time
        /// </summary>
        Task<SupportProject?> GetSupportProjectAsOfAsync(
            SupportProjectId supportProjectId,
            DateTime asOfDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all versions of a SupportProject that existed between two dates
        /// </summary>
        Task<ICollection<SupportProject>> GetSupportProjectBetweenDatesAsync(
            SupportProjectId supportProjectId,
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all versions of a SupportProject from a specific date onwards
        /// </summary>
        Task<ICollection<SupportProject>> GetSupportProjectFromDateAsync(
            SupportProjectId supportProjectId,
            DateTime fromDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all SupportProjects that were active/existed at a specific point in time
        /// </summary>
        Task<ICollection<SupportProject>> GetAllSupportProjectsAsOfAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the audit trail for a specific field of a SupportProject
        /// </summary>
        Task<ICollection<SupportProjectFieldAudit<T>>> GetFieldAuditTrailAsync<T>(
            SupportProjectId supportProjectId,
            Expression<Func<SupportProject, T>> fieldSelector,
            CancellationToken cancellationToken = default);
    }
}