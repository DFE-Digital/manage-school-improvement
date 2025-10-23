using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.CompleteAndSaveInitialDiagnosisAssessment;

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "saved-assessemnt-template-in-sharepoint-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Saved assessement template")]
    public DateTime? SavedAssessmentTemplateInSharePointDate { get; set; }

    [BindProperty(Name = "has-talk-to-adviser")]
    public bool? HasTalkToAdviserAboutFindings { get; set; }

    [BindProperty(Name = "complete-assessment-template")]
    public bool? HasCompleteAssessmentTemplate { get; set; }

    public string AssessmenToolOneLink { get; set; } = string.Empty;
    public bool ShowError { get; set; }

    // Using .NET 8 static interface members for cleaner implementation
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing(string displayName) =>
        "Enter the saved assessment template date in SharePoint";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Using object initializer with conditional assignment
        (SavedAssessmentTemplateInSharePointDate, HasTalkToAdviserAboutFindings, HasCompleteAssessmentTemplate, AssessmenToolOneLink) =
            (SupportProject?.SavedAssessmentTemplateInSharePointDate,
             SupportProject?.HasTalkToAdviserAboutFindings,
             SupportProject?.HasCompleteAssessmentTemplate,
             await sharePointResourceService.GetAssessmentToolOneLinkAsync(cancellationToken) ?? string.Empty);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        // Early assignment of SharePoint link
        AssessmenToolOneLink = await sharePointResourceService.GetAssessmentToolOneLinkAsync(cancellationToken) ?? string.Empty;

        // Early return pattern for validation
        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        // Using target-typed new expression (.NET 8 feature)
        var command = new SetCompleteAndSaveInitialDiagnosisTemplateCommand(
            new SupportProjectId(id),
            SavedAssessmentTemplateInSharePointDate,
            HasTalkToAdviserAboutFindings,
            HasCompleteAssessmentTemplate);

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
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await base.GetSupportProject(id, cancellationToken);
    }
}