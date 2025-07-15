using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RecordMatchingDecision
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = "decision-date", BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "record matching decision")]
        public DateTime? RegionalDirectorDecisionDate { get; set; }

        [BindProperty(Name = "HasSchoolMatchedWithSupportingOrganisation")]
        public string? HasSchoolMatchedWithSupportingOrganisation { get; set; }

        [BindProperty(Name = "NotMatchingNotes")]
        public string? NotMatchingNotes { get; set; }

        [BindProperty(Name = "UnableToAssessNotes")]
        public string? UnableToAssessNotes { get; set; }

        public required IList<RadioButtonsLabelViewModel> RadioButtonModels { get; set; }

        public bool ShowError { get; set; }

        public string? ErrorMessage { get; set; }

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

            // Use the new string property directly
            HasSchoolMatchedWithSupportingOrganisation = SupportProject.InitialDiagnosisMatchingDecision;
            RegionalDirectorDecisionDate = SupportProject.RegionalDirectorDecisionDate;

            // Populate the appropriate notes property based on the decision
            if (HasSchoolMatchedWithSupportingOrganisation == "Review school's progress")
            {
                NotMatchingNotes = SupportProject.InitialDiagnosisMatchingDecisionNotes;
            }
            else if (HasSchoolMatchedWithSupportingOrganisation == "Unable to assess")
            {
                UnableToAssessNotes = SupportProject.InitialDiagnosisMatchingDecisionNotes;
            }

            RadioButtonModels = RadioButtons;
            return Page();
        }

        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            bool hasValidationErrors = false;

            if (!ModelState.IsValid)
            {
                hasValidationErrors = true;
            }

            if (!IsNotMatchingNotesValid())
            {
                _errorService.AddError("radiobuttontextinput", "You must add a note");
                hasValidationErrors = true;
            }

            if (!IsUnableToAssessNotesValid())
            {
                _errorService.AddError("radiobuttontextinput", "You must add a note");
                hasValidationErrors = true;
            }

            if (hasValidationErrors)
            {
                RadioButtonModels = RadioButtons;
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            // Determine which notes to pass to the command based on the selection
            string? notesToPass = HasSchoolMatchedWithSupportingOrganisation switch
            {
                "Review school's progress" => NotMatchingNotes,
                "Unable to assess" => UnableToAssessNotes,
                _ => null
            };

            var request = new SetRecordInitialDiagnosisDecisionCommand(new SupportProjectId(id), RegionalDirectorDecisionDate, HasSchoolMatchedWithSupportingOrganisation, notesToPass);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            TaskUpdated = true;
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }

        private IList<RadioButtonsLabelViewModel> RadioButtons
        {
            get
            {
                var list = new List<RadioButtonsLabelViewModel>
                {
                    new() {
                        Id = "match-with-organisation",
                        Name = "Match with a supporting organisation",
                        Value = "Match with a supporting organisation"
                    },
                    new() {
                        Id = "review-school-progress",
                        Name = "Review school's progress",
                        Value = "Review school's progress",
                        Input = new TextFieldInputViewModel
                        {
                            Id = nameof(NotMatchingNotes),
                            ValidationMessage = "You must add a note",
                            Paragraph = "Copy and paste your notes from the overall recommendation within the summary tab in Assessment Tool 1.",
                            Value = NotMatchingNotes,
                            IsValid = IsNotMatchingNotesValid(),
                            IsTextArea = true
                        }
                    },
                    new() {
                        Id = "unable-to-assess",
                        Name = "Unable to assess",
                        Value = "Unable to assess",
                        Input = new TextFieldInputViewModel
                        {
                            Id = nameof(UnableToAssessNotes),
                            ValidationMessage = "You must add a note",
                            Paragraph = "Copy and paste your notes from the overall recommendation within the summary tab in Assessment Tool 1.",
                            Value = UnableToAssessNotes,
                            IsValid = IsUnableToAssessNotesValid(),
                            IsTextArea = true
                        }
                    }
                };

                return list;
            }
        }

        private bool IsNotMatchingNotesValid()
        {
            if (HasSchoolMatchedWithSupportingOrganisation == "Review school's progress" && string.IsNullOrWhiteSpace(NotMatchingNotes))
            {
                return false;
            }
            return true;
        }

        private bool IsUnableToAssessNotesValid()
        {
            if (HasSchoolMatchedWithSupportingOrganisation == "Unable to assess" && string.IsNullOrWhiteSpace(UnableToAssessNotes))
            {
                return false;
            }
            return true;
        }
    }
}
