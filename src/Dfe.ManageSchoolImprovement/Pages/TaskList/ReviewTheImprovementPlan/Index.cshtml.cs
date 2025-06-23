using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ReviewTheImprovementPlan
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),IDateValidationMessageProvider
    {
        [BindProperty(Name = "date-improvement-plan-received",BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        public DateTime? DateImprovementPlanReceived { get; set; }
        
        [BindProperty(Name = "review-improvement-plan")]
        public bool? ReviewImprovementAndExpenditurePlan { get; set; } 

        [BindProperty(Name = "confirm-plan-cleared-by-rise")]
        public bool? ConfirmPlanClearedByRiseGrantTeam { get; set; } 
        
        [BindProperty(Name = "funding-band")]
        public string? FundingBand { get; set; }
        
        public bool? ConfirmFundingBand  { get; set; }
        
        public string EmailAddress { get; set; } = "rise.grant@education.gov.uk";

        public string ConfirmFundingBandLink { get; set; } = "http://www.google.com";
        
        public string FundingBandGuidanceLink { get; set; } = "https://www.google.com";
        
        public required IList<RadioButtonsLabelViewModel> SelectFundingBandRadioButtons { get; set; }
        
        public bool ShowError { get; set; }
        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }

        string IDateValidationMessageProvider.AllMissing(string displayName)
        {
            return $"Enter date the improvement plan was received.";
        }
        
        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            DateImprovementPlanReceived = SupportProject.ImprovementPlanReceivedDate;
            ReviewImprovementAndExpenditurePlan = SupportProject.ReviewImprovementAndExpenditurePlan;
            ConfirmPlanClearedByRiseGrantTeam = SupportProject.ConfirmPlanClearedByRiseGrantTeam;

            ConfirmFundingBand = false;

            SelectFundingBandRadioButtons = GetRadioButtons();
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
                ConfirmPlanClearedByRiseGrantTeam);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); ;
            }

            TaskUpdated = true;
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
        
        private IList<RadioButtonsLabelViewModel> GetRadioButtons()
        {
            var list = new List<RadioButtonsLabelViewModel>
            {
                new() {
                    Id = "none",
                    Name = "No funding required",
                    Value = "No funding required"
                },
                new() {
                    Id = "40000",
                    Name = "Up to £40,000",
                    Value = "Up to £40,000"
                },
                new()
                {
                    Id = "80000",
                    Name = "Up to £80,000",
                    Value = "Up to £80,000"
                },
                new()
                {
                    Id = "120000",
                    Name = "Up to £120,000",
                    Value = "Up to £120,000"
                }
            };

            return list;
        }
        
    }
}
