using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc; 
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Utils;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ConfirmEligibility
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {

        [BindProperty(Name = "SchoolIsEligible")]
        public bool? SchoolIsEligible { get; set; }

        [BindProperty(Name = "SchoolIsNotEligibleNotes")]
        public string? SchoolIsNotEligibleNotes { get; set; }
        
        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

        public string? ErrorMessage { get; set; }  
        public bool ShowError { get; set; }
        

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            
            if (SupportProject.SupportProjectStatus == SupportProjectStatus.EligibleForSupport)
            {
                SchoolIsEligible = true;
            }
            
            if (SupportProject.SupportProjectStatus == SupportProjectStatus.NotEligibleForSupport)
            {
                SchoolIsEligible = false;
            }

            SchoolIsNotEligibleNotes = SupportProject.SchoolIsNotEligibleNotes;
            RadioButtons = EligibilityRadioButtons;
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
           
            
            if (!ModelState.IsValid || !SchoolIsNotEligible() || SchoolIsEligible == null)
            {
                if (SchoolIsEligible == null)
                {
                    ErrorMessage = "You must select an answer"; 
                    _errorService.AddError("eligibilityquestion", ErrorMessage);
                }
                
                RadioButtons = EligibilityRadioButtons;
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            if (SchoolIsEligible == true)
            {
                SchoolIsNotEligibleNotes = null;
            }

            var request = new SetEligibilityCommand(new SupportProjectId(id), SchoolIsEligible,SchoolIsNotEligibleNotes);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); 
            }

            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }


        private IList<RadioButtonsLabelViewModel> EligibilityRadioButtons
        {
            get
            {
                var list = new List<RadioButtonsLabelViewModel>
                {
                    new() {
                        Id = "yes",
                        Name = "Yes",
                        Value = "True"
                    },
                    new() {
                        Id = "no",
                        Name = "No",
                        Value = "False",
                        Input = new TextFieldInputViewModel
                        {
                            Id = nameof(SchoolIsNotEligibleNotes),
                            ValidationMessage = "You must add a note",
                            Paragraph = "If it is no longer eligible, give details.",
                            Value = SchoolIsNotEligibleNotes,
                            IsValid = SchoolIsNotEligible(),
                            IsTextArea = true
                        }
                    }
                };

                return list;
            }
        }
        private bool SchoolIsNotEligible()
        { 
            if (SchoolIsEligible == false && string.IsNullOrWhiteSpace(SchoolIsNotEligibleNotes))
            {
                return false;
            }
            return true;
        }
        
    }
}
