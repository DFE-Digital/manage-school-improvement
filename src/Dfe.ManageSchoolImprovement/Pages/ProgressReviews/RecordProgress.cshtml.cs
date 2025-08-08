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
    public string ProgressDetails { get; set; } = string.Empty;

    public string ObjectiveTitle { get; set; } = string.Empty;
    public string AreaOfImprovement { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
    public string ReviewerName { get; set; } = string.Empty;

    public string? ProgressStatusErrorMessage { get; set; } = null;
    public bool ShowProgressStatusError => ModelState.ContainsKey(nameof(ProgressStatus)) && ModelState[nameof(ProgressStatus)]?.Errors.Count > 0;
    public IList<RadioButtonsLabelViewModel> ProgressRadioButtons { get; set; } = [];

    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, int? objectiveId, CancellationToken cancellationToken)
    {
        ReturnPage = Links.ProgressReviews.Index.Page;


        await base.GetSupportProject(id, cancellationToken);

        // using readableId here as they are pastd through the url
        LoadPageData(reviewId, objectiveId);
        SetupProgressRadioButtons();

        return Page();
    }

    private void LoadPageData(int reviewId, int? objectiveId)
    {
        ReviewId = reviewId;
        ImprovementPlan = SupportProject?.ImprovementPlans?.First(x => x.ImprovementPlanReviews.Any(x => x.ReadableId == reviewId));
        Review = ImprovementPlan?.ImprovementPlanReviews.Single(x => x.ReadableId == reviewId);
        var objectives = ImprovementPlan?.ImprovementPlanObjectives.OrderBy(x => x.AreaOfImprovement).ThenBy(x => x.Order).ToList();

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

    public async Task<IActionResult> OnPostAsync(int id, int reviewId, int? objectiveId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        SetupProgressRadioButtons();
        LoadPageData(reviewId, objectiveId);

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
         if (NextObjectiveId.HasValue)
         {
             // Redirect to the next objective
             return RedirectToPage(Links.ProgressReviews.RecordProgress.Page, new { id, reviewId, objectiveId = NextObjectiveId });
         }
         else
         {
             // All objectives completed, redirect to summary
             return RedirectToPage(Links.ProgressReviews.ProgressSummary.Page, new { id, reviewId });
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
                Id = "progressing-well",
                Name = "Progressing well",
                Value = "Progressing well"
            },
            new() {
                Id = "not-progressing-as-required",
                Name = "Not progressing as required",
                Value = "Not progressing as required"
            },
            new() {
                Id = "not-started",
                Name = "Not started",
                Value = "Not started"
            },
            new() {
                Id = "review-not-taken-place",
                Name = "Review not taken place",
                Value = "Review not taken place"
            }
        };
    }
}