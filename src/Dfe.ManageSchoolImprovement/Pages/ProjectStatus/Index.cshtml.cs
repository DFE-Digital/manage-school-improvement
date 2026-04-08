using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
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
    public DateTime? DateSupportIsDueToEnd { get; set; }
    public SupportProjectEligibilityStatus? EligibilityStatus { get; set; }
    
    public IList<ProjectStatusChangeViewModel?> ProjectStatusAuditTrail { get; set; } = [];


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SupportProjectStatus = SupportProject?.ProjectStatus;
        DateOfDecision = SupportProject?.ProjectStatusChangedDate;
        AdditionalDetails = SupportProject?.ProjectStatusChangedDetails;
        DateSupportIsDueToEnd = SupportProject?.DateSupportIsDueToEnd;
        EligibilityStatus = SupportProject?.SupportProjectEligibilityStatus;

        var filteredProjectStatusAudits = new List<SupportProjectFieldAuditDto<ProjectStatusValue>>();
        var filteredEligiblityAudits = new List<SupportProjectFieldAuditDto<SupportProjectEligibilityStatus?>>();

        var projectStatusAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatus, cancellationToken);
        
        if (projectStatusAuditResult.IsSuccess)
        {
            filteredProjectStatusAudits = projectStatusAuditResult.Value!
                .Where(a => a.FieldValue != null)
                .ToList();
        }

        var eligibilityAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.SupportProjectEligibilityStatus, cancellationToken);

        if (eligibilityAuditResult.IsSuccess)
        {
            filteredEligiblityAudits = eligibilityAuditResult.Value!
                .Where(a => a.FieldValue != null)
                .ToList();
        }

        var statusValidFroms = filteredProjectStatusAudits.Skip(1).ToDictionary(a => a.ValidFrom);
        var eligibilityValidFroms = filteredEligiblityAudits.Skip(1).ToDictionary(a => a.ValidFrom);

        var allValidFroms = statusValidFroms.Keys.Union(eligibilityValidFroms.Keys).ToList();

        foreach (var validFrom in allValidFroms)
        {
            var result = await supportProjectAuditQueryService.GetSupportProjectAsOfAsync(id, validFrom, cancellationToken);
            var supportProjectHistory = SupportProjectViewModel.Create(result.Value!);

            var hasStatusChange = statusValidFroms.TryGetValue(validFrom, out var statusAudit);
            eligibilityValidFroms.TryGetValue(validFrom, out var eligibilityAudit);
            var auditEntryLastModified = hasStatusChange ? statusAudit!.LastModifiedOn : eligibilityAudit!.LastModifiedOn;
            var auditEntryValidFrom = hasStatusChange ? statusAudit!.ValidFrom : eligibilityAudit!.ValidFrom;

            ProjectStatusAuditTrail.Add(new ProjectStatusChangeViewModel
            {
                ProjectStatus = supportProjectHistory.ProjectStatus,
                ChangedBy = hasStatusChange ? supportProjectHistory.ProjectStatusChangedBy : supportProjectHistory.EligibilityChangedBy,
                ChangedDateOfDecision = hasStatusChange ? supportProjectHistory.ProjectStatusChangedDate : supportProjectHistory.DateEligibilityChanged,
                ChangedDetails = hasStatusChange ? supportProjectHistory.ProjectStatusChangedDetails : supportProjectHistory.EligibilityChangedDetails,
                LastModifiedOn = auditEntryLastModified,
                ValidFrom = auditEntryValidFrom,
                Eligibility = supportProjectHistory.SupportProjectEligibilityStatus,
                DateSupportIsDueToEnd = supportProjectHistory.DateSupportIsDueToEnd,
            });
        }
        
        ProjectStatusAuditTrail = ProjectStatusAuditTrail.OrderByDescending(a => a?.LastModifiedOn).ToList();

        return Page();
    }
}
