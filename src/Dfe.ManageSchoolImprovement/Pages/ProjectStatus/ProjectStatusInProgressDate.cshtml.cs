using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class ProjectStatusInProgressDateModel(ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "project-status-in-progress-date", BinderType = typeof(DateInputModelBinder))] 
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? InProgressDate { get; set; }
    
    [BindProperty]
    public ProjectStatusValue? ProjectStatus { get; set; }
    
    [BindProperty]
    public string? ChangedBy { get; set; }
    
    [BindProperty] public bool ChangeDateLinkClicked { get; set; }

    public bool ShowError { get; set; }
    
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, ProjectStatusValue? projectStatus, string? changedBy, bool changeDateLink, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.ChangeProjectStatus.Page;

        await base.GetSupportProject(id, cancellationToken);
        
        ProjectStatus = projectStatus ?? SupportProject?.ProjectStatus;
        ChangedBy = changedBy ?? SupportProject?.ProjectStatusChangedBy;
        
        ChangeDateLinkClicked = changeDateLink;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        if (!InProgressDate.HasValue)
        {
            ModelState.AddModelError("project-status-in-progress-date", "Enter a date");
        }
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        if (ChangeDateLinkClicked)
        {
            var request = new SetProjectStatusCommand(new SupportProjectId(id), SupportProject!.ProjectStatus,
                InProgressDate, SupportProject.ProjectStatusChangedBy,
                SupportProject.ProjectStatusChangedDetails);
            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }
            
            ChangeDateLinkClicked = false;

            return RedirectToPage(Links.ProjectStatusTab.ProjectStatusAnswers.Page, new { id });
        }
        
        return RedirectToPage(@Links.ProjectStatusTab.ProjectStatusInProgressDetails.Page,
            new
            {
                id,
                projectStatus = ProjectStatus,
                changedBy = ChangedBy,
                inProgressDate = InProgressDate?.ToString("yyyy-MM-dd")
            });    
    }
    
}
