using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RecordMatchingDecision;

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "decision-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "record matching decision")]
    public DateTime? RegionalDirectorDecisionDate { get; set; }

    [BindProperty(Name = "HasSchoolMatchedWithSupportingOrganisation")]
    public string? HasSchoolMatchedWithSupportingOrganisation { get; set; }

    [BindProperty(Name = "NotMatchingNotes")]
    public string? NotMatchingNotes { get; set; }

    [BindProperty(Name = "UnableToAssessNotes")]
    public string? UnableToAssessNotes { get; set; }

    public required IList<RadioButtonsLabelViewModel> RadioButtonModels { get; set; }
    public bool ShowError { get; set; }
    public string? ErrorMessage { get; set; }
    public string AssessmenToolOneLink { get; set; } = string.Empty;

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing(string displayName) =>
        "Enter the record matching decision date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Tuple deconstruction for multiple assignments
        (HasSchoolMatchedWithSupportingOrganisation, RegionalDirectorDecisionDate, AssessmenToolOneLink) = (
            SupportProject?.InitialDiagnosisMatchingDecision,
            SupportProject?.RegionalDirectorDecisionDate,
            await sharePointResourceService.GetAssessmentToolOneLinkAsync(cancellationToken) ?? string.Empty
        );

        // Conditional assignment using switch expression
        // Fix for CS8131 and CS8506: Ensure the switch expression has a clear type and the deconstruction is valid.
        var decisionNotes = HasSchoolMatchedWithSupportingOrganisation switch
        {
            "Review school's progress" => (SupportProject?.InitialDiagnosisMatchingDecisionNotes, null),
            "Unable to assess" => ((string?)null, SupportProject?.InitialDiagnosisMatchingDecisionNotes),
            _ => (null, null)
        };

        (NotMatchingNotes, UnableToAssessNotes) = decisionNotes;

        RadioButtonModels = RadioButtons;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        // Collect validation errors using collection expression (.NET 8)
        var validationErrors = new List<string>();

        if (!ModelState.IsValid)
            validationErrors.Add("ModelState invalid");

        if (!IsNotMatchingNotesValid())
        {
            _errorService.AddError(nameof(NotMatchingNotes), "You must add a note");
            validationErrors.Add("NotMatchingNotes");
        }

        if (!IsUnableToAssessNotesValid())
        {
            _errorService.AddError(nameof(UnableToAssessNotes), "You must add a note");
            validationErrors.Add("UnableToAssessNotes");
        }

        // Early return pattern for validation errors
        if (validationErrors.Count != 0)
            return await HandleValidationErrorsAsync(id, cancellationToken);

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

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.Index.Page, new { id });
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorsAsync(int id, CancellationToken cancellationToken)
    {
        AssessmenToolOneLink = await sharePointResourceService.GetAssessmentToolOneLinkAsync(cancellationToken) ?? string.Empty;
        RadioButtonModels = RadioButtons;
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await base.GetSupportProject(id, cancellationToken);
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
                ValidationMessage = "You must add a note",
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
                ValidationMessage = "You must add a note",
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
