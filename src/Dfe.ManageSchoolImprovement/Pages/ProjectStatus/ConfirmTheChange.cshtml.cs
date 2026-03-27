using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class ConfirmTheChangeModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
    IUserRepository userRepository,
    IMediator mediator)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService),IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public ProjectStatusValue PreviousProgressStatus { get; set; }
    
  

    [BindProperty(SupportsGet = true)]
    public SupportProjectEligibilityStatus PreviousEligibility { get; set; }

    [BindProperty(SupportsGet = true)]
    public ProjectStatusValue? SupportProjectStatus { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool? SchoolIsEligible { get; set; }
    
    public bool ShowError { get; set; }

    [BindProperty(SupportsGet = true)]

    public bool ProjectStatusChanged { get; set; }
   
    [BindProperty(SupportsGet = true)] 
        
    public bool EligibiltyChanged { get; set; }
        
        
    [BindProperty(SupportsGet = true)] 
    public DateTime? DateProjectStatusChanged { get; set; }
        
    [BindProperty(SupportsGet = true)] 
    public string? ProjectStatusChangedDetails { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public DateTime? DateSupportIsDueToEnd { get; set; }
    
    [BindProperty(Name = "eligibility-changed-date",BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateEligibilityChanged { get; set; }
    
  

    public async Task<IActionResult> OnGetAsync(int id,ProjectStatusValue supportProjectStatus,bool schoolIsEligible,  CancellationToken cancellationToken)
    {
        ReturnPage = Links.ProjectStatusTab.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        PreviousProgressStatus = SupportProject.ProjectStatus;

        DateEligibilityChanged = SupportProject.DateEligibilityChanged;
        
        if (SupportProject?.SupportProjectEligibilityStatus == SupportProjectEligibilityStatus.EligibleForSupport)
        {
            PreviousEligibility = SupportProjectEligibilityStatus.EligibleForSupport;
        }
            
        if (SupportProject?.SupportProjectEligibilityStatus == SupportProjectEligibilityStatus.NotEligibleForSupport)
        {
            PreviousEligibility = SupportProjectEligibilityStatus.NotEligibleForSupport;
        }
        
        SchoolIsEligible = schoolIsEligible;
        SupportProjectStatus = supportProjectStatus;

        
return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        
        if (!DateSupportIsDueToEnd.HasValue)
        {
            ModelState.AddModelError("eligibility-changed-date", "Enter a date");
        }
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        return RedirectToPage(Links.ProjectStatusTab.EnterDetailsAboutTheChange.Page, new
        {
            id,
            SupportProjectStatus,
            SchoolIsEligible,
            DateEligibilityChanged,
            DateSupportIsDueToEnd,
        });
    }
}