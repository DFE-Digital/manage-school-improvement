using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProgressReviews;

public class AddReviewModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IUserRepository userRepository)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty, ModelBinder(typeof(DateInputModelBinder))]
    public DateTime? ReviewDate { get; set; }

    [BindProperty]
    public string? ReviewerSelection { get; set; }
    public string? ReviewerSelectionErrorMessage { get; set; } = null;

    [BindProperty]
    public string? CustomReviewerName { get; set; }

    public IList<RadioButtonsLabelViewModel> ReviewerRadioButtons { get; set; } = [];

    public bool ShowReviewerSelectionError => ModelState.ContainsKey(nameof(ReviewerSelection)) && ModelState[nameof(ReviewerSelection)]?.Errors.Count > 0;
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        // Set the return page to the progress reviews index
        ReturnPage = Links.ProgressReviews.Index.Page;

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

        // TODO: Save the progress review data when the data model is implemented

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
    public string AllMissing(string displayName)
    {
        return $"Enter the {displayName.ToLower()}";
    }

    private bool IsCustomReviewerNameValid()
    {

        return !ModelState.TryGetValue(nameof(CustomReviewerName), out var entry) || entry.Errors.Count == 0;
    }

}