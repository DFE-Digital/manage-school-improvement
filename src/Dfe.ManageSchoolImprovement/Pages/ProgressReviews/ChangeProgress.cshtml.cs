using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.
    SetImprovementPlanObjectiveProgressDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class ChangeProgressModel(
    ISupportProjectQueryService supportProjectQueryService,
    IMediator mediator,
    ErrorService errorService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    public ImprovementPlanObjectiveViewModel? Objective { get; private set; }
    public ImprovementPlanObjectiveProgressViewModel? ObjectiveProgress { get; private set; }
    public ImprovementPlanReviewViewModel? ImprovementPlanReview { get; private set; }

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

    public ImprovementPlanId? ImprovementPlanId { get; set; }

    public string? ProgressStatusErrorMessage { get; set; } = null;

    public bool ShowProgressStatusError => ModelState.ContainsKey(nameof(ProgressStatus)) &&
                                           ModelState[nameof(ProgressStatus)]?.Errors.Count > 0;

    public IList<RadioButtonsLabelViewModel> ProgressRadioButtons { get; set; } = [];

    public bool ShowError => _errorService.HasErrors();

    public bool ShowDetailsError => ModelState.ContainsKey(nameof(ProgressDetails)) &&
                                    ModelState[nameof(ProgressDetails)]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, int objectiveProgressId, string? returnPage,
        CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.ProgressSummary.Page;


        await base.GetSupportProject(id, cancellationToken);

        // using readableId here as they are pastd through the url
        LoadPageData(objectiveProgressId);
        SetupProgressRadioButtons();

        ProgressStatus = ObjectiveProgress?.HowIsSchoolProgressing ?? string.Empty;
        ProgressDetails = ObjectiveProgress?.ProgressDetails ?? string.Empty;

        return Page();
    }

    private void LoadPageData(int objectiveProgressId)
    {
        if (SupportProject == null) return;

        // Find the objective progress across all improvement plans and reviews
        ObjectiveProgress = SupportProject.ImprovementPlans?
            .SelectMany(plan => plan.ImprovementPlanReviews)
            .SelectMany(review => review.ImprovementPlanObjectiveProgresses)
            .FirstOrDefault(progress => progress.ReadableId == objectiveProgressId);

        if (ObjectiveProgress == null) return;

        ImprovementPlanReview = SupportProject.ImprovementPlans?
            .SelectMany(plan => plan.ImprovementPlanReviews)
            .FirstOrDefault(review => review.Id == ObjectiveProgress.ImprovementPlanReviewId);

        Objective = SupportProject.ImprovementPlans?
            .SelectMany(plan => plan.ImprovementPlanObjectives)
            .FirstOrDefault(obj => obj.Id == ObjectiveProgress.ImprovementPlanObjectiveId);

        if (ImprovementPlanReview != null && Objective != null)
        {
            ObjectiveTitle = Objective.Details;
            AreaOfImprovement = Objective.AreaOfImprovement;
            ReviewDate = ImprovementPlanReview.ReviewDate;
            ReviewerName = ImprovementPlanReview.Reviewer;
            ImprovementPlanId = new ImprovementPlanId(ImprovementPlanReview.ImprovementPlanId);
        }
    }

    public async Task<IActionResult> OnPostAsync(int id, int objectiveProgressId, string? returnPage,
        CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.ProgressSummary.Page;

        await base.GetSupportProject(id, cancellationToken);
        SetupProgressRadioButtons();
        LoadPageData(objectiveProgressId);

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

        var result = await mediator.Send(new SetImprovementPlanObjectiveProgressDetailsCommand(
            new SupportProjectId(id),
            ImprovementPlanId,
            new ImprovementPlanReviewId(ImprovementPlanReview.Id),
            new ImprovementPlanObjectiveProgressId(ObjectiveProgress.Id),
            ProgressStatus!, ProgressDetails), cancellationToken);

        if (result == null)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        // All objectives completed, redirect to summary
        return RedirectToPage(ReturnPage, new { id, reviewId = ImprovementPlanReview.ReadableId });
    }


    private void SetupProgressRadioButtons()
    {
        ProgressRadioButtons = new List<RadioButtonsLabelViewModel>
        {
            new()
            {
                Id = "red",
                Name = "Red",
                Value = "Red",
                Hint =
                    "Improvement activities are off track and short-term or end-of-programme changes are unlikely without major intervention"
            },
            new()
            {
                Id = "amber",
                Name = "Amber",
                Value = "Amber",
                Hint =
                    "Improvement activities require attention. Short-term or end-of-programme changes are at risk but achievable with timely action"
            },
            new()
            {
                Id = "green",
                Name = "Green",
                Value = "Green",
                Hint =
                    "Improvement activities are on track and both short-term and end-of-programme changes are likely to be achieved"
            },
        };
    }
}