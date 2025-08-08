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
            // Group objectives by area of improvement
            ObjectiveProgressGroups = ImprovementPlan.ImprovementPlanObjectives
                .GroupBy(obj => obj.AreaOfImprovement)
                .Select(group => new ObjectiveProgressGroup
                {
                    AreaOfImprovement = group.Key,
                    Objectives = group.OrderBy(obj => obj.Order).Select(obj => new ObjectiveProgressViewModel
                    {
                        Id = obj.Id,
                        ReadableId = obj.ReadableId,
                        Title = obj.Details,
                        Progress = GetObjectiveProgress(obj.Id),
                        Details = GetObjectiveProgressDetails(obj.Id)
                    }).ToList()
                })
                .OrderBy(group => group.AreaOfImprovement)
                .ToList();
        }
    }
    
    private string GetObjectiveProgress(Guid objectiveId)
    {
        // For now return mock data - replace with actual progress lookup
        var mockProgress = new Dictionary<string, string>
        {
            { "Quality of education", "Progressing well" },
            { "Leadership and management", "Not progressing as required" }
        };
        
        var objective = ImprovementPlan?.ImprovementPlanObjectives?.FirstOrDefault(o => o.Id == objectiveId);
        if (objective != null && mockProgress.ContainsKey(objective.AreaOfImprovement))
        {
            return mockProgress[objective.AreaOfImprovement];
        }
        
        return "Complete"; // Default for other areas
    }
    
    private string GetObjectiveProgressDetails(Guid objectiveId)
    {
        // For now return mock data - replace with actual details lookup
        var mockDetails = new Dictionary<string, string>
        {
            { "Get better books", "Going along ok so far" },
            { "Teach better lessons", "Yep. Doing well." },
            { "Be better leaders", "This one still needs some work" },
            { "Do better management", "They've smashed this one out of the park" }
        };
        
        var objective = ImprovementPlan?.ImprovementPlanObjectives?.FirstOrDefault(o => o.Id == objectiveId);
        if (objective != null && mockDetails.ContainsKey(objective.Details))
        {
            return mockDetails[objective.Details];
        }
        
        return "No details provided";
    }
}

public class ObjectiveProgressGroup
{
    public string AreaOfImprovement { get; set; } = string.Empty;
    public List<ObjectiveProgressViewModel> Objectives { get; set; } = [];
}

public class ObjectiveProgressViewModel
{
    public Guid Id { get; set; }
    public int ReadableId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Progress { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
} 