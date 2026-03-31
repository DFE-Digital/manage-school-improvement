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

public class ConfirmChangeModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService,
    IUserRepository userRepository)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService),
        IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(SupportsGet = true)] public ProjectStatusValue SupportProjectStatus { get; set; }
    
    [BindProperty(SupportsGet = true)] public bool ProjectStatusChanged { get; set; }

    [BindProperty]
    [Display(Name = "Is this school still eligible for targeted intervention?")]
    public bool? SchoolIsEligible { get; set; } = false;
    
    [BindProperty(Name = "support-is-due-to-end-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportIsDueToEnd { get; set; }
    
    private string? CurrentUserName { get; set; }
    
    public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }
    
    public string? ErrorMessage { get; set; }

    public bool ShowError { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.ProjectStatusTab.ChangeProjectStatus.Page;

        await base.GetSupportProject(id, cancellationToken);

        DateSupportIsDueToEnd = SupportProject?.DateSupportIsDueToEnd;

        SchoolIsEligible = SupportProject?.SupportProjectEligibilityStatus == SupportProjectEligibilityStatus.EligibleForSupport;

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

        var previousSupportEndDate = SupportProject?.DateSupportIsDueToEnd;
        
        var eligiblityStatus = SchoolIsEligible == true ? SupportProjectEligibilityStatus.EligibleForSupport : SupportProjectEligibilityStatus.NotEligibleForSupport;

        var previousEligibilityStatus = SupportProject?.SupportProjectEligibilityStatus;
        
        // unsure whether to include date - sort out later
        var eligibilityChanged = eligiblityStatus != previousEligibilityStatus || DateSupportIsDueToEnd != previousSupportEndDate;
        
        return RedirectToPage(@Links.ProjectStatusTab.EnterEligibilityChangeDetails.Page,
            new
            {
                id,
                SupportProjectStatus,
                ProjectStatusChanged,
                eligiblityStatus,
                DateSupportIsDueToEnd,
                eligibilityChanged
            });
    }

    private IList<RadioButtonsLabelViewModel> RadioButtonModel
    {
        get
        {
            var list = new List<RadioButtonsLabelViewModel>
            {
                new()
                {
                    Id = "yes",
                    Name = "Yes",
                    Value = "True"
                },
                new()
                {
                    Id = "no",
                    Name = "No",
                    Value = "False",
                }
            };

            return list;
        }
    }
}

