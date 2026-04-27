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

    public bool CompletedReviews =>
        ImprovementPlan.ImprovementPlanReviews.Any(x =>
            x.ProgressStatus == ImprovementPlanReviewViewModel.ProgressStatusRecorded);

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, CancellationToken cancellationToken, string? returnPage)
    {
        ReturnPage = Links.ProgressReviews.Index.Page;
        ReviewId = reviewId;

        await base.GetSupportProject(id, cancellationToken);

        LoadPageData();

        return Page();
    }

    private void LoadPageData()
    {
        ImprovementPlan = SupportProject?.ImprovementPlans?
            .First(x => x.ImprovementPlanReviews.Any(r => r.ReadableId == ReviewId));

        Review = ImprovementPlan?.ImprovementPlanReviews
            .Single(x => x.ReadableId == ReviewId);

        if (ImprovementPlan?.ImprovementPlanObjectives == null || Review == null)
            return;

        var areaConfigurations = new Dictionary<string, int>
        {
            { "Quality of education", 1 },
            { "Leadership and management", 2 },
            { "Behaviour and attitudes", 3 },
            { "Attendance", 4 },
            { "Personal development", 5 }
        };

        //  Build lookup once 
        var progresses = Review.ImprovementPlanObjectiveProgresses;
        var progressLookup = new Dictionary<Guid, ImprovementPlanObjectiveProgressViewModel>(
            progresses?.Count ?? 0);

        if (progresses is not null)
        {
            foreach (var p in progresses)
            {
                // overwrite
                progressLookup[p.ImprovementPlanObjectiveId] = p;
            }
        }

        //  Single pass grouping
        ObjectiveProgressGroups = ImprovementPlan.ImprovementPlanObjectives
            .Where(o => areaConfigurations.ContainsKey(o.AreaOfImprovement))
            .GroupBy(o => o.AreaOfImprovement)
            .OrderBy(g => areaConfigurations[g.Key])
            .Select(group => new ObjectiveProgressGroup
            {
                AreaOfImprovement = group.Key,

                ObjectiveProgresses = group
                    .OrderBy(o => o.Order)
                    .Select(obj =>
                    {
                        progressLookup.TryGetValue(obj.Id, out var progress);

                        return new ObjectiveProgressViewModel
                        {
                            Id = progress?.Id ?? Guid.Empty,
                            ReadableId = progress?.ReadableId ?? 0,
                            ObjectiveReadableId = obj.ReadableId,
                            Title = obj.Details,
                            Progress = progress?.HowIsSchoolProgressing ?? "No progress recorded yet",
                            Details = progress?.ProgressDetails ?? "No details recorded yet"
                        };
                    })
                    .ToList()
            })
            .ToList();
    }
}