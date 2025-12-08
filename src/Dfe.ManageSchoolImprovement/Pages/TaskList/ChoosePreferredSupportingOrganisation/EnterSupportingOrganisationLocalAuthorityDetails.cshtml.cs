using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class EnterSupportingOrganisationLocalAuthorityDetailsModel(
    IGetLocalAuthority getLocalAuthority,
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "organisation-name")]
    public string? OrganisationName { get; set; }

    [BindProperty(Name = "la-code")]
    public string? LaCode { get; set; }

    [BindProperty(Name = "date-support-organisation-confirmed", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportOrganisationConfirmed { get; set; }

    public bool ShowError { get; set; }

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public string? OrganisationNameErrorMessage { get; set; }
    public string? LaCodeErrorMessage { get; set; }
    public string? DateConfirmedErrorMessage { get; set; }

    public bool LaCodeError => !string.IsNullOrEmpty(LaCodeErrorMessage);

    public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Enter the local authority name or code")]
    public string SearchQuery { get; set; } = "";

    public async Task<IActionResult> OnGetAsync(int id, string? previousSupportOrganisationType, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject?.SupportOrganisationType == previousSupportOrganisationType)
        {
            OrganisationName = SupportProject?.SupportOrganisationName;
            LaCode = SupportProject?.SupportOrganisationIdNumber;
            DateSupportOrganisationConfirmed = SupportProject?.DateSupportOrganisationChosen;
        }

        var searchEndpoint =
        $"/task-list/enter-supporting-organisation-local-authority-details/{id}?handler=Search&searchQuery=";

        AutoCompleteSearchModel = new AutoCompleteSearchModel(null!, SearchQuery, searchEndpoint);

        return Page();
    }

    public async Task<IActionResult> OnGetSearch(string searchQuery)
    {
        // Short-circuit on empty or very short queries
        if (string.IsNullOrWhiteSpace(searchQuery) || searchQuery.Trim().Length < 3)
        {
            return new JsonResult(Array.Empty<object>());
        }

        string[] searchSplit = SplitOnBrackets(searchQuery);
        string term = (searchSplit.Length > 0 ? searchSplit[0] : string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(term))
        {
            return new JsonResult(Array.Empty<object>());
        }

        try
        {
            IEnumerable<NameAndCodeDto> trusts = await getLocalAuthority.SearchLocalAuthorities(term);

            return new JsonResult(trusts.Select(s => new
            {
                suggestion = HighlightSearchMatch($"{s.Name} ({s.Code})", term, s),
                value = $"{s.Name} ({s.Code})"
            }));
        }
        catch
        {
            // Fail safe for autocomplete - never surface a 500 from a suggestion call
            return new JsonResult(Array.Empty<object>());
        }
    }
    private static string[] SplitOnBrackets(string input)
    {
        // return array containing one empty string if input string is null or empty
        if (string.IsNullOrWhiteSpace(input)) return new string[1] { string.Empty };

        return input.Split(new[] { '(', ')' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }
    private static string HighlightSearchMatch(string input, string toReplace, NameAndCodeDto localAuthority)
    {
        if (localAuthority == null || string.IsNullOrWhiteSpace(localAuthority.Code) || string.IsNullOrWhiteSpace(localAuthority.Name))
            return string.Empty;

        if (string.IsNullOrWhiteSpace(toReplace))
            return input;

        int index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
        if (index < 0)
            return input;

        string correctCaseSearchString = input.Substring(index, toReplace.Length);

        return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>",
            StringComparison.InvariantCultureIgnoreCase);
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        OrganisationName = OrganisationName?.Trim();
        LaCode = LaCode?.Trim();

        await base.GetSupportProject(id, cancellationToken);

        // Validate entries
        if (OrganisationName == null || LaCode == null || DateSupportOrganisationConfirmed == null)
        {
            if (OrganisationName == null)
            {
                OrganisationNameErrorMessage = "Enter the supporting organisation's name";
                ModelState.AddModelError("organisation-name", OrganisationNameErrorMessage);
            }

            if (LaCode == null)
            {
                LaCodeErrorMessage = "Enter the supporting organisation's GIAS LA Code";
                ModelState.AddModelError("la-code", LaCodeErrorMessage);
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
            LaCode,
            SupportProject?.SupportOrganisationType, // OrganisationType is maintained from the previous page
            DateSupportOrganisationConfirmed,
            SupportProject?.AssessmentToolTwoCompleted,
            SupportProject?.SupportingOrganisationAddress,
            SupportProject?.SupportingOrganisationContactName,
            SupportProject?.SupportingOrganisationContactEmailAddress,
            SupportProject?.SupportingOrganisationContactPhone,
            SupportProject?.SupportingOrganisationAddress,
            SupportProject?.DateSupportingOrganisationContactDetailsAdded);

        var result = await mediator.Send(command, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.ConfirmSupportingOrganisationDetails.Page, new { id, previousPage = Links.TaskList.EnterSupportingOrganisationLocalAuthorityDetails.Page });
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await base.GetSupportProject(id, cancellationToken);
    }
}
