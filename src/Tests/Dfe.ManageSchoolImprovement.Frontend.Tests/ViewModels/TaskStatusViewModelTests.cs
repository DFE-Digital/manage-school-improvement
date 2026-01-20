using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.ViewModels
{
    public class TaskStatusViewModelTests
    {
        public static readonly TheoryData<bool?, DateTime?, TaskListStatus>
            InitialContactResponsibleBodyTaskStatusCases = new()
            {
                { null, null, TaskListStatus.NotStarted },
                { false, null, TaskListStatus.InProgress },
                { true, DateTime.Now, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(InitialContactResponsibleBodyTaskStatusCases))]
        public void InitialContactResponsibleBodyTaskStatusShouldReturnCorrectStatus(
            bool? initialContactResponsibleBody, DateTime? initialContactResponsibleBodyDate,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                InitialContactResponsibleBody: initialContactResponsibleBody,
                InitialContactResponsibleBodyDate: initialContactResponsibleBodyDate));

            //Action 
            var taskListStatus = TaskStatusViewModel.ContactedTheResponsibleBodyTaskStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, bool?, bool?, DateTime?, TaskListStatus>
            SendFormalNotificationTaskStatusCases = new()
            {
                { null, null, null, null, null, TaskListStatus.NotStarted },
                { true, true, true, true, DateTime.Now, TaskListStatus.Complete },
                { true, null, null, null, null, TaskListStatus.InProgress },
                { null, null, true, null, null, TaskListStatus.InProgress },
                { true, null, null, true, DateTime.Now, TaskListStatus.InProgress }
            };

        [Theory, MemberData(nameof(SendFormalNotificationTaskStatusCases))]
        public void SendFormalNotificationTaskStatusShouldReturnCorrectStatus(
            bool? useEnrolmentLetterTemplateToDraftEmail,
            bool? attachTargetedInterventionInformationSheet,
            bool? addRecipientsForFormalNotification,
            bool? formalNotificationSent,
            DateTime? dateFormalNotificationSent,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1,
                DateTime.Now,
                DateTime.Now,
                UseEnrolmentLetterTemplateToDraftEmail: useEnrolmentLetterTemplateToDraftEmail,
                AttachTargetedInterventionInformationSheet: attachTargetedInterventionInformationSheet,
                AddRecipientsForFormalNotification: addRecipientsForFormalNotification,
                FormalNotificationSent: formalNotificationSent,
                DateFormalNotificationSent: dateFormalNotificationSent
                ));

            //Action 
            var taskListStatus = TaskStatusViewModel.SendFormalNotificationTaskStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, DateTime?, TaskListStatus>
            RecordTheSchoolResponseTaskStatusCases = new()
            {
                { null, null, TaskListStatus.NotStarted },
                { false, null, TaskListStatus.InProgress },
                { true, DateTime.Now, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(RecordTheSchoolResponseTaskStatusCases))]
        public void RecordTheSchoolResponseTaskStatusShouldReturnCorrectStatus(bool? hasSavedSchoolResponseinSharePoint,
            DateTime? schoolResponseDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate: schoolResponseDate,
                ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint: hasSavedSchoolResponseinSharePoint));

            //Action 
            var taskListStatus = TaskStatusViewModel.ResponsibleBodyResponseToTheConflictOfInterestRequestStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, DateTime?, TaskListStatus>
            SendIntroductoryEmailTaskListStatusCases = new()
            {
                { null, null, null, TaskListStatus.NotStarted },
                { false, false, null, TaskListStatus.InProgress },
                { false, true, null, TaskListStatus.InProgress },
                { true, true, DateTime.Now, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(SendIntroductoryEmailTaskListStatusCases))]
        public void SendIntroductoryEmailTaskListStatusShouldReturnCorrectStatus(bool? hasShareEmailTemplateWithAdviser,
            bool? remindAdviserToCopyRiseTeamWhenSentEmail,
            DateTime? introductoryEmailSentDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                IntroductoryEmailSentDate: introductoryEmailSentDate,
                HasShareEmailTemplateWithAdviser: hasShareEmailTemplateWithAdviser,
                RemindAdviserToCopyRiseTeamWhenSentEmail: remindAdviserToCopyRiseTeamWhenSentEmail));

            //Action 
            var taskListStatus = TaskStatusViewModel.SendIntroductoryEmailTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, DateTime?, TaskListStatus>
            CompleteAndSaveInitialDiagnosisTemplateTaskListStatusCases = new()
            {
                { null, null, null, TaskListStatus.NotStarted },
                { false, false, null, TaskListStatus.InProgress },
                { false, true, null, TaskListStatus.InProgress },
                { true, true, DateTime.Now, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(CompleteAndSaveInitialDiagnosisTemplateTaskListStatusCases))]
        public void CompleteAndSaveInitialDiagnosisTemplateTaskListStatusShouldReturnCorrectStatus(
            bool? hasTalkToAdviserAboutFindings, bool? hasCompleteAssessmentTemplate,
            DateTime? savedAssessmentTemplateInSharePointDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                SavedAssessmentTemplateInSharePointDate: savedAssessmentTemplateInSharePointDate,
                HasTalkToAdviserAboutFindings: hasTalkToAdviserAboutFindings,
                HasCompleteAssessmentTemplate: hasCompleteAssessmentTemplate));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.CompleteAndSaveInitialDiagnosisTemplateTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, DateTime?, TaskListStatus>
            CheckThePotentialAdviserConflictsOfInterestTaskListStatusCases = new()
            {
                { null, null, TaskListStatus.NotStarted },
                { null, DateTime.Now, TaskListStatus.InProgress },
                { true, null, TaskListStatus.InProgress },
                { true, DateTime.Now, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(CheckThePotentialAdviserConflictsOfInterestTaskListStatusCases))]
        public void CheckThePotentialAdviserConflictsOfInterestTaskListStatusShouldReturnCorrectStatus(
            bool? reviewAdvisersConflictOfInterestForm,
            DateTime? dateConflictOfInterestDeclarationChecked,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                ReviewAdvisersConflictOfInterestForm: reviewAdvisersConflictOfInterestForm,
                DateConflictOfInterestDeclarationChecked: dateConflictOfInterestDeclarationChecked));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.CheckThePotentialAdviserConflictsOfInterestTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<string?, DateTime?, TaskListStatus> CheckAllocateAdviserTaskListStatusCases =
            new()
            {
                { null, null, TaskListStatus.NotStarted },
                { "test@email.com", null, TaskListStatus.InProgress },
                { "test@email.com", DateTime.Now, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(CheckAllocateAdviserTaskListStatusCases))]
        public void CheckAllocateAdviserTaskListStatusShouldReturnCorrectStatus(string? adviserEmailAddress,
            DateTime? dateAdviserAllocated, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                DateAdviserAllocated: dateAdviserAllocated, AdviserEmailAddress: adviserEmailAddress));

            //Action 
            var taskListStatus = TaskStatusViewModel.CheckAllocateAdviserTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, TaskListStatus> AdviserVisitToSchoolTaskListStatusCases =
            new()
            {
                { null, null, TaskListStatus.NotStarted },
                { DateTime.Now, true, TaskListStatus.Complete },
                { DateTime.Now, null, TaskListStatus.InProgress },
                { null, true, TaskListStatus.InProgress }
            };

        [Theory, MemberData(nameof(AdviserVisitToSchoolTaskListStatusCases))]
        public void AdviserVisitToSchoolTaskListStatusShouldReturnCorrectStatus(DateTime? adviserVisitDate,
            bool? giveTheAdviserTheNoteOfVisitTemplate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(
                1,
                DateTime.Now,
                DateTime.Now,
                AdviserVisitDate: adviserVisitDate,
                GiveTheAdviserTheNoteOfVisitTemplate: giveTheAdviserTheNoteOfVisitTemplate));

            //Action 
            var taskListStatus = TaskStatusViewModel.AdviserVisitToSchoolTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, TaskListStatus> RecordVisitDateToVisitSchoolTaskListStatusCases =
            new()
            {
                { null, TaskListStatus.NotStarted },
                { DateTime.Now, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(RecordVisitDateToVisitSchoolTaskListStatusCases))]
        public void RecordVisitDateToVisitSchoolTaskListStatusShouldReturnCorrectStatus(DateTime? schoolVisitDate,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel =
                SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                    SchoolVisitDate: schoolVisitDate));

            //Action 
            var taskListStatus = TaskStatusViewModel.RecordVisitDateToVisitSchoolTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, string?, string?, TaskListStatus>
            RecordSupportDecisionTaskListStatusCases = new()
            {
                { null, null, null, TaskListStatus.NotStarted },
                { DateTime.Now, "Match with a supporting organisation", null, TaskListStatus.Complete },
                { DateTime.Now, "Review school's progress", "Notes", TaskListStatus.Complete },
                { DateTime.Now, "Unable to assess", "Notes", TaskListStatus.Complete },
                { null, "Match with a supporting organisation", null, TaskListStatus.InProgress }
            };

        [Theory, MemberData(nameof(RecordSupportDecisionTaskListStatusCases))]
        public void RecordMatchingDecisionTaskListStatusShouldReturnCorrectStatus(
            DateTime? regionalDirectorDecisionDate, string? initialDiagnosisMatchingDecision,
            string? initialDiagnosisMatchingDecisionNotes, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                RegionalDirectorDecisionDate: regionalDirectorDecisionDate,
                InitialDiagnosisMatchingDecision: initialDiagnosisMatchingDecision,
                InitialDiagnosisMatchingDecisionNotes: initialDiagnosisMatchingDecisionNotes));

            //Action 
            var taskListStatus = TaskStatusViewModel.RecordInitialDiagnosisDecisionTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, string?, string, bool?, TaskListStatus>
            ChoosePreferredSupportingOrganisationTaskListStatusCases = new()
            {
                { null, null, "", null, TaskListStatus.NotStarted },
                { DateTime.Now, "name", "12344f", true, TaskListStatus.Complete },
                { DateTime.Now, null, "12345f", null, TaskListStatus.InProgress }
            };

        [Theory, MemberData(nameof(ChoosePreferredSupportingOrganisationTaskListStatusCases))]
        public void ChoosePreferredSupportingOrganisationShouldReturnCorrectStatus(
            DateTime? datePreferredSupportOrganisationChosen, string? supportOrganisationName,
            string supportOrganisationId, bool? assessmentToolTwoCompleted, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                DateSupportOrganisationChosen: datePreferredSupportOrganisationChosen,
                SupportOrganisationName: supportOrganisationName,
                SupportOrganisationIdNumber: supportOrganisationId,
                AssessmentToolTwoCompleted: assessmentToolTwoCompleted));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.ChoosePreferredSupportingOrganisationTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, bool?, bool?, bool?, TaskListStatus>
            DueDiligenceOnPreferredSupportingOrganisationTaskListStatusCases = new()
            {
                { null, null, null, null, null, TaskListStatus.NotStarted },
                { DateTime.Now, true, true, true, true, TaskListStatus.Complete },
                { DateTime.Now, null, true, true, true, TaskListStatus.InProgress }
            };

        [Theory, MemberData(nameof(DueDiligenceOnPreferredSupportingOrganisationTaskListStatusCases))]
        public void DueDiligenceOnPreferredSupportingOrganisationShouldReturnCorrectStatus(
            DateTime? dateDueDiligenceCompleted,
            bool? checkOrganisationHasCapacityAndWillingToProvideSupport,
            bool? checkChoiceWithTrustRelationshipManagerOrLaLead,
            bool? discussChoiceWithSfso,
            bool? checkTheOrganisationHasAVendorAccount,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                CheckOrganisationHasCapacityAndWillingToProvideSupport:
                checkOrganisationHasCapacityAndWillingToProvideSupport,
                CheckChoiceWithTrustRelationshipManagerOrLaLead: checkChoiceWithTrustRelationshipManagerOrLaLead,
                DiscussChoiceWithSfso: discussChoiceWithSfso,
                CheckTheOrganisationHasAVendorAccount: checkTheOrganisationHasAVendorAccount,
                DateDueDiligenceCompleted: dateDueDiligenceCompleted));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.DueDiligenceOnPreferredSupportingOrganisationTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, string?, TaskListStatus>
            RecordSupportingDecisionTaskListStatusCases = new()
            {
                { null, null, null, TaskListStatus.NotStarted },
                { DateTime.Now, true, null, TaskListStatus.Complete },
                { DateTime.Now, false, "Notes", TaskListStatus.InProgress }
            };

        [Theory, MemberData(nameof(RecordSupportingDecisionTaskListStatusCases))]
        public void RecordSupportingOrganisationAppointmentTaskListStatusShouldReturnCorrectStatus(
            DateTime? regionalDirectorAppointmentDate, bool? hasConfirmedSupportingOrganisationAppointment,
            string? disapprovingSupportingOrganisationAppointmentNotes, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                RegionalDirectorAppointmentDate: regionalDirectorAppointmentDate,
                HasConfirmedSupportingOrganisationAppointment: hasConfirmedSupportingOrganisationAppointment,
                DisapprovingSupportingOrganisationAppointmentNotes:
                disapprovingSupportingOrganisationAppointmentNotes));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.SetRecordSupportingOrganisationAppointmentTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }


        public static readonly TheoryData<string?, string?, TaskListStatus>
            SupportingOrganisationContactDetailsTaskListStatusCases = new()
            {
                { null, null, TaskListStatus.NotStarted },
                { "name", "email@email.com", TaskListStatus.Complete },
                { null, "email@email.com", TaskListStatus.InProgress },
                { "name", null, TaskListStatus.InProgress }
            };

        [Theory, MemberData(nameof(SupportingOrganisationContactDetailsTaskListStatusCases))]
        public void SupportingOrganisationContactDetailsTaskListStatusShouldReturnCorrectStatus(
            string? supportingOrganisationContactName,
            string? supportingOrganisationContactEmail, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                SupportingOrganisationContactName: supportingOrganisationContactName,
                SupportingOrganisationContactEmailAddress: supportingOrganisationContactEmail!));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.SupportingOrganisationContactDetailsTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, string?, TaskListStatus>
            RecordImprovementPlanDecisionTaskListStatusCases = new()
            {
                { null, null, null, TaskListStatus.NotStarted },
                { DateTime.Now, true, null, TaskListStatus.Complete },
                { DateTime.Now, false, "Notes", TaskListStatus.InProgress }
            };


        public static readonly TheoryData<bool?, string?, bool?, DateTime?, TaskListStatus>
            ShareImprovementPlanTaskListStatusCases = new()
            {
                { null, null, null, null, TaskListStatus.NotStarted },
                { false, null, false, null, TaskListStatus.InProgress },
                { false, "test", true, null, TaskListStatus.InProgress },
                { true, "40000", true, DateTime.Now, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(ShareImprovementPlanTaskListStatusCases))]
        public void ShareImprovementPlanTaskListStatusCasesShouldReturnCorrectStatus(
            bool? indicativeFundingBandCalculated,
            string? indicativeFundingBand,
            bool?
                improvementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
            DateTime? dateTemplatesSent,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                IndicativeFundingBandCalculated: indicativeFundingBandCalculated,
                IndicativeFundingBand: indicativeFundingBand,
                ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody
                : improvementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
                DateTemplatesAndIndicativeFundingBandSent: dateTemplatesSent));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus(
                    supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        [Theory, MemberData(nameof(RecordImprovementPlanDecisionTaskListStatusCases))]
        public void RecordImprovementPlanDecisionTaskListStatusShouldReturnCorrectStatus(
            DateTime? regionalDirectorImprovementPlanDecisionDate, bool? hasApprovedImprovementPlanDecision,
            string? disapprovingImprovementPlanDecisionNotes, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                RegionalDirectorImprovementPlanDecisionDate: regionalDirectorImprovementPlanDecisionDate,
                HasApprovedImprovementPlanDecision: hasApprovedImprovementPlanDecision,
                DisapprovingImprovementPlanDecisionNotes: disapprovingImprovementPlanDecisionNotes));

            //Action 
            var taskListStatus = TaskStatusViewModel.RecordImprovementPlanDecisionTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, TaskListStatus>
            SendAgreedImprovementPlanForApprovalTaskListStatusCases = new()
            {
                { null, null, TaskListStatus.NotStarted },
                { true, true, TaskListStatus.Complete },
                { true, false, TaskListStatus.InProgress },
                { true, null, TaskListStatus.InProgress },
                { null, true, TaskListStatus.InProgress },
                { false, true, TaskListStatus.InProgress }
            };

        [Theory, MemberData(nameof(SendAgreedImprovementPlanForApprovalTaskListStatusCases))]
        public void SendAgreedImprovementPlanForApprovalTaskListStatusShouldReturnCorrectStatus(
            bool? hasSavedImprovementPlanInSharePoint, bool? hasEmailedAgreedPlanToRegionalDirectorForApproval,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                HasSavedImprovementPlanInSharePoint: hasSavedImprovementPlanInSharePoint,
                HasEmailedAgreedPlanToRegionalDirectorForApproval: hasEmailedAgreedPlanToRegionalDirectorForApproval));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.SendAgreedImprovementPlanForApprovalTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, bool?, bool?, bool?, TaskListStatus>
            RequestPlanningGrantOfferLetterTaskListStatusCases = new()
            {
                { null, false, false, false, false, TaskListStatus.NotStarted },
                { DateTime.UtcNow, true, false, false, true, TaskListStatus.InProgress },
                { DateTime.UtcNow, true, true, true, true, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(RequestPlanningGrantOfferLetterTaskListStatusCases))]
        public void RequestPlanningGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(
            DateTime? dateGrantsTeamContacted,
            bool? includeContactDetails,
            bool? amountFundingRequested,
            bool? copyRegionalDirector,
            bool? emailRiseGrantTeam,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(
                1,
                DateTime.Now,
                DateTime.Now,
                DateTeamContactedForRequestingPlanningGrantOfferLetter: dateGrantsTeamContacted,
                IncludeContactDetailsRequestingPlanningGrantOfferEmail: includeContactDetails,
                ConfirmAmountOfPlanningGrantFundingRequested: amountFundingRequested,
                CopyInRegionalDirectorRequestingPlanningGrantOfferEmail: copyRegionalDirector,
                SendRequestingPlanningGrantOfferEmailToRiseGrantTeam: emailRiseGrantTeam
            ));

            //Action 
            var taskListStatus = TaskStatusViewModel.RequestPlanningGrantOfferLetterTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, bool?, string?, bool?, TaskListStatus>
            ReviewTheImprovementPlanTaskListStatusCases = new()
            {
                { null, null, null, null, null, TaskListStatus.NotStarted },
                { DateTime.Now, true, true, "££££££", true, TaskListStatus.Complete },
                { null, true, null, "money", null, TaskListStatus.InProgress },
                { DateTime.Now, null, true, null, null, TaskListStatus.InProgress },
            };

        [Theory, MemberData(nameof(ReviewTheImprovementPlanTaskListStatusCases))]
        public void ReviewTheImprovementPlanTaskListStatusShouldReturnCorrectStatus(
            DateTime? improvementPlanReceivedDate,
            bool? reviewImprovementAndExpenditurePlan,
            bool? confirmFundingBand,
            string? fundingBand,
            bool? confirmPlanClearedByRiseGrantTeam,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                ImprovementPlanReceivedDate: improvementPlanReceivedDate,
                ReviewImprovementAndExpenditurePlan: reviewImprovementAndExpenditurePlan,
                ConfirmFundingBand: confirmFundingBand,
                FundingBand: fundingBand,
                ConfirmPlanClearedByRiseGrantTeam: confirmPlanClearedByRiseGrantTeam));

            //Action 
            var taskListStatus = TaskStatusViewModel.ReviewTheImprovementPlanTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, bool?, bool?, bool?, TaskListStatus>
            RequestImprovementGrantOfferLetterTaskListStatusCases = new()
            {
                { null, null, null, null, null, TaskListStatus.NotStarted },
                { DateTime.Now, true, true, true, true, TaskListStatus.Complete },
                { DateTime.Now, true, true, true, null, TaskListStatus.InProgress },
                { DateTime.Now, true, true, null, true, TaskListStatus.InProgress },
                { DateTime.Now, true, null, true, true, TaskListStatus.InProgress },
                { DateTime.Now, null, true, true, true, TaskListStatus.InProgress },
                { null, true, true, true, true, TaskListStatus.InProgress },
            };

        [Theory, MemberData(nameof(RequestImprovementGrantOfferLetterTaskListStatusCases))]
        public void RequestImprovementGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(
            DateTime? dateGrantsTeamContacted,
            bool? includeContactDetails,
            bool? attachSchoolImprovementPlan,
            bool? copyInRegionalDirector,
            bool? sendEmailToGrantTeam, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                DateTeamContactedForRequestingImprovementGrantOfferLetter: dateGrantsTeamContacted,
                IncludeContactDetails: includeContactDetails,
                AttachSchoolImprovementPlan: attachSchoolImprovementPlan,
                CopyInRegionalDirector: copyInRegionalDirector,
                SendEmailToGrantTeam: sendEmailToGrantTeam));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.RequestImprovementGrantOfferLetterTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, TaskListStatus>
            ConfirmPlanningGrantOfferLetterTaskListStatusCases = new()
            {
                { null, TaskListStatus.NotStarted },
                { DateTime.UtcNow, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(ConfirmPlanningGrantOfferLetterTaskListStatusCases))]
        public void ConfirmPlanningGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(
            DateTime? dateGrantsLetterConfirmed, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                DateTeamContactedForConfirmingPlanningGrantOfferLetter: dateGrantsLetterConfirmed));

            //Action 
            var taskListStatus = TaskStatusViewModel.ConfirmPlanningGrantOfferLetterTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, TaskListStatus>
            ConfirmImprovementGrantOfferLetterTaskListStatusCases = new()
            {
                { null, TaskListStatus.NotStarted },
                { DateTime.UtcNow, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(ConfirmImprovementGrantOfferLetterTaskListStatusCases))]
        public void ConfirmImprovementGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(
            DateTime? dateGrantsLetterConfirmed, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                DateImprovementGrantOfferLetterSent: dateGrantsLetterConfirmed));

            //Action 
            var taskListStatus =
                TaskStatusViewModel.ConfirmImprovementGrantOfferLetterTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<SupportProjectStatus?, TaskListStatus> ConfirmEligibilityTaskListStatusCases =
            new()
            {
                { SupportProjectStatus.EligibleForSupport, TaskListStatus.Complete },
                { SupportProjectStatus.NotEligibleForSupport, TaskListStatus.Complete },
                { null, TaskListStatus.NotStarted },
            };

        [Theory, MemberData(nameof(ConfirmEligibilityTaskListStatusCases))]
        public void ConfirmEligibilityTaskListStatusShouldReturnCorrectStatus(SupportProjectStatus? isEligible,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel =
                SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                    SupportProjectStatus: isEligible));

            //Action 
            var taskListStatus = TaskStatusViewModel.ConfirmEligibilityTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, TaskListStatus> FundingHistoryTaskListStatusCases = new()
        {
            { null, null, TaskListStatus.NotStarted },
            { true, null, TaskListStatus.InProgress },
            { true, false, TaskListStatus.InProgress },
            { true, true, TaskListStatus.Complete },
            { false, null, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(FundingHistoryTaskListStatusCases))]
        public void FundingHistoryTaskListStatusShouldReturnCorrectStatus(bool? hasReceivedFunding,
            bool? fundingHistoryComplete, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                HasReceivedFundingInThelastTwoYears: hasReceivedFunding,
                FundingHistoryDetailsComplete: fundingHistoryComplete));

            //Action 
            var taskListStatus = TaskStatusViewModel.FundingHistoryTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, bool?, TaskListStatus>
            EnterImprovementPlanObjectivesTaskListStatusCases = new()
            {
                // NotStarted cases
                { null, null, null, TaskListStatus.NotStarted },
                { false, null, null, TaskListStatus.NotStarted },
                { true, null, null, TaskListStatus.NotStarted },
                { true, false, null, TaskListStatus.NotStarted },

                // InProgress cases - has objectives but not marked complete
                { true, true, null, TaskListStatus.InProgress },
                { true, true, false, TaskListStatus.InProgress },

                // Complete case - has objectives and marked complete
                { true, true, true, TaskListStatus.Complete }
            };

        [Theory, MemberData(nameof(EnterImprovementPlanObjectivesTaskListStatusCases))]
        public void EnterImprovementPlanObjectivesTaskListStatusShouldReturnCorrectStatus(bool? hasImprovementPlans,
            bool? hasObjectives, bool? objectivesSectionComplete, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            IEnumerable<ImprovementPlanDto>? improvementPlans = null;

            if (hasImprovementPlans == true)
            {
                var objectives = hasObjectives == true
                    ? new List<ImprovementPlanObjectiveDto>
                    {
                        new(Guid.NewGuid(), 1, Guid.NewGuid(), 1, "QualityOfEducation", "Test objective")
                    }
                    : new List<ImprovementPlanObjectiveDto>();

                improvementPlans = new List<ImprovementPlanDto>
                {
                    new(Guid.NewGuid(), 1, 1, objectivesSectionComplete, objectives)
                };
            }
            else if (hasImprovementPlans == false)
            {
                improvementPlans = new List<ImprovementPlanDto>();
            }

            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                ImprovementPlans: improvementPlans!));

            //Action 
            var taskListStatus = TaskStatusViewModel.EnterImprovementPlanObjectivesTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        [Fact]
        public void EnterImprovementPlanObjectivesTaskListStatus_WithMultipleImprovementPlans_UsesFirstPlan()
        {
            // Arrange - Create multiple improvement plans where first is incomplete, second is complete
            var improvementPlans = new List<ImprovementPlanDto>
            {
                // First plan - InProgress (has objectives but not complete)
                new(Guid.NewGuid(), 1, 1, false, new List<ImprovementPlanObjectiveDto>
                {
                    new(Guid.NewGuid(), 1, Guid.NewGuid(), 1, "QualityOfEducation", "First plan objective")
                }),
                // Second plan - Complete (has objectives and marked complete)
                new(Guid.NewGuid(), 2, 1, true, new List<ImprovementPlanObjectiveDto>
                {
                    new(Guid.NewGuid(), 2, Guid.NewGuid(), 1, "LeadershipAndManagement", "Second plan objective")
                })
            };

            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTime.Now,
                ImprovementPlans: improvementPlans));

            //Action 
            var taskListStatus = TaskStatusViewModel.EnterImprovementPlanObjectivesTaskListStatus(supportProjectModel);

            //Assert - Should use the first plan's status (InProgress)
            Assert.Equal(TaskListStatus.InProgress, taskListStatus);
        }
    }
}