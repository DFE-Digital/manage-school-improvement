using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class IsThisSchoolEligibleForInterventionModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
    IUserRepository userRepository,
    IMediator mediator)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService),IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(SupportsGet = true)] 
    public ProjectStatusValue SupportProjectStatus { get; set; }
    
    [BindProperty]
    [Display(Name = "Is this school still eligible for targeted intervention?")]
   
    public bool? SchoolIsEligible { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public bool ShowError { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public bool ProjectStatusChanged { get; set; }
    
    public bool EligibilityChanged { get; set; }
    private string? CurrentUserName { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public DateTime? DateProjectStatusChanged { get; set; }
    
    [BindProperty(SupportsGet = true)] 
    public string? ProjectStatusChangedDetails { get; set; }
    
    
    [BindProperty(Name = "support-is-due-to-end-date",BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportIsDueToEnd { get; set; }

    public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }
    
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    
    
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.Index.Page;
        

        //SupportProjectStatus = projectStatus;
        
        await base.GetSupportProject(id, cancellationToken);
        
        DateSupportIsDueToEnd = SupportProject.DateSupportIsDueToEnd;

        SchoolIsEligible = MapEligibilityStatusToBool(SupportProject.SupportProjectEligibilityStatus);

        RadioButtons = RadioButtonModel;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            RadioButtons = RadioButtonModel;
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
        
        var previousSupportEndDate = SupportProject.DateSupportIsDueToEnd;

        var previousEligibilityStatus = MapEligibilityStatusToBool(SupportProject.SupportProjectEligibilityStatus);

        if (SchoolIsEligible == previousEligibilityStatus && DateSupportIsDueToEnd == previousSupportEndDate)
        {
            if (ProjectStatusChanged == false)
            {
                return RedirectToPage(@Links.ProjectStatusTab.Index.Page, new { id });
            }

            return RedirectToPage(@Links.ProjectStatusTab.CheckYourAnswers.Page,
                new
                {
                    id,
                    SupportProjectStatus,
                    ProjectStatusChanged,
                    DateProjectStatusChanged,
                    ProjectStatusChangedDetails,
                    EligibilityChanged = false
                });  
        }


        return RedirectToPage(@Links.ProjectStatusTab.EnterEligibilityChangeDetails.Page,
            new
            {
                id,
                SupportProjectStatus,
                ProjectStatusChanged,
                DateProjectStatusChanged,
                ProjectStatusChangedDetails,
                SchoolIsEligible,
                DateSupportIsDueToEnd,
            });  
    }

    private IList<RadioButtonsLabelViewModel> RadioButtonModel
    {
        get
        {
            var list = new List<RadioButtonsLabelViewModel>
            {
                new() {
                    Id = "yes",
                    Name = "Yes",
                    Value = "True"
                },
                new() {
                    Id = "no",
                    Name = "No",
                    Value = "False",
                }
            };

            return list;
        }
    }
    
    private bool? MapEligibilityStatusToBool(SupportProjectEligibilityStatus? status)
    {
        if (status == SupportProjectEligibilityStatus.EligibleForSupport)
            return true;
    
        if (status == SupportProjectEligibilityStatus.NotEligibleForSupport)
            return false;

        return null; // if unknown or null
    }
}

