using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class DateOfDecisionModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }
    
    [BindProperty(Name = "escalate-decision-date")]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [ModelBinder(BinderType = typeof(DateInputModelBinder))]
    public DateTime? DateOfDecision { get; set; }
    
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.EngagementConcern.ReasonForEscalation.Page;
        
        await base.GetSupportProject(id, cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id,
        bool confirmStepsTaken,
        string primaryReason,
        string escalationDetails,
        CancellationToken cancellationToken)
    {
        if (!DateOfDecision.HasValue)
        {
            ModelState.AddModelError("escalate-decision-date", "You must enter a date");
        }
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            // ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        // request create EngagementConcernEscalationModel passing in all prev values plus date
        
        Console.WriteLine($"Confirm steps taken: {confirmStepsTaken}");
        Console.WriteLine($"Primary reason: {primaryReason}");
        Console.WriteLine($"Escalation details: {escalationDetails}");
        Console.WriteLine($"Date of decision: {DateOfDecision}");
        
        return RedirectToPage(@Links.EngagementConcern.EscalationConfirmation.Page, new { id });
    }
}