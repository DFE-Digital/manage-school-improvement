using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }
    
    public ProjectStatusValue? SupportProjectStatus { get; set; }
    public DateTime? DateOfDecision { get; set; }
    public string? AdditionalDetails { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SupportProjectStatus = SupportProject?.ProjectStatus;
        DateOfDecision = SupportProject?.ProjectStatusChangedDate;
        AdditionalDetails = SupportProject?.ProjectStatusChangedDetails;

        return Page();
    }
}