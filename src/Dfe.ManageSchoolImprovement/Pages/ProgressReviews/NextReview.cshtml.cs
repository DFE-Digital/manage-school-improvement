using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.SetImprovementPlanNextReviewDate;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class NextReviewModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty]
    public int ReviewId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Select if another review is needed")]
    public string? IsAnotherReviewNeeded { get; set; }
    public string? IsAnotherReviewNeededErrorMessage { get; set; } = null;

    [BindProperty(Name = "NextReviewDate")]
    [ModelBinder(BinderType = typeof(DateInputModelBinder))]
    public DateTime? NextReviewDate { get; set; }

    public bool ShowIsAnotherReviewNeededError => ModelState.ContainsKey(nameof(IsAnotherReviewNeeded)) && ModelState[nameof(IsAnotherReviewNeeded)]?.Errors.Count > 0;
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        ReturnPage = Links.ProgressReviews.Index.Page;
        ReviewId = reviewId;

        await base.GetSupportProject(id, cancellationToken);


        // Get the improvement plan and review from the support project
        var improvementPlan = SupportProject?.ImprovementPlans?.First(x => x.ImprovementPlanReviews.Any(x => x.ReadableId == ReviewId));
        var review = improvementPlan?.ImprovementPlanReviews.Single(x => x.ReadableId == ReviewId);

        if (review != null && review.NextReviewDate.HasValue)
        {
            IsAnotherReviewNeeded = "yes";
            NextReviewDate = review.NextReviewDate;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Additional validation for date when "Yes" is selected
        if (IsAnotherReviewNeeded == "yes" && !NextReviewDate.HasValue)
        {
            ModelState.AddModelError(nameof(NextReviewDate), "Enter a date for the next review");
        }

        // Get the improvement plan and review from the support project
        var improvementPlan = SupportProject?.ImprovementPlans?.First(x => x.ImprovementPlanReviews.Any(x => x.ReadableId == ReviewId));
        var review = improvementPlan?.ImprovementPlanReviews.Single(x => x.ReadableId == ReviewId);

        if (improvementPlan == null || review == null)
        {
            _errorService.AddApiError();
            return Page();
        }

        if (NextReviewDate <= review.ReviewDate)
        {
            ModelState.AddModelError(nameof(NextReviewDate), "Enter a valid date for the next review, it should be after the date of the most recently added review");
        }

        if (!ModelState.IsValid)
        {
            if (ShowIsAnotherReviewNeededError)
            {
                IsAnotherReviewNeededErrorMessage = "Select if another review is needed";
                _errorService.AddError("yes", IsAnotherReviewNeededErrorMessage);
            }

            _errorService.AddErrors(Request.Form.Keys, ModelState);
            return Page();
        }





        var result = await mediator.Send(new SetImprovementPlanNextReviewDateCommand(
            new SupportProjectId(id),
            new ImprovementPlanId(improvementPlan.Id),
            new ImprovementPlanReviewId(review.Id),
            IsAnotherReviewNeeded == "yes" ? NextReviewDate : null), cancellationToken);

        if (result == null)
        {
            _errorService.AddApiError();
            return Page();
        }

        // Redirect back to the progress reviews index
        return RedirectToPage(Links.ProgressReviews.Index.Page, new { id });
    }



    // Implementation of IDateValidationMessageProvider
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    
    string IDateValidationMessageProvider.AllMissing => "Enter a date";
}