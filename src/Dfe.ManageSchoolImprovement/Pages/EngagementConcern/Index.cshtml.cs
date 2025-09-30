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

    public int MultipleActiveEngagementConcernsWithoutIebCount { get; set; } = 0;
    public int? EngagementConcernWithoutIebReadableId { get; set; } = null;
    public int MultipleActiveEngagementConcernsWithoutInformationPowersCount { get; set; } = 0;
    public int? EngagementConcernWithoutInformationPowersId { get; set; } = null;


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        EngagementConcerns = SupportProject?.EngagementConcerns?.OrderByDescending(x => x.EngagementConcernRaisedDate).ToList() ?? [];

        // we need to know what page to navigate to associate ieb and information powers, if there are multiple per category then we have to go to the select relevant concern page
        // else we go to the next avaialable concern
        MultipleActiveEngagementConcernsWithoutIebCount = EngagementConcerns.Where(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated != true).Count();

        if (MultipleActiveEngagementConcernsWithoutIebCount == 1)
        {
            EngagementConcernWithoutIebReadableId = EngagementConcerns.Single(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated != true).ReadableId;
        }

        MultipleActiveEngagementConcernsWithoutInformationPowersCount = EngagementConcerns.Where(x => x.EngagementConcernResolved != true && x.InformationPowersInUse != true).Count();

        if (MultipleActiveEngagementConcernsWithoutInformationPowersCount == 1)
        {
            EngagementConcernWithoutInformationPowersId = EngagementConcerns.Single(x => x.EngagementConcernResolved != true && x.InformationPowersInUse != true).ReadableId;
        }

        return Page();
    }
}