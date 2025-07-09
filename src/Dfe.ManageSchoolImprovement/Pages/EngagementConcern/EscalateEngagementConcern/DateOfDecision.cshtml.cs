using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class DateOfDecisionModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseEngagementConcernPageModel(supportProjectQueryService, errorService, mediator), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "escalate-decision-date")]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [ModelBinder(BinderType = typeof(DateInputModelBinder))]
    public DateTime? DateOfDecision { get; set; }

    public bool ShowError => _errorService.HasErrors();

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return "You must enter a date";
    }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    public async Task<IActionResult> OnGetAsync(int id, string returnPage, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.EngagementConcern.ReasonForEscalation.Page;

        await base.GetSupportProject(id, cancellationToken);

        DateOfDecision = SupportProject.EngagementConcernEscalationDateOfDecision;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id,
        bool? confirmStepsTaken,
        string? primaryReason,
        string? escalationDetails,
        bool? changeLinkClicked,
        CancellationToken cancellationToken)
    {
        if (!DateOfDecision.HasValue)
        {
            ModelState.AddModelError("escalate-decision-date", "You must enter a date");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            return await base.GetSupportProject(id, cancellationToken);
        }

        return await HandleEscalationPost(
            id,
            new EngagementConcernEscalationDetails
            {
                ConfirmStepsTaken = confirmStepsTaken,
                PrimaryReason = primaryReason,
                Details = escalationDetails,
                DateOfDecision = DateOfDecision
            },
            changeLinkClicked,
            cancellationToken);
    }

    protected internal override IActionResult GetDefaultRedirect(int id, object? routeValues = default)
    {
        return RedirectToPage(@Links.EngagementConcern.EscalationConfirmation.Page, new { id });
    }
}