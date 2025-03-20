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

        [BindProperty(Name = "HasConfirmedSupportingOrgnaisationAppointment")]
        public bool? HasConfirmedSupportingOrgnaisationAppointment { get; set; }

        [BindProperty(Name = "DisapprovingSupportingOrgnaisationAppointmentNotes")]
        public string? DisapprovingSupportingOrgnaisationAppointmentNotes { get; set; }

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
            HasConfirmedSupportingOrgnaisationAppointment = SupportProject.HasConfirmedSupportingOrgnaisationAppointment;
            RegionalDirectorAppointmentDate = SupportProject.RegionalDirectorAppointmentDate;
            DisapprovingSupportingOrgnaisationAppointmentNotes = SupportProject.DisapprovingSupportingOrgnaisationAppointmentNotes;
            RadioButtoons = RadioButtons;
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || !IsDisapprovingSupportingOrgnaisationAppointmentNotesValid())
            {
                RadioButtoons = RadioButtons;
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new SetRecordSupportingOrganisationAppointmentCommand(new SupportProjectId(id), RegionalDirectorAppointmentDate, HasConfirmedSupportingOrgnaisationAppointment, DisapprovingSupportingOrgnaisationAppointmentNotes);

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
                        Input = new TextFieldInputViewModel
                        {
                            Id = nameof(DisapprovingSupportingOrgnaisationAppointmentNotes),
                            ValidationMessage = "You must add a note",
                            Paragraph = "Provide some details about why approval was not given.",
                            Value = DisapprovingSupportingOrgnaisationAppointmentNotes,
                            IsValid = IsDisapprovingSupportingOrgnaisationAppointmentNotesValid(),
                            IsTextArea = true
                        }
                    }
                };

                return list;
            }
        }
        private bool IsDisapprovingSupportingOrgnaisationAppointmentNotesValid()
        {
            if (HasConfirmedSupportingOrgnaisationAppointment == false && string.IsNullOrWhiteSpace(DisapprovingSupportingOrgnaisationAppointmentNotes))
            {
                return false;
            }
            return true;
        }
    }
}
