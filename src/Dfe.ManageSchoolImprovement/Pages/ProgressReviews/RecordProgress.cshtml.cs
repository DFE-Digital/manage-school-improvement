using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.AddImprovementPlanObjectiveProgress;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class RecordProgressModel(
    ISupportProjectQueryService supportProjectQueryService,
    IMediator mediator,
    ErrorService errorService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty]
    public int ReviewId { get; set; }

    [BindProperty]
    public int? NextObjectiveId { get; set; }

    public ImprovementPlanObjectiveViewModel Objective { get; private set; }
    public ImprovementPlanReviewViewModel Review { get; private set; }
    public ImprovementPlanViewModel ImprovementPlan { get; private set; }

    [BindProperty]
    [Required(ErrorMessage = "Select how the school is progressing with this objective")]
    public string ProgressStatus { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Enter details about how the school is progressing with this objective")]
    public string ProgressDetails { get; set; } = string.Empty;

    public string ObjectiveTitle { get; set; } = string.Empty;
    public string AreaOfImprovement { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
    public string ReviewerName { get; set; } = string.Empty;

    public bool ShowSkipObjectiveLink { get; set; } = false;

    public string? ProgressStatusErrorMessage { get; set; } = null;
    public bool ShowProgressStatusError => ModelState.ContainsKey(nameof(ProgressStatus)) && ModelState[nameof(ProgressStatus)]?.Errors.Count > 0;
    public IList<RadioButtonsLabelViewModel> ProgressRadioButtons { get; set; } = [];

    public bool ShowError => _errorService.HasErrors();
    public bool ShowDetailsError => ModelState.ContainsKey(nameof(ProgressDetails)) && ModelState[nameof(ProgressDetails)]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, int? objectiveId, string? returnPage, bool? enableSkip, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.OverallProgress.Page;

        await base.GetSupportProject(id, cancellationToken);

        // using readableId here as they are pastd through the url
        LoadPageData(reviewId, objectiveId);
        SetupProgressRadioButtons();

        ShowSkipObjectiveLink = enableSkip ?? false;
        return Page();
    }

    private void LoadPageData(int reviewId, int? objectiveId)
    {
        ReviewId = reviewId;
        ImprovementPlan = SupportProject?.ImprovementPlans?.First(x => x.ImprovementPlanReviews.Any(x => x.ReadableId == reviewId));
        Review = ImprovementPlan?.ImprovementPlanReviews.Single(x => x.ReadableId == reviewId);

        var areaConfigurations = new Dictionary<string, int>
            {
                { "Quality of education", 1 },
                { "Leadership and management", 2 },
                { "Behaviour and attitudes", 3 },
                { "Attendance", 4 },
                { "Personal development", 5 }
            };

        var objectives = ImprovementPlan.ImprovementPlanObjectives
            .Where(o => areaConfigurations.ContainsKey(o.AreaOfImprovement))
            .GroupBy(o => o.AreaOfImprovement)
            .OrderBy(group => areaConfigurations[group.Key])
            .SelectMany(group => group.OrderBy(o => o.Order))
            .ToList();

        if (objectiveId == null)
        {

            Objective = objectives?.First();
            NextObjectiveId = objectives?[1].ReadableId;
        }
        else
        {
            Objective = objectives?.Single(x => x.ReadableId == objectiveId);

            // Find the current objective's index and get the next one
            var currentIndex = objectives?.FindIndex(x => x.ReadableId == objectiveId) ?? -1;
            var nextIndex = currentIndex + 1;

            NextObjectiveId = (objectives?.Count > nextIndex) ? objectives?[nextIndex].ReadableId : null;
        }

        ObjectiveTitle = Objective.Details;
        AreaOfImprovement = Objective.AreaOfImprovement;
        ReviewDate = Review.ReviewDate;
        ReviewerName = Review.Reviewer;
    }

    public async Task<IActionResult> OnPostAsync(int id, int reviewId, int? objectiveId, string? returnPage, bool? enableSkip, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.OverallProgress.Page;

        await base.GetSupportProject(id, cancellationToken);
        SetupProgressRadioButtons();
        LoadPageData(reviewId, objectiveId);
        ShowSkipObjectiveLink = enableSkip ?? false;

        if (!ModelState.IsValid)
        {
            if (ShowProgressStatusError)
            {
                ProgressStatusErrorMessage = "Select how the school is progressing with this objective";
                _errorService.AddError(ProgressRadioButtons.First().Id, ProgressStatusErrorMessage);
            }

            _errorService.AddErrors(Request.Form.Keys, ModelState);
            return Page();
        }

        var result = await mediator.Send(new AddImprovementPlanObjectiveProgressCommand(
            new SupportProjectId(id),
            new ImprovementPlanId(ImprovementPlan.Id),
            new ImprovementPlanReviewId(Review.Id),
            new ImprovementPlanObjectiveId(Objective.Id),
            ProgressStatus!, ProgressDetails), cancellationToken);

        if (result == null)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        // Check if there are more objectives to review
        if (NextObjectiveId.HasValue && returnPage != Links.ProgressReviews.ProgressSummary.Page)
        {
            // Redirect to the next objective
            return RedirectToPage(Links.ProgressReviews.RecordProgress.Page, new { id, reviewId, objectiveId = NextObjectiveId, enableSkip });
        }
        else
        {
            // All objectives completed, redirect to summary
            return RedirectToPage(returnPage, new { id, reviewId });
        }
    }



    private void SetupProgressRadioButtons()
    {
        ProgressRadioButtons = new List<RadioButtonsLabelViewModel>
        {
            new() {
                Id = "complete",
                Name = "Complete",
                Value = "Complete"
            },
            new() {
                Id = "ahead-of-schedule",
                Name = "Ahead of schedule",
                Value = "Ahead of schedule"
            },
            new() {
                Id = "on-schedule",
                Name = "On schedule",
                Value = "On schedule"
            },
            new() {
                Id = "behind-schedule",
                Name = "Behind schedule",
                Value = "Behind schedule"
            },
            new() {
                Id = "not-started",
                Name = "Not started",
                Value = "Not started"
            },
        };
    }
}