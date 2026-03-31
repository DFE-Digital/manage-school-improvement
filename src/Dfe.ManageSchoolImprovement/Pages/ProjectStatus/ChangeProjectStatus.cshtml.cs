using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class ChangeProjectStatusModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty] public ProjectStatusValue SupportProjectStatus { get; set; }

    bool ProjectStatusChanged { get; set; }

    private string? CurrentUserName { get; set; }

    public string? ErrorMessage { get; set; }

    public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject != null)
        {
            SupportProjectStatus = SupportProject.ProjectStatus;
        }

        RadioButtons = GetRadioButtons();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        ProjectStatusChanged = SupportProject?.ProjectStatus != SupportProjectStatus;
        CurrentUserName = User.Identity!.Name;

        return RedirectToPage(Links.ProjectStatusTab.IsThisSchoolEligibleForIntervention.Page,
            new { id, SupportProjectStatus, changedBy = CurrentUserName, ProjectStatusChanged });
    }

    private static IList<RadioButtonsLabelViewModel> GetRadioButtons()
    {
        return new List<RadioButtonsLabelViewModel>
        {
            new()
            {
                Id = "in-progress",
                Name = ProjectStatusValue.InProgress.GetDisplayName(),
                Value = "InProgress",
                Hint = "Work on the project has begun and is progressing."
            },
            new()
            {
                Id = "paused",
                Name = ProjectStatusValue.Paused.GetDisplayName(),
                Value = "Paused",
                Hint = "Work is temporarily paused while a decision is made about whether to continue."
            },
            new()
            {
                Id = "stopped",
                Name = ProjectStatusValue.Stopped.GetDisplayName(),
                Value = "Stopped",
                Hint =
                    "Work has ended because the school has improved sufficiently, or is now undergoing structural intervention."
            }
        };
    }
}