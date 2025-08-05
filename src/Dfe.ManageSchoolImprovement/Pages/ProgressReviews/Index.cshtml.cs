using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) 
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;
    
    // For now, we'll assume there are no progress reviews until the data model is implemented
    public bool NoProgressReviewsRecorded => true;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        // Set the return page to the improvement plan tab
        ReturnPage = Links.ImprovementPlan.ImprovementPlanTab.Page;
        
        await base.GetSupportProject(id, cancellationToken);
        
        return Page();
    }
} 