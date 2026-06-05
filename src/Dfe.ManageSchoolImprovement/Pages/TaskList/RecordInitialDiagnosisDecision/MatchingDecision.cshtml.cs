using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RecordInitialDiagnosisDecision;

public class MatchingDecisionModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "decision-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Enter date the regional director made this decision")]
    public DateTime? RegionalDirectorDecisionDate { get; set; }

    [BindProperty(Name = "HasSchoolMatchedWithSupportingOrganisation")]
    [Display(Name = "Record decision")]
    public string? HasSchoolMatchedWithSupportingOrganisation { get; set; }
    
    public required IList<RadioButtonsLabelViewModel> RadioButtonModels { get; set; }
    public bool ShowError { get; set; }
    public string? ErrorMessage { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject != null)
        {
            HasSchoolMatchedWithSupportingOrganisation = SupportProject.InitialDiagnosisMatchingDecision;
        }

        RadioButtonModels = RadioButtons;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (!RegionalDirectorDecisionDate.HasValue)
        {
            ModelState.AddModelError("decision-date", "Enter a date");
        }
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            RadioButtonModels = RadioButtons;
            return await base.GetSupportProject(id, cancellationToken);
        }

        var command = new SetRecordInitialDiagnosisDecisionCommand(
            new SupportProjectId(id),
            RegionalDirectorDecisionDate,
            HasSchoolMatchedWithSupportingOrganisation,
            null);

        var result = await mediator.Send(command, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        if (RegionalDirectorDecisionDate.HasValue && RegionalDirectorDecisionDate.Value < DateTime.UtcNow)
        {
            // if school changes from review progress to match with a supporting organisation, update the delivery milestone if already set to termly reviews
            if (HasSchoolMatchedWithSupportingOrganisation == "Match with a supporting organisation" &&
                SupportProject!.CurrentDeliveryMilestone == Milestone.TermlyReviews)
            {
                var updateMilestoneRequest = new SetCurrentDeliveryMilestoneCommand(new SupportProjectId(id), Milestone.InitialDiagnosis);
                
                var updateMilestoneResult = await mediator.Send(updateMilestoneRequest, cancellationToken);

                if (!updateMilestoneResult)
                {
                    _errorService.AddApiError();
                }
            }
            await base.UpdateCurrentDeliveryMilestone(id, SupportProject!.CurrentDeliveryMilestone, Milestone.InitialDiagnosis, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.Index.Page, new { id });
    }

    // Property with computed value using collection expressions (.NET 8)
    private IList<RadioButtonsLabelViewModel> RadioButtons =>
    [
        new() {
            Id = "match-with-organisation",
            Name = "Yes, we will find a preferred supporting organisation",
            Value = "Match with a supporting organisation"
        },
        new() {
            Id = "review-school-progress",
            Name = "No, we have decided to review progress",
            Value = "Review school's progress"
        }
    ];
}
