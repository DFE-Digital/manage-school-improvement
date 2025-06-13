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
    public string EscalationDetails { get; set; }
    
    public required IList<RadioButtonsLabelViewModel> PrimaryReasonRadioButtons { get; set; }
    
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.EngagementConcern.EscalateEngagementConcern.Page;
        
        await base.GetSupportProject(id, cancellationToken);

        PrimaryReasonRadioButtons = GetRadioButtons();
        return Page();
    }
    
    private IList<RadioButtonsLabelViewModel> GetRadioButtons()
    {
            var list = new List<RadioButtonsLabelViewModel>
            {
                new() {
                    Id = "communication",
                    Name = "Lack of communication or cooperation",
                    Value = "False"
                },
                new() {
                    Id = "information",
                    Name = "Witheld information from adviser",
                    Value = "False"
                },
                new()
                {
                    Id = "rejected",
                    Name = "Rejected DfE or supporting organisation decisions",
                    Value = "False"
                }
            };

            return list;
    }
}