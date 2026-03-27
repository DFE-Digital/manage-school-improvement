using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class EnterEligibilityChangeDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
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
    public bool ProjectStatusChanged { get; set; }

    [BindProperty(SupportsGet = true)]
    
    public string? ChangedBy { get; set; }

    private string? CurrentUserName { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public DateTime? DateSupportIsDueToEnd { get; set; }
    
    [BindProperty(Name = "eligibility-changed-details")]
    public string? EligibilityChangedDetails { get; set; }
    
    
    [BindProperty(SupportsGet = true)]
  
    public DateTime? DateProjectStatusChanged { get; set; }
    
    [BindProperty(Name = "eligibility-changed-date",BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateEligibilityChanged { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? ProjectStatusChangedDetails { get; set; }
    
    public bool EligibilityChanged { get; set; }
    
    

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.ProjectStatusPausedDate.Page;

        await base.GetSupportProject(id, cancellationToken);
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id ,CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        if (string.IsNullOrEmpty(EligibilityChangedDetails))
        {
            ModelState.AddModelError("eligibility-changed-details", "Enter details");
        }
        
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        

        return RedirectToPage(Links.ProjectStatusTab.CheckYourAnswers.Page, new
        {
            id,
            SupportProjectStatus, 
            ProjectStatusChangedDetails,
            DateProjectStatusChanged,
            ProjectStatusChanged,
            SchoolIsEligible,
            EligibilityChangedDetails,
            DateSupportIsDueToEnd,
            DateEligibilityChanged,
            EligibilityChanged = true
            
        });
    }
}
