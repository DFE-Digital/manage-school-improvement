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
    public int Id { get; set; }
    

    [BindProperty(SupportsGet = true)]
    public ProjectStatusValue? SupportProjectStatus { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool? SchoolIsEligible { get; set; }
    
    public bool ShowError { get; set; }

    [BindProperty(SupportsGet = true)]
    
    public string? ChangedBy { get; set; }

    private string? CurrentUserName { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public DateTime? DateSupportIsDueToEnd { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public DateTime? DateEligibilityChanged { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public string? EligibilityChangedDetails { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public bool EligibilityChanged { get; set; }

    [BindProperty(SupportsGet =  true)]
    
    public string? ProjectStatusChangedDetails { get; set; }
    
    [BindProperty(SupportsGet = true)]
  
    public DateTime? DateProjectStatusChanged { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool ProjectStatusChanged { get; set; }
   
    public ProjectStatusValue? PreviousProgressStatus { get; set; }
    public SupportProjectEligibilityStatus? PreviousEligibility { get; set; }
    
    public DateTime? PreviousDateSupportIsDueToEnd { get; set; }
    
    public string DateSupportDueToEnd { get; set; }

    public string PreviousDateSupportDueToEnd { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.Index.Page;
        
        await base.GetSupportProject(id, cancellationToken);
        
        PreviousProgressStatus = SupportProject?.ProjectStatus;
        
        if (SupportProject?.SupportProjectEligibilityStatus == SupportProjectEligibilityStatus.EligibleForSupport)
        {
            PreviousEligibility = SupportProjectEligibilityStatus.EligibleForSupport;
        }
            
        if (SupportProject?.SupportProjectEligibilityStatus == SupportProjectEligibilityStatus.NotEligibleForSupport)
        {
            PreviousEligibility = SupportProjectEligibilityStatus.NotEligibleForSupport;
        }
        
        PreviousDateSupportIsDueToEnd = SupportProject?.DateSupportIsDueToEnd;

        ProjectStatusAndEligibilityUtils.MapEligibilityStatusToBool(PreviousEligibility);

        if (DateSupportIsDueToEnd.HasValue)
        {
            DateSupportDueToEnd = DateSupportIsDueToEnd.Value
                .ToString("d MMMM yyyy");
        }
        
        if (PreviousDateSupportIsDueToEnd.HasValue)
        {
            PreviousDateSupportDueToEnd  = PreviousDateSupportIsDueToEnd.Value
                .ToString("d MMMM yyyy");
        }


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
        
        await base.GetSupportProject(id, cancellationToken);
        
        var eligibilityStatus = SchoolIsEligible == true
            ? SupportProjectEligibilityStatus.EligibleForSupport
            : SupportProjectEligibilityStatus.NotEligibleForSupport;

        if (ProjectStatusChanged)
        {
            var request = new SetProjectStatusCommand(new SupportProjectId(id), SupportProjectStatus!.Value, DateProjectStatusChanged,
                CurrentUserName, ProjectStatusChangedDetails);
            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            TempData["ProjectStatusUpdated"] = true;
        }

        if (EligibilityChanged)
        {


            var request = new UpdateEligibilityCommand(new SupportProjectId(id), eligibilityStatus, DateEligibilityChanged, CurrentUserName, 
                DateSupportIsDueToEnd, EligibilityChangedDetails);
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
    
    //  private bool? MapEligibilityStatusToBool(SupportProjectEligibilityStatus? status)
    // {
    //     if (status == SupportProjectEligibilityStatus.EligibleForSupport)
    //         return true;
    //
    //     if (status == SupportProjectEligibilityStatus.NotEligibleForSupport)
    //         return false;
    //
    //     return null; 
    // }
}
