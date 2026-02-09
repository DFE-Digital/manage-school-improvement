using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using GovUK.Dfe.CoreLibs.Contracts.ExternalApplications.Models.Response;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;

public interface ISupportProjectAuditQueryService
{
    /// <summary>
    /// Gets all historical versions of a SupportProject by its ID
    /// </summary>
    /// <param name="supportProjectId">The support project ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all historical versions of the support project</returns>
    Task<Result<IEnumerable<SupportProjectDto>>> GetSupportProjectHistoryAsync(
        int supportProjectId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the SupportProject as it was at a specific point in time
    /// </summary>
    /// <param name="supportProjectId">The support project ID</param>
    /// <param name="asOfDate">The date/time to query for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The support project as it existed at the specified date, or null if not found</returns>
    Task<Result<SupportProjectDto?>> GetSupportProjectAsOfAsync(
        int supportProjectId,
        DateTime asOfDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all versions of a SupportProject that existed between two dates
    /// </summary>
    /// <param name="supportProjectId">The support project ID</param>
    /// <param name="fromDate">Start date (inclusive)</param>
    /// <param name="toDate">End date (inclusive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of support project versions that existed in the date range</returns>
    Task<Result<IEnumerable<SupportProjectDto>>> GetSupportProjectBetweenDatesAsync(
        int supportProjectId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all versions of a SupportProject from a specific date onwards
    /// </summary>
    /// <param name="supportProjectId">The support project ID</param>
    /// <param name="fromDate">Start date (inclusive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of support project versions from the specified date</returns>
    Task<Result<IEnumerable<SupportProjectDto>>> GetSupportProjectFromDateAsync(
        int supportProjectId,
        DateTime fromDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all SupportProjects that were active/existed at a specific point in time
    /// </summary>
    /// <param name="asOfDate">The date/time to query for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all support projects that existed at the specified date</returns>
    Task<Result<IEnumerable<SupportProjectDto>>> GetAllSupportProjectsAsOfAsync(
        DateTime asOfDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the audit trail for a specific field of a SupportProject
    /// </summary>
    /// <param name="supportProjectId">The support project ID</param>
    /// <param name="fieldSelector">Expression to select the field to audit</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of field changes over time with timestamps</returns>
    Task<Result<IEnumerable<SupportProjectFieldAuditDto<T>>>> GetFieldAuditTrailAsync<T>(
        int supportProjectId,
        Expression<Func<Domain.Entities.SupportProject.SupportProject, T>> fieldSelector,
        CancellationToken cancellationToken = default);
}