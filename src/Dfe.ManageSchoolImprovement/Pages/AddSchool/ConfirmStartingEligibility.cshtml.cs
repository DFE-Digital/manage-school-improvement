using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AddSchool
{
    public class ConfirmStartingEligibilityModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "SchoolIsEligible")]
        [Display(Name = "Is this school still eligible for targeted intervention?")]
        public bool? SchoolIsEligible { get; set; }

        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

        public string? ErrorMessage { get; set; }
        public bool ShowError { get; set; }


        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (SupportProject?.SupportProjectEligibilityStatus == SupportProjectEligibilityStatus.EligibleForSupport)
            {
                SchoolIsEligible = true;
            }

            if (SupportProject?.SupportProjectEligibilityStatus ==
                SupportProjectEligibilityStatus.NotEligibleForSupport)
            {
                SchoolIsEligible = false;
            }

            RadioButtons = EligibilityRadioButtons;
            return Page();
        }

        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || SchoolIsEligible == null)
            {
                if (SchoolIsEligible == null)
                {
                    ErrorMessage = "Select an answer";
                    _errorService.AddError("eligibilityquestion", ErrorMessage);
                }

                RadioButtons = EligibilityRadioButtons;
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            if (SchoolIsEligible == true)
            {
                var request = new SetEligibilityCommand(new SupportProjectId(id), SupportProjectEligibilityStatus.EligibleForSupport, null, null);

                var result = await mediator.Send(request, cancellationToken);

                if (!result)
                {
                    _errorService.AddApiError();
                    return await base.GetSupportProject(id, cancellationToken); 
                }

                TaskUpdated = true;
                return RedirectToPage(@Links.TaskList.Index.Page, new { id });
            }

            // direct to next page
            return RedirectToPage(@Links.AddSchool.EligibilityCheckDate.Page, new { id });
        }


        private IList<RadioButtonsLabelViewModel> EligibilityRadioButtons
        {
            get
            {
                var list = new List<RadioButtonsLabelViewModel>
                {
                    new()
                    {
                        Id = "yes",
                        Name = "Yes",
                        Value = "True"
                    },
                    new()
                    {
                        Id = "no",
                        Name = "No",
                        Value = "False",
                    }
                };

                return list;
            }
        }
    }
}