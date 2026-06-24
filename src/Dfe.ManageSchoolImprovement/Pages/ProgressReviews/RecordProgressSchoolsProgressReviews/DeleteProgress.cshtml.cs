using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.
    AddImprovementPlanObjectiveProgress;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews.RecordProgressSchoolsProgressReviews;

public class DeleteProgressModel(
    ISupportProjectQueryService supportProjectQueryService,
    IMediator mediator,
    ErrorService errorService)
    : BaseImprovementPlanPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty] public int ReviewId { get; set; }
    
    public ProgressReviewViewModel? ProgressReview { get; set; }

    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, string? returnPage,
        CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        ReviewId = reviewId;
        
        ProgressReview = SupportProject?.ProgressReviews?.SingleOrDefault(x => x.ReadableId == ReviewId);
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id, int reviewId, string? returnPage,
        bool? enableSkip, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        ProgressReview = SupportProject?.ProgressReviews?.SingleOrDefault(x => x.ReadableId == ReviewId);
        
        var result = await mediator.Send(new DeleteProgressCommand(
            new SupportProjectId(id),
            new ProgressReviewId(ProgressReview.Id)), cancellationToken);
        
        if (result == null)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        if (returnPage is not null)
        {
            return RedirectToPage(returnPage, new { id, reviewId });
        }
        
        return RedirectToPage(Links.ImprovementPlan.Index.Page, new { id, reviewId });
    }
}