using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject
{
    public class SupportProjectTests
    {
        private readonly MockRepository mockRepository;

        public SupportProjectTests() => mockRepository = new MockRepository(MockBehavior.Strict);

        [Fact]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange Act
            var supportProject = CreateSupportProject();

            // Assert
            supportProject.Should().NotBeNull();
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetSchoolResponse_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            DateTime? schoolResponseDate = DateTime.Now;
            bool? hasAcceptedTargetedSupport = true;
            bool? hasSavedSchoolResponseinSharePoint = true;

            // Act
            supportProject.SetSchoolResponse(
                schoolResponseDate,
                hasAcceptedTargetedSupport,
                hasSavedSchoolResponseinSharePoint);

            // Assert
            supportProject.SchoolResponseDate.Should().Be(schoolResponseDate);
            supportProject.HasAcknowledgedAndWillEngage.Should().Be(hasAcceptedTargetedSupport);
            supportProject.HasSavedSchoolResponseinSharePoint.Should().Be(hasSavedSchoolResponseinSharePoint);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetSendIntroductoryEmail_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var introductoryEmailSentDate = DateTime.UtcNow;
            var hasShareEmailTemplateWithAdviser = true;
            var remindAdviserToCopyRiseTeamWhenSentEmail = true;

            // Act
            supportProject.SetSendIntroductoryEmail(
                introductoryEmailSentDate,
                hasShareEmailTemplateWithAdviser,
                remindAdviserToCopyRiseTeamWhenSentEmail);

            // Assert
            supportProject.IntroductoryEmailSentDate.Should().Be(introductoryEmailSentDate);
            supportProject.HasShareEmailTemplateWithAdviser.Should().Be(hasShareEmailTemplateWithAdviser);
            supportProject.RemindAdviserToCopyRiseTeamWhenSentEmail.Should().Be(remindAdviserToCopyRiseTeamWhenSentEmail);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetCompleteAndSaveInitialDiagnosisTemplate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var savedAssessmentTemplateInSharePointDate = DateTime.UtcNow;
            var hasTalkToAdviser = true;
            var hasCompleteAssessmentTemplate = true;

            // Act
            supportProject.SetCompleteAndSaveInitialDiagnosisTemplate(
                savedAssessmentTemplateInSharePointDate,
                hasTalkToAdviser,
                hasCompleteAssessmentTemplate);

            // Assert
            supportProject.SavedAssessmentTemplateInSharePointDate.Should().Be(savedAssessmentTemplateInSharePointDate);
            supportProject.HasTalkToAdviserAboutFindings.Should().Be(hasTalkToAdviser);
            supportProject.HasCompleteAssessmentTemplate.Should().Be(hasCompleteAssessmentTemplate);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetSchoolVisitDate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var schoolVisitDate = DateTime.UtcNow;

            // Act
            supportProject.SetSchoolVisitDate(schoolVisitDate);

            // Assert
            supportProject.SchoolVisitDate.Should().Be(schoolVisitDate);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetAdviserConflictOfInterestDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? sendConflictOfInterestFormToProposedAdviserAndTheSchool = true;
            bool? receiveCompletedConflictOfInterestForm = true;
            bool? saveCompletedConflictOfinterestFormInSharePoint = true;
            DateTime? dateConflictsOfInterestWereChecked = DateTime.UtcNow;

            // Act
            supportProject.SetAdviserConflictOfInterestDetails(
                sendConflictOfInterestFormToProposedAdviserAndTheSchool,
                receiveCompletedConflictOfInterestForm,
                saveCompletedConflictOfinterestFormInSharePoint,
                dateConflictsOfInterestWereChecked);

            // Assert
            supportProject.SendConflictOfInterestFormToProposedAdviserAndTheSchool.Should().Be(sendConflictOfInterestFormToProposedAdviserAndTheSchool);
            supportProject.ReceiveCompletedConflictOfInterestForm.Should().Be(receiveCompletedConflictOfInterestForm);
            supportProject.SaveCompletedConflictOfinterestFormInSharePoint.Should().Be(saveCompletedConflictOfinterestFormInSharePoint);
            supportProject.DateConflictsOfInterestWereChecked.Should().Be(dateConflictsOfInterestWereChecked);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetAdviserDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            string? adviserEmailAddress = "test";
            DateTime? dateAdviserAllocated = DateTime.UtcNow;
            string? assignedAdviserFullName = "Test Adviser";

            // Act
            supportProject.SetAdviserDetails(
                adviserEmailAddress,
                dateAdviserAllocated,
                assignedAdviserFullName);

            // Assert
            supportProject.AdviserEmailAddress.Should().Be(adviserEmailAddress);
            supportProject.DateAdviserAllocated.Should().Be(dateAdviserAllocated);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetDeliveryOfficerDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            string? deliveryOfficerEmailAddress = "test@Email.com";
            string? deliveryOfficerName = "Test Name";

            // Act
            supportProject.SetDeliveryOfficer(
                deliveryOfficerName,
                deliveryOfficerEmailAddress);

            // Assert
            supportProject.AssignedDeliveryOfficerEmailAddress.Should().Be(deliveryOfficerEmailAddress);
            supportProject.AssignedDeliveryOfficerFullName.Should().Be(deliveryOfficerName);
            mockRepository.VerifyAll();
        }

        private static Domain.Entities.SupportProject.SupportProject CreateSupportProject(
            string schoolName = "Default School",
            string schoolUrn = "DefaultURN",
            string localAuthority = "Default Authority",
            string region = "Default Region")
        {
            return Domain.Entities.SupportProject.SupportProject.Create(
                 schoolName,
                 schoolUrn,
                 localAuthority,
                 region);
        }

        [Fact]
        public void SetContactTheResponsibleBody_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? discussTheBestApproach = true;
            bool? emailTheResponsibleBody = true;
            DateTime? responsibleBodyContactedDate = DateTime.UtcNow;

            // Act
            supportProject.SetContactTheResponsibleBodyDetails(
                discussTheBestApproach,
                emailTheResponsibleBody,
                responsibleBodyContactedDate);

            // Assert
            supportProject.DiscussTheBestApproach.Should().Be(discussTheBestApproach);
            supportProject.EmailTheResponsibleBody.Should().Be(emailTheResponsibleBody);
            supportProject.ContactedTheResponsibleBodyDate.Should().Be(responsibleBodyContactedDate);
            mockRepository.VerifyAll();
        }

        [Fact]

        public void SetAdviserVisitDate_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime? adviserVisitDate = DateTime.UtcNow;
            bool? giveAdviserNoteOfVisitTemplate = true;

            // Act
            supportProject.SetAdviserVisitDate(
                adviserVisitDate,
                giveAdviserNoteOfVisitTemplate);

            // Assert
            supportProject.AdviserVisitDate.Should().Be(adviserVisitDate);
            supportProject.GiveTheAdviserTheNoteOfVisitTemplate.Should().Be(giveAdviserNoteOfVisitTemplate);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void SetRecordSupportDecision_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            string? initialDiagnosisMatchingDecision = "Review school's progress";
            DateTime? regionalDirectorDecisionDate = DateTime.UtcNow;
            string? initialDiagnosisMatchingDecisionNotes = "Notes only if choose no";

            // Act
            supportProject.SetRecordInitialDiagnosisMatchingDecision(
                regionalDirectorDecisionDate,
                initialDiagnosisMatchingDecision,
                initialDiagnosisMatchingDecisionNotes);

            // Assert
            supportProject.InitialDiagnosisMatchingDecision.Should().Be(initialDiagnosisMatchingDecision);
            supportProject.RegionalDirectorDecisionDate.Should().Be(regionalDirectorDecisionDate);
            supportProject.InitialDiagnosisMatchingDecisionNotes.Should().Be(initialDiagnosisMatchingDecisionNotes);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetRecordSupportDecision_KeepsNotes_OnConfirmingTargetSupport()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            string? initialDiagnosisMatchingDecision = "Match with a supporting organisation";
            DateTime? regionalDirectorDecisionDate = DateTime.UtcNow;
            string? initialDiagnosisMatchingDecisionNotes = "Notes only if choose no";

            // Act
            supportProject.SetRecordInitialDiagnosisMatchingDecision(
                regionalDirectorDecisionDate,
                initialDiagnosisMatchingDecision,
                initialDiagnosisMatchingDecisionNotes);

            // Assert
            supportProject.InitialDiagnosisMatchingDecision.Should().Be(initialDiagnosisMatchingDecision);
            supportProject.RegionalDirectorDecisionDate.Should().Be(regionalDirectorDecisionDate);
            supportProject.InitialDiagnosisMatchingDecisionNotes.Should().Be(initialDiagnosisMatchingDecisionNotes);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetChoosePreferredSupportOrganisation_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime? dateSupportOrganisationChosen = DateTime.UtcNow;
            string? supportOrgansiationName = "name";
            string? supportOrganisationId = "1234a";
            bool? assessmentToolTwoCompleted = true;

            // Act
            supportProject.SetChoosePreferredSupportOrganisation(
                dateSupportOrganisationChosen,
                supportOrgansiationName,
                supportOrganisationId,
                assessmentToolTwoCompleted
                );

            // Assert
            supportProject.DateSupportOrganisationChosen.Should().Be(dateSupportOrganisationChosen);
            supportProject.SupportOrganisationName.Should().Be(supportOrgansiationName);
            supportProject.SupportOrganisationIdNumber.Should().Be(supportOrganisationId);
            supportProject.AssessmentToolTwoCompleted.Should().Be(assessmentToolTwoCompleted);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void SetDueDiligenceOnPreferredSupportingOrganisationDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? checkOrganisationHasCapacityAndWillingToProvideSupport = true;
            bool? checkChoiceWithTrustRelationshipManagerOrLaLead = false;
            bool? discussChoiceWithSfso = true;
            bool? checkTheOrganisationHasAVendorAccount = true;
            DateTime? dateDueDiligenceCompleted = DateTime.UtcNow;

            // Act
            supportProject.SetDueDiligenceOnPreferredSupportingOrganisationDetails(
                checkOrganisationHasCapacityAndWillingToProvideSupport,
                checkChoiceWithTrustRelationshipManagerOrLaLead,
                discussChoiceWithSfso,
                checkTheOrganisationHasAVendorAccount, dateDueDiligenceCompleted);

            // Assert
            supportProject.CheckOrganisationHasCapacityAndWillingToProvideSupport.Should().Be(checkOrganisationHasCapacityAndWillingToProvideSupport);
            supportProject.CheckChoiceWithTrustRelationshipManagerOrLaLead.Should().Be(checkChoiceWithTrustRelationshipManagerOrLaLead);
            supportProject.DiscussChoiceWithSfso.Should().Be(discussChoiceWithSfso);
            supportProject.CheckTheOrganisationHasAVendorAccount.Should().Be(checkTheOrganisationHasAVendorAccount);
            supportProject.DateDueDiligenceCompleted.Should().Be(dateDueDiligenceCompleted);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void SetRecordSupportOrganisationAppointment_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? hasConfirmedSupportingOrganisationAppointment = false;
            DateTime? regionalDirectorAppointmentDate = DateTime.UtcNow;
            string? disapprovingSupportingOrganisationAppointmentNotes = "Notes only if choose no";

            // Act
            supportProject.SetRecordSupportingOrganisationAppointment(
                regionalDirectorAppointmentDate,
                hasConfirmedSupportingOrganisationAppointment,
                disapprovingSupportingOrganisationAppointmentNotes);

            // Assert
            supportProject.HasConfirmedSupportingOrganisationAppointment.Should().Be(hasConfirmedSupportingOrganisationAppointment);
            supportProject.RegionalDirectorAppointmentDate.Should().Be(regionalDirectorAppointmentDate);
            supportProject.DisapprovingSupportingOrganisationAppointmentNotes.Should().Be(disapprovingSupportingOrganisationAppointmentNotes);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetRecordSupportOrganisationAppointment_KeepsNotes_OnConfirmingTargetSupport()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? hasConfirmedSupportingOrganisationAppointment = true;
            DateTime? regionalDirectorAppointmentDate = DateTime.UtcNow;
            string? disapprovingSupportingOrganisationAppointmentNotes = "Notes only if choose no";

            // Act
            supportProject.SetRecordSupportingOrganisationAppointment(
                regionalDirectorAppointmentDate,
                hasConfirmedSupportingOrganisationAppointment,
                disapprovingSupportingOrganisationAppointmentNotes);

            // Assert
            supportProject.HasConfirmedSupportingOrganisationAppointment.Should().Be(hasConfirmedSupportingOrganisationAppointment);
            supportProject.RegionalDirectorAppointmentDate.Should().Be(regionalDirectorAppointmentDate);
            supportProject.DisapprovingSupportingOrganisationAppointmentNotes.Should().Be(disapprovingSupportingOrganisationAppointmentNotes);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetSupportingOrganisationContactDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime? dateSupportingOrganisationContactDetailAdded = DateTime.UtcNow;
            string? supportOrgansiationContactName = "name";
            string? supportOrganisationContactEmailAddress = "1234a";

            // Act
            supportProject.SetSupportingOrganisationContactDetails(
                dateSupportingOrganisationContactDetailAdded,
                supportOrgansiationContactName,
                supportOrganisationContactEmailAddress);

            // Assert
            supportProject.DateSupportingOrganisationContactDetailsAdded.Should().Be(dateSupportingOrganisationContactDetailAdded);
            supportProject.SupportingOrganisationContactName.Should().Be(supportOrgansiationContactName);
            supportProject.SupportingOrganisationContactEmailAddress.Should().Be(supportOrganisationContactEmailAddress);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetReviewImprovementPlan_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime improvementPlanReceivedDate = DateTime.UtcNow;
            bool reviewImprovementAndExpenditurePlan = true;
            bool confirmFundingBand = true;
            string fundingBand = "lots of money";
            bool confirmPlanClearedByRiseGrantTeam = true;

            // Act
            supportProject.SetReviewTheImprovementPlan(
                improvementPlanReceivedDate,
                reviewImprovementAndExpenditurePlan,
                confirmFundingBand,
                fundingBand,
                confirmPlanClearedByRiseGrantTeam);

            // Assert
            supportProject.ImprovementPlanReceivedDate.Should().Be(improvementPlanReceivedDate);
            supportProject.ReviewImprovementAndExpenditurePlan.Should().Be(reviewImprovementAndExpenditurePlan);
            supportProject.ConfirmFundingBand.Should().Be(confirmFundingBand);
            supportProject.FundingBand.Should().Be(fundingBand);
            supportProject.ConfirmPlanClearedByRiseGrantTeam.Should().Be(confirmPlanClearedByRiseGrantTeam);
            mockRepository.VerifyAll();
        }


        [Fact]
        public void SetRecordImprovementPlanDecision_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? hasApprovedImprovementPlanDecision = true;
            DateTime? regionalDirectorImprovementPlanDecisionDate = DateTime.UtcNow;
            string? disapprovingImprovementPlanDecisionNotes = "Notes only if choose no";

            // Act
            supportProject.SetRecordImprovementPlanDecision(
                regionalDirectorImprovementPlanDecisionDate,
                hasApprovedImprovementPlanDecision,
                disapprovingImprovementPlanDecisionNotes);

            // Assert
            supportProject.HasApprovedImprovementPlanDecision.Should().Be(hasApprovedImprovementPlanDecision);
            supportProject.RegionalDirectorImprovementPlanDecisionDate.Should().Be(regionalDirectorImprovementPlanDecisionDate);
            supportProject.DisapprovingImprovementPlanDecisionNotes.Should().Be(disapprovingImprovementPlanDecisionNotes);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetShareImprovementPlanDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? calculateFundingBand = true;
            string? fundingBand = "40000";
            bool? sendTemplate = true;
            DateTime? dateTemplatesSent = DateTime.UtcNow;

            // Act
            supportProject.SetIndicativeFundingBandAndImprovementPlanTemplateDetails(
                calculateFundingBand,
                fundingBand,
                sendTemplate,
                dateTemplatesSent);

            // Assert
            supportProject.IndicativeFundingBandCalculated.Should().Be(calculateFundingBand);
            supportProject.IndicativeFundingBand.Should().Be(fundingBand);
            supportProject.ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody.Should().Be(sendTemplate);
            supportProject.DateTemplatesAndIndicativeFundingBandSent.Should().Be(dateTemplatesSent);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetSendAgreedImprovementPlanForApproval_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? hasSavedImprovementPlanInSharePoint = true;
            bool? hasEmailedAgreedPlanToRegionalDirectorForApproval = true;

            // Act
            supportProject.SetSendAgreedImprovementPlanForApproval(
                hasSavedImprovementPlanInSharePoint,
                hasEmailedAgreedPlanToRegionalDirectorForApproval);

            // Assert
            supportProject.HasSavedImprovementPlanInSharePoint.Should().Be(hasSavedImprovementPlanInSharePoint);
            supportProject.HasEmailedAgreedPlanToRegionalDirectorForApproval.Should().Be(hasEmailedAgreedPlanToRegionalDirectorForApproval);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetRequestImprovementGrantOfferLetter_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime? dateTeamContactedForRequestingImprovementGrantOfferLetter = DateTime.Now;
            bool? includeContactDetails = true;
            bool? attachSchoolImprovementPlan = true;
            bool? copyInRegionalDirector = true;
            bool? sendEmailToGrantTeam = true;

            // Act
            supportProject.SetRequestImprovementGrantOfferLetter(dateTeamContactedForRequestingImprovementGrantOfferLetter, includeContactDetails,
                attachSchoolImprovementPlan,
                copyInRegionalDirector,
                sendEmailToGrantTeam);

            // Assert
            supportProject.DateTeamContactedForRequestingImprovementGrantOfferLetter.Should().Be(dateTeamContactedForRequestingImprovementGrantOfferLetter);
            supportProject.IncludeContactDetails.Should().Be(includeContactDetails);
            supportProject.AttachSchoolImprovementPlan.Should().Be(attachSchoolImprovementPlan);
            supportProject.CopyInRegionalDirector.Should().Be(copyInRegionalDirector);
            supportProject.SendEmailToGrantTeam.Should().Be(sendEmailToGrantTeam);
            mockRepository.VerifyAll();
        }
        [Fact]
        public void SetRequestPlanningGrantOfferLetterDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime? dateGrantTeamContacted = DateTime.UtcNow;

            // Act
            supportProject.SetRequestPlanningGrantOfferLetterDetails(dateGrantTeamContacted, true, false, false, false);

            // Assert
            supportProject.DateTeamContactedForRequestingPlanningGrantOfferLetter.Should().Be(dateGrantTeamContacted);
            supportProject.IncludeContactDetailsRequestingPlanningGrantOfferEmail.Should().BeTrue();
            supportProject.ConfirmAmountOfPlanningGrantFundingRequested.Should().BeFalse();
            supportProject.CopyInRegionalDirectorRequestingPlanningGrantOfferEmail.Should().BeFalse();
            supportProject.SendRequestingPlanningGrantOfferEmailToRiseGrantTeam.Should().BeFalse();
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetConfirmPlanningGrantOfferLetterDate_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime? dateLetterConfirmed = DateTime.UtcNow;

            // Act
            supportProject.SetConfirmPlanningGrantOfferLetterDate(dateLetterConfirmed);

            // Assert
            supportProject.DateTeamContactedForConfirmingPlanningGrantOfferLetter.Should().Be(dateLetterConfirmed);
            mockRepository.VerifyAll();
        }


        [Fact]
        public void SetConfirmImprovementGrantOfferLetterDate_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime? dateLetterConfirmed = DateTime.UtcNow;

            // Act
            supportProject.SetConfirmImprovementGrantOfferLetterDetails(dateLetterConfirmed);

            // Assert
            supportProject.DateImprovementGrantOfferLetterSent.Should().Be(dateLetterConfirmed);
            mockRepository.VerifyAll();
        }


        [Fact]
        public void SetEligibility_WithValidDetails_Eligible_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? isEligible = true;
            string note = "note";
            // Act
            supportProject.SetEligibility(isEligible, note);

            // Assert
            supportProject.SupportProjectStatus.Should().Be(SupportProjectStatus.EligibleForSupport);
            supportProject.SchoolIsNotEligibleNotes.Should().Be(note);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetEligibility_WithValidDetails_NotEligible_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? isEligible = false;
            string note = "note";
            // Act
            supportProject.SetEligibility(isEligible, note);

            // Assert
            supportProject.SupportProjectStatus.Should().Be(SupportProjectStatus.NotEligibleForSupport);
            supportProject.SchoolIsNotEligibleNotes.Should().Be(note);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetSoftDeleted_SetsProjectSoftDeleted()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var deletedBy = "first.last@education.gov.uk";

            // Act
            supportProject.SetSoftDeleted(deletedBy);

            // Assert
            supportProject.DeletedAt.Should().NotBeNull();
            supportProject.DeletedBy.Should().Be(deletedBy);
        }

        [Fact]
        public void AddNote_SetsNotes()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var supportProjectNoteId = new SupportProjectNoteId(Guid.NewGuid());
            var note = "Note";
            var author = "Author";
            var date = DateTime.UtcNow;
            var supportProjectId = new SupportProjectId(1);

            // Act
            supportProject.AddNote(supportProjectNoteId, note, author, date, supportProjectId);

            // Assert
            supportProject.Notes.Should().NotBeNull();
            foreach (var projectNote in supportProject.Notes)
            {
                projectNote.Note.Should().Be(note);
                projectNote.CreatedBy.Should().Be(author);
                projectNote.CreatedOn.Should().Be(date);
                projectNote.SupportProjectId.Should().Be(supportProjectId);
                projectNote.Id.Should().Be(supportProjectNoteId);
            }
        }

        [Fact]
        public void SetHasReceivedFundingInThelastTwoYears_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? hasReceivedFundingInThelastTwoYears = true;

            // Act
            supportProject.SetHasReceivedFundingInThelastTwoYears(
                hasReceivedFundingInThelastTwoYears);

            // Assert
            supportProject.HasReceivedFundingInThelastTwoYears.Should().Be(hasReceivedFundingInThelastTwoYears);
            supportProject.FundingHistories.Count().Should().Be(0);
            supportProject.FundingHistoryDetailsComplete.Should().BeNull();
            mockRepository.VerifyAll();
        }
        [Fact]
        public void SetSetFundingHistoryComplete_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? fundingHistoryComplete = true;

            // Act
            supportProject.SetFundingHistoryComplete(
                fundingHistoryComplete);

            // Assert
            supportProject.FundingHistoryDetailsComplete.Should().Be(fundingHistoryComplete);
            mockRepository.VerifyAll();
        }
        [Fact]
        public void AddFundingHistory_SetsFundingHistories()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var id = new FundingHistoryId(Guid.NewGuid());
            var fundingType = "funding type";
            var fundingAmount = (decimal)100.10;
            var financialYear = "financial year";
            var fundingRounds = 10;
            var comments = "comments";
            var supportProjectId = new SupportProjectId(1);
            // Act
            supportProject.AddFundingHistory(id, supportProjectId, fundingType, fundingAmount, financialYear, fundingRounds, comments);

            // Assert
            supportProject.FundingHistories.Should().NotBeNull();
            supportProject.FundingHistories.Count().Should().Be(1);

            var fundingHistory = supportProject.FundingHistories.First();

            fundingHistory.FundingType.Should().Be(fundingType);
            fundingHistory.FundingAmount.Should().Be(fundingAmount);
            fundingHistory.FinancialYear.Should().Be(financialYear);
            fundingHistory.FundingRounds.Should().Be(fundingRounds);
            fundingHistory.Comments.Should().Be(comments);

        }

        [Fact]
        public void AddContact_SetsContact()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var supportProjectContactId = new SupportProjectContactId(Guid.NewGuid());
            var name = "John";
            var author = "Author";
            var organisation = "Organisation";
            var email = "john@school.gov.uk";
            var phone = "0123456789";
            var roleId = RolesIds.DirectorOfEducation;
            var otherRoleName = "Other Role";
            var supportProjectId = new SupportProjectId(1);
            var createdOn = DateTime.UtcNow;

            // Act
            supportProject.AddContact(supportProjectContactId, name, roleId, otherRoleName, organisation, email, phone, author, createdOn, supportProjectId);

            // Assert
            supportProject.Contacts.Should().NotBeNull();
            foreach (var contact in supportProject.Contacts)
            {
                contact.Name.Should().Be(name);
                contact.RoleId.Should().Be(roleId);
                contact.OtherRoleName.Should().Be(otherRoleName);
                contact.Organisation.Should().Be(organisation);
                contact.Email.Should().Be(email);
                contact.Phone.Should().Be(phone);
                contact.CreatedOn.Should().Be(createdOn);
                contact.CreatedBy.Should().Be(author);
                contact.SupportProjectId.Should().Be(supportProjectId);
                contact.Id.Should().Be(supportProjectContactId);
                contact.LastModifiedBy.Should().BeNullOrWhiteSpace();
                contact.LastModifiedOn.Should().BeNull();
            }
        }

        [Fact]
        public void EditContact_SetsContact()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var name = "John";
            var author = "Author";
            var organisation = "Organisation";
            var email = "john@school.gov.uk";
            var phone = "0123456789";
            var createdOn = DateTime.UtcNow;

            var supportProjectContactId = new SupportProjectContactId(Guid.NewGuid());
            supportProject.AddContact(
                supportProjectContactId,
                name,
                RolesIds.ChairOfGovernors,
                "",
               organisation,
                email,
                phone,
                author,
                createdOn,
                supportProject.Id);

            var roleId = RolesIds.Other;
            var otherRoleName = "Other Role";
            var lastModifiedOn = DateTime.UtcNow;

            // Act
            supportProject.EditContact(supportProjectContactId, name, roleId, otherRoleName, organisation, email, phone, author, lastModifiedOn);

            // Assert
            supportProject.Contacts.Should().NotBeNull();
            foreach (var contact in supportProject.Contacts)
            {
                contact.Name.Should().Be(name);
                contact.RoleId.Should().Be(roleId);
                contact.OtherRoleName.Should().Be(otherRoleName);
                contact.Organisation.Should().Be(organisation);
                contact.Email.Should().Be(email);
                contact.Phone.Should().Be(phone);
                contact.CreatedOn.Should().Be(createdOn);
                contact.CreatedBy.Should().Be(author);
                contact.SupportProjectId.Should().Be(supportProject.Id);
                contact.Id.Should().Be(supportProjectContactId);
                contact.LastModifiedBy.Should().Be(author);
                contact.LastModifiedOn.Should().Be(lastModifiedOn);
            }
        }

        [Fact]
        public void SetCaseStudyCandidateDetials_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? caseStudyCandidate = true;
            string? caseStudyDetails = "test details";

            // Act
            supportProject.SetCaseStudyDetails(
                caseStudyCandidate, caseStudyDetails);

            // Assert
            supportProject.CaseStudyCandidate.Should().Be(caseStudyCandidate);
            supportProject.CaseStudyDetails.Should().Be(caseStudyDetails);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetEngagementConcernDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            var engagementConcernRecorded = true;
            var engagementConcernDetails = "test details";
            var engagementConcernRaisedDate = DateTime.UtcNow;

            // Act
            supportProject.SetEngagementConcernDetails(
                engagementConcernRecorded, engagementConcernDetails, engagementConcernRaisedDate);

            // Assert
            supportProject.EngagementConcernRecorded.Should().Be(engagementConcernRecorded);
            supportProject.EngagementConcernDetails.Should().Be(engagementConcernDetails);
            supportProject.EngagementConcernRaisedDate.Should().Be(engagementConcernRaisedDate);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetEngagementConcernEscalation_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            var confirmStepsTaken = true;
            var primaryReason = "this is a reason";
            var escalationDetails = "this is some details";
            var dateOfDecision = DateTime.UtcNow;

            // Act
            supportProject.SetEngagementConcernEscalation(
                confirmStepsTaken, primaryReason, escalationDetails, dateOfDecision);

            // Assert
            supportProject.EngagementConcernEscalationConfirmStepsTaken.Should().Be(confirmStepsTaken);
            supportProject.EngagementConcernEscalationPrimaryReason.Should().Be(primaryReason);
            supportProject.EngagementConcernEscalationDetails.Should().Be(escalationDetails);
            supportProject.EngagementConcernEscalationDateOfDecision.Should().Be(dateOfDecision);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void AddImprovementPlan_WithValidParameters_AddsImprovementPlanToCollection()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());

            // Act
            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);

            // Assert
            supportProject.ImprovementPlans.Should().HaveCount(1);
            var addedPlan = supportProject.ImprovementPlans.First();
            addedPlan.Id.Should().Be(improvementPlanId);
            addedPlan.SupportProjectId.Should().Be(supportProject.Id);
        }

        [Fact]
        public void AddImprovementPlan_MultipleImprovementPlans_AddsAllToCollection()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var plan1Id = new ImprovementPlanId(Guid.NewGuid());
            var plan2Id = new ImprovementPlanId(Guid.NewGuid());

            // Act
            supportProject.AddImprovementPlan(plan1Id, supportProject.Id);
            supportProject.AddImprovementPlan(plan2Id, supportProject.Id);

            // Assert
            supportProject.ImprovementPlans.Should().HaveCount(2);
            supportProject.ImprovementPlans.Should().Contain(p => p.Id == plan1Id);
            supportProject.ImprovementPlans.Should().Contain(p => p.Id == plan2Id);
        }

        [Fact]
        public void AddImprovementPlanObjective_WithValidParameters_AddsObjectiveToImprovementPlan()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var areaOfImprovement = "QualityOfEducation";
            var details = "Improve mathematics outcomes";

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);

            // Act
            supportProject.AddImprovementPlanObjective(objectiveId, improvementPlanId, areaOfImprovement, details);

            // Assert
            var improvementPlan = supportProject.ImprovementPlans.First();
            improvementPlan.ImprovementPlanObjectives.Should().HaveCount(1);
            var addedObjective = improvementPlan.ImprovementPlanObjectives.First();
            addedObjective.Id.Should().Be(objectiveId);
            addedObjective.AreaOfImprovement.Should().Be(areaOfImprovement);
            addedObjective.Details.Should().Be(details);
            addedObjective.Order.Should().Be(1); // First objective should have order 1
        }

        [Fact]
        public void AddImprovementPlanObjective_WithNonExistentImprovementPlan_ThrowsInvalidOperationException()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var nonExistentPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                supportProject.AddImprovementPlanObjective(objectiveId, nonExistentPlanId, "QualityOfEducation", "Test details"));

            exception.Message.Should().Be($"Improvement plan with id {nonExistentPlanId} not found.");
        }

        [Fact]
        public void AddImprovementPlanObjective_MultipleObjectives_CalculatesOrderCorrectlyPerImprovementArea()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var qualityObjective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var leadershipObjective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var qualityObjective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var leadershipObjective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var behaviorObjective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);

            // Act - Add objectives in mixed order to test area-specific ordering
            supportProject.AddImprovementPlanObjective(qualityObjective1Id, improvementPlanId, "QualityOfEducation", "Quality Objective 1");
            supportProject.AddImprovementPlanObjective(leadershipObjective1Id, improvementPlanId, "LeadershipAndManagement", "Leadership Objective 1");
            supportProject.AddImprovementPlanObjective(qualityObjective2Id, improvementPlanId, "QualityOfEducation", "Quality Objective 2");
            supportProject.AddImprovementPlanObjective(leadershipObjective2Id, improvementPlanId, "LeadershipAndManagement", "Leadership Objective 2");
            supportProject.AddImprovementPlanObjective(behaviorObjective1Id, improvementPlanId, "BehaviourAndAttitudes", "Behavior Objective 1");

            // Assert
            var improvementPlan = supportProject.ImprovementPlans.First();
            var objectives = improvementPlan.ImprovementPlanObjectives.ToList();

            objectives.Should().HaveCount(5);

            // Quality of Education objectives should have order 1, 2
            objectives.First(o => o.Id == qualityObjective1Id).Order.Should().Be(1);
            objectives.First(o => o.Id == qualityObjective2Id).Order.Should().Be(2);

            // Leadership and Management objectives should have order 1, 2
            objectives.First(o => o.Id == leadershipObjective1Id).Order.Should().Be(1);
            objectives.First(o => o.Id == leadershipObjective2Id).Order.Should().Be(2);

            // Behaviour and Attitudes objective should have order 1
            objectives.First(o => o.Id == behaviorObjective1Id).Order.Should().Be(1);
        }

        [Fact]
        public void AddImprovementPlanObjective_MultipleObjectivesInSameArea_CalculatesOrderSequentially()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective3Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective4Id = new ImprovementPlanObjectiveId(Guid.NewGuid());

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);

            // Act - Add multiple objectives to the same improvement area
            supportProject.AddImprovementPlanObjective(objective1Id, improvementPlanId, "QualityOfEducation", "First objective");
            supportProject.AddImprovementPlanObjective(objective2Id, improvementPlanId, "QualityOfEducation", "Second objective");
            supportProject.AddImprovementPlanObjective(objective3Id, improvementPlanId, "QualityOfEducation", "Third objective");
            supportProject.AddImprovementPlanObjective(objective4Id, improvementPlanId, "QualityOfEducation", "Fourth objective");

            // Assert
            var improvementPlan = supportProject.ImprovementPlans.First();
            var qualityObjectives = improvementPlan.ImprovementPlanObjectives
                .Where(o => o.AreaOfImprovement == "QualityOfEducation")
                .ToList();

            qualityObjectives.Should().HaveCount(4);
            qualityObjectives.First(o => o.Id == objective1Id).Order.Should().Be(1);
            qualityObjectives.First(o => o.Id == objective2Id).Order.Should().Be(2);
            qualityObjectives.First(o => o.Id == objective3Id).Order.Should().Be(3);
            qualityObjectives.First(o => o.Id == objective4Id).Order.Should().Be(4);
        }

        [Theory]
        [InlineData("QualityOfEducation", "Improve reading comprehension")]
        [InlineData("LeadershipAndManagement", "Develop leadership capacity")]
        [InlineData("BehaviourAndAttitudes", "Improve student behavior")]
        [InlineData("Attendance", "Increase attendance rates")]
        [InlineData("PersonalDevelopment", "Enhance character education")]
        public void AddImprovementPlanObjective_WithDifferentAreasOfImprovement_AddsObjectiveCorrectly(string areaOfImprovement, string details)
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);

            // Act
            supportProject.AddImprovementPlanObjective(objectiveId, improvementPlanId, areaOfImprovement, details);

            // Assert
            var objective = supportProject.ImprovementPlans.First().ImprovementPlanObjectives.First();
            objective.AreaOfImprovement.Should().Be(areaOfImprovement);
            objective.Details.Should().Be(details);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetImprovementPlanObjectivesComplete_WithValidParameters_SetsCompletionStatus(bool objectivesSectionComplete)
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);

            // Act
            supportProject.SetImprovementPlanObjectivesComplete(improvementPlanId, objectivesSectionComplete);

            // Assert
            var improvementPlan = supportProject.ImprovementPlans.First();
            improvementPlan.ObjectivesSectionComplete.Should().Be(objectivesSectionComplete);
        }

        [Fact]
        public void SetImprovementPlanObjectivesComplete_WithNonExistentImprovementPlan_ThrowsInvalidOperationException()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var nonExistentPlanId = new ImprovementPlanId(Guid.NewGuid());

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                supportProject.SetImprovementPlanObjectivesComplete(nonExistentPlanId, true));

            exception.Message.Should().Be($"Improvement plan with id {nonExistentPlanId} not found.");
        }

        [Fact]
        public void SetImprovementPlanObjectiveDetails_WithValidParameters_UpdatesObjectiveDetails()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var originalDetails = "Original details";
            var updatedDetails = "Updated comprehensive details";

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);
            supportProject.AddImprovementPlanObjective(objectiveId, improvementPlanId, "QualityOfEducation", originalDetails);

            // Act
            supportProject.SetImprovementPlanObjectiveDetails(objectiveId, improvementPlanId, updatedDetails);

            // Assert
            var objective = supportProject.ImprovementPlans.First().ImprovementPlanObjectives.First();
            objective.Details.Should().Be(updatedDetails);
        }

        [Fact]
        public void SetImprovementPlanObjectiveDetails_WithNonExistentImprovementPlan_ThrowsInvalidOperationException()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var nonExistentPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                supportProject.SetImprovementPlanObjectiveDetails(objectiveId, nonExistentPlanId, "New details"));

            exception.Message.Should().Be($"Improvement plan with id {nonExistentPlanId} not found.");
        }

        [Fact]
        public void SetImprovementPlanObjectiveDetails_WithNonExistentObjective_ThrowsKeyNotFoundException()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var nonExistentObjectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                supportProject.SetImprovementPlanObjectiveDetails(nonExistentObjectiveId, improvementPlanId, "New details"));

            exception.Message.Should().Be($"Improvement plan objective with id {nonExistentObjectiveId} not found");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Short details")]
        [InlineData("Very comprehensive and detailed improvement objective with specific implementation strategies")]
        public void SetImprovementPlanObjectiveDetails_WithVariousDetailLengths_UpdatesDetailsCorrectly(string details)
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);
            supportProject.AddImprovementPlanObjective(objectiveId, improvementPlanId, "QualityOfEducation", "Original details");

            // Act
            supportProject.SetImprovementPlanObjectiveDetails(objectiveId, improvementPlanId, details);

            // Assert
            var objective = supportProject.ImprovementPlans.First().ImprovementPlanObjectives.First();
            objective.Details.Should().Be(details);
        }

        [Fact]
        public void SetImprovementPlanObjectiveDetails_WithMultipleObjectives_UpdatesCorrectObjective()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var updatedDetails = "Updated details for objective 2";

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);
            supportProject.AddImprovementPlanObjective(objective1Id, improvementPlanId, "QualityOfEducation", "Objective 1 details");
            supportProject.AddImprovementPlanObjective(objective2Id, improvementPlanId, "LeadershipAndManagement", "Objective 2 details");

            // Act
            supportProject.SetImprovementPlanObjectiveDetails(objective2Id, improvementPlanId, updatedDetails);

            // Assert
            var improvementPlan = supportProject.ImprovementPlans.First();
            var objective1 = improvementPlan.ImprovementPlanObjectives.First(o => o.Id == objective1Id);
            var objective2 = improvementPlan.ImprovementPlanObjectives.First(o => o.Id == objective2Id);

            objective1.Details.Should().Be("Objective 1 details"); // Unchanged
            objective2.Details.Should().Be(updatedDetails); // Updated
        }

        [Fact]
        public void ImprovementPlans_ReturnsReadOnlyCollection()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());

            supportProject.AddImprovementPlan(improvementPlanId, supportProject.Id);

            // Act
            var improvementPlans = supportProject.ImprovementPlans;

            // Assert
            improvementPlans.Should().BeAssignableTo<IEnumerable<ImprovementPlan>>();
            improvementPlans.Should().HaveCount(1);
        }
    }
}
