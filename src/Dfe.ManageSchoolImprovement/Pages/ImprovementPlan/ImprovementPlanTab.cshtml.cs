using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ImprovementPlan;

public class ImprovementPlanTabModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }
    
    public ImprovementPlanViewModel? ImprovementPlan { get; set; }
    
    public List<ImprovementPlanObjectiveViewModel> Objectives { get; set; } = new List<ImprovementPlanObjectiveViewModel>();
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.SchoolList.Index.Page;
        
        await base.GetSupportProject(id, cancellationToken);
        
        // Ensure SupportProject is not null before accessing ImprovementPlans
        if (SupportProject?.ImprovementPlans != null)
        {
            Objectives = SupportProject.ImprovementPlans.Objectives;
        }

        return Page();
    }
}