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
    IUserRepository userRepository,
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

    public bool ShowReviewerSelectionError => ModelState.ContainsKey(nameof(ReviewerSelection)) && ModelState[nameof(ReviewerSelection)]?.Errors.Count > 0;
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, Guid improvementPlanId, CancellationToken cancellationToken)
    {
        // Set the return page to the progress reviews index
        ReturnPage = Links.ProgressReviews.Index.Page;
        ImprovementPlanId = improvementPlanId;

        await base.GetSupportProject(id, cancellationToken);
        SetupRadioButtons();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        // Validate the form
        if (!ReviewDate.HasValue)
        {
            ModelState.AddModelError(nameof(ReviewDate), "Enter the date of the review");
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
            await base.GetSupportProject(id, cancellationToken);
            SetupRadioButtons();

            if (ShowReviewerSelectionError)
            {
                ReviewerSelectionErrorMessage = "Select who carried out the review";
                _errorService.AddError(ReviewerRadioButtons.First().Id, ReviewerSelectionErrorMessage);
            }

            _errorService.AddErrors(Request.Form.Keys, ModelState);

            return Page();
        }

        var reviewer = ReviewerSelection == "someone-else" ? CustomReviewerName : ReviewerSelection;
        var result = await mediator.Send(new AddImprovementPlanReviewCommand(new SupportProjectId(id),
            new ImprovementPlanId(ImprovementPlanId), reviewer, ReviewDate!.Value), cancellationToken);

        // For now, redirect back to the progress reviews index
        return RedirectToPage(Links.ProgressReviews.Index.Page, new { id });
    }

    private void SetupRadioButtons()
    {

        var radioButtons = new List<RadioButtonsLabelViewModel>();

        radioButtons.Add(new RadioButtonsLabelViewModel
        {
            Id = "delivery-officer",
            Name = SupportProject.AssignedDeliveryOfficerFullName,
            Value = SupportProject.AssignedDeliveryOfficerFullName
        });

        radioButtons.Add(new RadioButtonsLabelViewModel
        {
            Id = "adviser",
            Name = SupportProject.AdviserFullName,
            Value = SupportProject.AdviserFullName
        });


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