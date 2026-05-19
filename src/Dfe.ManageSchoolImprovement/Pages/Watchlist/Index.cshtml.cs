using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Watchlist;

public class IndexModel(
    IWatchlistQueryService watchlistQueryService,
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IApplicationSettingsResourceService applicationSettingsResourceService) : PageModel
{
    protected readonly IWatchlistQueryService _watchlistQueryService = watchlistQueryService;
    protected readonly ISupportProjectQueryService _supportProjectQueryService = supportProjectQueryService;
    protected readonly ErrorService _errorService = errorService;

    public string ReturnPage { get; set; }

    public string? CurrentUser { get; set; }
    public Result<IEnumerable<Domain.Entities.SupportProject.Watchlist>> WatchlistSupportProjects { get; set; }

    public List<WatchlistViewModel> Watchlist { get; set; } = new();

    public string RiseDeliveryDashboardLink { get; set; } = string.Empty;
    public string RiseDataTablesLink { get; set; } = string.Empty;
    public string RiseMonitoringReportsLink { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.Watchlist.Index.Page;

        CurrentUser = User.Identity?.Name;

        WatchlistSupportProjects =
            await _watchlistQueryService.GetAllSchoolsForUser(CurrentUser ?? string.Empty, cancellationToken);

        if (WatchlistSupportProjects.Value != null && WatchlistSupportProjects.Value.Any())
        {
            foreach (var school in WatchlistSupportProjects.Value)
            {
                var result =
                    await _supportProjectQueryService.GetSupportProject(school.SupportProjectId.Value,
                        cancellationToken);

                if (result.IsSuccess && result.Value != null)
                {
                    var supportProject = result.Value;
                    Watchlist.Add(
                        new WatchlistViewModel
                        {
                            WatchlistId = school.Id.Value,
                            ReadableId = school.ReadableId,
                            SupportProjectId = supportProject.Id,
                            User = CurrentUser,
                            SchoolName = supportProject.SchoolName,
                            DateAdded = supportProject.CreatedOn,
                            AssignedTo = supportProject.AssignedDeliveryOfficerFullName,
                            SupportingOrganisationName = supportProject.SupportOrganisationName,
                            Status = supportProject.ProjectStatus,
                            CurrentDeliveryMilestone = supportProject.CurrentDeliveryMilestone
                        }
                    );
                }
            }
        }

        RiseDeliveryDashboardLink =
            await applicationSettingsResourceService.GetRISEDeliveryDashboardLinkAsync(cancellationToken) ??
            string.Empty;
        RiseDataTablesLink = await applicationSettingsResourceService.GetRISEDataTablesLinkAsync(cancellationToken) ??
                             string.Empty;
        RiseMonitoringReportsLink =
            await applicationSettingsResourceService.GetRISEMonitoringReportsLinkAsync(cancellationToken) ??
            string.Empty;

        return Page();
    }
}