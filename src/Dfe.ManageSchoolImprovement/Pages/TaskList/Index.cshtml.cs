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

            // Phase two tasks
            SendIntroductoryEmailTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.SendIntroductoryEmailTaskListStatus(SupportProject);
            ArrangeAdvisersFirstFaceToFaceVisitTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.AdviserVisitToSchoolTaskListStatus(SupportProject);
            RecordVisitDateToVisitSchoolTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.RecordVisitDateToVisitSchoolTaskListStatus(SupportProject);
            CompleteAndSaveInitialDiagnosisTemplateTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.CompleteAndSaveInitialDiagnosisTemplateTaskListStatus(SupportProject);
            RecordSupportDecisionTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.RecordInitialDiagnosisDecisionTaskListStatus(SupportProject);
            ChosePreferredSupportingOrganisationTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.ChoosePreferredSupportingOrganisationTaskListStatus(SupportProject);
            DueDiligenceOnPreferredSupportingOrganisationTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.DueDiligenceOnPreferredSupportingOrganisationTaskListStatus(SupportProject);
            SetRecordSupportingOrganisationAppointment = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.SetRecordSupportingOrganisationAppointmentTaskListStatus(SupportProject);
            SupportingOrganisationContactDetailsTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.SupportingOrganisationContactDetailsTaskListStatus(SupportProject);

            // Phase three tasks
            RequestPlanningGrantOfferLetterTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.RequestPlanningGrantOfferLetterTaskListStatus(SupportProject);
            ConfirmPlanningGrantOfferLetterTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.ConfirmPlanningGrantOfferLetterTaskListStatus(SupportProject);
            ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus(
                    SupportProject);
            ReviewTheImprovementPlanTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.ReviewTheImprovementPlanTaskListStatus(SupportProject);
            SendAgreedImprovementPlanForApprovalTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.SendAgreedImprovementPlanForApprovalTaskListStatus(SupportProject);
            RecordImprovementPlanDecisionTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.RecordImprovementPlanDecisionTaskListStatus(SupportProject);
            EnterImprovementPlanObjectivesTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.EnterImprovementPlanObjectivesTaskListStatus(SupportProject);
            RequestImprovementGrantOfferLetterTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.RequestImprovementGrantOfferLetterTaskListStatus(SupportProject);
            ConfirmImprovementGrantOfferLetterTaskListStatus = projectStatusPausedOrStopped
                ? TaskListStatus.CannotProgress
                : TaskStatusViewModel.ConfirmImprovementGrantOfferLetterTaskListStatus(SupportProject);
        }

        return Page();
    }
}