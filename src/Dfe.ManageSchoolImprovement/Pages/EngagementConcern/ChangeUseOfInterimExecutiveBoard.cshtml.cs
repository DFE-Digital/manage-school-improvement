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
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

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

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    
    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(a => a.ReadableId == readableEngagementConcernId);
        if (engagementConcern != null)
        {
            RemoveInterimExecutiveBoard = !engagementConcern.InterimExecutiveBoardCreated;
            InterimExecutiveBoardCreatedDetails = engagementConcern.InterimExecutiveBoardCreatedDetails;
            InterimExecutiveBoardCreatedDate = engagementConcern.InterimExecutiveBoardCreatedDate;
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(a => a.ReadableId == readableEngagementConcernId);


        if (engagementConcern?.Id == null)
        {
            throw new InvalidOperationException($"Engagement concern with readable ID {readableEngagementConcernId} not found");
        }
        

        if (RemoveInterimExecutiveBoard == false && string.IsNullOrEmpty(InterimExecutiveBoardCreatedDetails))
        {
            ModelState.AddModelError("ieb-created-details", "You must enter details");
        }
        
        if (RemoveInterimExecutiveBoard == false && !InterimExecutiveBoardCreatedDate.HasValue)
        {
            ModelState.AddModelError("ieb-created-date", "Enter a date");
        }
        
        if (RemoveInterimExecutiveBoard == true)
        {
            InterimExecutiveBoardCreatedDetails = null;
            InterimExecutiveBoardCreatedDate = null;
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            if (_errorService.HasErrors()) return await base.GetSupportProject(id, cancellationToken);
        }

        var request = new SetSupportProjectIebDetailsCommand(
            engagementConcern.Id,
            new SupportProjectId(id),
            !RemoveInterimExecutiveBoard,
            InterimExecutiveBoardCreatedDetails,
            InterimExecutiveBoardCreatedDate);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TempData["InterimExecutiveBoardUpdated"] = engagementConcern.InterimExecutiveBoardCreated is true && RemoveInterimExecutiveBoard is false &&
                                               engagementConcern.InterimExecutiveBoardCreatedDetails != InterimExecutiveBoardCreatedDetails;
        TempData["InterimExecutiveBoardRemoved"] = engagementConcern.InterimExecutiveBoardCreated == true && RemoveInterimExecutiveBoard is true;
        
            return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
        }

}
