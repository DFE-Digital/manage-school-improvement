using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AboutTheSchool;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    IEstablishmentsClient establishmentsClient,
    ITrustsClient trustsClient,
    ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }

    public void SetErrorPage(string errorPage)
    {
        TempData["ErrorPage"] = errorPage;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        if (!int.TryParse(SupportProject?.SchoolUrn, out var schoolUrn))
        {
            schoolUrn = 0; // Default value if parsing fails
        }

        //var establishmentContacts = await establishmentsClient.GetAllPersonsAssociatedWithAcademyByUrnAsync(schoolUrn, cancellationToken).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(SupportProject?.TrustName))
        {
            var trustContacts = await trustsClient.GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(SupportProject.TrustReferenceNumber, cancellationToken).ConfigureAwait(false);
        }

        return Page();
    }
}
