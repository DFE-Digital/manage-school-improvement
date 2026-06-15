using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.SetImprovementPlanNextReviewDate;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class NextReviewModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseImprovementPlanPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
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
    
    private DateTime? PreviouslyEnteredReviewDate { get; set; }
    private ImprovementPlanViewModel? ImprovementPlan { get; set; }
    private ImprovementPlanReviewViewModel? ImprovementPlanReview { get; set; }
    private ProgressReviewViewModel? ProgressReview { get; set; }

    public bool ShowIsAnotherReviewNeededError => ModelState.ContainsKey(nameof(IsAnotherReviewNeeded)) && ModelState[nameof(IsAnotherReviewNeeded)]?.Errors.Count > 0;
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        ReturnPage = Links.ProgressReviews.Index.Page;
        ReviewId = reviewId;

        await base.GetSupportProjectProgressReviews(id, cancellationToken);

        if (SupportProject != null &&
            SupportProject.InitialDiagnosisMatchingDecision == "Match with supporting organisation")
        {
            // Get the improvement plan and review from the support project
            ImprovementPlan = SupportProject?.ImprovementPlans?.First(x => x.ImprovementPlanReviews.Any(x => x.ReadableId == ReviewId));
            ImprovementPlanReview = ImprovementPlan?.ImprovementPlanReviews.Single(x => x.ReadableId == ReviewId);
            
            if (ImprovementPlanReview != null && ImprovementPlanReview.NextReviewDate.HasValue)
            {
                IsAnotherReviewNeeded = "yes";
                NextReviewDate = ImprovementPlanReview.NextReviewDate;
            }
        }
        else
        {
            ProgressReview = SupportProject?.ProgressReviews?.SingleOrDefault(x => x.ReadableId == ReviewId);
            if (ProgressReview != null && ProgressReview.NextReviewDate.HasValue)
            {
                IsAnotherReviewNeeded = "yes";
                NextReviewDate = ProgressReview.NextReviewDate;
            }
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Additional validation for date when "Yes" is selected
        if (IsAnotherReviewNeeded == "yes" && !NextReviewDate.HasValue)
        {
            ModelState.AddModelError(nameof(NextReviewDate), "Enter a date");
        }

        if (SupportProject != null &&
            SupportProject.InitialDiagnosisMatchingDecision == "Match with supporting organisation")
        {
            // Get the improvement plan and review from the support project
            ImprovementPlan = SupportProject?.ImprovementPlans?.First(x => x.ImprovementPlanReviews.Any(x => x.ReadableId == ReviewId));
            ImprovementPlanReview = ImprovementPlan?.ImprovementPlanReviews.Single(x => x.ReadableId == ReviewId);
            
            PreviouslyEnteredReviewDate = ImprovementPlanReview?.ReviewDate;

            if (ImprovementPlan == null || ImprovementPlanReview == null)
            {
                _errorService.AddApiError();
                return Page();
            }
        }

        if (SupportProject != null &&
            SupportProject.InitialDiagnosisMatchingDecision == "Review school's progress")
        {
            ProgressReview = SupportProject?.ProgressReviews?.SingleOrDefault(x => x.ReadableId == ReviewId);
            PreviouslyEnteredReviewDate = ProgressReview?.ReviewDate;
            
            if (ProgressReview == null)
            {
                _errorService.AddApiError();
                return Page();
            }
        }

        if (NextReviewDate <= PreviouslyEnteredReviewDate)
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


        if (SupportProject.InitialDiagnosisMatchingDecision == "Match with supporting organisation")
        {
            // add check for type of school, call appropriate command
            var result = await mediator.Send(new SetImprovementPlanNextReviewDateCommand(
                new SupportProjectId(id),
                new ImprovementPlanId(ImprovementPlan.Id),
                new ImprovementPlanReviewId(ImprovementPlanReview.Id),
                IsAnotherReviewNeeded == "yes" ? NextReviewDate : null), cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return Page();
            }
        }

        if (SupportProject.InitialDiagnosisMatchingDecision == "Review school's progress")
        {
            var progressReviewResult = await mediator.Send(new SetProgressReviewNextReviewDateCommand(
                new SupportProjectId(id),
                new ProgressReviewId(ProgressReview.Id),
                IsAnotherReviewNeeded == "yes" ? NextReviewDate : null), cancellationToken);

            if (progressReviewResult == null)
            {
                _errorService.AddApiError();
                return Page();
            }
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