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
    public ImprovementPlanReviewViewModel? CurrentReview { get; set; }
    public bool IsAdviserAllocated { get; set; }

    public bool NoObjectivesRecorded => ImprovementPlan?.ImprovementPlanObjectives == null || ImprovementPlan.ImprovementPlanObjectives.Count == 0;
    public bool ObjectivesComplete => ImprovementPlan?.ObjectivesSectionComplete ?? false;

    public List<ObjectiveProgressGroup> ObjectiveProgressGroups { get; set; } = [];

    public List<ImprovementPlanObjectiveViewModel> QualityOfEducationObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Quality of education").OrderBy(o => o.Order).ToList() ?? new();

    public List<ImprovementPlanObjectiveViewModel> LeadershipAndManagementObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Leadership and management").OrderBy(o => o.Order).ToList() ?? new();

    public List<ImprovementPlanObjectiveViewModel> BehaviourAndAttitudesObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Behaviour and attitudes").OrderBy(o => o.Order).ToList() ?? new();

    public List<ImprovementPlanObjectiveViewModel> AttendanceObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Attendance").OrderBy(o => o.Order).ToList() ?? new();

    public List<ImprovementPlanObjectiveViewModel> PersonalDevelopmentObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Personal development").OrderBy(o => o.Order).ToList() ?? new();

    public bool NoProgressReviewsRecorded =>
        ImprovementPlan?.ImprovementPlanReviews == null || ImprovementPlan.ImprovementPlanReviews.Count == 0;
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        LoadPageData();

        return Page();
    }

    private void LoadPageData()
    {
        // Ensure SupportProject is not null before accessing ImprovementPlans
        if (SupportProject?.ImprovementPlans != null)
        {
            ImprovementPlan = SupportProject.ImprovementPlans.FirstOrDefault();
            IsAdviserAllocated = SupportProject.AdviserEmailAddress != null;
            CurrentReview = ImprovementPlan?.ImprovementPlanReviews.OrderByDescending(x => x.Order).FirstOrDefault();

            if (CurrentReview != null && ImprovementPlan?.ImprovementPlanObjectives != null)
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
                    .SelectMany(group => BuildObjectiveProgressGroups(group.OrderBy(o => o.Order).ToList(), CurrentReview.Id))
                    .ToList();
            }
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
                    var objectiveProgress = CurrentReview?.ImprovementPlanObjectiveProgresses?
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