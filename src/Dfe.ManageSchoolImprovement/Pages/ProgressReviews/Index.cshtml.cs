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

        ProgressReviews = SupportProject?.ImprovementPlans?.FirstOrDefault()?.ImprovementPlanReviews.OrderByDescending(x => x.ReviewDate).ToList() ?? [];
        SetCurrentProgressReview();
        ComputeReviewTitles();

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

    private void ComputeReviewTitles()
    {
        ReviewTitleByReadableId = ProgressReviews
            .OrderBy(r => r.ReviewDate)
            .Select((r, index) => new { r.ReadableId, Title = $"{ToOrdinalWord(index + 1)} review" })
            .ToDictionary(x => x.ReadableId, x => x.Title);
    }

    private static string ToOrdinalWord(int number)
    {
        // Common words for first few ordinals; fallback to numeric suffix for larger numbers
        return number switch
        {
            1 => "First",
            2 => "Second",
            3 => "Third",
            4 => "Fourth",
            5 => "Fifth",
            6 => "Sixth",
            7 => "Seventh",
            8 => "Eighth",
            9 => "Ninth",
            10 => "Tenth",
            11 => "Eleventh",
            12 => "Twelfth",
            _ => ToOrdinal(number)
        };
    }

    private static string ToOrdinal(int number)
    {
        int abs = Math.Abs(number);
        int lastTwo = abs % 100;
        string suffix = (lastTwo is 11 or 12 or 13) ? "th" : (abs % 10) switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };
        return $"{number}{suffix}";
    }
}
