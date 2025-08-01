using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record SupportProjectDto(int Id,
        DateTime CreatedOn,
        string SchoolName = "",
        string SchoolUrn = "",
        string LocalAuthority = "",
        string Region = "",
        string? TrustName = null,
        string? TrustReferenceNumber = null,
        string AssignedDeliveryOfficerFullName = "",
        string AssignedDeliveryOfficerEmailAddress = "",
        bool? discussTheBestApproach = null,
        bool? emailTheResponsibleBody = null,
        DateTime? contactedTheResponsibleBodyDate = null,
        bool? SendConflictOfInterestFormToProposedAdviserAndTheSchool = null,
        bool? ReceiveCompletedConflictOfInterestForm = null,
        bool? SaveCompletedConflictOfinterestFormInSharePoint = null,
        DateTime? DateConflictsOfInterestWereChecked = null,
        DateTime? SchoolResponseDate = null,
        bool? HasAcknowledgedAndWillEngage = null,
        bool? HasSavedSchoolResponseinSharePoint = null,
        DateTime? DateAdviserAllocated = null,
        string? AdviserEmailAddress = null,
        string? AdviserFullName = null,
        DateTime? IntroductoryEmailSentDate = null,
        bool? HasShareEmailTemplateWithAdviser = null,
        bool? RemindAdviserToCopyRiseTeamWhenSentEmail = null,
        DateTime? AdviserVisitDate = null,
        DateTime? SavedAssessmentTemplateInSharePointDate = null,
        bool? HasTalkToAdviserAboutFindings = null,
        bool? HasCompleteAssessmentTemplate = null,
        bool? GiveTheAdviserTheNoteOfVisitTemplate = null,
        DateTime? SchoolVisitDate = null,
        DateTime? DateSupportOrganisationChosen = null,
        string? SupportOrganisationName = null,
        string SupportOrganisationIdNumber = "",
        DateTime? RegionalDirectorDecisionDate = null,
        bool? HasSchoolMatchedWithSupportingOrganisation = null,
        string? NotMatchingSchoolWithSupportingOrgNotes = null,
        string? InitialDiagnosisMatchingDecision = null,
        string? InitialDiagnosisMatchingDecisionNotes = null,
        bool? CheckOrganisationHasCapacityAndWillingToProvideSupport = null,
        bool? CheckChoiceWithTrustRelationshipManagerOrLaLead = null,
        bool? DiscussChoiceWithSfso = null,
        bool? CheckTheOrganisationHasAVendorAccount = null,
        DateTime? DateDueDiligenceCompleted = null,
        DateTime? RegionalDirectorAppointmentDate = null,
        bool? HasConfirmedSupportingOrganisationAppointment = null,
        string? DisapprovingSupportingOrganisationAppointmentNotes = null,
        DateTime? DateSupportingOrganisationContactDetailsAdded = null,
        string? SupportingOrganisationContactName = null,
        string SupportingOrganisationContactEmailAddress = "",
        DateTime? RegionalDirectorImprovementPlanDecisionDate = null,
        bool? HasApprovedImprovementPlanDecision = null,
        string? DisapprovingImprovementPlanDecisionNotes = null,
        bool? HasSavedImprovementPlanInSharePoint = null,
        bool? HasEmailedAgreedPlanToRegionalDirectorForApproval = null,
        DateTime? DateTeamContactedForRequestingPlanningGrantOfferLetter = null,
        bool? IncludeContactDetailsRequestingPlanningGrantOfferEmail = null,
        bool? ConfirmAmountOfPlanningGrantFundingRequested = null,
        bool? CopyInRegionalDirectorRequestingPlanningGrantOfferEmail = null,
        bool? SendRequestingPlanningGrantOfferEmailToRiseGrantTeam = null,
        DateTime? ImprovementPlanReceivedDate = null,
        bool? ReviewImprovementAndExpenditurePlan = null,
        bool? ConfirmFundingBand = null,
        string? FundingBand = null,
        bool? ConfirmPlanClearedByRiseGrantTeam = null,
        DateTime? DateTeamContactedForRequestingImprovementGrantOfferLetter = null,
        DateTime? DateTeamContactedForConfirmingPlanningGrantOfferLetter = null,
        DateTime? DateImprovementGrantOfferLetterSent = null,
        SupportProjectStatus? SupportProjectStatus = null,
        string? SchoolIsNotEligibleNotes = null,
        IEnumerable<SupportProjectContact> Contacts = null!,
        IEnumerable<SupportProjectNote> Notes = null!,
        bool? HasReceivedFundingInThelastTwoYears = null,
        bool? FundingHistoryDetailsComplete = null,
        IEnumerable<FundingHistoryDto> FundingHistories = null!,
        bool? CaseStudyCandidate = null,
        string? CaseStudyDetails = null,
        bool? EngagementConcernRecorded = null,
        string? EngagementConcernDetails = null,
        bool? IncludeContactDetails = null,
        bool? AttachSchoolImprovementPlan = null,
        bool? CopyInRegionalDirector = null,
        bool? SendEmailToGrantTeam = null,
        bool? EngagementConcernEscalationConfirmStepsTaken = null,
        string? EngagementConcernEscalationPrimaryReason = null,
        string? EngagementConcernEscalationDetails = null,
        DateTime? EngagementConcernEscalationDateOfDecision = null,
        DateTime? EngagementConcernRaisedDate = null,
        bool? InformationPowersInUse = null,
        string? InformationPowersDetails = null,
        DateTime? PowersUsedDate = null,
        bool? AssessmentToolTwoCompleted = null,
        bool? IndicativeFundingBandCalculated = null,
        string? IndicativeFundingBand = null,
        bool? ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody = null,
        DateTime? DateTemplatesAndIndicativeFundingBandSent = null,
        IEnumerable<ImprovementPlanDto> ImprovementPlans = null!
    )
    { }
}
