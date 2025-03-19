using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.ViewModels;

public static class TaskStatusViewModel
{
    public static TaskListStatus ContactedTheSchoolTaskStatus(SupportProjectViewModel SupportProject)
    {
        if (SupportProject.AttachRiseInfoToEmail.Equals(true) &&
            SupportProject.FindSchoolEmailAddress.Equals(true) &&
            SupportProject.UseTheNotificationLetterToCreateEmail.Equals(true) &&
            SupportProject.ContactedTheSchoolDate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (SupportProject.AttachRiseInfoToEmail.Equals(false) &&
            SupportProject.FindSchoolEmailAddress.Equals(false) &&
            SupportProject.UseTheNotificationLetterToCreateEmail.Equals(false) &&
            !SupportProject.ContactedTheSchoolDate.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus RecordTheSchoolResponseTaskStatus(SupportProjectViewModel SupportProject)
    {
        if (SupportProject.HasSavedSchoolResponseinSharePoint.Equals(true) &&
            SupportProject.HasAcceptedTargetedSupport.Equals(true) &&
            SupportProject.SchoolResponseDate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!SupportProject.HasSavedSchoolResponseinSharePoint.HasValue &&
            !SupportProject.HasAcceptedTargetedSupport.HasValue &&
            !SupportProject.SchoolResponseDate.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus CheckThePotentialAdviserConflictsOfInterestTaskListStatus(
        SupportProjectViewModel supportProject)
    {
        if (supportProject.SendConflictOfInterestFormToProposedAdviserAndTheSchool.HasValue
            && supportProject.ReceiveCompletedConflictOfInterestForm.HasValue
            && supportProject.SaveCompletedConflictOfinterestFormInSharePoint.HasValue
            && supportProject.DateConflictsOfInterestWereChecked.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.SendConflictOfInterestFormToProposedAdviserAndTheSchool.HasValue
            && !supportProject.ReceiveCompletedConflictOfInterestForm.HasValue
            && !supportProject.SaveCompletedConflictOfinterestFormInSharePoint.HasValue
            && !supportProject.DateConflictsOfInterestWereChecked.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus CheckAllocateAdviserTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.AdviserEmailAddress != null
            && supportProject.DateAdviserAllocated.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (supportProject.AdviserEmailAddress == null
            && !supportProject.DateAdviserAllocated.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus SendIntroductoryEmailTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.HasShareEmailTemplateWithAdviser.HasValue
            && supportProject.RemindAdviserToCopyRiseTeamWhenSentEmail.HasValue
            && supportProject.IntroductoryEmailSentDate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.HasShareEmailTemplateWithAdviser.HasValue
            && !supportProject.RemindAdviserToCopyRiseTeamWhenSentEmail.HasValue
            && !supportProject.IntroductoryEmailSentDate.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus AdviserVisitToSchoolTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.AdviserVisitDate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        return TaskListStatus.NotStarted;
    }

    public static TaskListStatus CompleteAndSaveAssessmentTemplateTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.SavedAssessmentTemplateInSharePointDate.HasValue
            && supportProject.HasTalkToAdviserAboutFindings.HasValue
            && supportProject.HasCompleteAssessmentTemplate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.SavedAssessmentTemplateInSharePointDate.HasValue
            && !supportProject.HasTalkToAdviserAboutFindings.HasValue
            && !supportProject.HasCompleteAssessmentTemplate.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus NoteOfVsistTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.AskTheAdviserToSendYouTheirNotes.HasValue
            && supportProject.GiveTheAdviserTheNoteOfVisitTemplate.HasValue
            && supportProject.DateNoteOfVisitSavedInSharePoint.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.AskTheAdviserToSendYouTheirNotes.HasValue
            && !supportProject.GiveTheAdviserTheNoteOfVisitTemplate.HasValue
            && !supportProject.DateNoteOfVisitSavedInSharePoint.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }
    public static TaskListStatus RecordVisitDateToVisitSchoolTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.SchoolVisitDate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        return TaskListStatus.NotStarted;
    }

    public static TaskListStatus ChoosePreferredSupportingOrganisationTaskListStatus(
        SupportProjectViewModel supportProject)
    {
        if (supportProject.DateSupportOrganisationChosen.HasValue
            && supportProject.SupportOrganisationName != null
            && !string.IsNullOrWhiteSpace(supportProject.SupportOrganisationIdNumber))
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateSupportOrganisationChosen.HasValue
            && supportProject.SupportOrganisationName == null
            && string.IsNullOrWhiteSpace(supportProject.SupportOrganisationIdNumber))
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }
    public static TaskListStatus RecordSupportDecisionTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.RegionalDirectorDecisionDate.HasValue
            && supportProject.HasSchoolMatchedWithSupportingOrganisation.HasValue
            && supportProject.HasSchoolMatchedWithSupportingOrganisation.Equals(true))
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.RegionalDirectorDecisionDate.HasValue
            && !supportProject.HasSchoolMatchedWithSupportingOrganisation.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus DueDiligenceOnPreferredSupportingOrganisationTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.CheckOrganisationHasCapacityAndWillingToProvideSupport.HasValue &&
           supportProject.CheckChoiceWithTrustRelationshipManagerOrLaLead.HasValue &&
           supportProject.DiscussChoiceWithSfso.HasValue &&
           supportProject.CheckFinancialConcernsAtSupportingOrganisation.HasValue &&
           supportProject.CheckTheOrganisationHasAVendorAccount.HasValue &&
           supportProject.DateDueDiligenceCompleted.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.CheckOrganisationHasCapacityAndWillingToProvideSupport.HasValue &&
           !supportProject.CheckChoiceWithTrustRelationshipManagerOrLaLead.HasValue &&
           !supportProject.DiscussChoiceWithSfso.HasValue &&
           !supportProject.CheckFinancialConcernsAtSupportingOrganisation.HasValue &&
           !supportProject.CheckTheOrganisationHasAVendorAccount.HasValue &&
           !supportProject.DateDueDiligenceCompleted.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus SupportingOrganisationContactDetailsTaskListStatus(
        SupportProjectViewModel supportProject)
    {
        if (supportProject.DateSupportingOrganisationContactDetailsAdded.HasValue
            && supportProject.SupportingOrganisationContactName != null
            && supportProject.SupportingOrganisationContactEmailAddress != null)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateSupportingOrganisationContactDetailsAdded.HasValue
            && supportProject.SupportingOrganisationContactName == null
            && string.IsNullOrEmpty(supportProject.SupportingOrganisationContactEmailAddress))
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus SetRecordSupportingOrganisationAppointmentTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.RegionalDirectorAppointmentDate.HasValue
            && supportProject.HasConfirmedSupportingOrganisationAppointment.HasValue
            && supportProject.HasConfirmedSupportingOrganisationAppointment.Equals(true))
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.RegionalDirectorAppointmentDate.HasValue
            && !supportProject.HasConfirmedSupportingOrganisationAppointment.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus ShareTheImprovementPlanTemplateTaskListStatus(SupportProjectViewModel supportProject)
    {
        {
            if (supportProject.SendTheTemplateToTheSupportingOrganisation.HasValue
                && supportProject.SendTheTemplateToTheSchoolsResponsibleBody.HasValue
                && supportProject.DateTemplatesSent.HasValue)
            {
                return TaskListStatus.Complete;
            }

            if (!supportProject.SendTheTemplateToTheSupportingOrganisation.HasValue
                && !supportProject.SendTheTemplateToTheSchoolsResponsibleBody.HasValue
                && !supportProject.DateTemplatesSent.HasValue)
            {
                return TaskListStatus.NotStarted;
            }

            return TaskListStatus.InProgress;
        }
    }

    public static TaskListStatus RecordImprovementPlanDecisionTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.RegionalDirectorImprovementPlanDecisionDate.HasValue
            && supportProject.HasApprovedImprovementPlanDecision.HasValue
            && supportProject.HasApprovedImprovementPlanDecision.Equals(true))
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.RegionalDirectorImprovementPlanDecisionDate.HasValue
            && !supportProject.HasApprovedImprovementPlanDecision.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus SendAgreedImprovementPlanForApprovalTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.HasSavedImprovementPlanInSharePoint.HasValue
            && supportProject.HasSavedImprovementPlanInSharePoint.Equals(true)
            && supportProject.HasEmailedAgreedPlanToRegionalDirectorForApproval.HasValue
            && supportProject.HasEmailedAgreedPlanToRegionalDirectorForApproval.Equals(true))
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.HasEmailedAgreedPlanToRegionalDirectorForApproval.HasValue
            && !supportProject.HasSavedImprovementPlanInSharePoint.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }
    public static TaskListStatus RequestPlanningGrantOfferLetterTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.DateTeamContactedForRequestingPlanningGrantOfferLetter.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateTeamContactedForRequestingPlanningGrantOfferLetter.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus ReviewTheImprovementPlanTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.ImprovementPlanReceivedDate.HasValue
            && supportProject.ReviewImprovementPlanWithTeam.Equals(true))

        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.ImprovementPlanReceivedDate.HasValue
            && !supportProject.ReviewImprovementPlanWithTeam.Equals(true))
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus RequestImprovementGrantOfferLetterTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.DateTeamContactedForRequestingImprovementGrantOfferLetter.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateTeamContactedForRequestingImprovementGrantOfferLetter.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus ConfirmPlanningGrantOfferLetterTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.DateTeamContactedForConfirmingPlanningGrantOfferLetter.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateTeamContactedForConfirmingPlanningGrantOfferLetter.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus ConfirmImprovementGrantOfferLetterTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.DateImprovementGrantOfferLetterSent.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateImprovementGrantOfferLetterSent.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }
}
