using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.FundingHistory.EditFundingHistory;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.FundingHistory
{
    public class EditFundingHistoryModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "funding-type")]
        [Required(ErrorMessage = "You must enter a funding type")]
        public string? FundingType { get; set; }

        [BindProperty(Name = "funding-amount")]
        [Required(ErrorMessage = "You must enter the total funding amount")]
        [RegularExpression(@"^\�?\d+(\.\d{1,2})?$", ErrorMessage = "Funding amount must be a number. It can be a decimal, to represent pounds and pence.")]
        public decimal? FundingAmount { get; set; }

        [BindProperty(Name = "financial-year-input")]
        [Required(ErrorMessage = "You must enter the financial year payment was made")]
        public string? FinancialYear { get; set; }

        [BindProperty(Name = "funding-rounds")]
        [Required(ErrorMessage = "You must enter the number of payments made in the financial year")]
        [Range(1, int.MaxValue, ErrorMessage = "Funding rounds must be a whole number.")]
        [Display(Name = "Funding rounds")]
        public int? FundingRounds { get; set; }

        [BindProperty(Name = "additional-comments")]
        public string? AdditionalComments { get; set; }

        [BindProperty(Name = "funding-history-Id")]
        public Guid FundingHistoryId { get; set; }

        public bool ShowError { get; set; }

        public async Task<IActionResult> OnGet(int id, int readableFundingHistoryId, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            if (readableFundingHistoryId != null)
            {
                var fundingHistory = SupportProject.FundingHistories.SingleOrDefault(x => x.ReadableId == readableFundingHistoryId);
                if (fundingHistory != null)
                {
                    FundingHistoryId = fundingHistory.Id;
                    FundingType = fundingHistory.FundingType;
                    FinancialYear = fundingHistory.FinancialYear;
                    FundingRounds = fundingHistory.FundingRounds;
                    FundingAmount = fundingHistory.FundingAmount;
                    AdditionalComments = fundingHistory.Comments;
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, int readableFundingHistoryId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new EditFundingHistoryCommand(new FundingHistoryId(FundingHistoryId), new SupportProjectId(id), FundingType, FundingAmount.Value, FinancialYear, FundingRounds.Value, AdditionalComments);

            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            return RedirectToPage(@Links.TaskList.FundingHistoryDetails.Page, new { id });
        }
    }
}
