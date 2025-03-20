using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.FundingHistory.AddFundingHistory;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.FundingHistory.EditFundingHistory;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.FundingHistory
{
    public class EditFundingHistoryModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "funding-type")]
        public string? FundingType { get; set; }
        [BindProperty(Name = "financial-year")]
        public string? FinancialYear { get; set; }
        [BindProperty(Name = "funding-rounds")]
        public int? FundingRounds { get; set; }
        [BindProperty(Name = "additional-comments")]
        public string? AdditionalComments { get; set; }
        [BindProperty(Name = "funding-amount")]
        public double? FundingAmount { get; set; }


        public bool ShowError { get; set; }

        public async Task<IActionResult> OnGet(int id, Guid? fundingHistoryId, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            if (fundingHistoryId != null)
            {
                var fundingHistory = SupportProject.FundingHistories.SingleOrDefault(x => x.Id == fundingHistoryId.Value);
                if (fundingHistory != null)
                {
                    FundingType = fundingHistory.FundingType;
                    FinancialYear = fundingHistory.FinancialYear;
                    FundingRounds = fundingHistory.FundingRounds;
                    FundingAmount = fundingHistory.FundingAmount;
                    AdditionalComments = fundingHistory.Comments;
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, Guid? fundingHistoryId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }
            IRequest<FundingHistoryId> request;

            if (fundingHistoryId is null)
            {
                request = new AddFundingHistoryCommand(new SupportProjectId(id), FundingType, FundingAmount.Value, FinancialYear, FundingRounds.Value, AdditionalComments);
            }
            else
            {
                request = new EditFundingHistoryCommand(new FundingHistoryId(fundingHistoryId.Value), new SupportProjectId(id), FundingType, FundingAmount.Value, FinancialYear, FundingRounds.Value, AdditionalComments);
            }


            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); ;
            }

            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
    }
}
