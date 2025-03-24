using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.FundingHistory.AddFundingHistory;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.FundingHistory
{
    public class AddFundingHistoryModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "funding-type")]
        [Required(ErrorMessage = "You must enter a funding type")]
        public string? FundingType { get; set; }

        [BindProperty(Name = "funding-amount")]
        [Required(ErrorMessage = "You must enter the total funding amount")]
        [RegularExpression(@"^\£?\d+(\.\d{2})?$", ErrorMessage = "Funding amount must be a number greater than zero with up to 2 decimal places")]
        public double? FundingAmount { get; set; }

        [BindProperty(Name = "financial-year")]
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

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

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

            var request = new AddFundingHistoryCommand(new SupportProjectId(id), FundingType, FundingAmount.Value, FinancialYear, FundingRounds.Value, AdditionalComments);

            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); ;
            }

            return RedirectToPage(@Links.TaskList.FundingHistoryDetails.Page, new { id });
        }
    }
}
