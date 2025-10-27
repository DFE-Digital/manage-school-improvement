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

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "date-templates-sent", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date templates sent")]
    public DateTime? DateTemplatesSent { get; set; }

    [BindProperty(Name = "calculate-funding-band")]
    public bool? CalculateFundingBand { get; set; }

    [BindProperty(Name = "FundingBand")]
    public string? FundingBand { get; set; }

    [BindProperty(Name = "send-template")]
    public bool? SendTemplate { get; set; }

    public bool ShowError { get; set; }
    public string? FundingBandErrorMessage { get; set; }
    public string AssessmentToolThreeLink { get; set; } = string.Empty;
    public string AssessmentToolThreeGuidanceLink { get; set; } = string.Empty;
    public string ImprovementPlanTemplateLink { get; set; } = string.Empty;

    // Collection expression for funding band options (.NET 8)
    public IList<RadioButtonsLabelViewModel> FundingBandOptions() =>
        [.. Enum.GetValues<FundingBand>()
            .Select(band => new RadioButtonsLabelViewModel
            {
                Id = $"funding-band-{band.GetDisplayShortName()}",
                Name = band.GetDisplayName(),
                Value = band.GetDisplayName()
            })];

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing(string displayName) =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Tuple deconstruction for property assignments
        (DateTemplatesSent, CalculateFundingBand, FundingBand, SendTemplate) = (
            SupportProject?.DateTemplatesAndIndicativeFundingBandSent,
            SupportProject?.IndicativeFundingBandCalculated,
            SupportProject?.IndicativeFundingBand,
            SupportProject?.ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody
        );

        // Sequential SharePoint link retrieval to avoid DbContext threading issues
        await LoadSharePointLinksAsync(cancellationToken);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        // Load SharePoint links early for both success and error paths
        await LoadSharePointLinksAsync(cancellationToken);

        // Early return for validation errors
        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        var command = new SetIndicativeFundingBandAndImprovementPlanTemplateDetailsCommand(
            new SupportProjectId(id),
            CalculateFundingBand,
            FundingBand,
            SendTemplate,
            DateTemplatesSent);

        var result = await mediator.Send(command, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.Index.Page, new { id });
    }

    // Sequential method for SharePoint link loading to avoid DbContext threading issues
    private async Task LoadSharePointLinksAsync(CancellationToken cancellationToken)
    {
        // Sequential calls to avoid DbContext concurrency issues
        // Each call completes before the next one starts, preventing concurrent access to DbContext
        AssessmentToolThreeLink = await sharePointResourceService.GetAssessmentToolThreeLinkAsync(cancellationToken) ?? string.Empty;
        AssessmentToolThreeGuidanceLink = await sharePointResourceService.GetTargetedInterventionGuidanceLinkAsync(cancellationToken) ?? string.Empty;
        ImprovementPlanTemplateLink = await sharePointResourceService.GetImprovementPlanTemplateLinkAsync(cancellationToken) ?? string.Empty;
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await base.GetSupportProject(id, cancellationToken);
    }
}
