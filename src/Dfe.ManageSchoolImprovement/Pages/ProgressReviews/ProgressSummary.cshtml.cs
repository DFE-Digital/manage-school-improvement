using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class ProgressSummaryModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty]
    public int ReviewId { get; set; }

    public ImprovementPlanReviewViewModel Review { get; private set; }
    public ImprovementPlanViewModel ImprovementPlan { get; private set; }

    public List<ObjectiveProgressGroup> ObjectiveProgressGroups { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        ReturnPage = Links.ProgressReviews.Index.Page;
        ReviewId = reviewId;

        await base.GetSupportProject(id, cancellationToken);

        LoadPageData();

        return Page();
    }

    private void LoadPageData()
    {
        ImprovementPlan = SupportProject?.ImprovementPlans?.First(x => x.ImprovementPlanReviews.Any(x => x.ReadableId == ReviewId));
        Review = ImprovementPlan?.ImprovementPlanReviews.Single(x => x.ReadableId == ReviewId);

        if (ImprovementPlan?.ImprovementPlanObjectives != null)
        {
            // Define areas with display order and any additional metadata
            var areaConfigurations = new Dictionary<string, int>
            {
                { "Quality of education", 1 },
                { "Leadership and management", 2 },
                { "Behaviour and attitudes", 3 },
                { "Attendance", 4 },
                { "Personal development", 5 }
            };

            ObjectiveProgressGroups = ImprovementPlan.ImprovementPlanObjectives
                .Where(o => areaConfigurations.ContainsKey(o.AreaOfImprovement))
                .GroupBy(o => o.AreaOfImprovement)
                .OrderBy(group => areaConfigurations[group.Key])
                .SelectMany(group => BuildObjectiveProgressGroups(group.OrderBy(o => o.Order).ToList(), Review.Id))
                .ToList();
        }
    }

    private List<ObjectiveProgressGroup> BuildObjectiveProgressGroups(List<ImprovementPlanObjectiveViewModel> improvementPlanObjectives, Guid reviewId)
    {
        return improvementPlanObjectives
            .GroupBy(obj => obj.AreaOfImprovement)
            .Select(group => new ObjectiveProgressGroup
            {
                AreaOfImprovement = group.Key,
                ObjectiveProgresses = group.OrderBy(obj => obj.Order).Select(obj =>
                {
                    var objectiveProgress = Review?.ImprovementPlanObjectiveProgresses?
                        .FirstOrDefault(op => op.ImprovementPlanObjectiveId == obj.Id);

                    return new ObjectiveProgressViewModel
                    {
                        Id = objectiveProgress?.Id ?? Guid.Empty,
                        ReadableId = objectiveProgress?.ReadableId ?? 0,
                        ObjectiveReadableId = obj.ReadableId,
                        Title = obj.Details,
                        Progress = objectiveProgress?.HowIsSchoolProgressing ?? "No progress recorded yet",
                        Details = objectiveProgress?.ProgressDetails ?? "No details recorded yet"
                    };
                }).ToList()
            })
            .OrderBy(group => group.AreaOfImprovement)
            .ToList();
    }
}