using Dfe.ManageSchoolImprovement.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Services;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AboutTheSchool;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, IGetEstablishment getEstablishment, ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService,getEstablishment,errorService)
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
        
        return Page();
    }
}
