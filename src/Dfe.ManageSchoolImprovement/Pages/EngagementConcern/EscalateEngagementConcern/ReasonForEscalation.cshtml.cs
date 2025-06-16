using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class ReasonForEscalationModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }
    
    [BindProperty(Name = "PrimaryReason")]
    public string? PrimaryReason { get; set; }
    
    [BindProperty(Name = "escalation-details")]
    public string EscalationDetails { get; set; } = null!;

    public required IList<RadioButtonsLabelViewModel> PrimaryReasonRadioButtons { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.EngagementConcern.EscalateEngagementConcern.Page;
        
        await base.GetSupportProject(id, cancellationToken);

        PrimaryReasonRadioButtons = GetRadioButtons();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, bool confirmStepsTaken, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || EscalationDetails == null || PrimaryReason == null)
        {
            if (PrimaryReason == null)
            {
                ErrorMessage = "You must select a primary reason"; 
                _errorService.AddError("PrimaryReason", ErrorMessage);
            }
                
            if (EscalationDetails == null)
            {
                ErrorMessage = "You must enter details";
                _errorService.AddError("escalation-details",ErrorMessage);
            }
                
            PrimaryReasonRadioButtons = GetRadioButtons();
            
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        return RedirectToPage(@Links.EngagementConcern.DateOfDecision.Page, new { id, confirmStepsTaken, PrimaryReason, EscalationDetails });
    }
    
    private IList<RadioButtonsLabelViewModel> GetRadioButtons()
    {
            var list = new List<RadioButtonsLabelViewModel>
            {
                new() {
                    Id = "communication",
                    Name = "Lack of communication or cooperation",
                    Value = "Lack of communication or cooperation"
                },
                new() {
                    Id = "information",
                    Name = "Withheld information from adviser",
                    Value = "Withheld information from adviser"
                },
                new()
                {
                    Id = "rejected",
                    Name = "Rejected DfE or supporting organisation decisions",
                    Value = "Rejected DfE or supporting organisation decisions"
                }
            };

            return list;
    }
}