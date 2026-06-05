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

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    IApplicationSettingsResourceService applicationSettingsResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "decision-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Enter date the regional director made this decision")]
    public DateTime? RegionalDirectorDecisionDate { get; set; }

    [BindProperty(Name = "HasSchoolMatchedWithSupportingOrganisation")]
    [Display(Name = "Record decision")]
    public string? HasSchoolMatchedWithSupportingOrganisation { get; set; }

    [BindProperty(Name = "NotMatchingNotes")]
    [Display(Name = "Review school's progress notes")]
    public string? NotMatchingNotes { get; set; }

    [BindProperty(Name = "UnableToAssessNotes")]
    [Display(Name = "Unable to assess notes")]
    public string? UnableToAssessNotes { get; set; }

    public TaskListStatus? TaskListStatus { get; set; }
    public ProjectStatusValue? ProjectStatus { get; set; }

    public bool TaskIsComplete { get; set; }
    
    public required IList<RadioButtonsLabelViewModel> RadioButtonModels { get; set; }
    public bool ShowError { get; set; }
    public string? ErrorMessage { get; set; }
    public string AssessmentToolOneLink { get; set; } = string.Empty;

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject != null)
        {
            (HasSchoolMatchedWithSupportingOrganisation, RegionalDirectorDecisionDate, AssessmentToolOneLink) = (
                SupportProject.InitialDiagnosisMatchingDecision,
                SupportProject.RegionalDirectorDecisionDate,
                await applicationSettingsResourceService.GetAssessmentToolOneLinkAsync(cancellationToken) ?? string.Empty
            );
            
            // Conditional assignment using switch expression
            // Fix for CS8131 and CS8506: Ensure the switch expression has a clear type and the deconstruction is valid.
            var decisionNotes = HasSchoolMatchedWithSupportingOrganisation switch
            {
                "Review school's progress" => (SupportProject.InitialDiagnosisMatchingDecisionNotes, null),
                "Unable to assess" => ((string?)null, SupportProject.InitialDiagnosisMatchingDecisionNotes),
                _ => (null, null)
            };
            
            (NotMatchingNotes, UnableToAssessNotes) = decisionNotes;
            
            TaskListStatus = TaskStatusViewModel.RecordInitialDiagnosisDecisionTaskListStatus(SupportProject);
            ProjectStatus = SupportProject.ProjectStatus;
            
            TaskIsComplete = SupportProject.InitialDiagnosisMatchingDecision is "Match with a supporting organisation" or "Review school's progress";
        }

        RadioButtonModels = RadioButtons;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (!IsNotMatchingNotesValid())
        {
            ModelState.AddModelError(nameof(NotMatchingNotes), "Enter notes");
        }

        if (!IsUnableToAssessNotesValid())
        {
            ModelState.AddModelError(nameof(UnableToAssessNotes), "Enter notes");
        }

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

        // Switch expression for notes selection
        var notesToPass = HasSchoolMatchedWithSupportingOrganisation switch
        {
            "Review school's progress" => NotMatchingNotes,
            "Unable to assess" => UnableToAssessNotes,
            _ => null
        };

        var command = new SetRecordInitialDiagnosisDecisionCommand(
            new SupportProjectId(id),
            RegionalDirectorDecisionDate,
            HasSchoolMatchedWithSupportingOrganisation,
            notesToPass);

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
            Name = "Match with a supporting organisation",
            Value = "Match with a supporting organisation"
        },
        new() {
            Id = "review-school-progress",
            Name = "Review school's progress",
            Value = "Review school's progress",
            Input = new TextFieldInputViewModel
            {
                Id = nameof(NotMatchingNotes),
                ValidationMessage = "Enter notes",
                Paragraph = "Copy and paste your notes from the overall recommendation within the summary tab in Assessment Tool 1.",
                Value = NotMatchingNotes,
                IsValid = IsNotMatchingNotesValid(),
                IsTextArea = true
            }
        },
        new() {
            Id = "unable-to-assess",
            Name = "Unable to assess",
            Value = "Unable to assess",
            Input = new TextFieldInputViewModel
            {
                Id = nameof(UnableToAssessNotes),
                ValidationMessage = "Enter notes",
                Paragraph = "Copy and paste your notes from the overall recommendation within the summary tab in Assessment Tool 1.",
                Value = UnableToAssessNotes,
                IsValid = IsUnableToAssessNotesValid(),
                IsTextArea = true
            }
        }
    ];

    // Expression-bodied validation methods
    private bool IsNotMatchingNotesValid() =>
        HasSchoolMatchedWithSupportingOrganisation != "Review school's progress" ||
        !string.IsNullOrWhiteSpace(NotMatchingNotes);

    private bool IsUnableToAssessNotesValid() =>
        HasSchoolMatchedWithSupportingOrganisation != "Unable to assess" ||
        !string.IsNullOrWhiteSpace(UnableToAssessNotes);
}
