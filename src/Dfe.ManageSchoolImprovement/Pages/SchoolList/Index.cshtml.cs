using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
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

    public SupportProjectSearchRequest SearchRequest => new()
    {
        Title = Filters.Title,
        States = Filters.SelectedStatuses,
        AssignedUsers = Filters.SelectedOfficers,
        AssignedAdvisers = Filters.SelectedAdvisers,
        Regions = Filters.SelectedRegions,
        LocalAuthorities = Filters.SelectedLocalAuthorities,
        Trusts = Filters.SelectedTrusts,
        Months = Filters.SelectedMonths,
        Years = Filters.SelectedYears,
        PagePath = Pagination.PagePath,
        Page = Pagination.CurrentPage,
        Count = Pagination.PageSize
    };
    
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        Filters.PersistUsing(TempData).PopulateFrom(Request.Query);

        Pagination.PagePath = "/SchoolList/Index";
        
        Filters.YearsChecked = new List<string>(Filters.SelectedYears);
        
        Filters.RemoveMonthsIfYearUnchecked();
        Filters.RemoveYearsInSelectedMonths(Request.Query);

        var result =
           await supportProjectQueryService.SearchForSupportProjects(
               SearchRequest,
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

        var assignedAdvisersResult = await supportProjectQueryService.GetAllProjectAssignedAdvisers(cancellationToken);

        if (assignedAdvisersResult.IsSuccess && assignedAdvisersResult.Value != null)
        {
            Filters.AvailableAdvisers = assignedAdvisersResult.Value.ToList();
        }


        var trustsResult = await supportProjectQueryService.GetAllProjectTrusts(cancellationToken);

        if (trustsResult.IsSuccess && trustsResult.Value != null)
        {
            Filters.AvailableTrusts = trustsResult.Value.ToList();
        }
        
        var datesResult = await supportProjectQueryService.GetAllProjectYears(cancellationToken);
        
        if (datesResult.IsSuccess && datesResult.Value != null)
        {
            Filters.AvailableYears = datesResult.Value.ToList();
        }

        return Page();
    }
}
