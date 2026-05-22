using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RecordSupportingOrganisationAppointment
{
    public class IndexModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService,
        IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
        IDateValidationMessageProvider
    {
        public DateTime? RegionalDirectorAppointmentDate { get; set; }

        [BindProperty(Name = "HasConfirmedSupportingOrganisationAppointment")]
        [Display(Name = "Has a regional director approved the appointment of this supporting organisation?")]
        public bool? HasConfirmedSupportingOrganisationAppointment { get; set; }

        public string? DisapprovingSupportingOrganisationAppointmentNotes { get; set; }
        public bool? AssessmentToolTwoCompleted { get; set; }

        public TaskListStatus? TaskListStatus { get; set; }
        public ProjectStatusValue? ProjectStatus { get; set; }

        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

        public bool ShowError { get; set; }


        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (SupportProject != null)
            {
                HasConfirmedSupportingOrganisationAppointment =
                    SupportProject.HasConfirmedSupportingOrganisationAppointment;
                RegionalDirectorAppointmentDate = SupportProject.RegionalDirectorAppointmentDate;
                DisapprovingSupportingOrganisationAppointmentNotes =
                    SupportProject.DisapprovingSupportingOrganisationAppointmentNotes;
                AssessmentToolTwoCompleted = SupportProject.AssessmentToolTwoCompleted;

                TaskListStatus =
                    TaskStatusViewModel.SetRecordSupportingOrganisationAppointmentTaskListStatus(SupportProject);
                ProjectStatus = SupportProject.ProjectStatus;
            }

            RadioButtons = RadioButtonsModel;
            return Page();
        }

        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (!ModelState.IsValid)
            {
                RadioButtons = RadioButtonsModel;
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new SetRecordSupportingOrganisationAppointmentCommand(new SupportProjectId(id),
                SupportProject?.RegionalDirectorAppointmentDate, HasConfirmedSupportingOrganisationAppointment,
                SupportProject?.DisapprovingSupportingOrganisationAppointmentNotes,
                SupportProject?.AssessmentToolTwoCompleted);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            if (HasConfirmedSupportingOrganisationAppointment == true)
            {
                return RedirectToPage(@Links.TaskList.RecordSupportingOrganisationAppointmentDetails.Page, new { id });
            }
            
            return RedirectToPage(@Links.TaskList.RecordSupportingOrganisationApprovalNotGiven.Page, new { id });
        }


        private IList<RadioButtonsLabelViewModel> RadioButtonsModel
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