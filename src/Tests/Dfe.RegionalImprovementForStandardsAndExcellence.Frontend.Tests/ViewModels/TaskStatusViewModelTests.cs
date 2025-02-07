﻿using Dfe.RegionalImprovementForStandardsAndExcellence.Application.SupportProject.Models;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Models;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Models.SupportProject;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.ViewModels;

namespace Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Tests.ViewModels
{
    public class TaskStatusViewModelTests
    {
        public static readonly TheoryData<bool, bool, bool, DateTime?, TaskListStatus> ContactedTheSchoolTaskStatusCases = new()
        {
            {false, false, false, null, TaskListStatus.NotStarted },
            {false, true, false, null, TaskListStatus.InProgress},
            {true, true, true, DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(ContactedTheSchoolTaskStatusCases))]
        public void ContactedTheSchoolTaskStatusShouldReturnCorrectStatus(bool attachRiseInfoToEmail, bool findSchoolEmailAddress, bool useTheNotificationLetterToCreateEmail, DateTime? contactedTheSchoolDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, FindSchoolEmailAddress: findSchoolEmailAddress, UseTheNotificationLetterToCreateEmail: useTheNotificationLetterToCreateEmail,
                AttachRiseInfoToEmail: attachRiseInfoToEmail!, ContactedTheSchoolDate: contactedTheSchoolDate));

            //Action 
            var taskListStatus = TaskStatusViewModel.ContactedTheSchoolTaskStatus(supportProjectModel);

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
        public void RecordTheSchoolResponseTaskStatusShouldReturnCorrectStatus(bool? hasSavedSchoolResponseinSharePoint, bool? hasAcceeptedTargetedSupport, DateTime? schoolResponseDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, SchoolResponseDate: schoolResponseDate, HasAcceeptedTargetedSupport: hasAcceeptedTargetedSupport,
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
        public void SendIntroductoryEmailTaskListStatusShouldReturnCorrectStatus(bool? hasShareEmailTemplateWithAdvisor, bool? remindAdvisorToCopyRiseTeamWhenSentEmail,
            DateTime? introductoryEmailSentDate, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, IntroductoryEmailSentDate: introductoryEmailSentDate, HasShareEmailTemplateWithAdvisor: hasShareEmailTemplateWithAdvisor,
                RemindAdvisorToCopyRiseTeamWhenSentEmail: remindAdvisorToCopyRiseTeamWhenSentEmail));

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

