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
    
    public IList<ProjectStatusChangeViewModel?> ProjectStatusAuditTrail { get; set; } = [];


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SupportProjectStatus = SupportProject?.ProjectStatus;
        DateOfDecision = SupportProject?.ProjectStatusChangedDate;
        AdditionalDetails = SupportProject?.ProjectStatusChangedDetails;

        var filteredProjectStatusAudits = new List<SupportProjectFieldAuditDto<ProjectStatusValue>>();

        var projectStatusAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.ProjectStatus, cancellationToken);
        
        if (projectStatusAuditResult.IsSuccess)
        {
            filteredProjectStatusAudits = projectStatusAuditResult.Value!
                .Where(a => a.FieldValue != null)
                .ToList();
        }

        foreach (var project in filteredProjectStatusAudits.Skip(1))
        {
            var result = await supportProjectAuditQueryService.GetSupportProjectAsOfAsync(id, project.ValidFrom, cancellationToken);
            
            var supportProjectHistory = SupportProjectViewModel.Create(result.Value!);
            
            ProjectStatusAuditTrail.Add(new ProjectStatusChangeViewModel
            {
                ProjectStatus = supportProjectHistory.ProjectStatus,
                ChangedBy = supportProjectHistory.ProjectStatusChangedBy,
                ChangedDateOfDecision = supportProjectHistory.ProjectStatusChangedDate,
                ChangedDetails = supportProjectHistory.ProjectStatusChangedDetails,
                LastModifiedOn = project.LastModifiedOn
            });
        }
        
        ProjectStatusAuditTrail = ProjectStatusAuditTrail.OrderByDescending(a => a?.LastModifiedOn).ToList();

        return Page();
    }
}
