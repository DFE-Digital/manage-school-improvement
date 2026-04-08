using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Eligibility;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class CheckYourAnswersModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
    IUserRepository userRepository,
    IMediator mediator)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService),
        IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(SupportsGet = true)]
    public ProjectStatusValue? SupportProjectStatus { get; set; }

    [BindProperty(SupportsGet = true)]
    public SupportProjectEligibilityStatus? EligibilityStatus { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public DateTime? DateSupportIsDueToEnd { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public DateTime? StatusOrEligiblityChangeDate { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public string? ChangeDetails { get; set; }

    private string? CurrentUserName { get; set; }
    
    public bool ShowError { get; set; }


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.EnterDetailsAboutTheChange.Page;
        
        await base.GetSupportProject(id, cancellationToken);
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        if (SupportProjectStatus == null && EligibilityStatus == null)
        {
            ModelState.AddModelError("change-status-eligibility", "Enter the missing details using the change link");
            _errorService.AddError("change-status-eligibility", "Enter the missing details using the change link");
        }
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }
        
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

        var projectStatusChanged = SupportProject?.ProjectStatus != SupportProjectStatus;
        var eligibilityChanged = SupportProject?.SupportProjectEligibilityStatus != EligibilityStatus;
        
        if (projectStatusChanged)
        {
            var request = new SetProjectStatusCommand(new SupportProjectId(id), SupportProjectStatus!.Value, StatusOrEligiblityChangeDate,
                CurrentUserName, ChangeDetails);
            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            TempData["ProjectStatusUpdated"] = true;
        }
        

        if (eligibilityChanged)
        {
            var request = new UpdateEligibilityCommand(new SupportProjectId(id), EligibilityStatus, StatusOrEligiblityChangeDate, CurrentUserName, 
                DateSupportIsDueToEnd, ChangeDetails);
            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            TempData["EligibilityUpdated"] = true;
        }

        return RedirectToPage(Links.ProjectStatusTab.Index.Page, new
        {
            id
        });
    }
}
