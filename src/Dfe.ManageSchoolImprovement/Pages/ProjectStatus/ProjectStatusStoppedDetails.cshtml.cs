using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class ProjectStatusStoppedDetailsModel(ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "project-status-stopped-details")]
    public string? StoppedDetails { get; set; }
    
    [BindProperty(Name = "projectStatus")] public ProjectStatusValue ProjectStatus { get; set; }

    [BindProperty(Name = "stoppedDate")] public DateTime? StoppedDate { get; set; }

    [BindProperty(Name = "changedBy")] public string? ChangedBy { get; set; }

    public bool ShowError { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, ProjectStatusValue projectStatus, string changedBy,
        DateTime stoppedDate, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.ProjectStatusTab.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        
        ProjectStatus = projectStatus;
        StoppedDate = stoppedDate;
        ChangedBy = changedBy;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        if (string.IsNullOrEmpty(StoppedDetails))
        {
            ModelState.AddModelError("project-status-stopped-details", "Enter details");
        }
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }

        if (SupportProject?.ProjectStatus != null)
        {
            var request = new SetProjectStatusCommand(new SupportProjectId(id), ProjectStatus, StoppedDate, ChangedBy, StoppedDetails);
            var result = await mediator.Send(request, cancellationToken);
        
            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }
        }
        
        TempData["projectStatusUpdated"] = true;
        
        return RedirectToPage(@Links.ProjectStatusTab.ProjectStatusAnswers.Page, new { id });
    }
    
}