        public static readonly TheoryData<string?, DateTime?, TaskListStatus> CheckAssignAdviserTaskListStatusCases = new()
        {
            {null, null, TaskListStatus.NotStarted },
            {"test@email.com", null, TaskListStatus.InProgress},
            {"test@email.com", DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(CheckAssignAdviserTaskListStatusCases))]
        public void CheckAssignAdviserTaskListStatusShouldReturnCorrectStatus(string? adviserEmailAddress, DateTime? dateAdviserAssigned, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateAdviserAssigned: dateAdviserAssigned, AdviserEmailAddress: adviserEmailAddress));

            //Action 
            var taskListStatus = TaskStatusViewModel.CheckAssignAdviserTaskListStatus(supportProjectModel);

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
        public void RecordSupportDecisionTaskListStatusShouldReturnCorrectStatus(DateTime? regionalDirectorDecisionDate, bool? hasConfirmedSchoolGetTargetSupport, string? disapprovingTargetedSupportNotes, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, RegionalDirectorDecisionDate: regionalDirectorDecisionDate, HasConfirmedSchoolGetTargetSupport: hasConfirmedSchoolGetTargetSupport,
                DisapprovingTargetedSupportNotes: disapprovingTargetedSupportNotes));

            //Action 
            var taskListStatus = TaskStatusViewModel.RecordSupportDecisionTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, string?, string, TaskListStatus> ChoosePreferredSupportingOrganisationTaskListStatusCases = new()
        {
            { null, null, "", TaskListStatus.NotStarted },
            { DateTime.Now, "name", "12344f", TaskListStatus.Complete },
            { DateTime.Now, null, "12345f", TaskListStatus.InProgress }
        };

        [Theory, MemberData(nameof(ChoosePreferredSupportingOrganisationTaskListStatusCases))]
        public void ChoosePreferredSupportingOrganisationShouldReturnCorrectStatus(DateTime? datePreferredSupportOrganisationChosen, string? supportOrganisationName, string supportOrganisationId, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateSupportOrganisationChosen: datePreferredSupportOrganisationChosen, SupportOrganisationName: supportOrganisationName,
                SupportOrganisationIdNumber: supportOrganisationId));

            //Action 
            var taskListStatus = TaskStatusViewModel.ChoosePreferredSupportingOrganisationTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, bool?, bool?, bool?, bool?, TaskListStatus> DueDiligenceOnPreferredSupportingOrganisationTaskListStatusCases = new()
        {
            { null, null, null, null, null, null, TaskListStatus.NotStarted },
            { DateTime.Now, true, true, true, true, true, TaskListStatus.Complete },
            { DateTime.Now, null, true, true, true, true,  TaskListStatus.InProgress }
        };

        [Theory, MemberData(nameof(DueDiligenceOnPreferredSupportingOrganisationTaskListStatusCases))]
        public void DueDiligenceOnPreferredSupportingOrganisationShouldReturnCorrectStatus(
         DateTime? dateDueDiligenceCompleted,
         bool? checkOrganisationHasCapacityAndWillingToProvideSupport,
         bool? checkChoiceWithTrustRelationshipManagerOrLaLead,
         bool? discussChoiceWithSfso,
         bool? checkFinancialConcernsAtSupportingOrganisation,
         bool? checkTheOrganisationHasAVendorAccount,
         TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now,
                CheckOrganisationHasCapacityAndWillingToProvideSupport: checkOrganisationHasCapacityAndWillingToProvideSupport,
                CheckChoiceWithTrustRelationshipManagerOrLaLead: checkChoiceWithTrustRelationshipManagerOrLaLead,
                DiscussChoiceWithSfso: discussChoiceWithSfso,
                CheckFinancialConcernsAtSupportingOrganisation: checkFinancialConcernsAtSupportingOrganisation,
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
        public void RecordSupportingOrganisationAppointmentTaskListStatusShouldReturnCorrectStatus(DateTime? regionalDirectorAppointmentDate, bool? hasConfirmedSupportingOrgnaisationAppointment, string? disapprovingSupportingOrgnaisationAppointmentNotes, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, RegionalDirectorAppointmentDate: regionalDirectorAppointmentDate,
                HasConfirmedSupportingOrgnaisationAppointment: hasConfirmedSupportingOrgnaisationAppointment, DisapprovingSupportingOrgnaisationAppointmentNotes: disapprovingSupportingOrgnaisationAppointmentNotes));

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


        public static readonly TheoryData<bool?, bool?, DateTime?, TaskListStatus> ShareImprovementPlanTaskListStatusCases = new()
   {
       {null, null, null, TaskListStatus.NotStarted },
       {false, false, null, TaskListStatus.InProgress },
       {false, true, null, TaskListStatus.InProgress},
       {true, true, DateTime.Now, TaskListStatus.Complete }
   };

        [Theory, MemberData(nameof(ShareImprovementPlanTaskListStatusCases))]
        public void ShareImprovementPlanTaskListStatusCasesShouldReturnCorrectStatus(bool? sendTheTemplateToTheSupportingOrganisation,
            bool? sendTheTemplateToTheSchoolsResponsibleBody,
            DateTime? dateTemplatesSent, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now,
                SendTheTemplateToTheSupportingOrganisation: sendTheTemplateToTheSupportingOrganisation,
                SendTheTemplateToTheSchoolsResponsibleBody: sendTheTemplateToTheSchoolsResponsibleBody,
                DateTemplatesSent: dateTemplatesSent));

            //Action 
            var taskListStatus = TaskStatusViewModel.ShareTheImprovementPlanTemplateTaskListStatus(supportProjectModel);

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

        public static readonly TheoryData<DateTime?, TaskListStatus> RequestPlanningGrantOfferLetterTaskListStatusCases = new()
        {
            { null, TaskListStatus.NotStarted },
            { DateTime.UtcNow, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(RequestPlanningGrantOfferLetterTaskListStatusCases))]
        public void RequestPlanningGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(DateTime? dateGrantsTeamContacted, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTeamContactedForRequestingPlanningGrantOfferLetter: dateGrantsTeamContacted));

            //Action 
            var taskListStatus = TaskStatusViewModel.RequestPlanningGrantOfferLetterTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }

        public static readonly TheoryData<DateTime?, bool?, TaskListStatus> ReviewTheImprovementPlanTaskListStatusCases = new()
        {
            { null, null, TaskListStatus.NotStarted },
            { DateTime.Now, true, TaskListStatus.Complete },
            { null, true, TaskListStatus.InProgress },
            { DateTime.Now, null, TaskListStatus.InProgress },
        };

        [Theory, MemberData(nameof(ReviewTheImprovementPlanTaskListStatusCases))]
        public void ReviewTheImprovementPlanTaskListStatusShouldReturnCorrectStatus(DateTime? improvementPlanReceivedDate, bool? reviewImprovementPlanWithTeam, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, ImprovementPlanReceivedDate: improvementPlanReceivedDate,
                ReviewImprovementPlanWithTeam: reviewImprovementPlanWithTeam));

            //Action 
            var taskListStatus = TaskStatusViewModel.ReviewTheImprovementPlanTaskListStatus(supportProjectModel);

            //Assert
            Assert.Equal(expectedTaskListStatus, taskListStatus);
        }
        public static readonly TheoryData<DateTime?, TaskListStatus> RequestImprovementGrantOfferLetterTaskListStatusCases = new()
        {
            { null, TaskListStatus.NotStarted },
            { DateTime.Now, TaskListStatus.Complete }
        };

        [Theory, MemberData(nameof(RequestImprovementGrantOfferLetterTaskListStatusCases))]
        public void RequestImprovementGrantOfferLetterTaskListStatusShouldReturnCorrectStatus(DateTime? dateGrantsTeamContacted, TaskListStatus expectedTaskListStatus)
        {
            // Arrange
            var supportProjectModel = SupportProjectViewModel.Create(new SupportProjectDto(1, DateTime.Now, DateTeamContactedForRequestingImprovementGrantOfferLetter: dateGrantsTeamContacted));

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
    }
}
