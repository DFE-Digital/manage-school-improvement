using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class ReasonForEscalationModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseEngagementConcernPageModel(supportProjectQueryService, errorService, mediator)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "PrimaryReason")]
    public string? PrimaryReason { get; set; }

    [BindProperty(Name = "escalation-details")]
    public string? EscalationDetails { get; set; } = null!;
    
    [BindProperty]
    public Guid EngagementConcernId { get; set; }

    public required IList<RadioButtonsLabelViewModel> PrimaryReasonRadioButtons { get; set; }

    public string PrimaryReasonErrorMessage { get; set; }

    public string DetailsErrorMessage { get; set; }
    
    private const string DetailsErrorKey = "escalation-details";
    
    public bool ShowDetailsError => ModelState.ContainsKey(DetailsErrorKey) && ModelState[DetailsErrorKey]?.Errors.Count > 0;

    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, Guid engagementConcernId, string returnPage, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.EngagementConcern.EscalateEngagementConcern.Page;

        await base.GetSupportProject(id, cancellationToken);
        EngagementConcernId = engagementConcernId;
        
        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(a => a.Id.Value == EngagementConcernId);

        if (engagementConcern != null)
        {
            PrimaryReason = engagementConcern.EngagementConcernEscalationPrimaryReason;
            EscalationDetails = engagementConcern.EngagementConcernEscalationDetails;
        }
        
        PrimaryReasonRadioButtons = GetRadioButtons();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id,
        bool? confirmStepsTaken,
        CancellationToken cancellationToken)
    {
        if (EscalationDetails == null || PrimaryReason == null)
        {
            if (PrimaryReason == null)
            {
                PrimaryReasonErrorMessage = "You must select a primary reason";
                _errorService.AddError("primary-reason", PrimaryReasonErrorMessage);
            }

            if (EscalationDetails == null)
            {
                DetailsErrorMessage = "You must enter details";
                _errorService.AddError(DetailsErrorKey, DetailsErrorMessage);
                ModelState.AddModelError(DetailsErrorKey, DetailsErrorMessage);
            }

            PrimaryReasonRadioButtons = GetRadioButtons();
            return await base.GetSupportProject(id, cancellationToken);
        }

        return await HandleEscalationPost(
            EngagementConcernId,
            id,
            new EngagementConcernEscalationDetails
            {
                ConfirmStepsTaken = confirmStepsTaken,
                PrimaryReason = PrimaryReason,
                Details = EscalationDetails,
                DateOfDecision = null // Date of decision will be handled in the next step
            },
            cancellationToken);
    }

    protected internal override IActionResult GetDefaultRedirect(int id, object? routeValues = default)
    {
        var values = new
        {
            id,
            engagementConcernId = EngagementConcernId
        };
        return RedirectToPage(@Links.EngagementConcern.DateOfDecision.Page, values);
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