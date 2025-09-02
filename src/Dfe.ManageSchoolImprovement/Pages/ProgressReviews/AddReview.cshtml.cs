using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlansReviews.AddImprovementPlanReview;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class AddReviewModel(
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
    public Guid ImprovementPlanId { get; set; }

    public IList<RadioButtonsLabelViewModel> ReviewerRadioButtons { get; set; } = [];
    
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, int readableImprovementPlanId, CancellationToken cancellationToken)
    {
        // Set the return page to the progress reviews index
        ReturnPage = Links.ProgressReviews.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        SetupRadioButtons();

        var improvementPlan = SupportProject?.ImprovementPlans?.SingleOrDefault(x => x.ReadableId == readableImprovementPlanId);

        if (improvementPlan != null)
        {
            ImprovementPlanId = improvementPlan.Id;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int readableImprovementPlanId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        // Get the previous review for validation
        var previousReview = SupportProject?.ImprovementPlans?.SingleOrDefault(x => x.ReadableId == readableImprovementPlanId)?.ImprovementPlanReviews
            .OrderByDescending(x => x.Order)
            .FirstOrDefault();

        var reviewDateErrorMessage = "Enter the date of the review";
        var reviewerSelectionErrorMessage = "Select who carried out the review";
        var wrongReviewDateErrorMessage = "The review date must be after the last review date";
        var customReviewerNameErrorMessage = "Enter the name of the person who did this review";

        // Validate the form
        if (!ReviewDate.HasValue)
        {
            _errorService.AddError(nameof(ReviewDate), reviewDateErrorMessage);
            ModelState.AddModelError(nameof(ReviewDate), reviewDateErrorMessage);
        }
        
        if (string.IsNullOrWhiteSpace(ReviewerSelection))
        {
            _errorService.AddError(nameof(ReviewerSelection), reviewerSelectionErrorMessage);
            ReviewerSelectionErrorMessage = reviewerSelectionErrorMessage;
            ModelState.AddModelError(nameof(ReviewerSelection), reviewerSelectionErrorMessage);
        }
        
        if (ReviewDate.HasValue && previousReview != null && ReviewDate.Value <= previousReview.ReviewDate)
        {
            _errorService.AddError(nameof(ReviewDate), wrongReviewDateErrorMessage);
            ModelState.AddModelError(nameof(ReviewDate), wrongReviewDateErrorMessage);
        }

        if (ReviewerSelection == "someone-else" && string.IsNullOrWhiteSpace(CustomReviewerName))
        {
            _errorService.AddError(nameof(CustomReviewerName), customReviewerNameErrorMessage);
            ModelState.AddModelError(nameof(CustomReviewerName), customReviewerNameErrorMessage);
        }

        if (!ModelState.IsValid)
        {
            SetupRadioButtons();
            return Page();
        }

        var reviewer = ReviewerSelection == "someone-else" ? CustomReviewerName : ReviewerSelection;
        var result = await mediator.Send(
            new AddImprovementPlanReviewCommand(
                new SupportProjectId(id),
                new ImprovementPlanId(ImprovementPlanId),
                reviewer,
                ReviewDate!.Value), cancellationToken);

        // get latest version of the support project
        await base.GetSupportProject(id, cancellationToken);

        var review = SupportProject.ImprovementPlans.SelectMany(x => x.ImprovementPlanReviews)
            .SingleOrDefault(x => x.Id == result.Value);

        // For now, redirect back to the progress reviews index
        return RedirectToPage(Links.ProgressReviews.NextReview.Page, new { id, reviewId = review.ReadableId });
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

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the date of review";
    }

    private bool IsCustomReviewerNameValid()
    {

        return !ModelState.TryGetValue(nameof(CustomReviewerName), out var entry) || entry.Errors.Count == 0;
    }
}