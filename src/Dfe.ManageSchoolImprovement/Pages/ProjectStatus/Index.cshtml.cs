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
    
    public IList<ProjectStatusChangeViewModel?> ProjectStatusAuditTrail { get; set; } = [];


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SupportProjectStatus = SupportProject?.ProjectStatus;
        DateOfDecision = SupportProject?.ProjectStatusChangedDate;
        AdditionalDetails = SupportProject?.ProjectStatusChangedDetails;
        
        // get each audit trail separately. save as Lists
        
        var filteredProjectStatusAudits = new List<SupportProjectFieldAuditDto<ProjectStatusValue>>();
        var filteredChangedByAudits = new List<SupportProjectFieldAuditDto<string?>>();
        var filteredDateOfDecisionAudits = new List<SupportProjectFieldAuditDto<DateTime?>>();
        var filteredDetailsAudits = new List<SupportProjectFieldAuditDto<string?>>();

        // Get audit trail for project status
        var projectStatusAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatus, cancellationToken);
        
        if (projectStatusAuditResult.IsSuccess)
        {
            filteredProjectStatusAudits = projectStatusAuditResult.Value!
                .Where(a => a.FieldValue != null)
                .ToList();
        }
        
        // Get audit trail for changed by
        var changedByAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatusChangedBy, cancellationToken);
        
        if (changedByAuditResult.IsSuccess)
        {
            filteredChangedByAudits = changedByAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
        }
        
        // Get audit trail for date of decision
        var dateOfDecisionAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatusChangedDate, cancellationToken);
        
        if (projectStatusAuditResult.IsSuccess)
        {
            filteredDateOfDecisionAudits = dateOfDecisionAuditResult.Value!
                .Where(a => a.FieldValue != null)
                .ToList();
        }
        
        // Get audit trail for details
        var detailsAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatusChangedDetails, cancellationToken);
        
        if (detailsAuditResult.IsSuccess)
        {
            filteredDetailsAudits = detailsAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
        }
        
        // loop through filteredProjectStatusAudits List retrieve fieldValue - this is ProjectStatus
        // for each item, loop through each of the other audit result lists to find items with matching LastModifiedOn
        // construct ProjectStatusChangeViewModel object from these elements - fieldValue will be the correct element
        // add object to ProjectStatusAuditTrail

        foreach (var project in filteredProjectStatusAudits)
        {
            var changedBy = filteredChangedByAudits
                .Where(a => a.LastModifiedOn == project.LastModifiedOn)
                .FirstOrDefault()?.FieldValue;
            var changedDateOfDecision = filteredDateOfDecisionAudits
                .Where(a => a.LastModifiedOn == project.LastModifiedOn)
                .FirstOrDefault()?.FieldValue;
            var changedDetails = filteredDetailsAudits
                .Where(a => a.LastModifiedOn == project.LastModifiedOn)
                .FirstOrDefault()?.FieldValue;

            if (changedBy != null && changedDateOfDecision != null && changedDetails != null)
            {
                ProjectStatusAuditTrail.Add(new ProjectStatusChangeViewModel
                {
                    ProjectStatus = project.FieldValue,
                    ChangedBy = changedBy,
                    ChangedDateOfDecision = changedDateOfDecision,
                    ChangedDetails = changedDetails,
                    LastModifiedOn = project.LastModifiedOn
                });
            }
            
            ProjectStatusAuditTrail = ProjectStatusAuditTrail.OrderByDescending(a => a.LastModifiedOn).ToList();
        }

        return Page();
    }
}
