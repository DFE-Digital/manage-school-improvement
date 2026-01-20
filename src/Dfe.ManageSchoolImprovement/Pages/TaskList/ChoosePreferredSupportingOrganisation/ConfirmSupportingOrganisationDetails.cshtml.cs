using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class ConfirmSupportingOrganisationDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "date-support-organisation-confirmed", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportOrganisationConfirmed { get; set; }

    [BindProperty(Name = "js-enabled")]
    public bool JavaScriptEnabled { get; set; }

    public string? OrganisationAddress { get; set; }


    public bool ShowError { get; set; }

    public string? DateConfirmedErrorMessage { get; private set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public string PreviousPage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, string? previousPage,
        CancellationToken cancellationToken = default)
    {
        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        await base.GetSupportProject(id, cancellationToken);

        DateSupportOrganisationConfirmed = SupportProject?.DateSupportOrganisationChosen;
        OrganisationAddress = SupportProject?.SupportingOrganisationAddress;

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, string? previousPage, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        if (DateSupportOrganisationConfirmed == null)
        {
            DateConfirmedErrorMessage = "Enter a date";
            ModelState.AddModelError("date-support-organisation-confirmed", DateConfirmedErrorMessage);
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await GetSupportProject(id, cancellationToken);
        }

        var command = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            SupportProject?.SupportOrganisationName,
            SupportProject?.SupportOrganisationIdNumber,
            SupportProject?.SupportOrganisationType,
            DateSupportOrganisationConfirmed,
            SupportProject?.AssessmentToolTwoCompleted,
            SupportProject?.SupportingOrganisationAddress,
            SupportProject?.SupportingOrganisationContactName,
            SupportProject?.SupportingOrganisationContactEmailAddress,
            SupportProject?.SupportingOrganisationContactPhone,
            SupportProject?.SupportingOrganisationContactAddress);

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
}