using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, IGetEstablishment getEstablishment, ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }

    [TempData]
    public bool? EngagementConcernRecorded { get; set; }

    [TempData]
    public bool? EngagementConcernRemoved { get; set; }

    [TempData]
    public bool? EngagementConcernUpdated { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        return Page();
    }

}