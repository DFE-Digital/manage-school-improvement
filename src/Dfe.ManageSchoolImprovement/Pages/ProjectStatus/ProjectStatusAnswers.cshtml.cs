using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class ProjectStatusAnswersModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService),
        IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);
        
        await base.GetSupportProject(id, cancellationToken);
        
        ReturnPage = SupportProject?.ProjectStatus switch
        {
            ProjectStatusValue.InProgress => @Links.ProjectStatusTab.ProjectStatusInProgressDetails.Page,
            ProjectStatusValue.Paused => @Links.ProjectStatusTab.ProjectStatusPausedDetails.Page,
            ProjectStatusValue.Stopped => @Links.ProjectStatusTab.ProjectStatusStoppedDetails.Page,
            _ => string.Empty
        };

        return Page();
    }
}
