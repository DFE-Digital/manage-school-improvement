using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class EscalationConfirmationModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IConfiguration configuration,
    ISharePointResourceService sharePointResourceService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }



    public bool ShowError => _errorService.HasErrors();

    public string SOPUCommissioningForm { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SOPUCommissioningForm = await sharePointResourceService.GetSOPUCommissioningFormLinkAsync(cancellationToken) ?? string.Empty;

        return Page();
    }
}