using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Shared;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Watchlist;

public class SelectSchoolModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService) : PageModel
{
    public string ReturnPage { get; set; }

    public AutoCompleteSearchModel AutoCompleteSearchModel { get; set; }

    private const string SearchLabel = "Find the school within MSI using its name or URN.";

    [BindProperty]
    [Required(ErrorMessage = "Enter the school name or URN")]
    public string SearchQuery { get; set; } = "";

    public bool ShowError { get; set; }


    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        ReturnPage = @Links.Watchlist.Index.Page;

        var searchEndpoint = "/watchlist/select-school?handler=Search&searchQuery=";

        AutoCompleteSearchModel = new AutoCompleteSearchModel(SearchLabel, SearchQuery, searchEndpoint);

        return Page();
    }

    public async Task<IActionResult> OnGetSearch(string searchQuery, CancellationToken cancellationToken = default)
    {
        // Short-circuit on empty or very short queries
        if (string.IsNullOrWhiteSpace(searchQuery) || searchQuery.Trim().Length < 3)
        {
            return new JsonResult(Array.Empty<object>());
        }

        string[] searchSplit = AutosearchUtils.SplitOnBrackets(searchQuery);
        string term = (searchSplit.Length > 0 ? searchSplit[0] : string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(term))
        {
            return new JsonResult(Array.Empty<object>());
        }

        try
        {
            var projectsResult = await supportProjectQueryService.GetAllSupportProjects(cancellationToken);

            if (!projectsResult.IsSuccess || projectsResult.Value == null)
            {
                return new JsonResult(Array.Empty<object>());
            }

            IEnumerable<SupportProjectDto> schools = projectsResult.Value;

            return new JsonResult(schools.Select(s => new
            {
                suggestion =
                    AutosearchUtils.HighlightSearchMatch($"{s.SchoolName} ({s.SchoolUrn})", searchSplit[0].Trim(), s),
                value = $"{s.SchoolName} ({s.SchoolUrn})"
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
        var projectsResult = await supportProjectQueryService.GetAllSupportProjects(cancellationToken);

        var searchEndpoint = "/watchlist/select-school?handler=Search&searchQuery=";

        AutoCompleteSearchModel = new AutoCompleteSearchModel(SearchLabel, SearchQuery, searchEndpoint,
            string.IsNullOrWhiteSpace(SearchQuery));

        string[] splitSearch = AutosearchUtils.SplitOnBrackets(SearchQuery);

        if (projectsResult.Value != null)
        {
            var expectedSupportProjectId = projectsResult.Value
                .FirstOrDefault(s => s.SchoolUrn == splitSearch[splitSearch.Length - 1])?.Id;

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                ModelState.AddModelError(nameof(SearchQuery), "Enter the school name or URN");
            }
            else
            {
                if (splitSearch.Length < 2)
                {
                    ModelState.AddModelError(nameof(SearchQuery),
                        "We could not find any schools matching your search criteria");
                }
                else if (splitSearch.Length > 2 && expectedSupportProjectId == null)
                {
                    ModelState.AddModelError(nameof(SearchQuery),
                        "We could not find a school matching your search criteria");
                }
            }


            if (!ModelState.IsValid)
            {
                ShowError = true;
                errorService.AddErrors(Request.Form.Keys, ModelState);
                return Page();
            }

            return RedirectToPage(Links.Watchlist.ConfirmSchool.Page, new { expectedSupportProjectId });
        }
        
        return Page();
    }
}