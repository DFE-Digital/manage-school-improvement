using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.SetImprovementPlanReviewOverallProgress;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class OverallProgressModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty]
    public int ReviewId { get; set; }

    public ImprovementPlanReviewViewModel Review { get; private set; }
    public ImprovementPlanViewModel ImprovementPlan { get; private set; }

    [BindProperty]
    // [Required(ErrorMessage = "Select how the school is progressing overall")]
    public string OverallProgressStatus { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Enter the next steps")]
    public string OverallProgressDetails { get; set; } = string.Empty;

    public DateTime ReviewDate { get; set; }
    public string ReviewerName { get; set; } = string.Empty;

    // public string? OverallProgressStatusErrorMessage { get; set; } = null;
    // public bool ShowOverallProgressStatusError => ModelState.ContainsKey(nameof(OverallProgressStatus)) && ModelState[nameof(OverallProgressStatus)]?.Errors.Count > 0;
    public IList<RadioButtonsLabelViewModel> OverallProgressRadioButtons { get; set; } = [];

    public bool ShowError => _errorService.HasErrors();
    public bool ShowDetailsError => ModelState.ContainsKey(nameof(OverallProgressDetails)) && ModelState[nameof(OverallProgressDetails)]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, string? returnPage, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        LoadPageData(reviewId);
        SetupOverallProgressRadioButtons();

        OverallProgressStatus = Review.HowIsTheSchoolProgressingOverall;
        OverallProgressDetails = Review.OverallProgressDetails;

        return Page();
    }

    private void LoadPageData(int reviewId)
    {
        ReviewId = reviewId;
        ImprovementPlan = SupportProject?.ImprovementPlans?.First(x => x.ImprovementPlanReviews.Any(x => x.ReadableId == reviewId));
        Review = ImprovementPlan?.ImprovementPlanReviews.Single(x => x.ReadableId == reviewId);

        ReviewDate = Review.ReviewDate;
        ReviewerName = Review.Reviewer;
    }

    public async Task<IActionResult> OnPostAsync(int id, int reviewId, string? returnPage, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        SetupOverallProgressRadioButtons();
        LoadPageData(reviewId);

        if (!ModelState.IsValid)
        {
            // if (ShowOverallProgressStatusError)
            // {
            //     OverallProgressStatusErrorMessage = "Select how the school is progressing overall";
            //     _errorService.AddError(OverallProgressRadioButtons.First().Id, OverallProgressStatusErrorMessage);
            // }

            _errorService.AddErrors(Request.Form.Keys, ModelState);
            return Page();
        }

        var result = await mediator.Send(new SetImprovementPlanReviewOverallProgressCommand(
            new SupportProjectId(id),
            new ImprovementPlanId(ImprovementPlan.Id),
            new ImprovementPlanReviewId(Review.Id),
            OverallProgressStatus, 
            OverallProgressDetails), cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        if (returnPage != null)
        {
            return RedirectToPage(returnPage, new { id, reviewId });

        }

        return RedirectToPage(Links.ProgressReviews.RecordProgress.Page, new { id, reviewId, enableSkip = true });
    }

    private void SetupOverallProgressRadioButtons()
    {
        OverallProgressRadioButtons = new List<RadioButtonsLabelViewModel>
        {
            new() {
                Id = "school-progressing-well",
                Name = "School progressing well",
                Value = "School progressing well"
            },
            new() {
                Id = "school-improved-rise-complete",
                Name = "School has improved/RISE targeted intervention complete",
                Value = "School has improved/RISE targeted intervention complete"
            },
            new() {
                Id = "school-not-improving",
                Name = "School not improving as required",
                Value = "School not improving as required"
            },
            new() {
                Id = "review-not-taken-place",
                Name = "Review not taken place",
                Value = "Review not taken place"
            },
            new() {
                Id = "school-not-engaging",
                Name = "School not engaging",
                Value = "School not engaging"
            },
        };
    }
}