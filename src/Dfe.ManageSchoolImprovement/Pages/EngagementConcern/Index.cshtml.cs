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
    public bool AllEngagementConcernsResolved { get; private set; } = true;
    public int ActiveEngagementConcernsWithoutIebCount { get; set; } = 0;
    public bool ActiveEngagementConcernsWithIeb { get; set; } = false;
    public int? EngagementConcernWithoutIebReadableId { get; set; } = null;
    public int ActiveEngagementConcernsWithoutInformationPowersCount { get; set; } = 0;
    public bool ActiveEngagementConcernsWithInformationPowers { get; private set; }
    public int? EngagementConcernWithoutInformationPowersId { get; set; } = null;


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        EngagementConcerns = SupportProject?.EngagementConcerns?.OrderByDescending(x => x.EngagementConcernRaisedDate).ToList() ?? [];
        AllEngagementConcernsResolved = !EngagementConcerns.Any(x => x.EngagementConcernResolved != true);
        // we need to know what page to navigate to associate ieb and information powers, if there are multiple per category then we have to go to the select relevant concern page
        // else we go to the next avaialable concern
        ActiveEngagementConcernsWithoutIebCount = EngagementConcerns.Where(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated != true).Count();
        ActiveEngagementConcernsWithIeb = EngagementConcerns.Where(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated == true).Any();

        if (ActiveEngagementConcernsWithoutIebCount == 1 && !ActiveEngagementConcernsWithIeb)
        {
            EngagementConcernWithoutIebReadableId = EngagementConcerns.Single(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated != true).ReadableId;
        }

        ActiveEngagementConcernsWithoutInformationPowersCount = EngagementConcerns.Where(x => x.EngagementConcernResolved != true && x.InformationPowersInUse != true).Count();
        ActiveEngagementConcernsWithInformationPowers = EngagementConcerns.Where(x => x.EngagementConcernResolved != true && x.InformationPowersInUse == true).Any();

        if (ActiveEngagementConcernsWithoutInformationPowersCount == 1 && !ActiveEngagementConcernsWithInformationPowers)
        {
            EngagementConcernWithoutInformationPowersId = EngagementConcerns.Single(x => x.EngagementConcernResolved != true && x.InformationPowersInUse != true).ReadableId;
        }

        return Page();
    }
}