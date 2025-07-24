using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ImprovementPlan
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "HasSchoolReceivedFundingInLastTwoYears")]
        [Required]
        [Display(Name = "Has the school received any funding in the last 2 financial years")]
        public bool? HasSchoolReceivedFundingInLastTwoYears { get; set; }

        public string? HasSchoolReceivedFundingInLastTwoYearsErrorMessage { get; set; } = null;

        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

        public bool ShowError { get; set; }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            HasSchoolReceivedFundingInLastTwoYears = SupportProject.HasReceivedFundingInThelastTwoYears;
            RadioButtons = RadioButtonModel;
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || HasSchoolReceivedFundingInLastTwoYears == null)
            {
                if (HasSchoolReceivedFundingInLastTwoYears == null)
                {
                    HasSchoolReceivedFundingInLastTwoYearsErrorMessage = "Select an answer";
                    _errorService.AddError("HasSchoolReceivedFundingInLastTwoYears", "You must select an answer");
                }

                RadioButtons = RadioButtonModel;
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new SetHasReceivedFundingInThelastTwoYearsCommand(new SupportProjectId(id), HasSchoolReceivedFundingInLastTwoYears);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            TaskUpdated = true;
            var redirectPage = HasSchoolReceivedFundingInLastTwoYears.HasValue && HasSchoolReceivedFundingInLastTwoYears.Value ? RedirectToPage(@Links.TaskList.FundingHistoryAdd.Page, new { id }) : RedirectToPage(@Links.TaskList.Index.Page, new { id });
            return redirectPage;
        }


        private IList<RadioButtonsLabelViewModel> RadioButtonModel
        {
            get
            {
                var list = new List<RadioButtonsLabelViewModel>
                {
                    new() {
                        Id = "yes",
                        Name = "Yes, school has received funding",
                        Value = "True"
                    },
                    new() {
                        Id = "no",
                        Name = "No, school has not received funding",
                        Value = "False",
                    }
                };

                return list;
            }
        }
    }
}
