using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Enums;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ReviewTheImprovementPlan;

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    private static readonly List<RadioButtonsLabelViewModel> _fundingBandOptions = CreateFundingBandOptions();

    [BindProperty(Name = "date-improvement-plan-received", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateImprovementPlanReceived { get; set; }

    [BindProperty(Name = "review-improvement-plan")]
    public bool? ReviewImprovementAndExpenditurePlan { get; set; }

    [BindProperty(Name = "confirm-plan-cleared-by-rise")]
    public bool? ConfirmPlanClearedByRiseGrantTeam { get; set; }

    [BindProperty(Name = "FundingBand")]
    public string? FundingBand { get; set; }

    [BindProperty(Name = "confirm-funding-band")]
    public bool? ConfirmFundingBand { get; set; }

    public required string EmailAddress { get; init; } = "rise.grant@education.gov.uk";

    public string ConfirmFundingBandLink { get; private set; } = string.Empty;

    public string FundingBandGuidanceLink { get; private set; } = string.Empty;

    public List<RadioButtonsLabelViewModel> FundingBandOptions => _fundingBandOptions;

    public bool ShowError { get; set; }

    // IDateValidationMessageProvider implementation with interpolated string handlers
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        => $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing(string displayName)
        => "Enter a date.";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        // Convert to tuple expression`

        await base.GetSupportProject(id, cancellationToken);
        await LoadSharePointLinksAsync(cancellationToken);

        // Populate form fields from support project data
        PopulateFormFields();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        // Load links concurrently with validation
        await LoadSharePointLinksAsync(cancellationToken);

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;

            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        // Create command using target-typed new
        SetReviewTheImprovementPlanCommand request = new(
            new SupportProjectId(id),
            DateImprovementPlanReceived,
            ReviewImprovementAndExpenditurePlan,
            ConfirmFundingBand,
            FundingBand,
            ConfirmPlanClearedByRiseGrantTeam);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TaskUpdated = true;
        return RedirectToPage(@Links.TaskList.Index.Page, new { id });
    }

    private async Task LoadSharePointLinksAsync(CancellationToken cancellationToken)
    {
        // Safe - calls happen one after another
        ConfirmFundingBandLink = await sharePointResourceService.GetAssessmentToolThreeLinkAsync(cancellationToken) ?? string.Empty;
        FundingBandGuidanceLink = await sharePointResourceService.GetFundingBandGuidanceLinkAsync(cancellationToken) ?? string.Empty;
    }

    private void PopulateFormFields()
    {
        if (SupportProject is null) return;

        // Convert multiple assignments to tuple expression
        (DateImprovementPlanReceived, ReviewImprovementAndExpenditurePlan, ConfirmPlanClearedByRiseGrantTeam, ConfirmFundingBand, FundingBand) =
            (SupportProject.ImprovementPlanReceivedDate,
             SupportProject.ReviewImprovementAndExpenditurePlan,
             SupportProject.ConfirmPlanClearedByRiseGrantTeam,
             SupportProject.ConfirmFundingBand,
             SupportProject.FundingBand);
    }

    private static List<RadioButtonsLabelViewModel> CreateFundingBandOptions()
    {
        return Enum.GetValues<FinalFundingBand>()
            .Select(band => new RadioButtonsLabelViewModel
            {
                Id = $"funding-band-{band.GetDisplayShortName()}",
                Name = band.GetDisplayName(),
                Value = band.GetDisplayName()
            })
            .ToList();
    }
}