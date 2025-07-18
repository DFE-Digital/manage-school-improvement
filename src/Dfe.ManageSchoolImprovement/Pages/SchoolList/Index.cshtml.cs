using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.SchoolList;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService) : PageModel
{
    public IEnumerable<SupportProjectViewModel> SupportProjects { get; set; } = [];

    [BindProperty]
    public ProjectListFilters Filters { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public PaginationViewModel Pagination { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int TotalProjects { get; set; }
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        Filters.PersistUsing(TempData).PopulateFrom(Request.Query);

        Pagination.PagePath = "/SchoolList/Index";

        var result =
           await supportProjectQueryService.SearchForSupportProjects(
               Filters.Title, Filters.SelectedStatuses, Filters.SelectedOfficers, Filters.SelectedRegions,
               Filters.SelectedLocalAuthorities, Filters.SelectedTrusts, Pagination.PagePath, Pagination.CurrentPage, Pagination.PageSize,
               cancellationToken);

        if (result.IsSuccess && result.Value != null)
        {
            Pagination.Paging = result.Value.Paging;
            TotalProjects = result.Value?.Paging?.RecordCount ?? 0;
            SupportProjects = result.Value?.Data.Select(SupportProjectViewModel.Create)!;
        }

        var regionsResult = await supportProjectQueryService.GetAllProjectRegions(cancellationToken);

        if (regionsResult.IsSuccess && regionsResult.Value != null)
        {
            Filters.AvailableRegions = regionsResult.Value.ToList();
        }

        var localAuthoritiesResult = await supportProjectQueryService.GetAllProjectLocalAuthorities(cancellationToken);

        if (localAuthoritiesResult.IsSuccess && localAuthoritiesResult.Value != null)
        {
            Filters.AvailableLocalAuthorities = localAuthoritiesResult.Value.ToList();
        }


        var assignedUsersResult = await supportProjectQueryService.GetAllProjectAssignedUsers(cancellationToken);

        if (assignedUsersResult.IsSuccess && assignedUsersResult.Value != null)
        {
            Filters.AvailableDeliveryOfficers = assignedUsersResult.Value.ToList();
        }

        var trustsResult = await supportProjectQueryService.GetAllProjectTrusts(cancellationToken);

        if (trustsResult.IsSuccess && trustsResult.Value != null)
        {
            Filters.AvailableTrusts = trustsResult.Value.ToList();
        }

        return Page();
    }
}
