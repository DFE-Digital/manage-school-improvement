using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectIebDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class ChangeUseOfInterimExecutiveBoardModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    // Fixed: Initialize with empty string to avoid CS8618
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty(Name = "ieb-created-details")]
    public string? InterimExecutiveBoardCreatedDetails { get; set; }

    [BindProperty(Name = "ieb-created-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? InterimExecutiveBoardCreatedDate { get; set; }

    [BindProperty(Name = "remove-ieb")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? RemoveInterimExecutiveBoard { get; set; } = false;

    public bool ShowError => _errorService.HasErrors();
    public bool ShowDetailsError => ModelState.ContainsKey("ieb-created-details") &&
                                    ModelState["ieb-created-details"]?.Errors.Count > 0;

    public string IEBGuidanceLink { get; set; } = string.Empty;

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken = default)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);
        ReturnPage = Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        await LoadIEBGuidanceLinkAsync(cancellationToken);

        // Populate form fields from engagement concern data
        PopulateFormFields(readableEngagementConcernId);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);
        await LoadIEBGuidanceLinkAsync(cancellationToken);

        var engagementConcern = GetEngagementConcern(readableEngagementConcernId);

        if (engagementConcern?.Id == null)
        {
            throw new InvalidOperationException($"Engagement concern with readable ID {readableEngagementConcernId} not found");
        }

        // Validate and clean up form data
        ValidateFormData();

        // Early return for validation errors
        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        // Target-typed new expression (.NET 8)
        SetSupportProjectIebDetailsCommand request = new(
            engagementConcern.Id,
            new SupportProjectId(id),
            !RemoveInterimExecutiveBoard,
            InterimExecutiveBoardCreatedDetails,
            InterimExecutiveBoardCreatedDate);

        var result = await mediator.Send(request, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        // Set TempData for success notifications using tuple deconstruction
        (TempData["InterimExecutiveBoardUpdated"], TempData["InterimExecutiveBoardRemoved"]) = (
            engagementConcern.InterimExecutiveBoardCreated is true &&
            RemoveInterimExecutiveBoard is false &&
            engagementConcern.InterimExecutiveBoardCreatedDetails != InterimExecutiveBoardCreatedDetails,

            engagementConcern.InterimExecutiveBoardCreated == true &&
            RemoveInterimExecutiveBoard is true
        );

        return RedirectToPage(Links.EngagementConcern.Index.Page, new { id });
    }

    // Extracted method for loading IEB guidance link
    private async Task LoadIEBGuidanceLinkAsync(CancellationToken cancellationToken)
    {
        IEBGuidanceLink = await sharePointResourceService
            .GetIEBGuidanceLinkAsync(cancellationToken) ?? string.Empty;
    }

    // Extracted method for getting engagement concern
    private dynamic? GetEngagementConcern(int readableEngagementConcernId)
    {
        return SupportProject?.EngagementConcerns?.FirstOrDefault(a => a.ReadableId == readableEngagementConcernId);
    }

    // Extracted method for populating form fields
    private void PopulateFormFields(int readableEngagementConcernId)
    {
        var engagementConcern = GetEngagementConcern(readableEngagementConcernId);

        if (engagementConcern == null) return;

        // Tuple deconstruction for property assignments
        (RemoveInterimExecutiveBoard, InterimExecutiveBoardCreatedDetails, InterimExecutiveBoardCreatedDate) = (
            !engagementConcern.InterimExecutiveBoardCreated,
            engagementConcern.InterimExecutiveBoardCreatedDetails,
            engagementConcern.InterimExecutiveBoardCreatedDate
        );
    }

    // Extracted method for form validation logic
    private void ValidateFormData()
    {
        if (RemoveInterimExecutiveBoard == false)
        {
            if (string.IsNullOrEmpty(InterimExecutiveBoardCreatedDetails))
            {
                ModelState.AddModelError("ieb-created-details", "You must enter details");
            }

            if (!InterimExecutiveBoardCreatedDate.HasValue)
            {
                ModelState.AddModelError("ieb-created-date", "Enter a date");
            }
        }
        else if (RemoveInterimExecutiveBoard == true)
        {
            // Clear data when removing IEB using tuple assignment
            (InterimExecutiveBoardCreatedDetails, InterimExecutiveBoardCreatedDate) = (null, null);
        }
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);

        if (_errorService.HasErrors())
        {
            await base.GetSupportProject(id, cancellationToken);
        }

        return Page();
    }
}
