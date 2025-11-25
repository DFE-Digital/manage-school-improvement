using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class EnterSupportingOrganisationLocalAuthorityTradedServiceDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "organisation-name")]
    public string? OrganisationName { get; set; }

    [BindProperty(Name = "companies-house-number")]
    public string? CompaniesHouseNumber { get; set; }

    [BindProperty(Name = "date-support-organisation-confirmed", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportOrganisationConfirmed { get; set; }

    public bool ShowError { get; set; }

    public string? OrganisationNameErrorMessage { get; set; }
    public string? CompaniesHouseNumberErrorMessage { get; set; }
    public string? DateConfirmedErrorMessage { get; set; }

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, string? previousSupportOrganisationType, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject?.SupportOrganisationType == previousSupportOrganisationType)
        {
            OrganisationName = SupportProject?.SupportOrganisationName;
            CompaniesHouseNumber = SupportProject?.SupportOrganisationIdNumber;
            DateSupportOrganisationConfirmed = SupportProject?.DateSupportOrganisationChosen;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        OrganisationName = OrganisationName?.Trim();
        CompaniesHouseNumber = CompaniesHouseNumber?.Trim();

        await base.GetSupportProject(id, cancellationToken);

        // Validate entries
        if (OrganisationName == null || CompaniesHouseNumber == null || DateSupportOrganisationConfirmed == null)
        {
            if (OrganisationName == null)
            {
                OrganisationNameErrorMessage = "Enter the supporting organisation's name";
                ModelState.AddModelError("organisation-name", OrganisationNameErrorMessage);
            }

            if (CompaniesHouseNumber == null)
            {
                CompaniesHouseNumberErrorMessage = "Enter the supporting organisation's companies house number";
                ModelState.AddModelError("companies-house-number", CompaniesHouseNumberErrorMessage);
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
            CompaniesHouseNumber,
            SupportProject?.SupportOrganisationType, // OrganisationType is maintained from the previous page
            DateSupportOrganisationConfirmed,
            SupportProject?.AssessmentToolTwoCompleted);

        var result = await mediator.Send(command, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.ConfirmSupportingOrganisationDetails.Page, new { id, previousPage = Links.TaskList.EnterSupportingOrganisationLocalAuthorityTradedServiceDetails.Page });
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await base.GetSupportProject(id, cancellationToken);
    }
}
