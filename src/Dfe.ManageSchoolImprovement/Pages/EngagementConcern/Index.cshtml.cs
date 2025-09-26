using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
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

    [TempData]
    public bool? InformationPowersRecorded { get; set; }

    [TempData]
    public bool? InformationPowersRemoved { get; set; }

    [TempData]
    public bool? InformationPowersUpdated { get; set; }

    [TempData]
    public bool? InterimExecutiveBoardRecorded { get; set; }

    [TempData]
    public bool? InterimExecutiveBoardRemoved { get; set; }

    [TempData]
    public bool? InterimExecutiveBoardUpdated { get; set; }

    [TempData]
    public bool? InterimExecutiveBoardDateUpdated { get; set; }

    public List<EngagementConcernViewModel> EngagementConcerns { get; set; } = [];

    public bool MultipleActiveEngagementConcernsWithoutIeb { get; set; } = false;
    public bool MultipleActiveEngagementConcernsWithoutInformationPowers { get; set; } = false;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        EngagementConcerns = SupportProject?.EngagementConcerns?.OrderByDescending(x => x.EngagementConcernRaisedDate).ToList() ?? [];

        //MultipleActiveEngagementConcernsWithoutIeb = EngagementConcerns.Where(x => x.EngagementConcernResolved is false && x.)


        return Page();
    }
}