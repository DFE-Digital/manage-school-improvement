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
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = "appointment-date", BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "record supporting organisation appointment")]
        public DateTime? RegionalDirectorAppointmentDate { get; set; }

        [BindProperty(Name = "HasConfirmedSupportingOrganisationAppointment")]
        public bool? HasConfirmedSupportingOrganisationAppointment { get; set; }

        [BindProperty(Name = "DisapprovingSupportingOrganisationAppointmentNotes")]
        public string? DisapprovingSupportingOrganisationAppointmentNotes { get; set; }

        public required IList<RadioButtonsLabelViewModel> RadioButtoons { get; set; }

        public bool ShowError { get; set; }

        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }

        string IDateValidationMessageProvider.AllMissing(string displayName)
        {
            return $"Enter the record matching decision date";
        }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            HasConfirmedSupportingOrganisationAppointment = SupportProject.HasConfirmedSupportingOrganisationAppointment;
            RegionalDirectorAppointmentDate = SupportProject.RegionalDirectorAppointmentDate;
            DisapprovingSupportingOrganisationAppointmentNotes = SupportProject.DisapprovingSupportingOrganisationAppointmentNotes;
            RadioButtoons = RadioButtons;
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || !IsDisapprovingSupportingOrganisationAppointmentNotesValid())
            {
                RadioButtoons = RadioButtons;
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new SetRecordSupportingOrganisationAppointmentCommand(new SupportProjectId(id), RegionalDirectorAppointmentDate, HasConfirmedSupportingOrganisationAppointment, DisapprovingSupportingOrganisationAppointmentNotes);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); ;
            }

            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }


        private IList<RadioButtonsLabelViewModel> RadioButtons
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
                        Input = new TextAreaInputViewModel
                        {
                            Id = nameof(DisapprovingSupportingOrganisationAppointmentNotes),
                            ValidationMessage = "You must add a note",
                            Paragraph = "Provide some details about why approval was not given.",
                            Value = DisapprovingSupportingOrganisationAppointmentNotes,
                            IsValid = IsDisapprovingSupportingOrganisationAppointmentNotesValid()
                        }
                    }
                };

                return list;
            }
        }
        private bool IsDisapprovingSupportingOrganisationAppointmentNotesValid()
        {
            if (HasConfirmedSupportingOrganisationAppointment == false && string.IsNullOrWhiteSpace(DisapprovingSupportingOrganisationAppointmentNotes))
            {
                return false;
            }
            return true;
        }
    }
}
