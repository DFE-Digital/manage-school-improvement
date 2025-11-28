using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class EnterSupportingOrganisationSchoolDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "organisation-name")]
    public string? OrganisationName { get; set; }

    [BindProperty(Name = "urn")]
    public string? URN { get; set; }

    [BindProperty(Name = "date-support-organisation-confirmed", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportOrganisationConfirmed { get; set; }

    public bool ShowError { get; set; }

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public string? OrganisationNameErrorMessage { get; private set; }
    public string? UrnErrorMessage { get; private set; }
    public string? DateConfirmedErrorMessage { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, string? previousSupportOrganisationType, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject?.SupportOrganisationType == previousSupportOrganisationType)
        {
            OrganisationName = SupportProject?.SupportOrganisationName;
            URN = SupportProject?.SupportOrganisationIdNumber;
            DateSupportOrganisationConfirmed = SupportProject?.DateSupportOrganisationChosen;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        OrganisationName = OrganisationName?.Trim();
        URN = URN?.Trim();

        await base.GetSupportProject(id, cancellationToken);

        // Validate entries
        if (OrganisationName == null || URN == null || DateSupportOrganisationConfirmed == null)
        {
            if (OrganisationName == null)
            {
                OrganisationNameErrorMessage = "Enter the supporting organisation's name";
                ModelState.AddModelError("organisation-name", OrganisationNameErrorMessage);
            }

            if (URN == null)
            {
                UrnErrorMessage = "Enter the supporting organisation's URN";
                ModelState.AddModelError("urn", UrnErrorMessage);
            }

            if (DateSupportOrganisationConfirmed == null)
            {
                DateConfirmedErrorMessage = "Enter a date";
                ModelState.AddModelError("date-support-organisation-confirmed", DateConfirmedErrorMessage);
            }
        }
        // Early return for validation errors
        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        var command = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            OrganisationName,
            URN,
            "School", // OrganisationType is "School" for this page
            DateSupportOrganisationConfirmed,
            SupportProject?.AssessmentToolTwoCompleted,
            SupportProject?.SupportingOrganisationAddress);

        var result = await mediator.Send(command, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.ConfirmSupportingOrganisationDetails.Page, new { id, previousPage = Links.TaskList.EnterSupportingOrganisationSchoolDetails.Page });
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await base.GetSupportProject(id, cancellationToken);
    }
}
