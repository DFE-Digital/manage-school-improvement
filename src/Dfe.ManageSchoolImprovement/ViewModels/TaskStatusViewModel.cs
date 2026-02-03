using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.ViewModels;

public static class TaskStatusViewModel
{
    public static TaskListStatus ContactedTheResponsibleBodyTaskStatus(SupportProjectViewModel SupportProject)
    {
        if (SupportProject.InitialContactResponsibleBody.Equals(true) &&
            SupportProject.InitialContactResponsibleBodyDate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (SupportProject.InitialContactResponsibleBody.Equals(null) &&
            !SupportProject.InitialContactResponsibleBodyDate.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus SendFormalNotificationTaskStatus(SupportProjectViewModel SupportProject)
    {
        if (SupportProject.UseEnrolmentLetterTemplateToDraftEmail.Equals(true) &&
            SupportProject.AttachTargetedInterventionInformationSheet.Equals(true) &&
            SupportProject.AddRecipientsForFormalNotification.Equals(true) &&
            SupportProject.FormalNotificationSent.Equals(true) &&
            SupportProject.DateFormalNotificationSent.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (SupportProject.UseEnrolmentLetterTemplateToDraftEmail.Equals(null) &&
            SupportProject.AttachTargetedInterventionInformationSheet.Equals(null) &&
            SupportProject.AddRecipientsForFormalNotification.Equals(null) &&
            SupportProject.FormalNotificationSent.Equals(null) &&
            !SupportProject.DateFormalNotificationSent.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus ResponsibleBodyResponseToTheConflictOfInterestRequestStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint.HasValue &&
            supportProject.ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint.HasValue &&
            !supportProject.ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus CheckThePotentialAdviserConflictsOfInterestTaskListStatus(
        SupportProjectViewModel supportProject)
    {
        if (supportProject.ReviewAdvisersConflictOfInterestForm.HasValue
            && supportProject.DateConflictOfInterestDeclarationChecked.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.ReviewAdvisersConflictOfInterestForm.HasValue
            && !supportProject.DateConflictOfInterestDeclarationChecked.HasValue)
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
        if (supportProject.RemindAdviserToCopyRiseTeamWhenSentEmail.HasValue
            && supportProject.IntroductoryEmailSentDate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.RemindAdviserToCopyRiseTeamWhenSentEmail.HasValue
            && !supportProject.IntroductoryEmailSentDate.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus AdviserVisitToSchoolTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.AdviserVisitDate.HasValue
            && supportProject.GiveTheAdviserTheNoteOfVisitTemplate.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.AdviserVisitDate.HasValue
            && !supportProject.GiveTheAdviserTheNoteOfVisitTemplate.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus CompleteAndSaveInitialDiagnosisTemplateTaskListStatus(SupportProjectViewModel supportProject)
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
            && !string.IsNullOrWhiteSpace(supportProject.SupportOrganisationName)
            && !string.IsNullOrWhiteSpace(supportProject.SupportOrganisationIdNumber)
            && supportProject.AssessmentToolTwoCompleted == true)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateSupportOrganisationChosen.HasValue
            && string.IsNullOrWhiteSpace(supportProject.SupportOrganisationName)
            && string.IsNullOrWhiteSpace(supportProject.SupportOrganisationIdNumber)
            && supportProject.AssessmentToolTwoCompleted == null)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus RecordInitialDiagnosisDecisionTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.RegionalDirectorDecisionDate.HasValue
            && !string.IsNullOrEmpty(supportProject.InitialDiagnosisMatchingDecision))
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.RegionalDirectorDecisionDate.HasValue
            && string.IsNullOrEmpty(supportProject.InitialDiagnosisMatchingDecision))
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus DueDiligenceOnPreferredSupportingOrganisationTaskListStatus(
        SupportProjectViewModel supportProject)
    {
        if (supportProject.CheckOrganisationHasCapacityAndWillingToProvideSupport.HasValue &&
            supportProject.CheckChoiceWithTrustRelationshipManagerOrLaLead.HasValue &&
            supportProject.DiscussChoiceWithSfso.HasValue &&
            supportProject.CheckTheOrganisationHasAVendorAccount.HasValue &&
            supportProject.DateDueDiligenceCompleted.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.CheckOrganisationHasCapacityAndWillingToProvideSupport.HasValue &&
            !supportProject.CheckChoiceWithTrustRelationshipManagerOrLaLead.HasValue &&
            !supportProject.DiscussChoiceWithSfso.HasValue &&
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
        if (!string.IsNullOrEmpty(supportProject.SupportingOrganisationContactName)
            && !string.IsNullOrEmpty(supportProject.SupportingOrganisationContactEmailAddress)
            && !string.IsNullOrEmpty(supportProject.SupportingOrganisationContactAddress))
        {
            return TaskListStatus.Complete;
        }

        if (string.IsNullOrEmpty(supportProject.SupportingOrganisationContactName)
            && string.IsNullOrEmpty(supportProject.SupportingOrganisationContactEmailAddress))
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus SetRecordSupportingOrganisationAppointmentTaskListStatus(
        SupportProjectViewModel supportProject)
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

    public static TaskListStatus ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.IndicativeFundingBandCalculated.HasValue
            && supportProject.IndicativeFundingBand != null
            && supportProject.ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody.HasValue
            && supportProject.DateTemplatesAndIndicativeFundingBandSent.HasValue)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.IndicativeFundingBandCalculated.HasValue
            && supportProject.IndicativeFundingBand == null
            && !supportProject.ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody.HasValue
            && !supportProject.DateTemplatesAndIndicativeFundingBandSent.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
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

    public static TaskListStatus SendAgreedImprovementPlanForApprovalTaskListStatus(
        SupportProjectViewModel supportProject)
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
        if (supportProject.DateTeamContactedForRequestingPlanningGrantOfferLetter.HasValue
            && supportProject.IncludeContactDetailsRequestingPlanningGrantOfferEmail.Equals(true)
            && supportProject.ConfirmAmountOfPlanningGrantFundingRequested.Equals(true)
            && supportProject.CopyInRegionalDirectorRequestingPlanningGrantOfferEmail.Equals(true)
            && supportProject.SendRequestingPlanningGrantOfferEmailToRiseGrantTeam.Equals(true))
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateTeamContactedForRequestingPlanningGrantOfferLetter.HasValue
            && !supportProject.IncludeContactDetailsRequestingPlanningGrantOfferEmail.Equals(true)
            && !supportProject.ConfirmAmountOfPlanningGrantFundingRequested.Equals(true)
            && !supportProject.CopyInRegionalDirectorRequestingPlanningGrantOfferEmail.Equals(true)
            && !supportProject.SendRequestingPlanningGrantOfferEmailToRiseGrantTeam.Equals(true))
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus ReviewTheImprovementPlanTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.ImprovementPlanReceivedDate.HasValue
            && supportProject.ReviewImprovementAndExpenditurePlan.Equals(true)
            && supportProject.ConfirmFundingBand.Equals(true)
            && supportProject.FundingBand != null
            && supportProject.ConfirmPlanClearedByRiseGrantTeam.Equals(true))

        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.ImprovementPlanReceivedDate.HasValue
            && !supportProject.ReviewImprovementAndExpenditurePlan.Equals(true)
            && !supportProject.ConfirmFundingBand.Equals(true)
            && supportProject.FundingBand == null
            && !supportProject.ConfirmPlanClearedByRiseGrantTeam.Equals(true))
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus RequestImprovementGrantOfferLetterTaskListStatus(
        SupportProjectViewModel supportProject)
    {
        if (supportProject.DateTeamContactedForRequestingImprovementGrantOfferLetter.HasValue
            && supportProject.IncludeContactDetails.Equals(true)
            && supportProject.AttachSchoolImprovementPlan.Equals(true)
            && supportProject.CopyInRegionalDirector.Equals(true)
            && supportProject.SendEmailToGrantTeam.Equals(true))
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.DateTeamContactedForRequestingImprovementGrantOfferLetter.HasValue
            && !supportProject.IncludeContactDetails.Equals(true)
            && !supportProject.AttachSchoolImprovementPlan.Equals(true)
            && !supportProject.CopyInRegionalDirector.Equals(true)
            && !supportProject.SendEmailToGrantTeam.Equals(true))
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

    public static TaskListStatus ConfirmImprovementGrantOfferLetterTaskListStatus(
        SupportProjectViewModel supportProject)
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

    public static TaskListStatus ConfirmEligibilityTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.SupportProjectEligibilityStatus.HasValue)
        {
            return TaskListStatus.Complete;
        }


        return TaskListStatus.NotStarted;
    }


