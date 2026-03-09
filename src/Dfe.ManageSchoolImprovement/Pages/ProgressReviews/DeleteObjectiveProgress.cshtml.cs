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

public class DeleteObjectiveProgressModel(
    ISupportProjectQueryService supportProjectQueryService,
    IMediator mediator,
    ErrorService errorService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    [BindProperty]
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
    
    public int? ObjectiveProgressId { get; set; }
    
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int objectiveProgressId, string? returnPage,
        CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ImprovementPlan.ImprovementPlanTab.Page;


        await base.GetSupportProject(id, cancellationToken);

        // using readableId here as they are pastd through the url
        LoadPageData(objectiveProgressId);

        ProgressStatus = ObjectiveProgress?.HowIsSchoolProgressing ?? string.Empty;
        var details = ObjectiveProgress?.ProgressDetails ?? string.Empty;
        ProgressDetails = details.Length > 200 ? details[..200] + "…" : details;
        ObjectiveProgressId = ObjectiveProgress?.ReadableId;

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

    public async Task<IActionResult> OnPostAsync(int id, int objectiveProgressId,
        CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        LoadPageData(objectiveProgressId);

        // if (!ModelState.IsValid)
        // {
        //     _errorService.AddErrors(Request.Form.Keys, ModelState);
        //     return Page();
        // }

        // var result = await mediator.Send(new SetImprovementPlanObjectiveProgressDetailsCommand(
        //     new SupportProjectId(id),
        //     ImprovementPlanId,
        //     new ImprovementPlanReviewId(ImprovementPlanReview.Id),
        //     new ImprovementPlanObjectiveProgressId(ObjectiveProgress.Id),
        //     ProgressStatus!, ProgressDetails), cancellationToken);

        // if (result == null)
        // {
        //     _errorService.AddApiError();
        //     return await base.GetSupportProject(id, cancellationToken);
        // }

        var targetPage = string.IsNullOrEmpty(ReturnPage)
            ? Links.ImprovementPlan.ImprovementPlanTab.Page
            : ReturnPage;

        if (Links.ProgressReviews.ProgressSummary.Page.Equals(targetPage, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToPage(targetPage, new { id, reviewId = ImprovementPlanReview?.ReadableId });
        }

        return RedirectToPage(targetPage, new { id });
    }
    
}
