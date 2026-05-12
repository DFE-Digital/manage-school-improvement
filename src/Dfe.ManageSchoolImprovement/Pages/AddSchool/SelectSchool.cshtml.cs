using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Shared;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AddSchool;

public class SelectSchoolModel(IGetEstablishment getEstablishment, ErrorService errorService, ISupportProjectQueryService supportProjectQueryService) : PageModel
{
    private const string SEARCH_LABEL = "Find the school using its name or URN.";
    private const string SEARCH_ENDPOINT = "/select-school?handler=Search&searchQuery=";

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
        string[] searchSplit = AutosearchUtils.SplitOnBrackets(searchQuery);

        IEnumerable<EstablishmentSearchResponse> schools = await getEstablishment.SearchEstablishments(searchSplit[0].Trim());

        return new JsonResult(schools.Select(s => new { suggestion = AutosearchUtils.HighlightSearchMatch($"{s.Name} ({s.Urn})", searchSplit[0].Trim(), s), value = $"{s.Name} ({s.Urn})" }));
    }

    public async Task<IActionResult> OnPost()
    {
        AutoCompleteSearchModel = new AutoCompleteSearchModel(SEARCH_LABEL, SearchQuery, SEARCH_ENDPOINT, string.IsNullOrWhiteSpace(SearchQuery));

        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            ModelState.AddModelError(nameof(SearchQuery), "Enter the school name or URN");
            errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }
        string[] splitSearch = AutosearchUtils.SplitOnBrackets(SearchQuery);
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
            ModelState.AddModelError(nameof(SearchQuery), "This school is already receiving targeted intervention. Select a different school");
            errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        return RedirectToPage(Links.AddSchool.Summary.Page, new { expectedEstablishment.Urn });
    }
}
