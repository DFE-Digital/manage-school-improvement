using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RecordSupportingOrganisationAppointment
{
    public class RecordSupportingOrganisationApprovalNotGivenModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = "supporting-organisation-approval-not-given")]
        [Display(Name = "Provide some details about why approval was not given")]
        public string? DisapprovingSupportingOrganisationAppointmentNotes { get; set; }

        public bool ShowError { get; set; }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (SupportProject != null)
            {
                DisapprovingSupportingOrganisationAppointmentNotes = SupportProject.DisapprovingSupportingOrganisationAppointmentNotes;
            }
            
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(DisapprovingSupportingOrganisationAppointmentNotes))
            {
                if (string.IsNullOrWhiteSpace(DisapprovingSupportingOrganisationAppointmentNotes))
                {
                    _errorService.AddError("textInput","Enter details");
                }

                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new SetRecordSupportingOrganisationAppointmentCommand(new SupportProjectId(id), SupportProject?.RegionalDirectorAppointmentDate, SupportProject?.HasConfirmedSupportingOrganisationAppointment, DisapprovingSupportingOrganisationAppointmentNotes);

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
