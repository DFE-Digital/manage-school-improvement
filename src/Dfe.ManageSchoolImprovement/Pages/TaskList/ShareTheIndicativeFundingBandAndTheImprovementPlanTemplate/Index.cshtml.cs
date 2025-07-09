using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Enums;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetIndicativeFundingBandAndImprovementPlanTemplateDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ShareTheIndicativeFundingBandAndTheImprovementPlanTemplate;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "date-templates-sent", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date templates sent")]
    public DateTime? DateTemplatesSent { get; set; }

    public bool ShowError { get; set; }

    [BindProperty(Name = "calculate-funding-band")]
    public bool? CalculateFundingBand { get; set; }

    [BindProperty(Name = "FundingBand")]
    public string? FundingBand { get; set; }

    [BindProperty(Name = "send-template")]
    public bool? SendTemplate { get; set; }

    public string? FundingBandErrorMessage { get; set; }
    public IList<RadioButtonsLabelViewModel> FundingBandOptions() => Enum.GetValues<FundingBand>()
        .Select(band => new RadioButtonsLabelViewModel
        {
            Id = $"funding-band-{band.GetDisplayShortName()}",
            Name = band.GetDisplayName(),
            Value = band.GetDisplayName()
        })
        .ToList();

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the date templates sent";
    }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        DateTemplatesSent = SupportProject.DateTemplatesAndIndicativeFundingBandSent;
        CalculateFundingBand = SupportProject.IndicativeFundingBandCalculated;
        FundingBand = SupportProject.IndicativeFundingBand;
        SendTemplate = SupportProject.ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody;

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }

        var request = new SetIndicativeFundingBandAndImprovementPlanTemplateDetailsCommand(
            new SupportProjectId(id),
            CalculateFundingBand,
            FundingBand, // Use the ShortName value,
            SendTemplate,
            DateTemplatesSent);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(@Links.TaskList.Index.Page, new { id });
    }

}
