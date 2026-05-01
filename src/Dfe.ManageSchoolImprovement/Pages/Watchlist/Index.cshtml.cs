using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Watchlist;

public class IndexModel(IWatchlistQueryService watchlistQueryService,
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IApplicationSettingsResourceService applicationSettingsResourceService) : PageModel
{
    protected readonly IWatchlistQueryService _watchlistQueryService = watchlistQueryService;
    protected readonly ISupportProjectQueryService _supportProjectQueryService = supportProjectQueryService;
    protected readonly ErrorService _errorService = errorService;

    public string ReturnPage { get; set; }

    public string? CurrentUser { get; set; }
    public Result<IEnumerable<int>>? WatchlistSupportProjectIds { get; set; }

    public List<WatchlistViewModel> Watchlist { get; set; } = new();

    public string RiseDeliveryDashboardLink { get; set; } = string.Empty;
    public string RiseDataTablesLink { get; set; } = string.Empty;
    public string RiseMonitoringReportsLink { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.Watchlist.Index.Page;

        CurrentUser = User.Identity?.Name;

        WatchlistSupportProjectIds =
            await _watchlistQueryService.GetAllSchoolsForUser(CurrentUser ?? string.Empty, cancellationToken);

        if (WatchlistSupportProjectIds.Value != null && WatchlistSupportProjectIds.Value.Any())
        {
            foreach (var schoolId in WatchlistSupportProjectIds.Value)
            {
                var result = await _supportProjectQueryService.GetSupportProject(schoolId, cancellationToken);

                if (result.IsSuccess && result.Value != null)
                {
                    Watchlist.Add(
                        new WatchlistViewModel
                        {
                            SupportProjectId = result.Value.Id,
                            User = CurrentUser,
                            SchoolName = result.Value.SchoolName,
                            DateAdded = result.Value.CreatedOn,
                            AssignedTo = result.Value.AssignedDeliveryOfficerFullName,
                            SupportingOrganisationName = result.Value.SupportOrganisationName,
                            Status = result.Value.ProjectStatus
                        }
                        );
                }
            }
        }

        RiseDeliveryDashboardLink = await applicationSettingsResourceService.GetRISEDeliveryDashboardLinkAsync(cancellationToken) ?? string.Empty;
        RiseDataTablesLink = await applicationSettingsResourceService.GetRISEDataTablesLinkAsync(cancellationToken) ?? string.Empty;
        RiseMonitoringReportsLink = await applicationSettingsResourceService.GetRISEMonitoringReportsLinkAsync(cancellationToken) ?? string.Empty;

        return Page();
    }
}
