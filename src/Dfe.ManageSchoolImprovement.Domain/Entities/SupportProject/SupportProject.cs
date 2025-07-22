using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;

public class SupportProject : BaseAggregateRoot, IEntity<SupportProjectId>
{
    private SupportProject() { }
    public SupportProject(
        SupportProjectId id,
        string schoolName,
        string schoolUrn,
        string localAuthority,
        string region,
        string? trustName = null,
        string? trustReferenceNumber = null)
    {
        Id = id;
        SchoolName = schoolName;
        SchoolUrn = schoolUrn;
        LocalAuthority = localAuthority;
        Region = region;
        TrustName = trustName;
        TrustReferenceNumber = trustReferenceNumber;
    }
    #region Properties
    public SupportProjectId Id { get; private set; }

    public string SchoolName { get; private set; }

    public string SchoolUrn { get; private set; }

    public string Region { get; private set; }

    public string LocalAuthority { get; private set; }

    public string? TrustName { get; private set; }

    public string? TrustReferenceNumber { get; private set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? AssignedDeliveryOfficerFullName { get; private set; }


    public string? AssignedDeliveryOfficerEmailAddress { get; private set; }

    public IEnumerable<SupportProjectNote> Notes => _notes.AsReadOnly();
    public IEnumerable<SupportProjectContact> Contacts => _contacts.AsReadOnly();

    private readonly List<SupportProjectNote> _notes = new();
    private readonly List<SupportProjectContact> _contacts = new();


    public bool? DiscussTheBestApproach { get; private set; }

    public bool? EmailTheResponsibleBody { get; private set; }

    public DateTime? ContactedTheResponsibleBodyDate { get; private set; }

    public bool? SendConflictOfInterestFormToProposedAdviserAndTheSchool { get; private set; }

    public bool? ReceiveCompletedConflictOfInterestForm { get; private set; }

    public bool? SaveCompletedConflictOfinterestFormInSharePoint { get; private set; }

    public DateTime? DateConflictsOfInterestWereChecked { get; private set; }

    public DateTime? SchoolResponseDate { get; private set; }

    public bool?
        HasAcknowledgedAndWillEngage
    { get; private set; }

    public bool? HasSavedSchoolResponseinSharePoint { get; private set; }

    public DateTime? DateAdviserAllocated { get; private set; }
    public string? AdviserEmailAddress { get; private set; }

    public DateTime? IntroductoryEmailSentDate { get; private set; }

    public bool? HasShareEmailTemplateWithAdviser { get; private set; }

    public bool? RemindAdviserToCopyRiseTeamWhenSentEmail { get; private set; }

    public DateTime? AdviserVisitDate { get; private set; }

    public DateTime? SavedAssessmentTemplateInSharePointDate { get; private set; }

    public bool? HasTalkToAdviserAboutFindings { get; private set; }

    public bool? HasCompleteAssessmentTemplate { get; private set; }

    public bool? GiveTheAdviserTheNoteOfVisitTemplate { get; private set; }

    public DateTime? SchoolVisitDate { get; private set; }

    public DateTime? DateSupportOrganisationChosen { get; private set; }

    public string? SupportOrganisationName { get; private set; }

    public string? SupportOrganisationIdNumber { get; private set; }
    public bool? AssessmentToolTwoCompleted { get; private set; }
    public DateTime? RegionalDirectorDecisionDate { get; private set; }

    public string? InitialDiagnosisMatchingDecision { get; private set; }
    public string? InitialDiagnosisMatchingDecisionNotes { get; private set; }

    public DateTime? DateSupportingOrganisationContactDetailsAdded { get; private set; }

    public string? SupportingOrganisationContactName { get; private set; }

    public string? SupportingOrganisationContactEmailAddress { get; private set; }


    public bool? CheckOrganisationHasCapacityAndWillingToProvideSupport { get; set; }

    public bool? CheckChoiceWithTrustRelationshipManagerOrLaLead { get; set; }

    public bool? DiscussChoiceWithSfso { get; set; }
    public bool? CheckTheOrganisationHasAVendorAccount { get; set; }
    public DateTime? DateDueDiligenceCompleted { get; set; }

    public DateTime? RegionalDirectorAppointmentDate { get; private set; }
    public bool? HasConfirmedSupportingOrganisationAppointment { get; private set; }
    public string? DisapprovingSupportingOrganisationAppointmentNotes { get; private set; }

    public DateTime? RegionalDirectorImprovementPlanDecisionDate { get; private set; }
    public bool? HasApprovedImprovementPlanDecision { get; private set; }
    public string? DisapprovingImprovementPlanDecisionNotes { get; private set; }
    public bool? HasSavedImprovementPlanInSharePoint { get; private set; }
    public bool? HasEmailedAgreedPlanToRegionalDirectorForApproval { get; private set; }

    public DateTime? DateTeamContactedForRequestingPlanningGrantOfferLetter { get; private set; }
    public bool? SendRequestingPlanningGrantOfferEmailToRiseGrantTeam { get; set; }
    public bool? IncludeContactDetailsRequestingPlanningGrantOfferEmail { get; set; }
    public bool? CopyInRegionalDirectorRequestingPlanningGrantOfferEmail { get; set; }
    public bool? ConfirmAmountOfPlanningGrantFundingRequested { get; set; }

    public DateTime? ImprovementPlanReceivedDate { get; private set; }
    public bool? ReviewImprovementAndExpenditurePlan { get; private set; }
    public bool? ConfirmFundingBand { get; private set; }
    public string? FundingBand { get; private set; }
    public bool? ConfirmPlanClearedByRiseGrantTeam { get; private set; }

    public DateTime? DateTeamContactedForRequestingImprovementGrantOfferLetter { get; private set; }

    public DateTime? DateTeamContactedForConfirmingPlanningGrantOfferLetter { get; private set; }

    public DateTime? DateImprovementGrantOfferLetterSent { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    public SupportProjectStatus? SupportProjectStatus { get; private set; }

    public string? SchoolIsNotEligibleNotes { get; private set; }

    public bool? HasReceivedFundingInThelastTwoYears { get; private set; }
    public bool? FundingHistoryDetailsComplete { get; private set; }

    public string? CaseStudyDetails { get; private set; }

    public bool? CaseStudyCandidate { get; private set; }

    public bool? EngagementConcernRecorded { get; private set; }

    public string? EngagementConcernDetails { get; private set; }

    public bool? EngagementConcernEscalationConfirmStepsTaken { get; private set; }

    public string? EngagementConcernEscalationPrimaryReason { get; private set; }

    public string? EngagementConcernEscalationDetails { get; private set; }

    public DateTime? EngagementConcernEscalationDateOfDecision { get; private set; }

    public DateTime? EngagementConcernRaisedDate { get; private set; }

    public IEnumerable<FundingHistory> FundingHistories => _fundingHistories.AsReadOnly();

    private readonly List<FundingHistory> _fundingHistories = new();

    public bool? IncludeContactDetails { get; private set; }

    public bool? AttachSchoolImprovementPlan { get; private set; }

    public bool? CopyInRegionalDirector { get; private set; }

    public bool? SendEmailToGrantTeam { get; private set; }

    public bool? InformationPowersInUse { get; private set; }
    public string? InformationPowersDetails { get; private set; }
    public DateTime? PowersUsedDate { get; private set; }
    public bool? IndicativeFundingBandCalculated { get; private set; }
    public bool? ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody { get; private set; }
    public string? IndicativeFundingBand { get; private set; }
    public DateTime? DateTemplatesAndIndicativeFundingBandSent { get; private set; }


    #endregion

    public static SupportProject Create(
        string schoolName,
        string schoolUrn,
        string localAuthority,
        string region,
        string? trustName = null,
        string? trustReferenceNumber = null,
        DateTime? deletedAt = null)
    {
        return new SupportProject()
        {
            SchoolName = schoolName,
            SchoolUrn = schoolUrn,
            LocalAuthority = localAuthority,
            Region = region,
            TrustName = trustName,
            TrustReferenceNumber = trustReferenceNumber,
            DeletedAt = deletedAt
        };
    }

    #region Methods
    public void SetDeliveryOfficer(string assignedDeliveryOfficerFullName, string assignedDeliveryOfficerEmailAddress)
    {
        AssignedDeliveryOfficerFullName = assignedDeliveryOfficerFullName;
        AssignedDeliveryOfficerEmailAddress = assignedDeliveryOfficerEmailAddress;
    }

    public void AddNote(SupportProjectNoteId id, string note, string author, DateTime date, SupportProjectId supportProjectId)
    {
        _notes.Add(new SupportProjectNote(id, note, author, date, supportProjectId));
    }

    public void AddContact(SupportProjectContactId id, string name, RolesIds roleId, string otherRoleName, string organisation, string email, string phone, string author, DateTime createOn, SupportProjectId supportProjectId)
    {
        _contacts.Add(new SupportProjectContact(id, name, roleId, otherRoleName, organisation, email, phone, author, createOn, supportProjectId));
    }

    public void EditContact(SupportProjectContactId id, string name, RolesIds roleId, string otherRoleName, string organisation, string email, string phone, string author, DateTime lastModifiedOn)
    {
        var contactToUpdate = _contacts.SingleOrDefault(x => x.Id == id);
        if (contactToUpdate != null)
        {
            contactToUpdate.SetContact(name, roleId, otherRoleName, organisation, email, phone, author, lastModifiedOn);
        }
    }

    public void EditSupportProjectNote(SupportProjectNoteId id, string note, string author, DateTime date)
    {
        var noteToUpdate = _notes.SingleOrDefault(x => x.Id == id);
        if (noteToUpdate != null)
        {
            noteToUpdate.SetNote(note, author, date);
        }
    }
    public void SetContactTheResponsibleBodyDetails(bool? discussTheBestApproach, bool? emailTheResponsibleBody, DateTime? responsibleBodyContactedDate)
    {
        DiscussTheBestApproach = discussTheBestApproach;
        EmailTheResponsibleBody = emailTheResponsibleBody;
        ContactedTheResponsibleBodyDate = responsibleBodyContactedDate;
    }

    public void SetAdviserConflictOfInterestDetails(bool? sendConflictOfInterestFormToProposedAdviserAndTheSchool, bool? receiveCompletedConflictOfInterestForm, bool? saveCompletedConflictOfinterestFormInSharePoint, DateTime? dateConflictsOfInterestWereChecked)
    {
        SendConflictOfInterestFormToProposedAdviserAndTheSchool = sendConflictOfInterestFormToProposedAdviserAndTheSchool;
        ReceiveCompletedConflictOfInterestForm = receiveCompletedConflictOfInterestForm;
        SaveCompletedConflictOfinterestFormInSharePoint = saveCompletedConflictOfinterestFormInSharePoint;
        DateConflictsOfInterestWereChecked = dateConflictsOfInterestWereChecked;
    }
    public void SetSchoolResponse(DateTime? schoolResponseDate, bool? hasAcknowledgedAndWillEngage, bool? hasSavedSchoolResponseinSharePoint)
    {
        SchoolResponseDate = schoolResponseDate;
        HasAcknowledgedAndWillEngage = hasAcknowledgedAndWillEngage;
        HasSavedSchoolResponseinSharePoint = hasSavedSchoolResponseinSharePoint;
    }

    public void SetAdviserDetails(string? adviserEmailAddress, DateTime? dateAdviserAllocated)
    {
        DateAdviserAllocated = dateAdviserAllocated;
        AdviserEmailAddress = adviserEmailAddress;
    }

    public void SetSendIntroductoryEmail(DateTime? introductoryEmailSentDate, bool? hasShareEmailTemplateWithAdviser, bool? remindAdviserToCopyRiseTeamWhenSentEmail)
    {
        IntroductoryEmailSentDate = introductoryEmailSentDate;
        HasShareEmailTemplateWithAdviser = hasShareEmailTemplateWithAdviser;
        RemindAdviserToCopyRiseTeamWhenSentEmail = remindAdviserToCopyRiseTeamWhenSentEmail;
    }

    public void SetAdviserVisitDate(DateTime? adviserVisitDate, bool? giveTheAdviserTheNoteOfVisitTemplate)
    {
        AdviserVisitDate = adviserVisitDate;
        GiveTheAdviserTheNoteOfVisitTemplate = giveTheAdviserTheNoteOfVisitTemplate;
    }

    public void SetCompleteAndSaveInitialDiagnosisTemplate(DateTime? savedAssessmentTemplateInSharePointDate, bool? hasTalkToAdviserAboutFindings, bool? hasCompleteAssessmentTemplate)
    {
        SavedAssessmentTemplateInSharePointDate = savedAssessmentTemplateInSharePointDate;
        HasTalkToAdviserAboutFindings = hasTalkToAdviserAboutFindings;
        HasCompleteAssessmentTemplate = hasCompleteAssessmentTemplate;
    }

    public void SetSchoolVisitDate(DateTime? schoolVisitDate)
    {
        SchoolVisitDate = schoolVisitDate;
    }

    public void SetChoosePreferredSupportOrganisation(DateTime? dateSupportOrganisationChosen,
        string? supportOrganisationName,
        string? supportOrganisationIdNumber,
        bool? assessmentToolTwoCompleted)
    {
        DateSupportOrganisationChosen = dateSupportOrganisationChosen;
        SupportOrganisationName = supportOrganisationName;
        SupportOrganisationIdNumber = supportOrganisationIdNumber;
        AssessmentToolTwoCompleted = assessmentToolTwoCompleted;
    }

    public void SetRecordInitialDiagnosisMatchingDecision(DateTime? regionalDirectorDecisionDate, string? initialDiagnosisMatchingDecision, string? initialDiagnosisMatchingDecisionNotes)
    {
        RegionalDirectorDecisionDate = regionalDirectorDecisionDate;
        InitialDiagnosisMatchingDecision = initialDiagnosisMatchingDecision;
        InitialDiagnosisMatchingDecisionNotes = initialDiagnosisMatchingDecisionNotes;
    }

    public void SetSupportingOrganisationContactDetails(DateTime? dateSupportingOrganisationContactDetailsAdded, string? supportingOrganisationContactName, string? supportingOrganisationContactEmailAddress)
    {
        DateSupportingOrganisationContactDetailsAdded = dateSupportingOrganisationContactDetailsAdded;
        SupportingOrganisationContactName = supportingOrganisationContactName;
        SupportingOrganisationContactEmailAddress = supportingOrganisationContactEmailAddress;
    }

    public void SetDueDiligenceOnPreferredSupportingOrganisationDetails(bool? checkOrganisationHasCapacityAndWillingToProvideSupport, bool? checkChoiceWithTrustRelationshipManagerOrLaLead, bool? discussChoiceWithSfso, bool? checkTheOrganisationHasAVendorAccount, DateTime? dateDueDiligenceCompleted)
    {
        CheckOrganisationHasCapacityAndWillingToProvideSupport = checkOrganisationHasCapacityAndWillingToProvideSupport;
        CheckChoiceWithTrustRelationshipManagerOrLaLead = checkChoiceWithTrustRelationshipManagerOrLaLead;
        DiscussChoiceWithSfso = discussChoiceWithSfso;
        CheckTheOrganisationHasAVendorAccount = checkTheOrganisationHasAVendorAccount;
        DateDueDiligenceCompleted = dateDueDiligenceCompleted;
    }

    public void SetRecordSupportingOrganisationAppointment(DateTime? regionalDirectorAppointmentDate, bool? hasConfirmedSupportingOrganisationAppointment, string? disapprovingSupportingOrganisationAppointmentNotes)
    {
        RegionalDirectorAppointmentDate = regionalDirectorAppointmentDate;
        HasConfirmedSupportingOrganisationAppointment = hasConfirmedSupportingOrganisationAppointment;
        DisapprovingSupportingOrganisationAppointmentNotes = disapprovingSupportingOrganisationAppointmentNotes;
    }

    public void SetRecordImprovementPlanDecision(DateTime? regionalDirectorImprovementPlanDecisionDate, bool? hasApprovedImprovementPlanDecision, string? disapprovingImprovementPlanDecisionNotes)
    {
        RegionalDirectorImprovementPlanDecisionDate = regionalDirectorImprovementPlanDecisionDate;
        HasApprovedImprovementPlanDecision = hasApprovedImprovementPlanDecision;
        DisapprovingImprovementPlanDecisionNotes = disapprovingImprovementPlanDecisionNotes;
    }

    public void SetIndicativeFundingBandAndImprovementPlanTemplateDetails(
        bool? indicativeFundingBandCalculated,
        string? indicativeFundingBand,
        bool? improvementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
        DateTime? dateTemplatesAndIndicativeFundingBandSent)
    {
        IndicativeFundingBandCalculated = indicativeFundingBandCalculated;
        ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody =
            improvementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody;
        IndicativeFundingBand = indicativeFundingBand;
        DateTemplatesAndIndicativeFundingBandSent = dateTemplatesAndIndicativeFundingBandSent;
    }

    public void SetSendAgreedImprovementPlanForApproval(bool? hasSavedImprovementPlanInSharePoint, bool? hasEmailedAgreedPlanToRegionalDirectorForApproval)
    {
        HasSavedImprovementPlanInSharePoint = hasSavedImprovementPlanInSharePoint;
        HasEmailedAgreedPlanToRegionalDirectorForApproval = hasEmailedAgreedPlanToRegionalDirectorForApproval;
    }

    public void SetRequestPlanningGrantOfferLetterDetails(
        DateTime? dateTeamContactedForRequestingPlanningGrantOfferLetter,
        bool? includeContactDetails,
        bool? confirmAmountOfFunding,
        bool? copyInRegionalDirector,
        bool? emailRiseGrantTeam)
    {
        DateTeamContactedForRequestingPlanningGrantOfferLetter = dateTeamContactedForRequestingPlanningGrantOfferLetter;
        IncludeContactDetailsRequestingPlanningGrantOfferEmail = includeContactDetails;
        ConfirmAmountOfPlanningGrantFundingRequested = confirmAmountOfFunding;
        CopyInRegionalDirectorRequestingPlanningGrantOfferEmail = copyInRegionalDirector;
        SendRequestingPlanningGrantOfferEmailToRiseGrantTeam = emailRiseGrantTeam;
    }

    public void SetReviewTheImprovementPlan(DateTime? improvementPlanReceivedDate,
        bool? reviewImprovementAndExpenditurePlan,
        bool? confirmFundingBand,
        string? fundingBand,
        bool? confirmPlanClearedByRiseGrantTeam)
    {
        ImprovementPlanReceivedDate = improvementPlanReceivedDate;
        ReviewImprovementAndExpenditurePlan = reviewImprovementAndExpenditurePlan;
        ConfirmFundingBand = confirmFundingBand;
        FundingBand = fundingBand;
        ConfirmPlanClearedByRiseGrantTeam = confirmPlanClearedByRiseGrantTeam;
    }

    public void SetRequestImprovementGrantOfferLetter(DateTime? dateTeamContactedForRequestingImprovementGrantOfferLetter,
            bool? includeContactDetails,
            bool? attachSchoolImprovementPlan,
            bool? copyInRegionalDirector,
            bool? sendEmailToGrantTeam)
    {
        DateTeamContactedForRequestingImprovementGrantOfferLetter = dateTeamContactedForRequestingImprovementGrantOfferLetter;
        IncludeContactDetails = includeContactDetails;
        AttachSchoolImprovementPlan = attachSchoolImprovementPlan;
        CopyInRegionalDirector = copyInRegionalDirector;
        SendEmailToGrantTeam = sendEmailToGrantTeam;
    }

    public void SetConfirmPlanningGrantOfferLetterDate(DateTime? dateTeamContactedForConfirmingPlanningGrantOfferLetter)
    {
        DateTeamContactedForConfirmingPlanningGrantOfferLetter = dateTeamContactedForConfirmingPlanningGrantOfferLetter;
    }

    public void SetConfirmImprovementGrantOfferLetterDetails(DateTime? dateImprovementGrantOfferLetterSent)
    {
        DateImprovementGrantOfferLetterSent = dateImprovementGrantOfferLetterSent;
    }

    public void SetEligibility(bool? schoolIsEligible, string? schoolIsNotEligibleNotes)
    {
        if (schoolIsEligible == true)
        {
            SupportProjectStatus = ValueObjects.SupportProjectStatus.EligibleForSupport;
        }

        if (schoolIsEligible == false)
        {
            SupportProjectStatus = ValueObjects.SupportProjectStatus.NotEligibleForSupport;
        }

        SchoolIsNotEligibleNotes = schoolIsNotEligibleNotes;
    }

    public void SetSoftDeleted(string deletedBy)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public void SetHasReceivedFundingInThelastTwoYears(bool? hasReceivedFundingInThelastTwoYearsCommand)
    {
        HasReceivedFundingInThelastTwoYears = hasReceivedFundingInThelastTwoYearsCommand;

        if (hasReceivedFundingInThelastTwoYearsCommand != true)
        {
            FundingHistoryDetailsComplete = null;
            _fundingHistories.Clear();
        }
    }

    public void AddFundingHistory(FundingHistoryId id,
            SupportProjectId supportProjectId,
            string fundingType,
            decimal fundingAmount,
            string financialYear,
            int fundingRounds,
            string comments)
    {
        _fundingHistories.Add(new FundingHistory(id, supportProjectId, fundingType, fundingAmount, financialYear, fundingRounds, comments));
    }

    public void EditFundingHistory(FundingHistoryId id,
            string fundingType,
            decimal fundingAmount,
            string financialYear,
            int fundingRounds,
            string comments)
    {
        var fundingHistory = _fundingHistories.SingleOrDefault(x => x.Id == id);
        if (fundingHistory != null)
        {
            fundingHistory.SetValues(fundingType,
             fundingAmount,
             financialYear,
             fundingRounds,
             comments);
        }
    }

    public void SetFundingHistoryComplete(bool? isComplete)
    {
        FundingHistoryDetailsComplete = isComplete;
    }

    public void SetCaseStudyDetails(bool? caseStudyCandidate, string? caseStudyDetails)
    {
        CaseStudyCandidate = caseStudyCandidate;
        CaseStudyDetails = caseStudyDetails;
    }

    public void SetEngagementConcernDetails(bool? engagementConcernRecorded, string? engagementConcernDetails, DateTime? engagementConcernRaisedDate)
    {
        EngagementConcernRecorded = engagementConcernRecorded;
        EngagementConcernDetails = engagementConcernDetails;
        EngagementConcernRaisedDate = engagementConcernRaisedDate;
    }

    public void SetEngagementConcernEscalation(bool? confirmStepsTaken, string? primaryReason,
        string? escalationDetails, DateTime? dateOfDecision)
    {
        EngagementConcernEscalationConfirmStepsTaken = confirmStepsTaken;
        EngagementConcernEscalationPrimaryReason = primaryReason;
        EngagementConcernEscalationDetails = escalationDetails;
        EngagementConcernEscalationDateOfDecision = dateOfDecision;
    }

    public void SetInformationPowersDetails(bool? informationPowersInUse, string? informationPowersDetails, DateTime? powersUsedDate)
    {
        InformationPowersInUse = informationPowersInUse;
        InformationPowersDetails = informationPowersDetails;
        PowersUsedDate = powersUsedDate;
    }

    #endregion
}
