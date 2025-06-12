using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectInformationPowersDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class RecordUseOfInformationPowersModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "information-powers-details")]
    public string? InformationPowersDetails { get; set; }

    public bool ShowError => _errorService.HasErrors();

    public bool ShowDetailsError => ModelState.ContainsKey("information-powers-details") &&
                                  ModelState["information-powers-details"]?.Errors.Count > 0;

    [BindProperty(Name = "powers-used-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? PowersUsedDate { get; set; }

    [BindProperty(Name = "information-powers-in-use")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? InformationPowersInUse { get; set; }


    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        //InformationPowersDetails = SupportProject.InformationPowersDetails;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (InformationPowersInUse == true && string.IsNullOrEmpty(InformationPowersDetails))
        {
            _errorService.AddError("information-powers-details", "You must enter details");
            ModelState.AddModelError("information-powers-details", "You must enter details");
            return Page();
        }

        if (InformationPowersInUse == false)
        {
            InformationPowersDetails = null;
            PowersUsedDate = null;
        }
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

        TempData["InformationPowersUpdated"] = true;
        return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
    }

}
