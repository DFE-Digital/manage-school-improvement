using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, IGetEstablishment getEstablishment, ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
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

    public void SetErrorPage(string errorPage)
    {
        TempData["ErrorPage"] = errorPage;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        var projectStatusPausedOrStopped = SupportProject.ProjectStatus != ProjectStatusValue.InProgress;

        InitialContactWithResponsibleBodyTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.ContactedTheResponsibleBodyTaskStatus(SupportProject);
        RecordTheSchoolResponseTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.ResponsibleBodyResponseToTheConflictOfInterestRequestStatus(SupportProject);
        CheckThePotentialAdviserConflictsOfInterestTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.CheckThePotentialAdviserConflictsOfInterestTaskListStatus(SupportProject);
        SendFormalNotificationTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.SendFormalNotificationTaskStatus(SupportProject);
        AllocateAdviserTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.CheckAllocateAdviserTaskListStatus(SupportProject);
        SendIntroductoryEmailTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.SendIntroductoryEmailTaskListStatus(SupportProject);
        ArrangeAdvisersFirstFaceToFaceVisitTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.AdviserVisitToSchoolTaskListStatus(SupportProject);
        CompleteAndSaveInitialDiagnosisTemplateTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.CompleteAndSaveInitialDiagnosisTemplateTaskListStatus(SupportProject);
        RecordVisitDateToVisitSchoolTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.RecordVisitDateToVisitSchoolTaskListStatus(SupportProject);
        ChosePreferredSupportingOrganisationTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : 
            TaskStatusViewModel.ChoosePreferredSupportingOrganisationTaskListStatus(SupportProject);
        RecordSupportDecisionTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.RecordInitialDiagnosisDecisionTaskListStatus(SupportProject);
        DueDiligenceOnPreferredSupportingOrganisationTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.DueDiligenceOnPreferredSupportingOrganisationTaskListStatus(SupportProject);
        SetRecordSupportingOrganisationAppointment = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.SetRecordSupportingOrganisationAppointmentTaskListStatus(SupportProject);
        SupportingOrganisationContactDetailsTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : 
            TaskStatusViewModel.SupportingOrganisationContactDetailsTaskListStatus(SupportProject);
        ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus = TaskStatusViewModel.ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus(SupportProject);
        RecordImprovementPlanDecisionTaskListStatus = TaskStatusViewModel.RecordImprovementPlanDecisionTaskListStatus(SupportProject);
        SendAgreedImprovementPlanForApprovalTaskListStatus = TaskStatusViewModel.SendAgreedImprovementPlanForApprovalTaskListStatus(SupportProject);
        RequestPlanningGrantOfferLetterTaskListStatus = TaskStatusViewModel.RequestPlanningGrantOfferLetterTaskListStatus(SupportProject);
        ConfirmPlanningGrantOfferLetterTaskListStatus =
            TaskStatusViewModel.ConfirmPlanningGrantOfferLetterTaskListStatus(SupportProject);
        ReviewTheImprovementPlanTaskListStatus =
            TaskStatusViewModel.ReviewTheImprovementPlanTaskListStatus(SupportProject);
        RequestImprovementGrantOfferLetterTaskListStatus = TaskStatusViewModel.RequestImprovementGrantOfferLetterTaskListStatus(SupportProject);
        ConfirmImprovementGrantOfferLetterTaskListStatus = TaskStatusViewModel.ConfirmImprovementGrantOfferLetterTaskListStatus(SupportProject);
        ConfirmEligibilityTaskListStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.ConfirmEligibilityTaskListStatus(SupportProject);
        FundingHistoryStatus = projectStatusPausedOrStopped ? TaskListStatus.CannotProgress : TaskStatusViewModel.FundingHistoryTaskListStatus(SupportProject);
        EnterImprovementPlanObjectivesTaskListStatus = TaskStatusViewModel.EnterImprovementPlanObjectivesTaskListStatus(SupportProject);
        return Page();

    }
}
