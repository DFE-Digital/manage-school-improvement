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

public class RecordProgressModel(
    ISupportProjectQueryService supportProjectQueryService,
    IMediator mediator,
    ErrorService errorService)
    : BaseImprovementPlanPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty] public int ReviewId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Select next steps")]
    public string NextSteps { get; set; } = string.Empty;

    [BindProperty]
    public string AdditionalDetails { get; set; } = string.Empty;
    
    private ProgressReviewViewModel? ProgressReview { get; set; }

    public string? NextStepsErrorMessage { get; set; } = null;

    public bool ShowNextStepsError => ModelState.ContainsKey(nameof(NextSteps)) &&
                                           ModelState[nameof(NextSteps)]?.Errors.Count > 0;

    public IList<RadioButtonsLabelViewModel> NextStepsRadioButtons { get; set; } = [];

    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, string? returnPage,
        CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        ReviewId = reviewId;
        
        ProgressReview = SupportProject?.ProgressReviews?.SingleOrDefault(x => x.ReadableId == ReviewId);
        
        SetupNextStepsRadioButtons();

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id, int reviewId, string? returnPage,
        bool? enableSkip, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        ProgressReview = SupportProject?.ProgressReviews?.SingleOrDefault(x => x.ReadableId == ReviewId);
        
        SetupNextStepsRadioButtons();

        if (string.IsNullOrWhiteSpace(NextSteps))
        {
            if (ShowNextStepsError)
            {
                NextStepsErrorMessage = "Select next steps";
                _errorService.AddError(NextStepsRadioButtons.First().Id, NextStepsErrorMessage);
            }

            return Page();
        }
        
        var result = await mediator.Send(new SetProgressDetailsCommand(
            new SupportProjectId(id),
            new ProgressReviewId(ProgressReview.Id),
            NextSteps,
            AdditionalDetails), cancellationToken);
        
        if (result == null)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        if (returnPage is not null)
        {
            return RedirectToPage(returnPage, new { id, reviewId });
        }
        
        return RedirectToPage(Links.ProgressReviews.ViewProgressReview.Page, new { id, reviewId });
    }


    private void SetupNextStepsRadioButtons()
    {
        NextStepsRadioButtons = new List<RadioButtonsLabelViewModel>
        {
            new() {
                Id = "review-school-progress",
                Name = "Continue to review progress",
                Value = "Continue to review progress"
            },
            new() {
                Id = "match-with-organisation",
                Name = "Match with a supporting organisation",
                Value = "Match with a supporting organisation"
            }
        };
    }
}