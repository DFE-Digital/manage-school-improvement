using Dfe.Academisation.ExtensionMethods;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService,
    ISupportProjectAuditQueryService supportProjectAuditQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }
    
    public ProjectStatusValue? SupportProjectStatus { get; set; }
    public DateTime? DateOfDecision { get; set; }
    public string? AdditionalDetails { get; set; }
    
    public IList<SupportProjectFieldAuditDto<ProjectStatusChangeViewModel?>> ProjectStatusAuditTrail { get; set; } = [];


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SupportProjectStatus = SupportProject?.ProjectStatus;
        DateOfDecision = SupportProject?.ProjectStatusChangedDate;
        AdditionalDetails = SupportProject?.ProjectStatusChangedDetails;
        
        var auditTrail = new List<SupportProjectFieldAuditDto<string?>>();

        // Get audit trail for project status
        var projectStatusAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatus.GetDisplayName(), cancellationToken);
        
        if (projectStatusAuditResult.IsSuccess)
        {
            var filteredProjectStatusAudits = projectStatusAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
            auditTrail.AddRange(filteredProjectStatusAudits!);
        }
        
        // Get audit trail for changed by
        var changedByAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatusChangedBy, cancellationToken);
        
        if (changedByAuditResult.IsSuccess)
        {
            var filteredChangedByAudits = changedByAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
            auditTrail.AddRange(filteredChangedByAudits);
        }
        
        // Get audit trail for date of decision
        var dateOfDecisionAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatusChangedDate.ToString(), cancellationToken);
        
        if (projectStatusAuditResult.IsSuccess)
        {
            var filteredDateOfDecisionAudits = dateOfDecisionAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
            auditTrail.AddRange(filteredDateOfDecisionAudits);
        }
        
        // Get audit trail for details
        var detailsAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatusChangedDetails, cancellationToken);
        
        if (detailsAuditResult.IsSuccess)
        {
            var filteredDetailsAudits = detailsAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
            auditTrail.AddRange(filteredDetailsAudits);
        }

        return Page();
    }
}