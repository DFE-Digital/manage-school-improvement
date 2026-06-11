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
    public string ProgressDetails { get; set; } = string.Empty;

    public string? NextStepsErrorMessage { get; set; } = null;

    public bool ShowNextStepsError => ModelState.ContainsKey(nameof(NextSteps)) &&
                                           ModelState[nameof(NextSteps)]?.Errors.Count > 0;

    public IList<RadioButtonsLabelViewModel> NextStepsRadioButtons { get; set; } = [];

    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, int? objectiveId, string? returnPage,
        bool? enableSkip, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        // using readableId here as they are pastd through the url
        ReviewId = reviewId;
        SetupNextStepsRadioButtons();

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id, int reviewId, int? objectiveId, string? returnPage,
        bool? enableSkip, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        SetupNextStepsRadioButtons();

        if (!ModelState.IsValid)
        {
            if (ShowNextStepsError)
            {
                NextStepsErrorMessage = "Select next steps";
                _errorService.AddError(NextStepsRadioButtons.First().Id, NextStepsErrorMessage);
            }

            _errorService.AddErrors(Request.Form.Keys, ModelState);
            return Page();
        }
        
        // call to new record progress command

        // var result = await mediator.Send(new AddImprovementPlanObjectiveProgressCommand(
        //     new SupportProjectId(id),
        //     new ImprovementPlanId(ImprovementPlan.Id),
        //     new ImprovementPlanReviewId(Review.Id),
        //     new ImprovementPlanObjectiveId(Objective.Id),
        //     NextSteps!, ProgressDetails), cancellationToken);
        //
        // if (result == null)
        // {
        //     _errorService.AddApiError();
        //     return await base.GetSupportProject(id, cancellationToken);
        // }
        

        // check if this is correct - I think there will only be one return route?
        // if (returnPage is not null)
        // {
        //     return RedirectToPage(returnPage, new { id, reviewId });
        // }
        
        return RedirectToPage(Links.ProgressReviews.Index.Page, new { id, reviewId });
    }


    private void SetupNextStepsRadioButtons()
    {
        NextStepsRadioButtons = new List<RadioButtonsLabelViewModel>
        {
            new() {
                Id = "match-with-organisation",
                Name = "Yes, we will find a preferred supporting organisation",
                Value = "Match with a supporting organisation"
            },
            new() {
                Id = "review-school-progress",
                Name = "No, we have decided to review progress",
                Value = "Review school's progress"
            }
        };
    }
}