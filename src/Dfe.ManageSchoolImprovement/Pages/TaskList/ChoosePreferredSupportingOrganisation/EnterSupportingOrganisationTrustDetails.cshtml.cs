using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class EnterSupportingOrganisationTrustDetailsModel(
    IGetTrust getTrust,
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "organisation-name")]
    public string? OrganisationName { get; set; }

    public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

    private const string SearchLabel = "Search by trust name or UKPRN (UK Provider Reference Number).";

    [BindProperty]
    [Required(ErrorMessage = "Enter the trust name or UKPRN")]
    public string SearchQuery { get; set; } = "";

    public bool ShowError { get; set; }

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, string? previousSupportOrganisationType,
        CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject?.SupportOrganisationType == previousSupportOrganisationType)
        {
            OrganisationName = SupportProject?.SupportOrganisationName;
        }
        
        var searchEndpoint =
            $"/task-list/enter-supporting-organisation-trust-details/{id}?handler=Search&searchQuery=";

        AutoCompleteSearchModel = new AutoCompleteSearchModel(SearchLabel, SearchQuery, searchEndpoint);

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
            IEnumerable<TrustSearchResponse> trusts = await getTrust.SearchTrusts(term);

            return new JsonResult(trusts.Select(s => new
            {
                suggestion = HighlightSearchMatch($"{s.Name} ({s.Ukprn})", term, s),
                value = $"{s.Name} ({s.Ukprn})"
            }));
        }
        catch
        {
            // Fail safe for autocomplete - never surface a 500 from a suggestion call
            return new JsonResult(Array.Empty<object>());
        }
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

         var searchEndpoint =
            $"/task-list/enter-supporting-organisation-trust-details/{id}?handler=Search&searchQuery=";
        
        AutoCompleteSearchModel = new AutoCompleteSearchModel(SearchLabel, SearchQuery, searchEndpoint, string.IsNullOrWhiteSpace(SearchQuery));
        
        string[] splitSearch = SplitOnBrackets(SearchQuery);
        
        string expectedUkprn = splitSearch[splitSearch.Length - 1];

        var expectedTrust = await getTrust.GetTrustByUkprn(expectedUkprn);

        // if (string.IsNullOrWhiteSpace(SearchQuery))
        // {
        //     ModelState.AddModelError(nameof(SearchQuery), "Enter the trust name or UKPRN");
        // }
        //
        // if (!string.IsNullOrWhiteSpace(SearchQuery) && splitSearch.Length < 2)
        // {
        //     ModelState.AddModelError(nameof(SearchQuery), "We could not find any trusts matching your search criteria");
        // }
        //
        // if (!string.IsNullOrWhiteSpace(SearchQuery) && splitSearch.Length > 2  && string.IsNullOrEmpty(expectedTrust.Name))
        // {
        //     ModelState.AddModelError(nameof(SearchQuery), "We could not find a trust matching your search criteria");
        // }
        
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            ModelState.AddModelError(nameof(SearchQuery), "Enter the trust name or UKPRN");
        }
        else
        {
            if (splitSearch.Length < 2)
            {
                ModelState.AddModelError(nameof(SearchQuery), "We could not find any trusts matching your search criteria");
            }
            else if (splitSearch.Length > 2 && string.IsNullOrEmpty(expectedTrust.Name))
            {
                ModelState.AddModelError(nameof(SearchQuery), "We could not find a trust matching your search criteria");
            }
        }

        if (!ModelState.IsValid)
        {
            ShowError = true;
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            return Page();
        }
        
        var address = string.Join(", ", new[]
        {
            expectedTrust.Address.Street,
            expectedTrust.Address.Locality,
            expectedTrust.Address.Town,
            expectedTrust.Address.County,
            expectedTrust.Address.Postcode
        }.Where(x => !string.IsNullOrWhiteSpace(x)));
        
        var command = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            expectedTrust.Name,
            expectedTrust.Ukprn,
            SupportProject?.SupportOrganisationType, // OrganisationType is maintained from the previous page
            SupportProject?.DateSupportOrganisationChosen,
            SupportProject?.AssessmentToolTwoCompleted,
            address,
            SupportProject?.SupportingOrganisationContactName,
            SupportProject?.SupportingOrganisationContactEmailAddress,
            SupportProject?.SupportingOrganisationContactPhone,
            SupportProject?.DateSupportingOrganisationContactDetailsAdded);
        
        var result = await mediator.Send(command, cancellationToken);
        
        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        TaskUpdated = true;

        return RedirectToPage(Links.TaskList.ConfirmSupportingOrganisationDetails.Page,
            new { id, previousPage = Links.TaskList.EnterSupportingOrganisationTrustDetails.Page });
    }

    private static string[] SplitOnBrackets(string input)
    {
        // return array containing one empty string if input string is null or empty
        if (string.IsNullOrWhiteSpace(input)) return new string[1] { string.Empty };

        return input.Split(new[] { '(', ')' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    private static string HighlightSearchMatch(string input, string toReplace, TrustSearchResponse trust)
    {
        if (trust == null || string.IsNullOrWhiteSpace(trust.Ukprn) || string.IsNullOrWhiteSpace(trust.Name))
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
}
