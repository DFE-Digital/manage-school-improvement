using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectInformationPowersDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class RecordUseOfInformationPowersModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "information-powers-details")]
    public string? InformationPowersDetails { get; set; }

    public bool ShowError => _errorService.HasErrors();

    public bool ShowDetailsError => ModelState.ContainsKey("information-powers-details") &&
                                  ModelState["information-powers-details"]?.Errors.Count > 0;

    [BindProperty(Name = "powers-used-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Required]
    public DateTime? PowersUsedDate { get; set; }

    [BindProperty(Name = "information-powers-in-use")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? InformationPowersInUse { get; set; }

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

        InformationPowersInUse = SupportProject.InformationPowersInUse;
        InformationPowersDetails = SupportProject.InformationPowersDetails;
        PowersUsedDate = SupportProject.PowersUsedDate;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (InformationPowersInUse == true && string.IsNullOrEmpty(InformationPowersDetails))
        {
            ModelState.AddModelError("information-powers-details", "You must enter details");
        }

        if (InformationPowersInUse != true)
        {
            InformationPowersDetails = null;
            PowersUsedDate = null;

            // Override the validation on the date helper as it is only required when InformationPowersInUse == true 
            ModelState.Remove("powers-used-date");
        }

        _errorService.AddErrors(Request.Form.Keys, ModelState);
        if (_errorService.HasErrors()) return await base.GetSupportProject(id, cancellationToken);

        var request = new SetSupportProjectInformationPowersDetailsCommand(
            new SupportProjectId(id),
            InformationPowersInUse,
            InformationPowersDetails,
            PowersUsedDate);

        var result = await mediator.Send(request, cancellationToken);

        if (result == false)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TempData["InformationPowersRecorded"] = (SupportProject.InformationPowersInUse == null || SupportProject.InformationPowersInUse == false) && InformationPowersInUse == true;
        TempData["InformationPowersRemoved"] = SupportProject.InformationPowersInUse == true && InformationPowersInUse == false;

        return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
    }

}
