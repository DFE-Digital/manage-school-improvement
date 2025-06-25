using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

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

        public string Diocese { get; set; }

        public string SchoolPhase { get; set; }
        public string? SchoolType { get; set; }

        public string ReligiousCharacter { get; set; }


        public string NumbersOnRoll { get; set; }

        public string AssignedDeliveryOfficerFullName { get; set; }

        public string AssignedDeliveryOfficerEmailAddress { get; set; }

        public string QualityOfEducation { get; set; }

        public string BehaviourAndAttitudes { get; set; }

        public string PersonalDevelopment { get; set; }

        public string LeadershipAndManagement { get; set; }

        public string OftedReportWeblink { get; set; }

        public string LastInspectionDate { get; set; }

        public bool? DiscussTheBestApproach { get; private set; }

        public bool? EmailTheResponsibleBody { get; private set; }

        public DateTime? ContactedTheResponsibleBodyDate { get; private set; }

        public bool? SendConflictOfInterestFormToProposedAdviserAndTheSchool { get; private set; }

        public bool? ReceiveCompletedConflictOfInterestForm { get; private set; }

        public bool? SaveCompletedConflictOfinterestFormInSharePoint { get; private set; }

        public DateTime? DateConflictsOfInterestWereChecked { get; private set; }

        public IEnumerable<SupportProjectNote> Notes { get; set; }

        public IEnumerable<SupportProjectContact> Contacts { get; set; }

        public DateTime? SchoolResponseDate { get; set; }

        public bool? HasAcknowledgedAndWillEngage { get; set; }

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

        public bool? CheckTheOrganisationHasAVendorAccount { get; set; }

        public DateTime? DateDueDiligenceCompleted { get; set; }

        public bool? HasConfirmedSupportingOrganisationAppointment { get; set; }
        public DateTime? RegionalDirectorAppointmentDate { get; set; }
        public string? DisapprovingSupportingOrganisationAppointmentNotes { get; set; }


        public DateTime? DateSupportingOrganisationContactDetailsAdded { get; set; }

        public string? SupportingOrganisationContactName { get; set; }

        public string? SupportingOrganisationContactEmailAddress { get; set; }

        public bool? IndicativeFundingBandCalculated { get; set; }
        public string? IndicativeFundingBand { get; set; }
        public bool? ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody { get; set; }
        public DateTime? DateTemplatesAndIndicativeFundingBandSent { get; set; }


        public DateTime? RegionalDirectorImprovementPlanDecisionDate { get; set; }
        public bool? HasApprovedImprovementPlanDecision { get; set; }
        public string? DisapprovingImprovementPlanDecisionNotes { get; set; }
        public bool? HasSavedImprovementPlanInSharePoint { get; set; }
        public bool? HasEmailedAgreedPlanToRegionalDirectorForApproval { get; set; }
        public DateTime? DateTeamContactedForRequestingPlanningGrantOfferLetter { get; set; }

        public DateTime? ImprovementPlanReceivedDate { get; set; }

        public bool? ReviewImprovementAndExpenditurePlan { get; set; }

        public bool? ConfirmPlanClearedByRiseGrantTeam { get; set; }
        public DateTime? DateTeamContactedForRequestingImprovementGrantOfferLetter { get; set; }

        public DateTime? DateTeamContactedForConfirmingPlanningGrantOfferLetter { get; set; }
        public bool? SendRequestingPlanningGrantOfferEmailToRiseGrantTeam { get; set; }
        public bool? IncludeContactDetailsRequestingPlanningGrantOfferEmail { get; set; }
        public bool? CopyInRegionalDirectorRequestingPlanningGrantOfferEmail { get; set; }
        public bool? ConfirmAmountOfPlanningGrantFundingRequested { get; set; }
        public DateTime? DateImprovementGrantOfferLetterSent { get; set; }
        public bool? HasReceivedFundingInThelastTwoYears { get; set; }
        public bool? FundingHistoryDetailsComplete { get; set; }
        public IEnumerable<FundingHistoryViewModel> FundingHistories { get; set; }

        public string? SchoolIsNotEligibleNotes { get; set; }
        public SupportProjectStatus? SupportProjectStatus { get; set; }
        public string? PreviousUrn { get; set; }
        public bool? CaseStudyCandidate { get; set; }
        public string? CaseStudyDetails { get; set; }

        public bool? EngagementConcernRecorded { get; set; }

        public string? EngagementConcernDetails { get; set; }
        public bool? IncludeContactDetails { get; set; }

        public bool? AttachSchoolImprovementPlan { get; set; }

        public bool? CopyInRegionalDirector { get; set; }

        public bool? SendEmailToGrantTeam { get; set; }

        public bool? EngagementConcernEscalationConfirmStepsTaken { get; set; }

        public string? EngagementConcernEscalationPrimaryReason { get; set; }

        public string? EngagementConcernEscalationDetails { get; set; }

        public DateTime? EngagementConcernEscalationDateOfDecision { get; set; }

        public DateTime? EngagementConcernRaisedDate { get; set; }

        public bool? InformationPowersInUse { get; set; }
        public string? InformationPowersDetails { get; set; }
        public DateTime? PowersUsedDate { get; set; }
        public bool? AssessmentToolTwoCompleted { get; set; }

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
                DiscussTheBestApproach = supportProjectDto.discussTheBestApproach,
                EmailTheResponsibleBody = supportProjectDto.emailTheResponsibleBody,
                ContactedTheResponsibleBodyDate = supportProjectDto.contactedTheResponsibleBodyDate,
                SendConflictOfInterestFormToProposedAdviserAndTheSchool = supportProjectDto.SendConflictOfInterestFormToProposedAdviserAndTheSchool,
                ReceiveCompletedConflictOfInterestForm = supportProjectDto.ReceiveCompletedConflictOfInterestForm,
                SaveCompletedConflictOfinterestFormInSharePoint = supportProjectDto.SaveCompletedConflictOfinterestFormInSharePoint,
                DateConflictsOfInterestWereChecked = supportProjectDto.DateConflictsOfInterestWereChecked,
                SchoolResponseDate = supportProjectDto.SchoolResponseDate,
                HasAcknowledgedAndWillEngage = supportProjectDto.HasAcknowledgedAndWillEngage,
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
                CheckTheOrganisationHasAVendorAccount = supportProjectDto.CheckTheOrganisationHasAVendorAccount,
                DateDueDiligenceCompleted = supportProjectDto.DateDueDiligenceCompleted,
                RegionalDirectorAppointmentDate = supportProjectDto.RegionalDirectorAppointmentDate,
                HasConfirmedSupportingOrganisationAppointment = supportProjectDto.HasConfirmedSupportingOrganisationAppointment,
                DisapprovingSupportingOrganisationAppointmentNotes = supportProjectDto.DisapprovingSupportingOrganisationAppointmentNotes,
                DateSupportingOrganisationContactDetailsAdded = supportProjectDto.DateSupportingOrganisationContactDetailsAdded,
                SupportingOrganisationContactName = supportProjectDto.SupportingOrganisationContactName,
                SupportingOrganisationContactEmailAddress = supportProjectDto.SupportingOrganisationContactEmailAddress,
                RegionalDirectorImprovementPlanDecisionDate = supportProjectDto.RegionalDirectorImprovementPlanDecisionDate,
                HasApprovedImprovementPlanDecision = supportProjectDto.HasApprovedImprovementPlanDecision,
                HasSavedImprovementPlanInSharePoint = supportProjectDto.HasSavedImprovementPlanInSharePoint,
                HasEmailedAgreedPlanToRegionalDirectorForApproval = supportProjectDto.HasEmailedAgreedPlanToRegionalDirectorForApproval,
                DisapprovingImprovementPlanDecisionNotes = supportProjectDto.DisapprovingImprovementPlanDecisionNotes,
                ImprovementPlanReceivedDate = supportProjectDto.ImprovementPlanReceivedDate,
                ReviewImprovementAndExpenditurePlan = supportProjectDto.ReviewImprovementAndExpenditurePlan,
                ConfirmPlanClearedByRiseGrantTeam = supportProjectDto.ConfirmPlanClearedByRiseGrantTeam,
                DateTeamContactedForRequestingPlanningGrantOfferLetter = supportProjectDto.DateTeamContactedForRequestingPlanningGrantOfferLetter,
                IncludeContactDetailsRequestingPlanningGrantOfferEmail = supportProjectDto.IncludeContactDetailsRequestingPlanningGrantOfferEmail,
                ConfirmAmountOfPlanningGrantFundingRequested = supportProjectDto.ConfirmAmountOfPlanningGrantFundingRequested,
                CopyInRegionalDirectorRequestingPlanningGrantOfferEmail = supportProjectDto.CopyInRegionalDirectorRequestingPlanningGrantOfferEmail,
                SendRequestingPlanningGrantOfferEmailToRiseGrantTeam = supportProjectDto.SendRequestingPlanningGrantOfferEmailToRiseGrantTeam,
                DateTeamContactedForRequestingImprovementGrantOfferLetter = supportProjectDto.DateTeamContactedForRequestingImprovementGrantOfferLetter,
                DateTeamContactedForConfirmingPlanningGrantOfferLetter = supportProjectDto.DateTeamContactedForConfirmingPlanningGrantOfferLetter,
                DateImprovementGrantOfferLetterSent = supportProjectDto.DateImprovementGrantOfferLetterSent,
                AssignedDeliveryOfficerFullName = supportProjectDto.AssignedDeliveryOfficerFullName,
                AssignedDeliveryOfficerEmailAddress = supportProjectDto.AssignedDeliveryOfficerEmailAddress,
                SupportProjectStatus = supportProjectDto.SupportProjectStatus,
                SchoolIsNotEligibleNotes = supportProjectDto.SchoolIsNotEligibleNotes,
                Contacts = supportProjectDto.Contacts,
                HasReceivedFundingInThelastTwoYears = supportProjectDto.HasReceivedFundingInThelastTwoYears,
                FundingHistoryDetailsComplete = supportProjectDto.FundingHistoryDetailsComplete,
                FundingHistories = supportProjectDto.FundingHistories?.Select(x => new FundingHistoryViewModel()
                {
                    Id = x.id,
                    SupportProjectId = x.supportProjectId,
                    ReadableId = x.readableId,
                    FundingType = x.fundingType,
                    FinancialYear = x.financialYear,
                    FundingAmount = x.fundingAmount,
                    FundingRounds = x.fundingRounds,
                    Comments = x.comments
                }) ?? new List<FundingHistoryViewModel>(),
                CaseStudyCandidate = supportProjectDto.CaseStudyCandidate,
                CaseStudyDetails = supportProjectDto.CaseStudyDetails,
                EngagementConcernRecorded = supportProjectDto.EngagementConcernRecorded,
                EngagementConcernDetails = supportProjectDto.EngagementConcernDetails,
                IncludeContactDetails = supportProjectDto.IncludeContactDetails,
                AttachSchoolImprovementPlan = supportProjectDto.AttachSchoolImprovementPlan,
                CopyInRegionalDirector = supportProjectDto.CopyInRegionalDirector,
                SendEmailToGrantTeam = supportProjectDto.SendEmailToGrantTeam,
                EngagementConcernEscalationConfirmStepsTaken = supportProjectDto.EngagementConcernEscalationConfirmStepsTaken,
                EngagementConcernEscalationPrimaryReason = supportProjectDto.EngagementConcernEscalationPrimaryReason,
                EngagementConcernEscalationDetails = supportProjectDto.EngagementConcernEscalationDetails,
                EngagementConcernEscalationDateOfDecision = supportProjectDto.EngagementConcernEscalationDateOfDecision,
                EngagementConcernRaisedDate = supportProjectDto.EngagementConcernRaisedDate,
                InformationPowersInUse = supportProjectDto.InformationPowersInUse,
                InformationPowersDetails = supportProjectDto.InformationPowersDetails,
                PowersUsedDate = supportProjectDto.PowersUsedDate,
                AssessmentToolTwoCompleted = supportProjectDto.AssessmentToolTwoCompleted,
                IndicativeFundingBandCalculated = supportProjectDto.IndicativeFundingBandCalculated,
                IndicativeFundingBand = supportProjectDto.IndicativeFundingBand,
                ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody = supportProjectDto.ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
                DateTemplatesAndIndicativeFundingBandSent = supportProjectDto.DateTemplatesAndIndicativeFundingBandSent
            };
        }
    }
}
