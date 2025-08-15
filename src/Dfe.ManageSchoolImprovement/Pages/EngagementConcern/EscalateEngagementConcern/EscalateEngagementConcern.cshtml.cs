using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class EscalateEngagementConcernModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }
    
    [BindProperty(Name = "confirm-steps-taken")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? ConfirmStepsTaken { get; set; }
    public bool ShowError => _errorService.HasErrors();
    
    public static string ConfirmStepsTakenError => "confirm-steps-taken";
    
    public bool ShowCheckboxError => ModelState.ContainsKey(ConfirmStepsTakenError) && ModelState[ConfirmStepsTakenError]?.Errors.Count > 0;


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.EngagementConcern.Index.Page;
        
        await base.GetSupportProject(id, cancellationToken);

        ConfirmStepsTaken = SupportProject.EngagementConcernEscalationConfirmStepsTaken;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        if (ConfirmStepsTaken is false)
        {
            _errorService.AddError(ConfirmStepsTakenError, "You must confirm you have taken the required steps");
            ModelState.AddModelError(ConfirmStepsTakenError, "You must confirm you have taken the required steps");
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }
        
        return RedirectToPage(@Links.EngagementConcern.ReasonForEscalation.Page, new { id, ConfirmStepsTaken });
    }
}