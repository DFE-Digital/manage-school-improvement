using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    // Mock data for now - will be replaced with real data model later
    public List<ImprovementPlanReviewViewModel> ProgressReviews { get; set; } = [];

    // For now, we'll show mock data to demonstrate the UI
    public bool NoProgressReviewsRecorded => ProgressReviews.Count == 0;

    public ImprovementPlanReviewViewModel? CurrentProgressReview { get; private set; }

    public Dictionary<int, string> ReviewTitleByReadableId { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        // Set the return page to the improvement plan tab
        ReturnPage = Links.ImprovementPlan.ImprovementPlanTab.Page;

        await base.GetSupportProject(id, cancellationToken);

        ProgressReviews = SupportProject?.ImprovementPlans?.FirstOrDefault()?.ImprovementPlanReviews.OrderByDescending(x => x.Order).ToList() ?? [];
        SetCurrentProgressReview();

        return Page();
    }

    private void SetCurrentProgressReview()
    {
        if (ProgressReviews.Any())
        {
            // Get the most recent review by review date
            CurrentProgressReview = ProgressReviews
                .OrderByDescending(review => review.ReviewDate)
                .FirstOrDefault();
        }
    }
}
