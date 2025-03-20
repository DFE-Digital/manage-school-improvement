using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record SupportProjectDto(int Id,
        DateTime CreatedOn,
        string SchoolName = "",
        string SchoolUrn = "",
        string LocalAuthority = "",
        string Region = "",
        string AssignedDeliveryOfficerFullName = "",
        string AssignedDeliveryOfficerEmailAddress = "",
        bool FindSchoolEmailAddress = false,
        bool UseTheNotificationLetterToCreateEmail = false,
        bool AttachRiseInfoToEmail = false,
        DateTime? ContactedTheSchoolDate = null,
        bool? SendConflictOfInterestFormToProposedAdviserAndTheSchool = null,
        bool? ReceiveCompletedConflictOfInterestForm = null,
        bool? SaveCompletedConflictOfinterestFormInSharePoint = null,
        DateTime? DateConflictsOfInterestWereChecked = null,
        DateTime? SchoolResponseDate = null,
        bool? HasAcceptedTargetedSupport = null,
        bool? HasSavedSchoolResponseinSharePoint = null,
        DateTime? DateAdviserAllocated = null,
        string? AdviserEmailAddress = null,
        DateTime? IntroductoryEmailSentDate = null,
        bool? HasShareEmailTemplateWithAdviser = null,
        bool? RemindAdviserToCopyRiseTeamWhenSentEmail = null,
        DateTime? AdviserVisitDate = null,
        DateTime? SavedAssessmentTemplateInSharePointDate = null,
        bool? HasTalkToAdviserAboutFindings = null,
        bool? HasCompleteAssessmentTemplate = null,
        bool? GiveTheAdviserTheNoteOfVisitTemplate = null,
        bool? AskTheAdviserToSendYouTheirNotes = null,
        DateTime? DateNoteOfVisitSavedInSharePoint = null,
        DateTime? SchoolVisitDate = null,
        DateTime? DateSupportOrganisationChosen = null,
        string? SupportOrganisationName = null,
        string SupportOrganisationIdNumber = "",
        DateTime? RegionalDirectorDecisionDate = null,
        bool? HasSchoolMatchedWithSupportingOrganisation = null,
        string? NotMatchingSchoolWithSupportingOrgNotes = null,
        bool? CheckOrganisationHasCapacityAndWillingToProvideSupport = null,
        bool? CheckChoiceWithTrustRelationshipManagerOrLaLead = null,
        bool? DiscussChoiceWithSfso = null,
        bool? CheckFinancialConcernsAtSupportingOrganisation = null,
        bool? CheckTheOrganisationHasAVendorAccount = null,
        DateTime? DateDueDiligenceCompleted = null,
        DateTime? RegionalDirectorAppointmentDate = null,
        bool? HasConfirmedSupportingOrganisationAppointment = null,
        string? DisapprovingSupportingOrganisationAppointmentNotes = null,
        DateTime? DateSupportingOrganisationContactDetailsAdded = null,
        string? SupportingOrganisationContactName = null,
        string SupportingOrganisationContactEmailAddress = "",
        bool? SendTheTemplateToTheSupportingOrganisation = null,
        bool? SendTheTemplateToTheSchoolsResponsibleBody = null,
        DateTime? DateTemplatesSent = null,
        DateTime? RegionalDirectorImprovementPlanDecisionDate = null,
        bool? HasApprovedImprovementPlanDecision = null,
        string? DisapprovingImprovementPlanDecisionNotes = null,
        bool? HasSavedImprovementPlanInSharePoint = null,
        bool? HasEmailedAgreedPlanToRegionalDirectorForApproval = null,
        DateTime? DateTeamContactedForRequestingPlanningGrantOfferLetter = null,
        DateTime? ImprovementPlanReceivedDate = null,
        bool? ReviewImprovementPlanWithTeam = null,
        DateTime? DateTeamContactedForRequestingImprovementGrantOfferLetter = null,
        DateTime? DateTeamContactedForConfirmingPlanningGrantOfferLetter = null,
        DateTime? DateImprovementGrantOfferLetterSent = null,
        bool? HasReceivedFundingInThelastTwoYears = null,
        IEnumerable<SupportProjectNote> Notes = null!,
        IEnumerable<FundingHistoryDto> FundingHistories = null!
    )
    { }
}
