using System.ComponentModel.DataAnnotations;
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
    [Display(Name = "Enter date improvement plan received")]
    public DateTime? DateImprovementPlanReceived { get; set; }

    [BindProperty(Name = "review-improvement-plan")]
    [Display(Name = "Review the draft improvement and expenditure plans with the adviser, regional team and RISE grant team")]
    public bool? ReviewImprovementAndExpenditurePlan { get; set; }

    [BindProperty(Name = "confirm-plan-cleared-by-rise")]
    [Display(Name = "Confirm plan has been cleared by the RISE grant team")]
    public bool? ConfirmPlanClearedByRiseGrantTeam { get; set; }

    [BindProperty(Name = "FundingBand")]
    [Display(Name = "Select confirmed funding band")]
    public string? FundingBand { get; set; }

    [BindProperty(Name = "confirm-funding-band")]
    [Display(Name = "Confirm funding band")]
    public bool? ConfirmFundingBand { get; set; }
    
    public TaskListStatus? TaskListStatus { get; set; }
    public ProjectStatusValue? ProjectStatus { get; set; }

    public required string EmailAddress { get; init; } = "rise.grant@education.gov.uk";

    public string ConfirmFundingBandLink { get; private set; } = string.Empty;

    public string FundingBandGuidanceLink { get; private set; } = string.Empty;

    public List<RadioButtonsLabelViewModel> FundingBandOptions => _fundingBandOptions;

    public bool ShowError { get; set; }

    // IDateValidationMessageProvider implementation with interpolated string handlers
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        => $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing
        => "Enter a date.";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);
        await LoadSharePointLinksAsync(cancellationToken);
        
        PopulateFormFields();

        if (SupportProject != null)
        {
            TaskListStatus = TaskStatusViewModel.ReviewTheImprovementPlanTaskListStatus(SupportProject);
            ProjectStatus = SupportProject.ProjectStatus; 
        }

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