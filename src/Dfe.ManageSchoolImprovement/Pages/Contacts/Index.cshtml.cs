using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, IGetEstablishment getEstablishment, ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
    {
        public string ReturnPage { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            ProjectListFilters.ClearFiltersFrom(TempData);
            ReturnPage = @Links.SchoolList.Index.Page;
            TempData["RoleId"] = null;
            TempData["OtherRole"] = null;
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }
    }
}
