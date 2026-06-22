using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews.RecordProgressSchoolsProgressReviews;

public class ProgressSummaryModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService)
    : BaseImprovementPlanPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty]
    public int ReviewId { get; set; }

    public ProgressReviewViewModel? Review { get; private set; }
    

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, CancellationToken cancellationToken, string? returnPage)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;
        ReviewId = reviewId;

        await base.GetSupportProject(id, cancellationToken);

        Review = SupportProject?.ProgressReviews?.OrderByDescending(x => x.Order).FirstOrDefault();

        return Page();
    }
}