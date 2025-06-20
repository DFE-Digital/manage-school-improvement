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
    
    public DateTime? DateRaised { get; set; }
    
    public bool EngagementConcernEscalated { get; set; }
    
    public string? EngagementConcernEscalationReason { get; set; }
    
    public DateTime? DateEscalated { get; set; }
    
    

    [TempData]
    public bool? InformationPowersRecorded { get; set; }

    [TempData]
    public bool? InformationPowersRemoved { get; set; }


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        DateRaised = SupportProject.EngagementConcernRaisedDate;
        EngagementConcernEscalated = 
            (SupportProject.EngagementConcernEscalationConfirmStepsTaken ?? false) &&
            !string.IsNullOrEmpty(SupportProject.EngagementConcernEscalationPrimaryReason) &&
            !string.IsNullOrEmpty(SupportProject.EngagementConcernEscalationDetails) &&
            SupportProject.EngagementConcernEscalationDateOfDecision.HasValue;
        EngagementConcernEscalationReason = SupportProject.EngagementConcernEscalationPrimaryReason;
        DateEscalated = SupportProject.EngagementConcernEscalationDateOfDecision;

        return Page();
    }

}