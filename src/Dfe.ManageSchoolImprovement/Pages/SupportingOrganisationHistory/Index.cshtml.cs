using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.SupportingOrganisationHistory;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService,
    ISupportProjectAuditQueryService supportProjectAuditQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }

    public void SetErrorPage(string errorPage)
    {
        TempData["ErrorPage"] = errorPage;
    }

    public IList<SupportProjectFieldAuditDto<string?>> SupportOrganisationAuditTrail { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        var auditTrail = new List<SupportProjectFieldAuditDto<string?>>();

        // Get audit trail for support organisation name
        var supportOrganisationNameAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.SupportOrganisationName, cancellationToken);

        if (supportOrganisationNameAuditResult.IsSuccess)
        {
            var filteredNameAudits = supportOrganisationNameAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
            auditTrail.AddRange(filteredNameAudits);
        }

        // Get audit trail for support organisation type
        var supportOrganisationTypeAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.SupportingOrganisationType, cancellationToken);

        if (supportOrganisationTypeAuditResult.IsSuccess)
        {
            var filteredTypeAudits = supportOrganisationTypeAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
            auditTrail.AddRange(filteredTypeAudits);
        }

        // Get audit trail for support organisation type
        var supportOrganisationIdNumberAuditResult = await supportProjectAuditQueryService.GetFieldAuditTrailAsync(
            id, sp => sp.SupportOrganisationIdNumber, cancellationToken);

        if (supportOrganisationIdNumberAuditResult.IsSuccess)
        {
            var filteredTypeAudits = supportOrganisationIdNumberAuditResult.Value!
                .Where(a => !string.IsNullOrEmpty(a.FieldValue))
                .ToList();
            auditTrail.AddRange(filteredTypeAudits);
        }

        // Sort the combined results
        SupportOrganisationAuditTrail = auditTrail
            .OrderByDescending(a => a.LastModifiedOn)
            .ToList();

        return Page();
    }
}
