using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernEscalation;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class DateOfDecisionModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
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
        await base.GetSupportProject(id, cancellationToken);
        // if confirmStepsTaken is null, use the default value from the SupportProject, as we have entered the flow from the change link
        confirmStepsTaken = confirmStepsTaken ?? SupportProject.EngagementConcernEscalationConfirmStepsTaken;
        primaryReason = primaryReason ?? SupportProject.EngagementConcernEscalationPrimaryReason;
        escalationDetails = escalationDetails ?? SupportProject.EngagementConcernEscalationDetails;
        
        var request = new SetSupportProjectEngagementConcernEscalationCommand(
            new SupportProjectId(id),
            confirmStepsTaken,
            primaryReason,
            escalationDetails,
            DateOfDecision);

        var result = await mediator.Send(request, cancellationToken);

        if (result == null)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }
        
        if (TempData["ChangeLinkClicked"] is true)
        {
            TempData.Remove("ChangeLinkClicked");
            return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
        }

        return RedirectToPage(@Links.EngagementConcern.EscalationConfirmation.Page, new { id });
    }
}