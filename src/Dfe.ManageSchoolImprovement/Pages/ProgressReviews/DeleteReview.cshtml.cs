using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.SetImprovementPlanReviewDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class DeleteReviewModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; } = string.Empty;
    public DateTime? ReviewDate { get; set; }

    public Guid ImprovementPlanReviewId { get; set; }
    
    public int ReviewReadableId { get; set; }

    public Guid ImprovementPlanId { get; set; }
    
    public string? ReviewStatus { get; set; }
    
    public string? ReviewStatusClass { get; set; }
    
    public string? ReviewTitle { get; set; }
    
    public string? ReviewerName { get; set; }
    
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        // Set the return page to the progress reviews index
        ReturnPage = Links.ProgressReviews.EditReview.Page;

        await base.GetSupportProject(id, cancellationToken);

        var improvementPlanReview = SupportProject?.ImprovementPlans?.SelectMany(x => x.ImprovementPlanReviews).SingleOrDefault(x => x.ReadableId == reviewId);

        if (improvementPlanReview != null)
        {
            ImprovementPlanId = improvementPlanReview.ImprovementPlanId;
            ImprovementPlanReviewId = improvementPlanReview.Id;
            ReviewDate = improvementPlanReview.ReviewDate;
            ReviewStatus = improvementPlanReview.ProgressStatus;
            ReviewStatusClass = improvementPlanReview.ProgressStatusClass;
            ReviewTitle = improvementPlanReview.Title;
            ReviewerName = improvementPlanReview.Reviewer;
            ReviewReadableId = reviewId;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        //
        // var result = await mediator.Send(new SetImprovementPlanReviewDetailsCommand(new SupportProjectId(id),
        //     new ImprovementPlanId(ImprovementPlanId),
        //     new ImprovementPlanReviewId(ImprovementPlanReviewId),
        //     reviewer ?? string.Empty, ReviewDate!.Value), cancellationToken);

        // get latest version of the support project
        await base.GetSupportProject(id, cancellationToken);

        // For now, redirect back to the progress reviews index
        return RedirectToPage(Links.ProgressReviews.Index.Page, new { id });
    }
}