using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class SupportProjectViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SchoolName { get; set; }

        public string SchoolUrn { get; set; }

        public string LocalAuthority { get; set; }

        public string Region { get; set; }

        public string AssignedDeliveryOfficerFullName { get; set; }

        public string AssignedDeliveryOfficerEmailAddress { get; set; }

        public string QualityOfEducation { get; set; }

        public string BehaviourAndAttitudes { get; set; }

        public string PersonalDevelopment { get; set; }

        public string LeadershipAndManagement { get; set; }

        public string OftedReportWeblink { get; set; }

        public string LastInspectionDate { get; set; }

        public bool? FindSchoolEmailAddress { get; private set; }

        public bool? UseTheNotificationLetterToCreateEmail { get; private set; }

        public bool? AttachRiseInfoToEmail { get; private set; }

        public DateTime? ContactedTheSchoolDate { get; private set; }

        public bool? SendConflictOfInterestFormToProposedAdviserAndTheSchool { get; private set; }

        public bool? ReceiveCompletedConflictOfInterestForm { get; private set; }

        public bool? SaveCompletedConflictOfinterestFormInSharePoint { get; private set; }

        public DateTime? DateConflictsOfInterestWereChecked { get; private set; }

        public IEnumerable<SupportProjectNote> Notes { get; set; }

        public DateTime? SchoolResponseDate { get; set; }

        public bool? HasAcceeptedTargetedSupport { get; set; }

        public bool? HasSavedSchoolResponseinSharePoint { get; set; }

        public DateTime? DateAdviserAllocated { get; private set; }
        public string? AdviserEmailAddress { get; private set; }

        public DateTime? IntroductoryEmailSentDate { get; set; }
        public bool? HasShareEmailTemplateWithAdviser { get; set; }

        public bool? RemindAdviserToCopyRiseTeamWhenSentEmail { get; set; }

        public DateTime? AdviserVisitDate { get; set; }
        public bool? GiveTheAdviserTheNoteOfVisitTemplate { get; private set; }
        public bool? AskTheAdviserToSendYouTheirNotes { get; private set; }
        public DateTime? DateNoteOfVisitSavedInSharePoint { get; private set; }

        public DateTime? SavedAssessmentTemplateInSharePointDate { get; set; }

        public bool? HasTalkToAdviserAboutFindings { get; set; }

        public bool? HasCompleteAssessmentTemplate { get; set; }

        public DateTime? SchoolVisitDate { get; set; }
        public bool? HasSchoolMatchedWithSupportingOrganisation { get; set; }
        public DateTime? RegionalDirectorDecisionDate { get; set; }
        public string? NotMatchingSchoolWithSupportingOrgNotes { get; set; }

        public DateTime? DateSupportOrganisationChosen { get; set; }

        public string? SupportOrganisationName { get; set; }

        public string? SupportOrganisationIdNumber { get; set; }

        public bool? CheckOrganisationHasCapacityAndWillingToProvideSupport { get; set; }

        public bool? CheckChoiceWithTrustRelationshipManagerOrLaLead { get; set; }

        public bool? DiscussChoiceWithSfso { get; set; }

        public bool? CheckFinancialConcernsAtSupportingOrganisation { get; set; }

        public bool? CheckTheOrganisationHasAVendorAccount { get; set; }

        public DateTime? DateDueDiligenceCompleted { get; set; }

        public bool? HasConfirmedSupportingOrgnaisationAppointment { get; set; }
        public DateTime? RegionalDirectorAppointmentDate { get; set; }
        public string? DisapprovingSupportingOrgnaisationAppointmentNotes { get; set; }


        public DateTime? DateSupportingOrganisationContactDetailsAdded { get; set; }

        public string? SupportingOrganisationContactName { get; set; }

        public string? SupportingOrganisationContactEmailAddress { get; set; }
        public bool? SendTheTemplateToTheSupportingOrganisation { get; set; }
        public bool? SendTheTemplateToTheSchoolsResponsibleBody { get; set; }
        public DateTime? DateTemplatesSent { get; set; }


        public DateTime? RegionalDirectorImprovementPlanDecisionDate { get; set; }
        public bool? HasApprovedImprovementPlanDecision { get; set; }
        public string? DisapprovingImprovementPlanDecisionNotes { get; set; }
        public bool? HasSavedImprovementPlanInSharePoint { get; set; }
        public bool? HasEmailedAgreedPlanToRegionalDirectorForApproval { get; set; }
        public DateTime? DateTeamContactedForRequestingPlanningGrantOfferLetter { get; set; }

        public DateTime? ImprovementPlanReceivedDate { get; set; }

        public bool? ReviewImprovementPlanWithTeam { get; set; }
        public DateTime? DateTeamContactedForRequestingImprovementGrantOfferLetter { get; set; }

        public DateTime? DateTeamContactedForConfirmingPlanningGrantOfferLetter { get; set; }
        public DateTime? DateImprovementGrantOfferLetterSent { get; set; }

        public static SupportProjectViewModel Create(SupportProjectDto supportProjectDto)
        {
            return new SupportProjectViewModel()
            {
                Id = supportProjectDto.Id,
                CreatedOn = supportProjectDto.CreatedOn,
                LocalAuthority = supportProjectDto.LocalAuthority,
                Region = supportProjectDto.Region,
                SchoolName = supportProjectDto.SchoolName,
                SchoolUrn = supportProjectDto.SchoolUrn,
                Notes = supportProjectDto.Notes,
                FindSchoolEmailAddress = supportProjectDto.FindSchoolEmailAddress,
                UseTheNotificationLetterToCreateEmail = supportProjectDto.UseTheNotificationLetterToCreateEmail,
                AttachRiseInfoToEmail = supportProjectDto.AttachRiseInfoToEmail,
                ContactedTheSchoolDate = supportProjectDto.ContactedTheSchoolDate,
                SendConflictOfInterestFormToProposedAdviserAndTheSchool = supportProjectDto.SendConflictOfInterestFormToProposedAdviserAndTheSchool,
                ReceiveCompletedConflictOfInterestForm = supportProjectDto.ReceiveCompletedConflictOfInterestForm,
                SaveCompletedConflictOfinterestFormInSharePoint = supportProjectDto.SaveCompletedConflictOfinterestFormInSharePoint,
                DateConflictsOfInterestWereChecked = supportProjectDto.DateConflictsOfInterestWereChecked,
                SchoolResponseDate = supportProjectDto.SchoolResponseDate,
                HasAcceeptedTargetedSupport = supportProjectDto.HasAcceeptedTargetedSupport,
                HasSavedSchoolResponseinSharePoint = supportProjectDto.HasSavedSchoolResponseinSharePoint,
                HasShareEmailTemplateWithAdviser = supportProjectDto.HasShareEmailTemplateWithAdviser,
                RemindAdviserToCopyRiseTeamWhenSentEmail = supportProjectDto.RemindAdviserToCopyRiseTeamWhenSentEmail,
                IntroductoryEmailSentDate = supportProjectDto.IntroductoryEmailSentDate,
                AdviserEmailAddress = supportProjectDto.AdviserEmailAddress,
                DateAdviserAllocated = supportProjectDto.DateAdviserAllocated,
                SavedAssessmentTemplateInSharePointDate = supportProjectDto.SavedAssessmentTemplateInSharePointDate,
                HasTalkToAdviserAboutFindings = supportProjectDto.HasTalkToAdviserAboutFindings,
                HasCompleteAssessmentTemplate = supportProjectDto.HasCompleteAssessmentTemplate,
                AdviserVisitDate = supportProjectDto.AdviserVisitDate,
                GiveTheAdviserTheNoteOfVisitTemplate = supportProjectDto.GiveTheAdviserTheNoteOfVisitTemplate,
                AskTheAdviserToSendYouTheirNotes = supportProjectDto.AskTheAdviserToSendYouTheirNotes,
                DateNoteOfVisitSavedInSharePoint = supportProjectDto.DateNoteOfVisitSavedInSharePoint,
                SchoolVisitDate = supportProjectDto.SchoolVisitDate,
                DateSupportOrganisationChosen = supportProjectDto.DateSupportOrganisationChosen,
                SupportOrganisationName = supportProjectDto.SupportOrganisationName,
                SupportOrganisationIdNumber = supportProjectDto.SupportOrganisationIdNumber,
                RegionalDirectorDecisionDate = supportProjectDto.RegionalDirectorDecisionDate,
                HasSchoolMatchedWithSupportingOrganisation = supportProjectDto.HasSchoolMatchedWithSupportingOrganisation,
                NotMatchingSchoolWithSupportingOrgNotes = supportProjectDto.NotMatchingSchoolWithSupportingOrgNotes,
                CheckOrganisationHasCapacityAndWillingToProvideSupport = supportProjectDto.CheckOrganisationHasCapacityAndWillingToProvideSupport,
                CheckChoiceWithTrustRelationshipManagerOrLaLead = supportProjectDto.CheckChoiceWithTrustRelationshipManagerOrLaLead,
                DiscussChoiceWithSfso = supportProjectDto.DiscussChoiceWithSfso,
                CheckFinancialConcernsAtSupportingOrganisation = supportProjectDto.CheckFinancialConcernsAtSupportingOrganisation,
                CheckTheOrganisationHasAVendorAccount = supportProjectDto.CheckTheOrganisationHasAVendorAccount,
                DateDueDiligenceCompleted = supportProjectDto.DateDueDiligenceCompleted,
                RegionalDirectorAppointmentDate = supportProjectDto.RegionalDirectorAppointmentDate,
                HasConfirmedSupportingOrgnaisationAppointment = supportProjectDto.HasConfirmedSupportingOrgnaisationAppointment,
                DisapprovingSupportingOrgnaisationAppointmentNotes = supportProjectDto.DisapprovingSupportingOrgnaisationAppointmentNotes,
                DateSupportingOrganisationContactDetailsAdded = supportProjectDto.DateSupportingOrganisationContactDetailsAdded,
                SupportingOrganisationContactName = supportProjectDto.SupportingOrganisationContactName,
                SupportingOrganisationContactEmailAddress = supportProjectDto.SupportingOrganisationContactEmailAddress,
                SendTheTemplateToTheSupportingOrganisation = supportProjectDto.SendTheTemplateToTheSupportingOrganisation,
                SendTheTemplateToTheSchoolsResponsibleBody = supportProjectDto.SendTheTemplateToTheSchoolsResponsibleBody,
                DateTemplatesSent = supportProjectDto.DateTemplatesSent,
                RegionalDirectorImprovementPlanDecisionDate = supportProjectDto.RegionalDirectorImprovementPlanDecisionDate,
                HasApprovedImprovementPlanDecision = supportProjectDto.HasApprovedImprovementPlanDecision,
                HasSavedImprovementPlanInSharePoint = supportProjectDto.HasSavedImprovementPlanInSharePoint,
                HasEmailedAgreedPlanToRegionalDirectorForApproval = supportProjectDto.HasEmailedAgreedPlanToRegionalDirectorForApproval,
                DisapprovingImprovementPlanDecisionNotes = supportProjectDto.DisapprovingImprovementPlanDecisionNotes,
                ImprovementPlanReceivedDate = supportProjectDto.ImprovementPlanReceivedDate,
                ReviewImprovementPlanWithTeam = supportProjectDto.ReviewImprovementPlanWithTeam,
                DateTeamContactedForRequestingPlanningGrantOfferLetter = supportProjectDto.DateTeamContactedForRequestingPlanningGrantOfferLetter,
                DateTeamContactedForRequestingImprovementGrantOfferLetter = supportProjectDto.DateTeamContactedForRequestingImprovementGrantOfferLetter,
                DateTeamContactedForConfirmingPlanningGrantOfferLetter = supportProjectDto.DateTeamContactedForConfirmingPlanningGrantOfferLetter,
                DateImprovementGrantOfferLetterSent = supportProjectDto.DateImprovementGrantOfferLetterSent,
                AssignedDeliveryOfficerFullName = supportProjectDto.AssignedDeliveryOfficerFullName,
                AssignedDeliveryOfficerEmailAddress = supportProjectDto.AssignedDeliveryOfficerEmailAddress,
            };
        }
    }
}
