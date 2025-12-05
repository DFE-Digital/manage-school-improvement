using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class EnterSupportingOrganisationSchoolDetailsModel(
    IGetEstablishment getEstablishment,
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "organisation-name")]
    public string? OrganisationName { get; set; }

    public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

    private const string SearchLabel = "Search by school name or URN (Unique Reference Number).";

    [BindProperty]
    [Required(ErrorMessage = "Enter the school name or URN")]
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
            $"/task-list/enter-supporting-organisation-school-details/{id}?handler=Search&searchQuery=";

        AutoCompleteSearchModel = new AutoCompleteSearchModel(SearchLabel, SearchQuery, searchEndpoint);

        return Page();
    }

    public async Task<IActionResult> OnGetSearch(string searchQuery)
    {
        string[] searchSplit = SplitOnBrackets(searchQuery);

        IEnumerable<EstablishmentSearchResponse> schools = await getEstablishment.SearchEstablishments(searchSplit[0].Trim());

        return new JsonResult(schools.Select(s => new { suggestion = HighlightSearchMatch($"{s.Name} ({s.Urn})", searchSplit[0].Trim(), s), value = $"{s.Name} ({s.Urn})" }));
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

         var searchEndpoint =
            $"/task-list/enter-supporting-organisation-school-details/{id}?handler=Search&searchQuery=";
        
        AutoCompleteSearchModel = new AutoCompleteSearchModel(SearchLabel, SearchQuery, searchEndpoint, string.IsNullOrWhiteSpace(SearchQuery));
        
        string[] splitSearch = SplitOnBrackets(SearchQuery);
        
        string expectedUrn = splitSearch[splitSearch.Length - 1];

        var expectedSchool = await getEstablishment.GetEstablishmentByUrn(expectedUrn);
        
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            ModelState.AddModelError(nameof(SearchQuery), "Enter the school name or URN");
        }
        else
        {
            if (splitSearch.Length < 2)
            {
                ModelState.AddModelError(nameof(SearchQuery), "We could not find any schools matching your search criteria");
            }
            else if (splitSearch.Length > 2 && string.IsNullOrEmpty(expectedSchool.Name))
            {
                ModelState.AddModelError(nameof(SearchQuery), "We could not find a school matching your search criteria");
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
            expectedSchool.Address.Street,
            expectedSchool.Address.Locality,
            expectedSchool.Address.Town,
            expectedSchool.Address.County,
            expectedSchool.Address.Postcode
        }.Where(x => !string.IsNullOrWhiteSpace(x)));
        
        var command = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            expectedSchool.Name,
            expectedSchool.Urn,
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
            new { id, previousPage = Links.TaskList.EnterSupportingOrganisationSchoolDetails.Page });
    }

    private static string[] SplitOnBrackets(string input)
    {
        // return array containing one empty string if input string is null or empty
        if (string.IsNullOrWhiteSpace(input)) return new string[1] { string.Empty };

        return input.Split(new[] { '(', ')' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    private static string HighlightSearchMatch(string input, string toReplace, EstablishmentSearchResponse school)
    {
        if (school == null || string.IsNullOrWhiteSpace(school.Ukprn) || string.IsNullOrWhiteSpace(school.Name))
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
