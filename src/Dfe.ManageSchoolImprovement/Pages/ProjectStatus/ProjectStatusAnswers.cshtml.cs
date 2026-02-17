using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class ProjectStatusAnswersModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService),
        IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }
    
    [BindProperty(Name = "projectStatus")] public ProjectStatusValue? ProjectStatus { get; set; }

    [BindProperty(Name = "changedDate")] public DateTime? ChangedDate { get; set; }
    
    [BindProperty(Name = "changedDetails")] public string? ChangedDetails { get; set; }

    [BindProperty(Name = "changedBy")] public string? ChangedBy { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, ProjectStatusValue? projectStatus, string? changedBy,
        DateTime? changedDate, string? changedDetails,  CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);
        
        await base.GetSupportProject(id, cancellationToken);
        
        ProjectStatus = projectStatus ?? SupportProject?.ProjectStatus;
        ChangedDate = changedDate ?? SupportProject?.ProjectStatusChangedDate;
        ChangedBy = changedBy ?? SupportProject?.ProjectStatusChangedBy;
        ChangedDetails = changedDetails ?? SupportProject?.ProjectStatusChangedDetails;
        
        ReturnPage = SupportProject?.ProjectStatus switch
        {
            ProjectStatusValue.InProgress => @Links.ProjectStatusTab.ProjectStatusInProgressDetails.Page,
            ProjectStatusValue.Paused => @Links.ProjectStatusTab.ProjectStatusPausedDetails.Page,
            ProjectStatusValue.Stopped => @Links.ProjectStatusTab.ProjectStatusStoppedDetails.Page,
            _ => string.Empty
        };

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        var request = new SetProjectStatusCommand(new SupportProjectId(id), (ProjectStatusValue)ProjectStatus, ChangedDate, ChangedBy,
           ChangedDetails);
        var result = await mediator.Send(request, cancellationToken);

        if (result == null)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        TempData["projectStatusUpdated"] = true;
        
        return RedirectToPage(@Links.ProjectStatusTab.Index.Page, new { id });
    }
}
