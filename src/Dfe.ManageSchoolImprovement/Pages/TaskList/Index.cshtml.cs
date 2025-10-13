using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
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

        InitialContactWithResponsibleBodyTaskListStatus = TaskStatusViewModel.ContactedTheResponsibleBodyTaskStatus(SupportProject);
        RecordTheSchoolResponseTaskListStatus = TaskStatusViewModel.ResponsibleBodyResponseToTheConflictOfInterestRequestStatus(SupportProject);
        CheckThePotentialAdviserConflictsOfInterestTaskListStatus = TaskStatusViewModel.CheckThePotentialAdviserConflictsOfInterestTaskListStatus(SupportProject);
        SendFormalNotificationTaskListStatus = TaskStatusViewModel.SendFormalNotificationTaskStatus(SupportProject);
        AllocateAdviserTaskListStatus = TaskStatusViewModel.CheckAllocateAdviserTaskListStatus(SupportProject);
        SendIntroductoryEmailTaskListStatus = TaskStatusViewModel.SendIntroductoryEmailTaskListStatus(SupportProject);
        ArrangeAdvisersFirstFaceToFaceVisitTaskListStatus = TaskStatusViewModel.AdviserVisitToSchoolTaskListStatus(SupportProject);
        CompleteAndSaveInitialDiagnosisTemplateTaskListStatus = TaskStatusViewModel.CompleteAndSaveInitialDiagnosisTemplateTaskListStatus(SupportProject);
        RecordVisitDateToVisitSchoolTaskListStatus = TaskStatusViewModel.RecordVisitDateToVisitSchoolTaskListStatus(SupportProject);
        ChosePreferredSupportingOrganisationTaskListStatus =
            TaskStatusViewModel.ChoosePreferredSupportingOrganisationTaskListStatus(SupportProject);
        RecordSupportDecisionTaskListStatus = TaskStatusViewModel.RecordInitialDiagnosisDecisionTaskListStatus(SupportProject);
        DueDiligenceOnPreferredSupportingOrganisationTaskListStatus = TaskStatusViewModel.DueDiligenceOnPreferredSupportingOrganisationTaskListStatus(SupportProject);
        SetRecordSupportingOrganisationAppointment = TaskStatusViewModel.SetRecordSupportingOrganisationAppointmentTaskListStatus(SupportProject);
        SupportingOrganisationContactDetailsTaskListStatus =
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
        ConfirmEligibilityTaskListStatus = TaskStatusViewModel.ConfirmEligibilityTaskListStatus(SupportProject);
        FundingHistoryStatus = TaskStatusViewModel.FundingHistoryTaskListStatus(SupportProject);
        EnterImprovementPlanObjectivesTaskListStatus = TaskStatusViewModel.EnterImprovementPlanObjectivesTaskListStatus(SupportProject);
        return Page();

    }
}
