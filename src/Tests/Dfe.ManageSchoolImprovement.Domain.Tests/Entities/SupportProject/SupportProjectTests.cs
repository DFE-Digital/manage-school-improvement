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
            supportProject.HasAcceptedTargetedSupport.Should().Be(hasAcceptedTargetedSupport);
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
        public void SetCompleteAndSaveAssessmentTemplate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var supportProject = CreateSupportProject();
            var savedAssessmentTemplateInSharePointDate = DateTime.UtcNow;
            var hasTalkToAdviser = true;
            var hasCompleteAssessmentTemplate = true;

            // Act
            supportProject.SetCompleteAndSaveAssessmentTemplate(
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

            // Act
            supportProject.SetAdviserDetails(
                adviserEmailAddress,
                dateAdviserAllocated);

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

            // Act
            supportProject.SetAdviserVisitDate(
                adviserVisitDate);

            // Assert
            supportProject.AdviserVisitDate.Should().Be(adviserVisitDate);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void SetNoteOfVisit_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? giveTheAdviserTheNoteOfVisitTemplate = false;
            bool? askTheAdviserToSendYouTheirNotes = false;
            DateTime? dateNoteOfVisitSavedInSharePoint = DateTime.UtcNow;

            // Act
            supportProject.SetNoteOfVisitDetails(
                giveTheAdviserTheNoteOfVisitTemplate,
                askTheAdviserToSendYouTheirNotes,
                dateNoteOfVisitSavedInSharePoint);

            // Assert
            supportProject.GiveTheAdviserTheNoteOfVisitTemplate.Should().Be(giveTheAdviserTheNoteOfVisitTemplate);
            supportProject.AskTheAdviserToSendYouTheirNotes.Should().Be(askTheAdviserToSendYouTheirNotes);
            supportProject.DateNoteOfVisitSavedInSharePoint.Should().Be(dateNoteOfVisitSavedInSharePoint);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetRecordSupportDecision_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? hasSchoolMatchedWithSupportingOrganisation = false;
            DateTime? regionalDirectorDecisionDate = DateTime.UtcNow;
            string? notMatchingSchoolWithSupportingOrgNotes = "Notes only if choose no";

            // Act
            supportProject.SetRecordMatchingDecision(
                regionalDirectorDecisionDate,
                hasSchoolMatchedWithSupportingOrganisation,
                notMatchingSchoolWithSupportingOrgNotes);

            // Assert
            supportProject.HasSchoolMatchedWithSupportingOrganisation.Should().Be(hasSchoolMatchedWithSupportingOrganisation);
            supportProject.RegionalDirectorDecisionDate.Should().Be(regionalDirectorDecisionDate);
            supportProject.NotMatchingSchoolWithSupportingOrgNotes.Should().Be(notMatchingSchoolWithSupportingOrgNotes);
            mockRepository.VerifyAll();
        }

        [Fact]
        public void SetRecordSupportDecision_KeepsNotes_OnConfirmingTargetSupport()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            bool? hasSchoolMatchedWithSupportingOrganisation = true;
            DateTime? regionalDirectorDecisionDate = DateTime.UtcNow;
            string? notMatchingSchoolWithSupportingOrgNotes = "Notes only if choose no";

            // Act
            supportProject.SetRecordMatchingDecision(
                regionalDirectorDecisionDate,
                hasSchoolMatchedWithSupportingOrganisation,
                notMatchingSchoolWithSupportingOrgNotes);

            // Assert
            supportProject.HasSchoolMatchedWithSupportingOrganisation.Should().Be(hasSchoolMatchedWithSupportingOrganisation);
            supportProject.RegionalDirectorDecisionDate.Should().Be(regionalDirectorDecisionDate);
            supportProject.NotMatchingSchoolWithSupportingOrgNotes.Should().Be(notMatchingSchoolWithSupportingOrgNotes);
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

            // Act
            supportProject.SetChoosePreferredSupportOrganisation(
                dateSupportOrganisationChosen,
                supportOrgansiationName,
                supportOrganisationId);

            // Assert
            supportProject.DateSupportOrganisationChosen.Should().Be(dateSupportOrganisationChosen);
            supportProject.SupportOrganisationName.Should().Be(supportOrgansiationName);
            supportProject.SupportOrganisationIdNumber.Should().Be(supportOrganisationId);
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
        public void SetRecordImprovementPlanDecision_KeepsNotes_OnConfirmingTargetSupport()
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
        public void SetShareImproveMentPlanDetails_WithValidDetails_SetsTheCorrectProperties()
        {
            // Arrange
            var supportProject = CreateSupportProject();

            DateTime? dateTemplatesSent = DateTime.UtcNow;
            bool? sendTheTemplateToTheSupportingOrganisation = true;
            bool? sendTheTemplateToTheSchoolsResponsibleBody = true;

            // Act
            supportProject.SetImprovementPlanTemplateDetails(
                sendTheTemplateToTheSupportingOrganisation,
                sendTheTemplateToTheSchoolsResponsibleBody,
                dateTemplatesSent);

            // Assert
            supportProject.DateTemplatesSent.Should().Be(dateTemplatesSent);
            supportProject.SendTheTemplateToTheSupportingOrganisation.Should().Be(sendTheTemplateToTheSupportingOrganisation);
            supportProject.SendTheTemplateToTheSchoolsResponsibleBody.Should().Be(sendTheTemplateToTheSchoolsResponsibleBody);
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

            // Act
            supportProject.SetRequestImprovementGrantOfferLetter(dateTeamContactedForRequestingImprovementGrantOfferLetter);

            // Assert
            supportProject.DateTeamContactedForRequestingImprovementGrantOfferLetter.Should().Be(dateTeamContactedForRequestingImprovementGrantOfferLetter);
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

            bool? engagementConcernRecorded = true;
            string? engagementConcernDetails = "test details";

            // Act
            supportProject.SetEngagementConcernDetails(
                engagementConcernRecorded, engagementConcernDetails);

            // Assert
            supportProject.EngagementConcernRecorded.Should().Be(engagementConcernRecorded);
            supportProject.EngagementConcernDetails.Should().Be(engagementConcernDetails);
            mockRepository.VerifyAll();
        }
    }
}
