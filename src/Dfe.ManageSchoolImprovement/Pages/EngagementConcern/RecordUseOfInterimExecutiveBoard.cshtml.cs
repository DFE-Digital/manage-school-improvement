using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectIebDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class RecordUseOfInterimExecutiveBoardModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "ieb-created-details")]
    public string? InterimExecutiveBoardCreatedDetails { get; set; }

    public bool ShowError => _errorService.HasErrors();

    public bool ShowDetailsError => ModelState.ContainsKey("information-powers-details") &&
                                  ModelState["information-powers-details"]?.Errors.Count > 0;

    [BindProperty(Name = "ieb-created-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Required]
    public DateTime? InterimExecutiveBoardCreatedDate { get; set; }

    [BindProperty(Name = "ieb-created")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? InterimExecutiveBoardCreated { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"You must enter a date";
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        InterimExecutiveBoardCreated = SupportProject.InterimExecutiveBoardCreated;
        InterimExecutiveBoardCreatedDetails = SupportProject.InterimExecutiveBoardCreatedDetails;
        InterimExecutiveBoardCreatedDate = SupportProject.InterimExecutiveBoardCreatedDate;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (InterimExecutiveBoardCreated == true && string.IsNullOrEmpty(InterimExecutiveBoardCreatedDetails))
        {
            ModelState.AddModelError("ieb-created-details", "You must enter details");
        }

        if (InterimExecutiveBoardCreated != true)
        {
            InterimExecutiveBoardCreatedDetails = null;
            InterimExecutiveBoardCreatedDate = null;

            // Override the validation on the date helper as it is only required when InterimExecutiveBoardCreated == true 
            ModelState.Remove("ieb-created-date");
        }

        _errorService.AddErrors(Request.Form.Keys, ModelState);
        if (_errorService.HasErrors()) return await base.GetSupportProject(id, cancellationToken);

        var request = new SetSupportProjectIebDetailsCommand(
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
        
        TempData["InterimExecutiveBoardCreated"] = (SupportProject.InterimExecutiveBoardCreated == null || SupportProject.InterimExecutiveBoardCreated == false) && InterimExecutiveBoardCreated == true;
        // TempData["InformationPowersRemoved"] = SupportProject.InterimExecutiveBoardCreated == true && InterimExecutiveBoardCreated == false;

        return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
    }

}
