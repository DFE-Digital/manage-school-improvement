using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlansReviews;
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
    
    public string ProgressStatus { get; set; } = string.Empty;
    
    public string ProgressDetails { get; set; } = string.Empty;

    public string ObjectiveTitle { get; set; } = string.Empty;

    public ImprovementPlanId? ImprovementPlanId { get; set; }

    public int? ObjectiveProgressId { get; set; }
    
    public int? ImprovementPlanReviewId { get; set; }
    
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int objectiveProgressId, string? returnPage,
        CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ImprovementPlan.ImprovementPlanTab.Page;


        await base.GetSupportProject(id, cancellationToken);
        
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
            ImprovementPlanId = new ImprovementPlanId(ImprovementPlanReview.ImprovementPlanId);
            ImprovementPlanReviewId = ImprovementPlanReview.ReadableId;
        }
    }

    public async Task<IActionResult> OnPostAsync(int id, int objectiveProgressId,
        CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        LoadPageData(objectiveProgressId);
        
        ObjectiveProgressId = ObjectiveProgress?.ReadableId;

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            return Page();
        }

        var result = await mediator.Send(new DeleteImprovementPlanObjectiveProgress.DeleteImprovementPlanObjectiveProgressCommand(
            new SupportProjectId(id),
            ImprovementPlanId,
            new ImprovementPlanReviewId(ImprovementPlanReview.Id),
            new ImprovementPlanObjectiveProgressId(ObjectiveProgress.Id),
            User.Identity?.Name!), cancellationToken);

        if (result == null)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        TempData["reviewDeleted"] = true;

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
