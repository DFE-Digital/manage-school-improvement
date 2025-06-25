using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.ViewModels
{
    public class TaskStatusViewModelTests
    {
        public static readonly TheoryData<bool?, bool?, DateTime?, TaskListStatus> ContactedTheResponsibleBodyTaskStatusCases = new()
        {
            {null, null, null, TaskListStatus.NotStarted },
            {true, false, null, TaskListStatus.InProgress},
            {true, true, DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(ContactedTheResponsibleBodyTaskStatusCases))]
        public void ContactedTheResponsibleBodyTaskStatusShouldReturnCorrectStatus(bool? discussTheBestApproach, bool? emailTheResponsibleBody, DateTime? contactedResponsibleBodyDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, discussTheBestApproach: discussTheBestApproach, emailTheResponsibleBody: emailTheResponsibleBody,
                 contactedTheResponsibleBodyDate: contactedResponsibleBodyDate));

            //Action 
            var taskListStatus = TaskStatusViewModel.ContactedTheResponsibleBodyTaskStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, DateTime?, TaskListStatus> RecordTheSchoolResponseTaskStatusCases = new()
        {
            {null, null, null, TaskListStatus.NotStarted },
            {false, false, null, TaskListStatus.InProgress },
            {false, true, null, TaskListStatus.InProgress},
            {true, true, DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(RecordTheSchoolResponseTaskStatusCases))]
        public void RecordTheSchoolResponseTaskStatusShouldReturnCorrectStatus(bool? hasSavedSchoolResponseinSharePoint, bool? hasAcceptedTargetedSupport, DateTime? schoolResponseDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, SchoolResponseDate: schoolResponseDate, HasAcknowledgedAndWillEngage: hasAcceptedTargetedSupport,
                HasSavedSchoolResponseinSharePoint: hasSavedSchoolResponseinSharePoint));

            //Action 
            var taskListStatus = TaskStatusViewModel.RecordTheSchoolResponseTaskStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, DateTime?, TaskListStatus> SendIntroductoryEmailTaskListStatusCases = new()
        {
            {null, null, null, TaskListStatus.NotStarted },
            {false, false, null, TaskListStatus.InProgress },
            {false, true, null, TaskListStatus.InProgress},
            {true, true, DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(SendIntroductoryEmailTaskListStatusCases))]
        public void SendIntroductoryEmailTaskListStatusShouldReturnCorrectStatus(bool? hasShareEmailTemplateWithAdviser, bool? remindAdviserToCopyRiseTeamWhenSentEmail,
            DateTime? introductoryEmailSentDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, IntroductoryEmailSentDate: introductoryEmailSentDate, HasShareEmailTemplateWithAdviser: hasShareEmailTemplateWithAdviser,
                RemindAdviserToCopyRiseTeamWhenSentEmail: remindAdviserToCopyRiseTeamWhenSentEmail));

            //Action 
            var taskListStatus = TaskStatusViewModel.SendIntroductoryEmailTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, DateTime?, TaskListStatus> CompleteAndSaveAssessmentTemplateTaskListStatusCases = new()
        {
            {null, null, null, TaskListStatus.NotStarted },
            {false, false, null, TaskListStatus.InProgress },
            {false, true, null, TaskListStatus.InProgress},
            {true, true, DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(CompleteAndSaveAssessmentTemplateTaskListStatusCases))]
        public void CompleteAndSaveAssessmentTemplateTaskListStatusShouldReturnCorrectStatus(bool? hasTalkToAdviserAboutFindings, bool? hasCompleteAssessmentTemplate,
            DateTime? savedAssessmentTemplateInSharePointDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, SavedAssessmentTemplateInSharePointDate: savedAssessmentTemplateInSharePointDate,
                HasTalkToAdviserAboutFindings: hasTalkToAdviserAboutFindings, HasCompleteAssessmentTemplate: hasCompleteAssessmentTemplate));

            //Action 
            var taskListStatus = TaskStatusViewModel.CompleteAndSaveAssessmentTemplateTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, DateTime?, TaskListStatus> NoteOfVsistTaskListStatusCases = new()
        {
            {null, null, null, TaskListStatus.NotStarted },
            {false, false, null, TaskListStatus.InProgress },
            {false, true, null, TaskListStatus.InProgress},
            {true, true, DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(NoteOfVsistTaskListStatusCases))]
        public void NoteOfVsistTaskListStatusShouldReturnCorrectStatus(bool? askTheAdviserToSendYouTheirNotes, bool? giveTheAdviserTheNoteOfVisitTemplate, DateTime? dateNoteOfVisitSavedInSharePoint, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, GiveTheAdviserTheNoteOfVisitTemplate: giveTheAdviserTheNoteOfVisitTemplate,
                AskTheAdviserToSendYouTheirNotes: askTheAdviserToSendYouTheirNotes, DateNoteOfVisitSavedInSharePoint: dateNoteOfVisitSavedInSharePoint));

            //Action 
            var taskListStatus = TaskStatusViewModel.NoteOfVsistTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, bool?, DateTime?, TaskListStatus> CheckThePotentialAdviserConflictsOfInterestTaskListStatusCases = new()
        {
            {null, null, null, null, TaskListStatus.NotStarted },
            {false, false, false, null, TaskListStatus.InProgress },
            {false, true, true, null, TaskListStatus.InProgress},
            {true, true, true, DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(CheckThePotentialAdviserConflictsOfInterestTaskListStatusCases))]
        public void CheckThePotentialAdviserConflictsOfInterestTaskListStatusShouldReturnCorrectStatus(bool? sendConflictOfInterestFormToProposedAdviserAndTheSchool, bool? receiveCompletedConflictOfInterestForm, bool? saveCompletedConflictOfinterestFormInSharePoint, DateTime? dateConflictsOfInterestWereChecked, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, SendConflictOfInterestFormToProposedAdviserAndTheSchool: sendConflictOfInterestFormToProposedAdviserAndTheSchool,
                ReceiveCompletedConflictOfInterestForm: receiveCompletedConflictOfInterestForm, SaveCompletedConflictOfinterestFormInSharePoint: saveCompletedConflictOfinterestFormInSharePoint,
                DateConflictsOfInterestWereChecked: dateConflictsOfInterestWereChecked));

            //Action 
            var taskListStatus = TaskStatusViewModel.CheckThePotentialAdviserConflictsOfInterestTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<string?, DateTime?, TaskListStatus> CheckAllocateAdviserTaskListStatusCases = new()
        {
            {null, null, TaskListStatus.NotStarted },
            {"test@email.com", null, TaskListStatus.InProgress},
            {"test@email.com", DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(CheckAllocateAdviserTaskListStatusCases))]
        public void CheckAllocateAdviserTaskListStatusShouldReturnCorrectStatus(string? adviserEmailAddress, DateTime? dateAdviserAllocated, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateAdviserAllocated: dateAdviserAllocated, AdviserEmailAddress: adviserEmailAddress));

            //Action 
            var taskListStatus = TaskStatusViewModel.CheckAllocateAdviserTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, TaskListStatus> AdviserVisitToSchoolTaskListStatusCases = new()
        {
            {null, TaskListStatus.NotStarted },
            {DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(AdviserVisitToSchoolTaskListStatusCases))]
        public void AdviserVisitToSchoolTaskListStatusShouldReturnCorrectStatus(DateTime? adviserVisitDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, AdviserVisitDate: adviserVisitDate));

            //Action 
            var taskListStatus = TaskStatusViewModel.AdviserVisitToSchoolTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, TaskListStatus> RecordVisitDateToVisitSchoolTaskListStatusCases = new()
        {
            {null, TaskListStatus.NotStarted },
            {DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(RecordVisitDateToVisitSchoolTaskListStatusCases))]
        public void RecordVisitDateToVisitSchoolTaskListStatusShouldReturnCorrectStatus(DateTime? schoolVisitDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, SchoolVisitDate: schoolVisitDate));

            //Action 
            var taskListStatus = TaskStatusViewModel.RecordVisitDateToVisitSchoolTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, string?, TaskListStatus> RecordSupportDecisionTaskListStatusCases = new()
        {
            { null, null, null, TaskListStatus.NotStarted },
            { DateTime.Now, true, null, TaskListStatus.Complete },
            { DateTime.Now, false, "Notes", TaskListStatus.InProgress }
        };

        [Theory, MemberData(nameof(RecordSupportDecisionTaskListStatusCases))]
        public void RecordMatchingDecisionTaskListStatusShouldReturnCorrectStatus(DateTime? regionalDirectorDecisionDate, bool? hasSchoolMatchedWithSupportingOrganisation, string? notMatchingSchoolWithSupportingOrgNotes, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, RegionalDirectorDecisionDate: regionalDirectorDecisionDate, HasSchoolMatchedWithSupportingOrganisation: hasSchoolMatchedWithSupportingOrganisation,
                NotMatchingSchoolWithSupportingOrgNotes: notMatchingSchoolWithSupportingOrgNotes));

            //Action 
            var taskListStatus = TaskStatusViewModel.RecordSupportDecisionTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, string?, string, bool?, TaskListStatus> ChoosePreferredSupportingOrganisationTaskListStatusCases = new()
        {
            { null, null, "", null, TaskListStatus.NotStarted },
            { DateTime.Now, "name", "12344f", true, TaskListStatus.Complete },
            { DateTime.Now, null, "12345f", null, TaskListStatus.InProgress }
        };

        [Theory, MemberData(nameof(ChoosePreferredSupportingOrganisationTaskListStatusCases))]
        public void ChoosePreferredSupportingOrganisationShouldReturnCorrectStatus(DateTime? datePreferredSupportOrganisationChosen, string? supportOrganisationName, string supportOrganisationId, bool? assessmentToolTwoCompleted, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateSupportOrganisationChosen: datePreferredSupportOrganisationChosen, SupportOrganisationName: supportOrganisationName,
                SupportOrganisationIdNumber: supportOrganisationId, AssessmentToolTwoCompleted: assessmentToolTwoCompleted));

            //Action 
            var taskListStatus = TaskStatusViewModel.ChoosePreferredSupportingOrganisationTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, bool?, bool?, bool?, TaskListStatus> DueDiligenceOnPreferredSupportingOrganisationTaskListStatusCases = new()
        {
            { null, null, null, null, null, TaskListStatus.NotStarted },
            { DateTime.Now, true, true, true, true, TaskListStatus.Complete },
            { DateTime.Now, null, true, true, true,  TaskListStatus.InProgress }
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
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now,
                CheckOrganisationHasCapacityAndWillingToProvideSupport: checkOrganisationHasCapacityAndWillingToProvideSupport,
                CheckChoiceWithTrustRelationshipManagerOrLaLead: checkChoiceWithTrustRelationshipManagerOrLaLead,
                DiscussChoiceWithSfso: discussChoiceWithSfso,
                CheckTheOrganisationHasAVendorAccount: checkTheOrganisationHasAVendorAccount,
                DateDueDiligenceCompleted: dateDueDiligenceCompleted));

            //Action 
            var taskListStatus = TaskStatusViewModel.DueDiligenceOnPreferredSupportingOrganisationTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, string?, TaskListStatus> RecordSupportingDecisionTaskListStatusCases = new()
        {
            { null, null, null, TaskListStatus.NotStarted },
            { DateTime.Now, true, null, TaskListStatus.Complete },
            { DateTime.Now, false, "Notes", TaskListStatus.InProgress }
        };

        [Theory, MemberData(nameof(RecordSupportingDecisionTaskListStatusCases))]
        public void RecordSupportingOrganisationAppointmentTaskListStatusShouldReturnCorrectStatus(DateTime? regionalDirectorAppointmentDate, bool? hasConfirmedSupportingOrganisationAppointment, string? disapprovingSupportingOrganisationAppointmentNotes, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, RegionalDirectorAppointmentDate: regionalDirectorAppointmentDate,
                HasConfirmedSupportingOrganisationAppointment: hasConfirmedSupportingOrganisationAppointment, DisapprovingSupportingOrganisationAppointmentNotes: disapprovingSupportingOrganisationAppointmentNotes));

            //Action 
            var taskListStatus = TaskStatusViewModel.SetRecordSupportingOrganisationAppointmentTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }


        public static readonly TheoryData<DateTime?, string?, string?, TaskListStatus> SupportingOrganisationContactDetailsTaskListStatusCases = new()
        {
            { null, null, null, TaskListStatus.NotStarted },
            { DateTime.Now, "name", "email@email.com", TaskListStatus.Complete },
            { DateTime.Now, null, "", TaskListStatus.InProgress }
        };

        [Theory, MemberData(nameof(SupportingOrganisationContactDetailsTaskListStatusCases))]
        public void SupportingOrganisationContactDetailsTaskListStatusShouldReturnCorrectStatus(DateTime? dateSupportingOrganisationDetailsAdded, string? supportingOrganisationContactName, string? supportingOrganisationContactEmail, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateSupportingOrganisationContactDetailsAdded: dateSupportingOrganisationDetailsAdded,
                SupportingOrganisationContactName: supportingOrganisationContactName, SupportingOrganisationContactEmailAddress: supportingOrganisationContactEmail));

            //Action 
            var taskListStatus = TaskStatusViewModel.SupportingOrganisationContactDetailsTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, string?, TaskListStatus> RecordImprovementPlanDecisionTaskListStatusCases = new()
        {
            { null, null, null, TaskListStatus.NotStarted },
            { DateTime.Now, true, null, TaskListStatus.Complete },
            { DateTime.Now, false, "Notes", TaskListStatus.InProgress }
        };


        public static readonly TheoryData<bool?, string?, bool?, DateTime?, TaskListStatus> ShareImprovementPlanTaskListStatusCases = new()
       {
           {null, null, null, null, TaskListStatus.NotStarted },
           {false, null , false, null, TaskListStatus.InProgress },
           {false,"test", true, null, TaskListStatus.InProgress},
           {true, "40000", true, DateTime.Now, TaskListStatus.Complete }
       };

        [Theory, MemberData(nameof(ShareImprovementPlanTaskListStatusCases))]
        public void ShareImprovementPlanTaskListStatusCasesShouldReturnCorrectStatus(bool? indicativeFundingBandCalculated,
            string? indicativeFundingBand,
            bool? improvementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
            DateTime? dateTemplatesSent,
            TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now,
                IndicativeFundingBandCalculated: indicativeFundingBandCalculated,
                IndicativeFundingBand: indicativeFundingBand,
                ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody: improvementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
                DateTemplatesAndIndicativeFundingBandSent: dateTemplatesSent));

            //Action 
            var taskListStatus = TaskStatusViewModel.ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        [Theory, MemberData(nameof(RecordImprovementPlanDecisionTaskListStatusCases))]
        public void RecordImprovementPlanDecisionTaskListStatusShouldReturnCorrectStatus(DateTime? regionalDirectorImprovementPlanDecisionDate, bool? hasApprovedImprovementPlanDecision, string? disapprovingImprovementPlanDecisionNotes, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, RegionalDirectorImprovementPlanDecisionDate: regionalDirectorImprovementPlanDecisionDate,
                HasApprovedImprovementPlanDecision: hasApprovedImprovementPlanDecision, DisapprovingImprovementPlanDecisionNotes: disapprovingImprovementPlanDecisionNotes));

            //Action 
            var taskListStatus = TaskStatusViewModel.RecordImprovementPlanDecisionTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<bool?, bool?, TaskListStatus> SendAgreedImprovementPlanForApprovalTaskListStatusCases = new()
        {
            { null, null, TaskListStatus.NotStarted },
            { true, true, TaskListStatus.Complete },
            { true, false, TaskListStatus.InProgress },
            { true, null, TaskListStatus.InProgress },
            { null, true, TaskListStatus.InProgress },
            { false, true, TaskListStatus.InProgress }
        };

        [Theory, MemberData(nameof(SendAgreedImprovementPlanForApprovalTaskListStatusCases))]
        public void SendAgreedImprovementPlanForApprovalTaskListStatusShouldReturnCorrectStatus(bool? hasSavedImprovementPlanInSharePoint, bool? hasEmailedAgreedPlanToRegionalDirectorForApproval, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, HasSavedImprovementPlanInSharePoint: hasSavedImprovementPlanInSharePoint,
                HasEmailedAgreedPlanToRegionalDirectorForApproval: hasEmailedAgreedPlanToRegionalDirectorForApproval));

            //Action 
            var taskListStatus = TaskStatusViewModel.SendAgreedImprovementPlanForApprovalTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, bool?, bool?, bool?, TaskListStatus> RequestPlanningGrantOfferLetterTaskListStatusCases = new()
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

        public static readonly TheoryData<DateTime?, bool?, bool?, string?, bool?, TaskListStatus> ReviewTheImprovementPlanTaskListStatusCases = new()
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
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now,
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

        public static readonly TheoryData<DateTime?, bool?, bool?, bool?, bool?, TaskListStatus> RequestImprovementGrantOfferLetterTaskListStatusCases = new()
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
        public void RequestImprovementGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(DateTime? dateGrantsTeamContacted,
            bool? includeContactDetails,
            bool? attachSchoolImprovementPlan,
            bool? copyInRegionalDirector,
            bool? sendEmailToGrantTeam, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now,
                DateTeamContactedForRequestingImprovementGrantOfferLetter: dateGrantsTeamContacted,
                IncludeContactDetails: includeContactDetails,
            AttachSchoolImprovementPlan: attachSchoolImprovementPlan,
            CopyInRegionalDirector: copyInRegionalDirector,
            SendEmailToGrantTeam: sendEmailToGrantTeam));

            //Action 
            var taskListStatus = TaskStatusViewModel.RequestImprovementGrantOfferLetterTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, TaskListStatus> ConfirmPlanningGrantOfferLetterTaskListStatusCases = new()
        {
            { null, TaskListStatus.NotStarted },
            { DateTime.UtcNow, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(ConfirmPlanningGrantOfferLetterTaskListStatusCases))]
        public void ConfirmPlanningGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(DateTime? dateGrantsLetterConfirmed, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTeamContactedForConfirmingPlanningGrantOfferLetter: dateGrantsLetterConfirmed));

            //Action 
            var taskListStatus = TaskStatusViewModel.ConfirmPlanningGrantOfferLetterTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, TaskListStatus> ConfirmImprovementGrantOfferLetterTaskListStatusCases = new()
        {
            { null, TaskListStatus.NotStarted },
            { DateTime.UtcNow, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(ConfirmImprovementGrantOfferLetterTaskListStatusCases))]
        public void ConfirmImprovementGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(DateTime? dateGrantsLetterConfirmed, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateImprovementGrantOfferLetterSent: dateGrantsLetterConfirmed));

            //Action 
            var taskListStatus = TaskStatusViewModel.ConfirmImprovementGrantOfferLetterTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<SupportProjectStatus?, TaskListStatus> ConfirmEligibilityTaskListStatusCases = new()
        {
            { SupportProjectStatus.EligibleForSupport, TaskListStatus.Complete },
            { SupportProjectStatus.NotEligibleForSupport, TaskListStatus.Complete },
            {null, TaskListStatus.NotStarted },
        };

        [Theory, MemberData(nameof(ConfirmEligibilityTaskListStatusCases))]
        public void ConfirmEligibilityTaskListStatusShouldReturnCorrectStatus(SupportProjectStatus? isEligible, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, SupportProjectStatus: isEligible));

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
        public void FundingHistoryTaskListStatusShouldReturnCorrectStatus(bool? hasReceivedFunding, bool? fundingHistoryComplete, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now,
                HasReceivedFundingInThelastTwoYears: hasReceivedFunding,
                FundingHistoryDetailsComplete: fundingHistoryComplete));

            //Action 
            var taskListStatus = TaskStatusViewModel.FundingHistoryTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }
    }
}