    public static TaskListStatus FundingHistoryTaskListStatus(SupportProjectViewModel supportProject)
    {
        if (supportProject.HasReceivedFundingInThelastTwoYears.HasValue && !supportProject.HasReceivedFundingInThelastTwoYears.Value)
        {
            return TaskListStatus.Complete;
        }

        if (supportProject.HasReceivedFundingInThelastTwoYears.HasValue && supportProject.HasReceivedFundingInThelastTwoYears.Value
            && supportProject.FundingHistoryDetailsComplete.HasValue && supportProject.FundingHistoryDetailsComplete.Value)
        {
            return TaskListStatus.Complete;
        }

        if (!supportProject.HasReceivedFundingInThelastTwoYears.HasValue)
        {
            return TaskListStatus.NotStarted;
        }

        return TaskListStatus.InProgress;
    }

    public static TaskListStatus EnterImprovementPlanObjectivesTaskListStatus(SupportProjectViewModel supportProject)
    {
        var improvementPlan = supportProject.ImprovementPlans?.FirstOrDefault();

        if (improvementPlan == null || improvementPlan.ImprovementPlanObjectives == null || improvementPlan.ImprovementPlanObjectives.Count == 0)
        {
            return TaskListStatus.NotStarted;
        }

        if (improvementPlan.ImprovementPlanObjectives != null
            && improvementPlan.ImprovementPlanObjectives.Count > 0
            && improvementPlan.ObjectivesSectionComplete is true)
        {
            return TaskListStatus.Complete;
        }

        return TaskListStatus.InProgress;
    }
}
