using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using GovUK.Dfe.CoreLibs.Contracts.ExternalApplications.Models.Response;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;

public class SupportProjectAuditQueryService(
    ISupportProjectAuditRepository supportProjectAuditRepository,
    IMapper mapper) : ISupportProjectAuditQueryService
{
    /// <inheritdoc />
    public async Task<Result<IEnumerable<SupportProjectDto>>> GetSupportProjectHistoryAsync(
        int supportProjectId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var supportProjectIdValue = new SupportProjectId(supportProjectId);
            var supportProjects = await supportProjectAuditRepository
                .GetSupportProjectHistoryAsync(supportProjectIdValue, cancellationToken);

            var result = supportProjects.Select(sp => mapper.Map<SupportProjectDto>(sp));

            return Result<IEnumerable<SupportProjectDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<SupportProjectDto>>.Failure($"Error retrieving support project history: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<Result<SupportProjectDto?>> GetSupportProjectAsOfAsync(
        int supportProjectId,
        DateTime asOfDate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var supportProjectIdValue = new SupportProjectId(supportProjectId);
            var supportProject = await supportProjectAuditRepository
                .GetSupportProjectAsOfAsync(supportProjectIdValue, asOfDate, cancellationToken);

            var result = mapper.Map<SupportProjectDto?>(supportProject);

            return Result<SupportProjectDto?>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<SupportProjectDto?>.Failure($"Error retrieving support project as of date: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<SupportProjectDto>>> GetSupportProjectBetweenDatesAsync(
        int supportProjectId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var supportProjectIdValue = new SupportProjectId(supportProjectId);
            var supportProjects = await supportProjectAuditRepository
                .GetSupportProjectBetweenDatesAsync(supportProjectIdValue, fromDate, toDate, cancellationToken);

            var result = supportProjects.Select(sp => mapper.Map<SupportProjectDto>(sp));

            return Result<IEnumerable<SupportProjectDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<SupportProjectDto>>.Failure($"Error retrieving support project between dates: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<SupportProjectDto>>> GetSupportProjectFromDateAsync(
        int supportProjectId,
        DateTime fromDate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var supportProjectIdValue = new SupportProjectId(supportProjectId);
            var supportProjects = await supportProjectAuditRepository
                .GetSupportProjectFromDateAsync(supportProjectIdValue, fromDate, cancellationToken);

            var result = supportProjects.Select(sp => mapper.Map<SupportProjectDto>(sp));

            return Result<IEnumerable<SupportProjectDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<SupportProjectDto>>.Failure($"Error retrieving support project from date: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<SupportProjectDto>>> GetAllSupportProjectsAsOfAsync(
        DateTime asOfDate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var supportProjects = await supportProjectAuditRepository
                .GetAllSupportProjectsAsOfAsync(asOfDate, cancellationToken);

            var result = supportProjects.Select(sp => mapper.Map<SupportProjectDto>(sp));

            return Result<IEnumerable<SupportProjectDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<SupportProjectDto>>.Failure($"Error retrieving all support projects as of date: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<SupportProjectFieldAuditDto<T>>>> GetFieldAuditTrailAsync<T>(
        int supportProjectId,
        Expression<Func<Domain.Entities.SupportProject.SupportProject, T>> fieldSelector,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var supportProjectIdValue = new SupportProjectId(supportProjectId);
            var fieldAuditTrail = await supportProjectAuditRepository
                .GetFieldAuditTrailAsync(supportProjectIdValue, fieldSelector, cancellationToken);

            // Manual mapping instead of AutoMapper
            var result = fieldAuditTrail.Select(audit => new SupportProjectFieldAuditDto<T>(
                audit.SupportProjectId.Value,
                audit.FieldName,
                audit.FieldValue,
                audit.ValidFrom,
                audit.ValidTo,
                audit.LastModifiedBy,
                audit.LastModifiedOn
            ));

            return Result<IEnumerable<SupportProjectFieldAuditDto<T>>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<SupportProjectFieldAuditDto<T>>>.Failure($"Error retrieving field audit trail: {ex.Message}");
        }
    }
}