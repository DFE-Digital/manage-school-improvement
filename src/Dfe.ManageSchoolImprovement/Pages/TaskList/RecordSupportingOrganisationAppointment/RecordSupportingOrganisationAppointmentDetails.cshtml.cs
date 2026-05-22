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
    public class RecordSupportingOrganisationAppointmentDetailsModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService,
        IMediator mediator,
        IApplicationSettingsResourceService applicationSettingsResourceService)
        : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = "appointment-date", BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "Date the regional director made this appointment")]
        public DateTime? RegionalDirectorAppointmentDate { get; set; }

        [BindProperty(Name = "assessment-tool-two-completed")]
        [Display(Name = "Assessment tool 2 has been completed and uploaded")]
        public bool? AssessmentToolTwoCompleted { get; set; }

        public string AssessmentToolTwoLink { get; set; } = string.Empty;
        public string AssessmentToolTwoSharePointFolderLink { get; set; } = string.Empty;

        public bool ShowError { get; set; }

        public string? AssessmentToolTwoErrorMessage { get; set; }
        public static string CompleteAssessmentToolError => "complete-assessment-tool";

        public bool ShowCompleteAssessmentToolError => ModelState.ContainsKey(CompleteAssessmentToolError) &&
                                                       ModelState[CompleteAssessmentToolError].Errors.Count > 0;


        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }

        string IDateValidationMessageProvider.AllMissing => "Enter a date";

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (SupportProject != null)
            {
                RegionalDirectorAppointmentDate = SupportProject.RegionalDirectorAppointmentDate;
                AssessmentToolTwoCompleted = SupportProject.AssessmentToolTwoCompleted;
            }

            await LoadSharePointLinksAsync(cancellationToken);
            return Page();
        }

        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (AssessmentToolTwoCompleted == null || AssessmentToolTwoCompleted == false)
            {
                AssessmentToolTwoErrorMessage = "Confirm you have completed the assessment tool";
                ModelState.AddModelError(CompleteAssessmentToolError, AssessmentToolTwoErrorMessage);
                _errorService.AddError(CompleteAssessmentToolError, AssessmentToolTwoErrorMessage);
            }
            
            if (!RegionalDirectorAppointmentDate.HasValue)
            {
                ModelState.AddModelError("appointment-date", "Enter a date");
            }

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new SetRecordSupportingOrganisationAppointmentCommand(new SupportProjectId(id),
                RegionalDirectorAppointmentDate, SupportProject?.HasConfirmedSupportingOrganisationAppointment,
                SupportProject?.DisapprovingSupportingOrganisationAppointmentNotes,
                AssessmentToolTwoCompleted);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            if (RegionalDirectorAppointmentDate.HasValue
                && RegionalDirectorAppointmentDate.Value < DateTime.UtcNow
                && SupportProject!.InitialDiagnosisMatchingDecision == "Match with a supporting organisation")
            {
                await base.UpdateCurrentDeliveryMilestone(id, SupportProject!.CurrentDeliveryMilestone,
                    Milestone.MatchingComplete, cancellationToken);
            }

            TaskUpdated = true;
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }


        private async Task LoadSharePointLinksAsync(CancellationToken cancellationToken)
        {
            AssessmentToolTwoLink =
                await applicationSettingsResourceService.GetAssessmentToolTwoLinkAsync(cancellationToken) ??
                string.Empty;
            AssessmentToolTwoSharePointFolderLink =
                await applicationSettingsResourceService.GetAssessmentToolTwoSharePointFolderLinkAsync(
                    cancellationToken) ?? string.Empty;
        }
    }
}