using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class SupportProjectViewModelTests
    {
        [Fact]
        public void Create_ShouldReturnSupportProjectViewModel_WithCorrectProperties()
        {
            // Arrange
            var supportProjectDto = new SupportProjectDto(
                Id: 1,
                CreatedOn: DateTime.Now,
                SchoolName: "School Name",
                SchoolUrn: "123456",
                LocalAuthority: "Local Authority",
                Region: "Region",
                FindSchoolEmailAddress: true,
                UseTheNotificationLetterToCreateEmail: true,
                AttachRiseInfoToEmail: true,
                ContactedTheSchoolDate: DateTime.Now,
                SendConflictOfInterestFormToProposedAdviserAndTheSchool: true,
                ReceiveCompletedConflictOfInterestForm: true,
                SaveCompletedConflictOfinterestFormInSharePoint: true,
                DateConflictsOfInterestWereChecked: DateTime.Now,
                SchoolResponseDate: DateTime.Now,
                HasAcceptedTargetedSupport: true,
                HasSavedSchoolResponseinSharePoint: true,
                DateAdviserAllocated: DateTime.Now,
                AdviserEmailAddress: "adviser@example.com",
                IntroductoryEmailSentDate: DateTime.Now,
                HasShareEmailTemplateWithAdviser: true,
                RemindAdviserToCopyRiseTeamWhenSentEmail: true,
                AdviserVisitDate: DateTime.Now,
                SavedAssessmentTemplateInSharePointDate: DateTime.Now,
                HasTalkToAdviserAboutFindings: true,
                HasCompleteAssessmentTemplate: true,
                GiveTheAdviserTheNoteOfVisitTemplate: true,
                AskTheAdviserToSendYouTheirNotes: true,
                DateNoteOfVisitSavedInSharePoint: DateTime.Now,
                SchoolVisitDate: DateTime.Now,
                DateSupportOrganisationChosen: DateTime.Now,
                SupportOrganisationName: "Support Org",
                SupportOrganisationIdNumber: "SO123",
                RegionalDirectorDecisionDate: DateTime.Now,
                HasSchoolMatchedWithSupportingOrganisation: true,
                NotMatchingSchoolWithSupportingOrgNotes: "Notes",
                CheckOrganisationHasCapacityAndWillingToProvideSupport: true,
                CheckChoiceWithTrustRelationshipManagerOrLaLead: true,
                DiscussChoiceWithSfso: true,
                CheckFinancialConcernsAtSupportingOrganisation: true,
                CheckTheOrganisationHasAVendorAccount: true,
                DateDueDiligenceCompleted: DateTime.Now,
                RegionalDirectorAppointmentDate: DateTime.Now,
                HasConfirmedSupportingOrganisationAppointment: true,
                DisapprovingSupportingOrganisationAppointmentNotes: "Notes",
                DateSupportingOrganisationContactDetailsAdded: DateTime.Now,
                SupportingOrganisationContactName: "Contact Name",
                SupportingOrganisationContactEmailAddress: "contact@example.com",
                SendTheTemplateToTheSupportingOrganisation: true,
                SendTheTemplateToTheSchoolsResponsibleBody: true,
                DateTemplatesSent: DateTime.Now,
                RegionalDirectorImprovementPlanDecisionDate: DateTime.Now,
                HasApprovedImprovementPlanDecision: true,
                DisapprovingImprovementPlanDecisionNotes: "Notes",
                HasSavedImprovementPlanInSharePoint: true,
                HasEmailedAgreedPlanToRegionalDirectorForApproval: true,
                ImprovementPlanReceivedDate: DateTime.Now,
                ReviewImprovementPlanWithTeam: true,
                DateTeamContactedForRequestingPlanningGrantOfferLetter: DateTime.Now,
                DateTeamContactedForRequestingImprovementGrantOfferLetter: DateTime.Now,
                DateTeamContactedForConfirmingPlanningGrantOfferLetter: DateTime.Now,
                DateImprovementGrantOfferLetterSent: DateTime.Now,
                Notes: new List<SupportProjectNote>()
            );

            // Act
            var viewModel = SupportProjectViewModel.Create(supportProjectDto);

            // Assert
            Assert.Equal(supportProjectDto.Id, viewModel.Id);
            Assert.Equal(supportProjectDto.CreatedOn, viewModel.CreatedOn);
            Assert.Equal(supportProjectDto.LocalAuthority, viewModel.LocalAuthority);
            Assert.Equal(supportProjectDto.Region, viewModel.Region);
            Assert.Equal(supportProjectDto.SchoolName, viewModel.SchoolName);
            Assert.Equal(supportProjectDto.SchoolUrn, viewModel.SchoolUrn);
            Assert.Equal(supportProjectDto.Notes, viewModel.Notes);
            Assert.Equal(supportProjectDto.FindSchoolEmailAddress, viewModel.FindSchoolEmailAddress);
            Assert.Equal(supportProjectDto.UseTheNotificationLetterToCreateEmail, viewModel.UseTheNotificationLetterToCreateEmail);
            Assert.Equal(supportProjectDto.AttachRiseInfoToEmail, viewModel.AttachRiseInfoToEmail);
            Assert.Equal(supportProjectDto.ContactedTheSchoolDate, viewModel.ContactedTheSchoolDate);
            Assert.Equal(supportProjectDto.SendConflictOfInterestFormToProposedAdviserAndTheSchool, viewModel.SendConflictOfInterestFormToProposedAdviserAndTheSchool);
            Assert.Equal(supportProjectDto.ReceiveCompletedConflictOfInterestForm, viewModel.ReceiveCompletedConflictOfInterestForm);
            Assert.Equal(supportProjectDto.SaveCompletedConflictOfinterestFormInSharePoint, viewModel.SaveCompletedConflictOfinterestFormInSharePoint);
            Assert.Equal(supportProjectDto.DateConflictsOfInterestWereChecked, viewModel.DateConflictsOfInterestWereChecked);
            Assert.Equal(supportProjectDto.SchoolResponseDate, viewModel.SchoolResponseDate);
            Assert.Equal(supportProjectDto.HasAcceptedTargetedSupport, viewModel.HasAcceptedTargetedSupport);
            Assert.Equal(supportProjectDto.HasSavedSchoolResponseinSharePoint, viewModel.HasSavedSchoolResponseinSharePoint);
            Assert.Equal(supportProjectDto.HasShareEmailTemplateWithAdviser, viewModel.HasShareEmailTemplateWithAdviser);
            Assert.Equal(supportProjectDto.RemindAdviserToCopyRiseTeamWhenSentEmail, viewModel.RemindAdviserToCopyRiseTeamWhenSentEmail);
            Assert.Equal(supportProjectDto.IntroductoryEmailSentDate, viewModel.IntroductoryEmailSentDate);
            Assert.Equal(supportProjectDto.AdviserEmailAddress, viewModel.AdviserEmailAddress);
            Assert.Equal(supportProjectDto.DateAdviserAllocated, viewModel.DateAdviserAllocated);
            Assert.Equal(supportProjectDto.SavedAssessmentTemplateInSharePointDate, viewModel.SavedAssessmentTemplateInSharePointDate);
            Assert.Equal(supportProjectDto.HasTalkToAdviserAboutFindings, viewModel.HasTalkToAdviserAboutFindings);
            Assert.Equal(supportProjectDto.HasCompleteAssessmentTemplate, viewModel.HasCompleteAssessmentTemplate);
            Assert.Equal(supportProjectDto.AdviserVisitDate, viewModel.AdviserVisitDate);
            Assert.Equal(supportProjectDto.GiveTheAdviserTheNoteOfVisitTemplate, viewModel.GiveTheAdviserTheNoteOfVisitTemplate);
            Assert.Equal(supportProjectDto.AskTheAdviserToSendYouTheirNotes, viewModel.AskTheAdviserToSendYouTheirNotes);
            Assert.Equal(supportProjectDto.DateNoteOfVisitSavedInSharePoint, viewModel.DateNoteOfVisitSavedInSharePoint);
            Assert.Equal(supportProjectDto.SchoolVisitDate, viewModel.SchoolVisitDate);
            Assert.Equal(supportProjectDto.DateSupportOrganisationChosen, viewModel.DateSupportOrganisationChosen);
            Assert.Equal(supportProjectDto.SupportOrganisationName, viewModel.SupportOrganisationName);
            Assert.Equal(supportProjectDto.SupportOrganisationIdNumber, viewModel.SupportOrganisationIdNumber);
            Assert.Equal(supportProjectDto.RegionalDirectorDecisionDate, viewModel.RegionalDirectorDecisionDate);
            Assert.Equal(supportProjectDto.HasSchoolMatchedWithSupportingOrganisation, viewModel.HasSchoolMatchedWithSupportingOrganisation);
            Assert.Equal(supportProjectDto.NotMatchingSchoolWithSupportingOrgNotes, viewModel.NotMatchingSchoolWithSupportingOrgNotes);
            Assert.Equal(supportProjectDto.CheckOrganisationHasCapacityAndWillingToProvideSupport, viewModel.CheckOrganisationHasCapacityAndWillingToProvideSupport);
            Assert.Equal(supportProjectDto.CheckChoiceWithTrustRelationshipManagerOrLaLead, viewModel.CheckChoiceWithTrustRelationshipManagerOrLaLead);
            Assert.Equal(supportProjectDto.DiscussChoiceWithSfso, viewModel.DiscussChoiceWithSfso);
            Assert.Equal(supportProjectDto.CheckFinancialConcernsAtSupportingOrganisation, viewModel.CheckFinancialConcernsAtSupportingOrganisation);
            Assert.Equal(supportProjectDto.CheckTheOrganisationHasAVendorAccount, viewModel.CheckTheOrganisationHasAVendorAccount);
            Assert.Equal(supportProjectDto.DateDueDiligenceCompleted, viewModel.DateDueDiligenceCompleted);
            Assert.Equal(supportProjectDto.RegionalDirectorAppointmentDate, viewModel.RegionalDirectorAppointmentDate);
            Assert.Equal(supportProjectDto.HasConfirmedSupportingOrganisationAppointment, viewModel.HasConfirmedSupportingOrganisationAppointment);
            Assert.Equal(supportProjectDto.DisapprovingSupportingOrganisationAppointmentNotes, viewModel.DisapprovingSupportingOrganisationAppointmentNotes);
            Assert.Equal(supportProjectDto.DateSupportingOrganisationContactDetailsAdded, viewModel.DateSupportingOrganisationContactDetailsAdded);
            Assert.Equal(supportProjectDto.SupportingOrganisationContactName, viewModel.SupportingOrganisationContactName);
            Assert.Equal(supportProjectDto.SupportingOrganisationContactEmailAddress, viewModel.SupportingOrganisationContactEmailAddress);
            Assert.Equal(supportProjectDto.SendTheTemplateToTheSupportingOrganisation, viewModel.SendTheTemplateToTheSupportingOrganisation);
            Assert.Equal(supportProjectDto.SendTheTemplateToTheSchoolsResponsibleBody, viewModel.SendTheTemplateToTheSchoolsResponsibleBody);
            Assert.Equal(supportProjectDto.DateTemplatesSent, viewModel.DateTemplatesSent);
            Assert.Equal(supportProjectDto.RegionalDirectorImprovementPlanDecisionDate, viewModel.RegionalDirectorImprovementPlanDecisionDate);
            Assert.Equal(supportProjectDto.HasApprovedImprovementPlanDecision, viewModel.HasApprovedImprovementPlanDecision);
            Assert.Equal(supportProjectDto.HasSavedImprovementPlanInSharePoint, viewModel.HasSavedImprovementPlanInSharePoint);
            Assert.Equal(supportProjectDto.HasEmailedAgreedPlanToRegionalDirectorForApproval, viewModel.HasEmailedAgreedPlanToRegionalDirectorForApproval);
            Assert.Equal(supportProjectDto.DisapprovingImprovementPlanDecisionNotes, viewModel.DisapprovingImprovementPlanDecisionNotes);
            Assert.Equal(supportProjectDto.ImprovementPlanReceivedDate, viewModel.ImprovementPlanReceivedDate);
            Assert.Equal(supportProjectDto.ReviewImprovementPlanWithTeam, viewModel.ReviewImprovementPlanWithTeam);
            Assert.Equal(supportProjectDto.DateTeamContactedForRequestingPlanningGrantOfferLetter, viewModel.DateTeamContactedForRequestingPlanningGrantOfferLetter);
            Assert.Equal(supportProjectDto.DateTeamContactedForRequestingImprovementGrantOfferLetter, viewModel.DateTeamContactedForRequestingImprovementGrantOfferLetter);
            Assert.Equal(supportProjectDto.DateTeamContactedForConfirmingPlanningGrantOfferLetter, viewModel.DateTeamContactedForConfirmingPlanningGrantOfferLetter);
            Assert.Equal(supportProjectDto.DateImprovementGrantOfferLetterSent, viewModel.DateImprovementGrantOfferLetterSent);
        }
    }
}
