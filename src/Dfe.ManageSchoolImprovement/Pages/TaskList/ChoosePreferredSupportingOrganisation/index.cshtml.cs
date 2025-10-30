using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "organisation-name")]
    public string? OrganisationName { get; set; }

    [BindProperty(Name = "id-number")]
    public string? IdNumber { get; set; }

    [BindProperty(Name = "date-support-organisation-chosen", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportOrganisationChosen { get; set; }

    [BindProperty(Name = "complete-assessment-tool")]
    public bool? CompleteAssessmentTool { get; set; }

    public bool ShowError { get; set; }
    public string AssessmentToolTwoLink { get; set; } = string.Empty;
    public string AssessmentToolTwoSharePointFolderLink { get; set; } = string.Empty;

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Tuple deconstruction for multiple assignments
        (OrganisationName, IdNumber, DateSupportOrganisationChosen, CompleteAssessmentTool) = (
            SupportProject?.SupportOrganisationName,
            SupportProject?.SupportOrganisationIdNumber,
            SupportProject?.DateSupportOrganisationChosen,
            SupportProject?.AssessmentToolTwoCompleted
        );

        await LoadSharePointLinksAsync(cancellationToken);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        OrganisationName = OrganisationName?.Trim();
        // Load SharePoint links early for both success and error paths
        await LoadSharePointLinksAsync(cancellationToken);

        // Early return for validation errors
        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        var command = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            OrganisationName,
            IdNumber,
            DateSupportOrganisationChosen,
            CompleteAssessmentTool);

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

    // Extracted method for loading SharePoint links concurrently
    private async Task LoadSharePointLinksAsync(CancellationToken cancellationToken)
    {
        AssessmentToolTwoLink = await sharePointResourceService.GetAssessmentToolTwoLinkAsync(cancellationToken) ?? string.Empty;
        AssessmentToolTwoSharePointFolderLink = await sharePointResourceService.GetAssessmentToolTwoSharePointFolderLinkAsync(cancellationToken) ?? string.Empty;
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await base.GetSupportProject(id, cancellationToken);
    }
}
