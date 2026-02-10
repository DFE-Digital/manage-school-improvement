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
    ErrorService errorService,
    IUserRepository userRepository,
    IMediator mediator)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty] public ProjectStatusValue SupportProjectStatus { get; set; }

    [BindProperty] public bool ChangeStatusLinkClicked { get; set; }

    private string? CurrentUserName { get; set; }

    public string? ErrorMessage { get; set; }

    public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, bool changeStatusLink, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject != null)
        {
            SupportProjectStatus = SupportProject.ProjectStatus;
        }

        ChangeStatusLinkClicked = changeStatusLink;

        RadioButtons = GetRadioButtons();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        IEnumerable<User> allUsers = await userRepository.GetAllUsers();

        var currentUser = allUsers.SingleOrDefault(u => u.EmailAddress == User.Identity?.Name);

        if (currentUser != null)
        {
            CurrentUserName = currentUser.FullName;
        }
        else
        {
            if (User.Identity?.Name != null)
            {
                CurrentUserName = User.Identity.Name.FullNameFromEmail();
            }
        }

        if (ChangeStatusLinkClicked)
        {
            var request = new SetProjectStatusCommand(new SupportProjectId(id), SupportProjectStatus,
                SupportProject?.ProjectStatusChangedDate, CurrentUserName,
                SupportProject?.ProjectStatusChangedDetails);
            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }
            
            ChangeStatusLinkClicked = false;

            return RedirectToPage(Links.ProjectStatusTab.ProjectStatusAnswers.Page, new { id });
        }

        if (SupportProjectStatus == ProjectStatusValue.Paused)
        {
            return RedirectToPage(Links.ProjectStatusTab.ProjectStatusPausedDate.Page,
                new { id, projectStatus = SupportProjectStatus, changedBy = CurrentUserName });
        }

        if (SupportProjectStatus == ProjectStatusValue.Stopped)
        {
            return RedirectToPage(Links.ProjectStatusTab.ProjectStatusStoppedDate.Page,
                new { id, projectStatus = SupportProjectStatus, changedBy = currentUser?.FullName });
        }

        return RedirectToPage(Links.ProjectStatusTab.ProjectStatusInProgressDate.Page,
            new { id, projectStatus = SupportProjectStatus, changedBy = currentUser?.FullName });
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
                Hint = "Work to support the school through targeted intervention has begun and is progressing."
            },
            new()
            {
                Id = "paused",
                Name = ProjectStatusValue.Paused.GetDisplayName(),
                Value = "Paused",
                Hint = "Work is temporarily paused while a decision is made about the school's eligibility."
            },
            new()
            {
                Id = "stopped",
                Name = ProjectStatusValue.Stopped.GetDisplayName(),
                Value = "Stopped",
                Hint =
                    "Work has ended because the school has improved sufficiently, or is undergoing structural intervention."
            }
        };
    }
}