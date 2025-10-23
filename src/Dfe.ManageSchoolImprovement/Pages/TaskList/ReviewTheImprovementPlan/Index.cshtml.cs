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

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ReviewTheImprovementPlan
{
    public class IndexModel(
        ISupportProjectQueryService supportProjectQueryService, 
        ErrorService errorService, 
        IMediator mediator, 
        IConfiguration configuration) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),IDateValidationMessageProvider
    {
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
        public bool? ConfirmFundingBand  { get; set; }
        
        public string EmailAddress { get; set; } = "rise.grant@education.gov.uk";

        public string ConfirmFundingBandLink { get; set; } = string.Empty;
        
        public string FundingBandGuidanceLink { get; set; } = string.Empty;
        
        public IList<RadioButtonsLabelViewModel> FundingBandOptions() => Enum.GetValues<FinalFundingBand>()
            .Select(band => new RadioButtonsLabelViewModel
            {
                Id = $"funding-band-{band.GetDisplayShortName()}",
                Name = band.GetDisplayName(),
                Value = band.GetDisplayName()
            })
            .ToList();
        public bool ShowError { get; set; }
        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }
        
        string IDateValidationMessageProvider.AllMissing => "Enter a date";
        
        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            
            ConfirmFundingBandLink = configuration.GetValue<string>("ConfirmFundingBandLink") ?? string.Empty;
            FundingBandGuidanceLink = configuration.GetValue<string>("FundingBandGuidanceLink") ?? string.Empty;
            
            DateImprovementPlanReceived = SupportProject.ImprovementPlanReceivedDate;
            ReviewImprovementAndExpenditurePlan = SupportProject.ReviewImprovementAndExpenditurePlan;
            ConfirmPlanClearedByRiseGrantTeam = SupportProject.ConfirmPlanClearedByRiseGrantTeam;
            ConfirmFundingBand = SupportProject.ConfirmFundingBand;
            FundingBand = SupportProject.FundingBand;
            
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

            var request = new SetReviewTheImprovementPlanCommand(new SupportProjectId(id), 
                DateImprovementPlanReceived,
                ReviewImprovementAndExpenditurePlan, 
                ConfirmFundingBand,
                FundingBand,
                ConfirmPlanClearedByRiseGrantTeam);

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
}
