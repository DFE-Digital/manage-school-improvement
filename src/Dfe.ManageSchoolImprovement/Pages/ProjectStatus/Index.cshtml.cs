using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }

    public void SetErrorPage(string errorPage)
    {
        TempData["ErrorPage"] = errorPage;
    }
    
    public Domain.ValueObjects.ProjectStatus? SupportProjectStatus { get; set; }
    public string? Reason { get; set; }
    public string? Details { get; set; }
    public DateTime? DateOfDecision { get; set; }
    public string? AdditionalDetails { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SupportProjectStatus = SupportProject?.ProjectStatus;
        Reason = "This is a reason";
        Details = "These are the details";
        DateOfDecision = DateTime.Now;
        AdditionalDetails = "These are additional details";

        return Page();
    }
}