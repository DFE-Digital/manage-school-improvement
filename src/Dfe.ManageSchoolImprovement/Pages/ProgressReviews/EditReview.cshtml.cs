using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.SetImprovementPlanReviewDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class EditReviewModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty(Name = "ReviewDate")]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [ModelBinder(BinderType = typeof(DateInputModelBinder))]
    public DateTime? ReviewDate { get; set; }

    [BindProperty]
    public string? ReviewerSelection { get; set; }
    public string? ReviewerSelectionErrorMessage { get; set; } = null;

    [BindProperty]
    public string? CustomReviewerName { get; set; }

    [BindProperty]
    public Guid ImprovementPlanReviewId { get; set; }

    [BindProperty]
    public Guid ImprovementPlanId { get; set; }

    public IList<RadioButtonsLabelViewModel> ReviewerRadioButtons { get; set; } = [];

    public bool ShowReviewerSelectionError => ModelState.ContainsKey(nameof(ReviewerSelection)) && ModelState[nameof(ReviewerSelection)]?.Errors.Count > 0;
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        // Set the return page to the progress reviews index
        ReturnPage = Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        var improvementPlanReview = SupportProject?.ImprovementPlans?.SelectMany(x => x.ImprovementPlanReviews).SingleOrDefault(x => x.ReadableId == reviewId);

        if (improvementPlanReview != null)
        {
            ImprovementPlanId = improvementPlanReview.ImprovementPlanId;
            ImprovementPlanReviewId = improvementPlanReview.Id;
            ReviewDate = improvementPlanReview.ReviewDate;

            if (improvementPlanReview.Reviewer != SupportProject?.AdviserFullName &&
                improvementPlanReview.Reviewer != SupportProject?.AssignedDeliveryOfficerFullName)
            {
                CustomReviewerName = improvementPlanReview.Reviewer;
                ReviewerSelection = "someone-else";
            }
            else
            {
                ReviewerSelection = improvementPlanReview.Reviewer;
            }
        }

        SetupRadioButtons();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int reviewId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        // Get the previous review for validation
        var reviews = SupportProject?.ImprovementPlans?
            .SingleOrDefault(x => x.Id == ImprovementPlanId)?
            .ImprovementPlanReviews.OrderByDescending(x => x.Order).ToList();

        // Find the current review being edited
        var currentReviewIndex = reviews?.FindIndex(r => r.Id == ImprovementPlanReviewId) ?? -1;

        // Get the previous review (next in the descending order list)
        var previousReview = currentReviewIndex >= 0 && currentReviewIndex < (reviews?.Count - 1)
            ? reviews?[currentReviewIndex + 1]
            : null;

        // Validate the form
        if (!ReviewDate.HasValue)
        {
            ModelState.AddModelError(nameof(ReviewDate), "Enter a date");
        }

        if (ReviewDate.HasValue && previousReview != null && ReviewDate.Value <= previousReview.ReviewDate)
        {
            ModelState.AddModelError(nameof(ReviewDate), "The review date must be after the last review date");
        }

        if (string.IsNullOrWhiteSpace(ReviewerSelection))
        {
            ModelState.AddModelError(nameof(ReviewerSelection), "Select who carried out the review");
        }

        if (ReviewerSelection == "someone-else" && string.IsNullOrWhiteSpace(CustomReviewerName))
        {
            ModelState.AddModelError(nameof(CustomReviewerName), "Enter the name of the person who did this review");
        }

        if (!ModelState.IsValid)
        {
            SetupRadioButtons();

            _errorService.AddErrors(Request.Form.Keys, ModelState);

            if (ShowReviewerSelectionError)
            {
                ReviewerSelectionErrorMessage = "Select who carried out the review";
                _errorService.AddError(ReviewerRadioButtons.First().Id, ReviewerSelectionErrorMessage);
            }

            return Page();
        }

        var reviewer = ReviewerSelection == "someone-else" ? CustomReviewerName : ReviewerSelection;
        var result = await mediator.Send(new SetImprovementPlanReviewDetailsCommand(new SupportProjectId(id),
            new ImprovementPlanId(ImprovementPlanId),
            new ImprovementPlanReviewId(ImprovementPlanReviewId),
            reviewer ?? string.Empty, ReviewDate!.Value), cancellationToken);

        // get latest version of the support project
        await base.GetSupportProject(id, cancellationToken);

        // For now, redirect back to the progress reviews index
        return RedirectToPage(Links.ProgressReviews.Index.Page, new { id });
    }

    private void SetupRadioButtons()
    {

        var radioButtons = new List<RadioButtonsLabelViewModel>();

        if (SupportProject != null && SupportProject.AdviserFullName != null)
        {

            // delivery officer is optional for reviews, so check if they are assigned
            if (SupportProject.AssignedDeliveryOfficerEmailAddress != null)
            {
                radioButtons.Add(new RadioButtonsLabelViewModel
                {
                    Id = "delivery-officer",
                    Name = SupportProject.AssignedDeliveryOfficerFullName,
                    Value = SupportProject.AssignedDeliveryOfficerFullName
                });
            }

            radioButtons.Add(new RadioButtonsLabelViewModel
            {
                Id = "adviser",
                Name = SupportProject.AdviserFullName,
                Value = SupportProject.AdviserFullName
            });

        }

        // Add "Someone else" option with conditional input
        radioButtons.Add(new RadioButtonsLabelViewModel
        {
            Id = "someone-else",
            Name = "Someone else",
            Value = "someone-else",
            Input = new TextFieldInputViewModel
            {
                Id = nameof(CustomReviewerName),
                ValidationMessage = "Enter the name of the person who did this review",
                Paragraph = "Enter the name of the person who did this review",
                Value = CustomReviewerName ?? string.Empty,
                IsValid = IsCustomReviewerNameValid(),
                IsTextArea = false
            }
        });

        ReviewerRadioButtons = radioButtons;
    }

    // Implementation of IDateValidationMessageProvider
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    
    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    private bool IsCustomReviewerNameValid()
    {

        return !ModelState.TryGetValue(nameof(CustomReviewerName), out var entry) || entry.Errors.Count == 0;
    }

}