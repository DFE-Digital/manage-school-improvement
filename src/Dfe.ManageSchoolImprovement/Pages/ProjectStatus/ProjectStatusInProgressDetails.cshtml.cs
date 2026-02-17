using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class ProjectStatusInProgressDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService),
        IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "project-status-in-progress-details")]
    public string? InProgressDetails { get; set; }

    [BindProperty(Name = "projectStatus")] public ProjectStatusValue? ProjectStatus { get; set; }

    [BindProperty(Name = "inProgressDate")]
    public DateTime? InProgressDate { get; set; }

    [BindProperty(Name = "changedBy")] public string? ChangedBy { get; set; }

    public bool ShowError { get; set; }


    public async Task<IActionResult> OnGetAsync(int id, ProjectStatusValue? projectStatus, string? changedBy,
        DateTime? inProgressDate, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.ProjectStatusInProgressDate.Page;

        await base.GetSupportProject(id, cancellationToken);

        ProjectStatus = projectStatus ?? SupportProject?.ProjectStatus;
        InProgressDate = inProgressDate ?? SupportProject?.ProjectStatusChangedDate;
        ChangedBy = changedBy ?? SupportProject?.ProjectStatusChangedBy;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (string.IsNullOrEmpty(InProgressDetails))
        {
            ModelState.AddModelError("project-status-in-progress-details", "Enter details");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }

        return RedirectToPage(@Links.ProjectStatusTab.ProjectStatusAnswers.Page,
            new
            {
                id, projectStatus = ProjectStatus, changedBy = ChangedBy, changedDate = InProgressDate,
                changedDetails = InProgressDetails
            });
    }
}