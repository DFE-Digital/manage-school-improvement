using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectIebDetails;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class RecordUseOfInterimExecutiveBoardModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "ieb-created-details")]
    public string? InterimExecutiveBoardCreatedDetails { get; set; }
    
    [BindProperty(Name = "ieb-created-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? InterimExecutiveBoardCreatedDate { get; set; }
    
    public bool? InterimExecutiveBoardCreated { get; set; }

    public bool ShowError => _errorService.HasErrors();

    public bool ShowDetailsError => ModelState.ContainsKey("ieb-created-details") &&
                                  ModelState["ieb-created-details"]?.Errors.Count > 0;

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"You must enter a date";
    }

    public async Task<IActionResult> OnGetAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(ec => ec.ReadableId == readableEngagementConcernId);

        if (engagementConcern != null)
        {
            InterimExecutiveBoardCreated = engagementConcern.InterimExecutiveBoardCreated;
            InterimExecutiveBoardCreatedDetails = engagementConcern.InterimExecutiveBoardCreatedDetails;
            InterimExecutiveBoardCreatedDate = engagementConcern.InterimExecutiveBoardCreatedDate;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (string.IsNullOrEmpty(InterimExecutiveBoardCreatedDetails))
        {
            ModelState.AddModelError("ieb-created-details", "Enter details");
        }
        
        if (!InterimExecutiveBoardCreatedDate.HasValue)
        {
            ModelState.AddModelError("ieb-created-date", "Enter a date");
        }

        InterimExecutiveBoardCreated = !string.IsNullOrEmpty(InterimExecutiveBoardCreatedDetails) && InterimExecutiveBoardCreatedDate.HasValue;

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            if (_errorService.HasErrors()) return await base.GetSupportProject(id, cancellationToken);
        }
        
        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(ec => ec.ReadableId == readableEngagementConcernId);

        if (engagementConcern?.Id == null)
        {
            throw new InvalidOperationException($"Engagement concern with readable ID {readableEngagementConcernId} not found");
        }
        
        var request = new SetSupportProjectIebDetailsCommand(
            engagementConcern.Id,
            new SupportProjectId(id),
            InterimExecutiveBoardCreated,
            InterimExecutiveBoardCreatedDetails,
            InterimExecutiveBoardCreatedDate);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TempData["InterimExecutiveBoardRecorded"] = (engagementConcern.InterimExecutiveBoardCreated == null || engagementConcern.InterimExecutiveBoardCreated == false) && InterimExecutiveBoardCreated == true;

        return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });    }

}
