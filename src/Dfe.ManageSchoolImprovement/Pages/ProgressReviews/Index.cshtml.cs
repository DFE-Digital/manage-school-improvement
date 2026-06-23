using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService)
    : BaseImprovementPlanPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;
    
    public List<ImprovementPlanReviewViewModel> ImprovementPlanProgressReviews { get; set; } = [];
    
    public List<ProgressReviewViewModel> ReviewProgressProgressReviews { get; set; } = [];
    
    public List<AllProgressReviewsViewModel> AllProgressReviews { get; set; } = [];
    
    public bool NoProgressReviewsRecorded => AllProgressReviews.Count == 0;

    public AllProgressReviewsViewModel? CurrentProgressReview { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        // Set the return page to the improvement plan tab
        ReturnPage = Links.ImprovementPlan.RecordProgress.Page;

        await base.GetSupportProject(id, cancellationToken);
        
        ImprovementPlanProgressReviews = SupportProject?.ImprovementPlans?.FirstOrDefault()?.ImprovementPlanReviews
            .OrderByDescending(x => x.Order).ToList() ?? [];
        
        ReviewProgressProgressReviews = SupportProject?.ProgressReviews?.OrderByDescending(x => x.Order).ToList() ?? [];
        
        foreach (var review in ImprovementPlanProgressReviews)
        {
            var allProgressReview = AllProgressReviewsViewModel.Create(review, review.ProgressStatusClass, review.ProgressStatus);
            AllProgressReviews.Add(allProgressReview);
        }

        foreach (var review in ReviewProgressProgressReviews)
        {
            var allProgressReview = AllProgressReviewsViewModel.Create(review, review.ProgressStatusClass, review.ProgressStatus);
            AllProgressReviews.Add(allProgressReview);
        }
        
        SetCurrentProgressReview();

        return Page();
    }

    private void SetCurrentProgressReview()
    {
        if (AllProgressReviews.Any())
        {
            // Get the most recent review by review date
            CurrentProgressReview = AllProgressReviews
                .OrderByDescending(review => review.ReviewDate)
                .FirstOrDefault();
        }
    }
}