using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AddSchool;

public class WhichSchoolNeedsHelpModel(IGetEstablishment getEstablishment, ErrorService errorService, ISupportProjectQueryService supportProjectQueryService) : PageModel
{
    private const string SEARCH_LABEL = "Find the school using its name or URN.";
    private const string SEARCH_ENDPOINT = "/which-school-needs-help?handler=Search&searchQuery=";

    [BindProperty]
    [Required(ErrorMessage = "Enter the school name or URN")]
    public string SearchQuery { get; set; } = "";

    public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

    public IActionResult OnGet()
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);

        return Page();
    }

    public async Task<IActionResult> OnGetSearch(string searchQuery)
    {
        string[] searchSplit = SplitOnBrackets(searchQuery);

        IEnumerable<EstablishmentSearchResponse> schools = await getEstablishment.SearchEstablishments(searchSplit[0].Trim());

        return new JsonResult(schools.Select(s => new { suggestion = HighlightSearchMatch($"{s.Name} ({s.Urn})", searchSplit[0].Trim(), s), value = $"{s.Name} ({s.Urn})" }));
    }

    public async Task<IActionResult> OnPost()
    {
        AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT);

        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            ModelState.AddModelError(nameof(SearchQuery), "Enter the school name or URN.");
            errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }
        string[] splitSearch = SplitOnBrackets(SearchQuery);
        if (splitSearch.Length < 2)
        {
            ModelState.AddModelError(nameof(SearchQuery), "We could not find any schools matching your search criteria");
            errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        string expectedUrn = splitSearch[splitSearch.Length - 1];

        var expectedEstablishment = await getEstablishment.GetEstablishmentByUrn(expectedUrn);

        if (expectedEstablishment.Name == null)
        {
            ModelState.AddModelError(nameof(SearchQuery), "We could not find a school matching your search criteria");
            errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        CancellationToken cancellationToken = new();
        var existingSupportProjects = await supportProjectQueryService.GetAllSupportProjects(cancellationToken);

        if (existingSupportProjects.Value != null && existingSupportProjects.Value.Any(a => a.SchoolUrn == expectedEstablishment.Urn))
        {
            ModelState.AddModelError(nameof(SearchQuery), "This school is already getting support, choose a different school");
            errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        return RedirectToPage(Links.AddSchool.Summary.Page, new { expectedEstablishment.Urn });
    }

    private static string HighlightSearchMatch(string input, string toReplace, EstablishmentSearchResponse school)
    {
        if (school == null || string.IsNullOrWhiteSpace(school.Urn) || string.IsNullOrWhiteSpace(school.Name)) return string.Empty;

        int index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
        string correctCaseSearchString = input.Substring(index, toReplace.Length);

        return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>", StringComparison.InvariantCultureIgnoreCase);
    }

    private static string[] SplitOnBrackets(string input)
    {
        // return array containing one empty string if input string is null or empty
        if (string.IsNullOrWhiteSpace(input)) return new string[1] { string.Empty };

        return input.Split(new[] { '(', ')' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }
}
