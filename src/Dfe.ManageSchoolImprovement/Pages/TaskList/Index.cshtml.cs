using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList;

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }

    public TaskListStatus ConfirmEligibilityTaskListStatus { get; set; }
    public TaskListStatus InitialContactWithResponsibleBodyTaskListStatus { get; set; }
    public TaskListStatus SendFormalNotificationTaskListStatus { get; set; }
    public TaskListStatus RecordTheSchoolResponseTaskListStatus { get; set; }
    public TaskListStatus CheckThePotentialAdviserConflictsOfInterestTaskListStatus { get; set; }
    public TaskListStatus SendIntroductoryEmailTaskListStatus { get; set; }
    public TaskListStatus AllocateAdviserTaskListStatus { get; set; }

    public TaskListStatus ArrangeAdvisersFirstFaceToFaceVisitTaskListStatus { get; set; }
    public TaskListStatus CompleteAndSaveInitialDiagnosisTemplateTaskListStatus { get; set; }
    public TaskListStatus NoteOfVisitTaskListStatus { get; set; }
    public TaskListStatus RecordVisitDateToVisitSchoolTaskListStatus { get; set; }

    public TaskListStatus ChosePreferredSupportingOrganisationTaskListStatus { get; set; }
    public TaskListStatus RecordSupportDecisionTaskListStatus { get; set; }
    public TaskListStatus DueDiligenceOnPreferredSupportingOrganisationTaskListStatus { get; set; }
    public TaskListStatus SetRecordSupportingOrganisationAppointment { get; set; }

    public TaskListStatus SupportingOrganisationContactDetailsTaskListStatus { get; set; }
    public TaskListStatus ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus { get; set; }

    public TaskListStatus RecordImprovementPlanDecisionTaskListStatus { get; set; }

    public TaskListStatus SendAgreedImprovementPlanForApprovalTaskListStatus { get; set; }
    public TaskListStatus RequestPlanningGrantOfferLetterTaskListStatus { get; set; }

    public TaskListStatus ConfirmPlanningGrantOfferLetterTaskListStatus { get; set; }

    public TaskListStatus ReviewTheImprovementPlanTaskListStatus { get; set; }

    public TaskListStatus RequestImprovementGrantOfferLetterTaskListStatus { get; set; }
    public TaskListStatus ConfirmImprovementGrantOfferLetterTaskListStatus { get; set; }
    public TaskListStatus FundingHistoryStatus { get; set; }
    public TaskListStatus EnterImprovementPlanObjectivesTaskListStatus { get; set; }

    public bool ProjectNotYetAssigned { get; set; }
    
    public bool? AdviserCanBeSet { get; set; }
    
    public bool? AdviserSet { get; set; }

    public void SetErrorPage(string errorPage)
    {
        TempData["ErrorPage"] = errorPage;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject != null)
        {
            var projectStatusPausedOrStopped = SupportProject.ProjectStatus != ProjectStatusValue.InProgress;
            ProjectNotYetAssigned = !SupportProject.InitialDeliveryOfficerAssigned;
            AdviserCanBeSet = SupportProject.AdviserCanBeSet;

            AdviserSet = !string.IsNullOrWhiteSpace(SupportProject.AdviserFullName);
            
            // phase one tasks
            ConfirmEligibilityTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.ConfirmEligibilityTaskListStatus(SupportProject);
            
            if (projectStatusPausedOrStopped)
            {
                FundingHistoryStatus = TaskListStatus.CannotProgress;
                InitialContactWithResponsibleBodyTaskListStatus = TaskListStatus.CannotProgress;
                CheckThePotentialAdviserConflictsOfInterestTaskListStatus = TaskListStatus.CannotProgress;
                SendFormalNotificationTaskListStatus = TaskListStatus.CannotProgress;
                RecordTheSchoolResponseTaskListStatus = TaskListStatus.CannotProgress;
            }
            else if (ProjectNotYetAssigned)
            {
                FundingHistoryStatus = TaskListStatus.CannotStartYet;
                InitialContactWithResponsibleBodyTaskListStatus = TaskListStatus.CannotStartYet;
                CheckThePotentialAdviserConflictsOfInterestTaskListStatus = TaskListStatus.CannotStartYet;
                SendFormalNotificationTaskListStatus = TaskListStatus.CannotStartYet;
                RecordTheSchoolResponseTaskListStatus = TaskListStatus.CannotStartYet;
            }
            else
            {
                FundingHistoryStatus = TaskStatusViewModel.FundingHistoryTaskListStatus(SupportProject);
                InitialContactWithResponsibleBodyTaskListStatus =
                    TaskStatusViewModel.ContactedTheResponsibleBodyTaskStatus(SupportProject);
                CheckThePotentialAdviserConflictsOfInterestTaskListStatus =
                    TaskStatusViewModel.CheckThePotentialAdviserConflictsOfInterestTaskListStatus(SupportProject);
                SendFormalNotificationTaskListStatus =
                    TaskStatusViewModel.SendFormalNotificationTaskStatus(SupportProject);
                RecordTheSchoolResponseTaskListStatus =
                    TaskStatusViewModel.ResponsibleBodyResponseToTheConflictOfInterestRequestStatus(SupportProject);
            }

            if (projectStatusPausedOrStopped)
            {
                AllocateAdviserTaskListStatus = TaskListStatus.CannotProgress;
            }
            else if (AdviserCanBeSet != true)
            {
                AllocateAdviserTaskListStatus = TaskListStatus.CannotStartYet;
            }
            else
            {
                AllocateAdviserTaskListStatus = TaskStatusViewModel.CheckAllocateAdviserTaskListStatus(SupportProject);
            }

            if(projectStatusPausedOrStopped)
            {
                // Phase 2
                SendIntroductoryEmailTaskListStatus = TaskListStatus.CannotProgress;
                ArrangeAdvisersFirstFaceToFaceVisitTaskListStatus = TaskListStatus.CannotProgress;
                RecordVisitDateToVisitSchoolTaskListStatus = TaskListStatus.CannotProgress;
                CompleteAndSaveInitialDiagnosisTemplateTaskListStatus = TaskListStatus.CannotProgress;
                RecordSupportDecisionTaskListStatus = TaskListStatus.CannotProgress;
                ChosePreferredSupportingOrganisationTaskListStatus = TaskListStatus.CannotProgress;
                DueDiligenceOnPreferredSupportingOrganisationTaskListStatus = TaskListStatus.CannotProgress;
                SetRecordSupportingOrganisationAppointment = TaskListStatus.CannotProgress;
                SupportingOrganisationContactDetailsTaskListStatus = TaskListStatus.CannotProgress;

                // Phase 3
                RequestPlanningGrantOfferLetterTaskListStatus = TaskListStatus.CannotProgress;
                ConfirmPlanningGrantOfferLetterTaskListStatus = TaskListStatus.CannotProgress;
                ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus = TaskListStatus.CannotProgress;
                ReviewTheImprovementPlanTaskListStatus = TaskListStatus.CannotProgress;
                SendAgreedImprovementPlanForApprovalTaskListStatus = TaskListStatus.CannotProgress;
                RecordImprovementPlanDecisionTaskListStatus = TaskListStatus.CannotProgress;
                EnterImprovementPlanObjectivesTaskListStatus = TaskListStatus.CannotProgress;
                RequestImprovementGrantOfferLetterTaskListStatus = TaskListStatus.CannotProgress;
                ConfirmImprovementGrantOfferLetterTaskListStatus = TaskListStatus.CannotProgress;
            }

            else if (AdviserSet != true)
            {
                //phase 2
                SendIntroductoryEmailTaskListStatus = TaskListStatus.CannotStartYet;
                ArrangeAdvisersFirstFaceToFaceVisitTaskListStatus = TaskListStatus.CannotStartYet;
                RecordVisitDateToVisitSchoolTaskListStatus = TaskListStatus.CannotStartYet;
                CompleteAndSaveInitialDiagnosisTemplateTaskListStatus = TaskListStatus.CannotStartYet;
                RecordSupportDecisionTaskListStatus = TaskListStatus.CannotStartYet;
                ChosePreferredSupportingOrganisationTaskListStatus = TaskListStatus.CannotStartYet;
                DueDiligenceOnPreferredSupportingOrganisationTaskListStatus = TaskListStatus.CannotStartYet;
                SetRecordSupportingOrganisationAppointment = TaskListStatus.CannotStartYet;
                SupportingOrganisationContactDetailsTaskListStatus = TaskListStatus.CannotStartYet;

                // Phase 3
                RequestPlanningGrantOfferLetterTaskListStatus = TaskListStatus.CannotStartYet;
                ConfirmPlanningGrantOfferLetterTaskListStatus = TaskListStatus.CannotStartYet;
                ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus = TaskListStatus.CannotStartYet;
                ReviewTheImprovementPlanTaskListStatus = TaskListStatus.CannotStartYet;
                SendAgreedImprovementPlanForApprovalTaskListStatus = TaskListStatus.CannotStartYet;
                RecordImprovementPlanDecisionTaskListStatus = TaskListStatus.CannotStartYet;
                EnterImprovementPlanObjectivesTaskListStatus = TaskListStatus.CannotStartYet;
                RequestImprovementGrantOfferLetterTaskListStatus = TaskListStatus.CannotStartYet;
                ConfirmImprovementGrantOfferLetterTaskListStatus = TaskListStatus.CannotStartYet;
            }
            // Phase two tasks

            else
            {
                // Phase 2
                SendIntroductoryEmailTaskListStatus =
                    TaskStatusViewModel.SendIntroductoryEmailTaskListStatus(SupportProject);
                ArrangeAdvisersFirstFaceToFaceVisitTaskListStatus =
                    TaskStatusViewModel.AdviserVisitToSchoolTaskListStatus(SupportProject);
                RecordVisitDateToVisitSchoolTaskListStatus =
                    TaskStatusViewModel.RecordVisitDateToVisitSchoolTaskListStatus(SupportProject);
                CompleteAndSaveInitialDiagnosisTemplateTaskListStatus =
                    TaskStatusViewModel.CompleteAndSaveInitialDiagnosisTemplateTaskListStatus(SupportProject);
                RecordSupportDecisionTaskListStatus =
                    TaskStatusViewModel.RecordInitialDiagnosisDecisionTaskListStatus(SupportProject);
                ChosePreferredSupportingOrganisationTaskListStatus =
                    TaskStatusViewModel.ChoosePreferredSupportingOrganisationTaskListStatus(SupportProject);
                DueDiligenceOnPreferredSupportingOrganisationTaskListStatus =
                    TaskStatusViewModel.DueDiligenceOnPreferredSupportingOrganisationTaskListStatus(SupportProject);
                SetRecordSupportingOrganisationAppointment =
                    TaskStatusViewModel.SetRecordSupportingOrganisationAppointmentTaskListStatus(SupportProject);
                SupportingOrganisationContactDetailsTaskListStatus =
                    TaskStatusViewModel.SupportingOrganisationContactDetailsTaskListStatus(SupportProject);

                // Phase 3
                RequestPlanningGrantOfferLetterTaskListStatus =
                    TaskStatusViewModel.RequestPlanningGrantOfferLetterTaskListStatus(SupportProject);
                ConfirmPlanningGrantOfferLetterTaskListStatus =
                    TaskStatusViewModel.ConfirmPlanningGrantOfferLetterTaskListStatus(SupportProject);
                ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus =
                    TaskStatusViewModel.ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus(
                        SupportProject);
                ReviewTheImprovementPlanTaskListStatus =
                    TaskStatusViewModel.ReviewTheImprovementPlanTaskListStatus(SupportProject);
                SendAgreedImprovementPlanForApprovalTaskListStatus =
                    TaskStatusViewModel.SendAgreedImprovementPlanForApprovalTaskListStatus(SupportProject);
                RecordImprovementPlanDecisionTaskListStatus =
                    TaskStatusViewModel.RecordImprovementPlanDecisionTaskListStatus(SupportProject);
                EnterImprovementPlanObjectivesTaskListStatus =
                    TaskStatusViewModel.EnterImprovementPlanObjectivesTaskListStatus(SupportProject);
                RequestImprovementGrantOfferLetterTaskListStatus =
                    TaskStatusViewModel.RequestImprovementGrantOfferLetterTaskListStatus(SupportProject);
                ConfirmImprovementGrantOfferLetterTaskListStatus =
                    TaskStatusViewModel.ConfirmImprovementGrantOfferLetterTaskListStatus(SupportProject);
            }
        }
        return Page();
    }
}