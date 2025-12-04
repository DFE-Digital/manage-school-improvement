using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;

public class SupportProject : BaseAggregateRoot, IEntity<SupportProjectId>
{
    private SupportProject()
    {
    }

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

    public SupportProjectId Id { get; private set; } = null!;

    public string SchoolName { get; private set; } = string.Empty;

    public string SchoolUrn { get; private set; } = string.Empty;

    public string Region { get; private set; } = string.Empty;

    public string LocalAuthority { get; private set; } = string.Empty;

    public string? TrustName { get; private set; }

    public string? TrustReferenceNumber { get; private set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? AssignedDeliveryOfficerFullName { get; private set; }


    public string? AssignedDeliveryOfficerEmailAddress { get; private set; }

    public IEnumerable<SupportProjectNote> Notes => _notes.AsReadOnly();
    public IEnumerable<SupportProjectContact> Contacts => _contacts.AsReadOnly();

    private readonly List<SupportProjectNote> _notes = new();
    private readonly List<SupportProjectContact> _contacts = new();

    public bool? UseEnrolmentLetterTemplateToDraftEmail { get; private set; }
    public bool? AttachTargetedInterventionInformationSheet { get; private set; }
    public bool? AddRecipientsForFormalNotification { get; private set; }
    public bool? FormalNotificationSent { get; private set; }
    public DateTime? DateFormalNotificationSent { get; private set; }

    public bool? InitialContactResponsibleBody { get; private set; }

    public DateTime? InitialContactResponsibleBodyDate { get; private set; }

    public bool? ReviewAdvisersConflictOfInterestForm { get; private set; }

    public bool? SaveCompletedConflictOfinterestFormInSharePoint { get; }

    public DateTime? DateConflictOfInterestDeclarationChecked { get; private set; }

    public DateTime? DateConflictsOfInterestWereChecked { get; }
    public bool? ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint { get; private set; }

    public DateTime? ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate { get; private set; }

    public DateTime? DateAdviserAllocated { get; private set; }
    public string? AdviserEmailAddress { get; private set; }
    public string? AdviserFullName { get; private set; }
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
    public string? SupportingOrganisationType { get; private set; }
    public bool? AssessmentToolTwoCompleted { get; private set; }
    public string? SupportingOrganisationAddress { get; private set; }
    public DateTime? RegionalDirectorDecisionDate { get; private set; }

    public string? InitialDiagnosisMatchingDecision { get; private set; }
    public string? InitialDiagnosisMatchingDecisionNotes { get; private set; }

    public DateTime? DateSupportingOrganisationContactDetailsAdded { get; private set; }

    public string? SupportingOrganisationContactName { get; private set; }

    public string? SupportingOrganisationContactEmailAddress { get; private set; }
    
    public string? SupportingOrganisationContactPhone { get; private set; }

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

    public IEnumerable<FundingHistory> FundingHistories => _fundingHistories.AsReadOnly();

    private readonly List<FundingHistory> _fundingHistories = new();

    public bool? IncludeContactDetails { get; private set; }

    public bool? AttachSchoolImprovementPlan { get; private set; }

    public bool? CopyInRegionalDirector { get; private set; }

    public bool? SendEmailToGrantTeam { get; private set; }

    public bool? IndicativeFundingBandCalculated { get; private set; }

    public bool?
        ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody
    {
        get;
        private set;
    }

    public string? IndicativeFundingBand { get; private set; }
    public DateTime? DateTemplatesAndIndicativeFundingBandSent { get; private set; }

    public IEnumerable<ImprovementPlan> ImprovementPlans => _improvementPlans.AsReadOnly();

    private readonly List<ImprovementPlan> _improvementPlans = new();

    public IEnumerable<EngagementConcern> EngagementConcerns => _engagementConcerns.AsReadOnly();

    private readonly List<EngagementConcern> _engagementConcerns = new();
    public string? Cohort { get; }

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

    public void AddNote(SupportProjectNoteId id, string note, string author, DateTime date,
        SupportProjectId supportProjectId)
    {
        _notes.Add(new SupportProjectNote(id, note, author, date, supportProjectId));
    }

    public void AddContact(SupportProjectContactId id, SupportProjectContactDetails details, string author,
        DateTime createOn, SupportProjectId supportProjectId)
    {
        _contacts.Add(new SupportProjectContact(id, details, author, createOn, supportProjectId));
    }

    public void EditContact(SupportProjectContactId id, SupportProjectContactDetails details, string author,
        DateTime lastModifiedOn)
    {
        var contactToUpdate = _contacts.SingleOrDefault(x => x.Id == id);
        if (contactToUpdate != null)
        {
            contactToUpdate.SetContact(details, author, lastModifiedOn);
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

    public void SetInitialContactTheResponsibleBodyDetails(bool? initialContactResponsibleBody,
        DateTime? responsibleBodyInitialContactDate)
    {
        InitialContactResponsibleBody = initialContactResponsibleBody;
        InitialContactResponsibleBodyDate = responsibleBodyInitialContactDate;
    }

    public void SetSendTheFormalNotification(bool? useEnrolmentLetterTemplateToDraftEmail,
        bool? attachTargetedInterventionInformationSheet, bool? addRecipientsForFormalNotification,
        bool? formalNotificationSent, DateTime? dateFormalNotificationSent)
    {
        UseEnrolmentLetterTemplateToDraftEmail = useEnrolmentLetterTemplateToDraftEmail;
        AttachTargetedInterventionInformationSheet = attachTargetedInterventionInformationSheet;
        AddRecipientsForFormalNotification = addRecipientsForFormalNotification;
        FormalNotificationSent = formalNotificationSent;
        DateFormalNotificationSent = dateFormalNotificationSent;
    }

    public void SetAdviserConflictOfInterestDetails(
        bool? reviewAdvisersConflictOfInterestForm,
        DateTime? dateConflictOfInterestDeclarationChecked)
    {
        ReviewAdvisersConflictOfInterestForm = reviewAdvisersConflictOfInterestForm;
        DateConflictOfInterestDeclarationChecked = dateConflictOfInterestDeclarationChecked;
    }

    public void SetResponsibleBodyResponseToTheConflictOfInterestRequest(
        DateTime? responsibleBodyResponseToTheConflictOfInterestRequestReceivedDate,
        bool? responsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint)
    {
        ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint =
            responsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint;
        ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate =
            responsibleBodyResponseToTheConflictOfInterestRequestReceivedDate;
    }

    public void SetAdviserDetails(string? adviserEmailAddress, DateTime? dateAdviserAllocated, string? adviserFullName)
    {
        DateAdviserAllocated = dateAdviserAllocated;
        AdviserEmailAddress = adviserEmailAddress;
        AdviserFullName = adviserFullName;
    }

    public void SetSendIntroductoryEmail(DateTime? introductoryEmailSentDate, bool? hasShareEmailTemplateWithAdviser,
        bool? remindAdviserToCopyRiseTeamWhenSentEmail)
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

    public void SetCompleteAndSaveInitialDiagnosisTemplate(DateTime? savedAssessmentTemplateInSharePointDate,
        bool? hasTalkToAdviserAboutFindings, bool? hasCompleteAssessmentTemplate)
    {
        SavedAssessmentTemplateInSharePointDate = savedAssessmentTemplateInSharePointDate;
        HasTalkToAdviserAboutFindings = hasTalkToAdviserAboutFindings;
        HasCompleteAssessmentTemplate = hasCompleteAssessmentTemplate;
    }

    public void SetSchoolVisitDate(DateTime? schoolVisitDate)
    {
        SchoolVisitDate = schoolVisitDate;
    }

    public void SetChoosePreferredSupportOrganisation(SupportingOrganisationDetails details)
    {
        DateSupportOrganisationChosen = details.DateSupportOrganisationChosen;
        SupportOrganisationName = details.SupportOrganisationName;
        SupportOrganisationIdNumber = details.SupportOrganisationIdNumber;
        SupportingOrganisationType = details.SupportOrganisationType;
        AssessmentToolTwoCompleted = details.AssessmentToolTwoCompleted;
        SupportingOrganisationAddress = details.SupportOrganisationAddress;
        SupportingOrganisationContactName = details.SupportOrganisationContactName;
        SupportingOrganisationContactEmailAddress = details.SupportOrganisationContactEmailAddress;
        SupportingOrganisationContactPhone = details.SupportOrganisationContactPhone;
        DateSupportingOrganisationContactDetailsAdded = details.DateSupportingOrganisationContactDetailsAdded;
    }

    public void SetRecordInitialDiagnosisMatchingDecision(DateTime? regionalDirectorDecisionDate,
        string? initialDiagnosisMatchingDecision, string? initialDiagnosisMatchingDecisionNotes)
    {
        RegionalDirectorDecisionDate = regionalDirectorDecisionDate;
        InitialDiagnosisMatchingDecision = initialDiagnosisMatchingDecision;
        InitialDiagnosisMatchingDecisionNotes = initialDiagnosisMatchingDecisionNotes;
    }

    public void SetSupportingOrganisationContactDetails(DateTime? dateSupportingOrganisationContactDetailsAdded,
        string? supportingOrganisationContactName, string? supportingOrganisationContactEmailAddress)
    {
        DateSupportingOrganisationContactDetailsAdded = dateSupportingOrganisationContactDetailsAdded;
        SupportingOrganisationContactName = supportingOrganisationContactName;
        SupportingOrganisationContactEmailAddress = supportingOrganisationContactEmailAddress;
    }

    public void SetDueDiligenceOnPreferredSupportingOrganisationDetails(
        bool? checkOrganisationHasCapacityAndWillingToProvideSupport,
        bool? checkChoiceWithTrustRelationshipManagerOrLaLead, bool? discussChoiceWithSfso,
        bool? checkTheOrganisationHasAVendorAccount, DateTime? dateDueDiligenceCompleted)
    {
        CheckOrganisationHasCapacityAndWillingToProvideSupport = checkOrganisationHasCapacityAndWillingToProvideSupport;
        CheckChoiceWithTrustRelationshipManagerOrLaLead = checkChoiceWithTrustRelationshipManagerOrLaLead;
        DiscussChoiceWithSfso = discussChoiceWithSfso;
        CheckTheOrganisationHasAVendorAccount = checkTheOrganisationHasAVendorAccount;
        DateDueDiligenceCompleted = dateDueDiligenceCompleted;
    }

    public void SetRecordSupportingOrganisationAppointment(DateTime? regionalDirectorAppointmentDate,
        bool? hasConfirmedSupportingOrganisationAppointment, string? disapprovingSupportingOrganisationAppointmentNotes)
    {
        RegionalDirectorAppointmentDate = regionalDirectorAppointmentDate;
        HasConfirmedSupportingOrganisationAppointment = hasConfirmedSupportingOrganisationAppointment;
        DisapprovingSupportingOrganisationAppointmentNotes = disapprovingSupportingOrganisationAppointmentNotes;
    }

    public void SetRecordImprovementPlanDecision(DateTime? regionalDirectorImprovementPlanDecisionDate,
        bool? hasApprovedImprovementPlanDecision, string? disapprovingImprovementPlanDecisionNotes)
    {
        RegionalDirectorImprovementPlanDecisionDate = regionalDirectorImprovementPlanDecisionDate;
        HasApprovedImprovementPlanDecision = hasApprovedImprovementPlanDecision;
        DisapprovingImprovementPlanDecisionNotes = disapprovingImprovementPlanDecisionNotes;
    }

    public void SetIndicativeFundingBandAndImprovementPlanTemplateDetails(
        bool? indicativeFundingBandCalculated,
        string? indicativeFundingBand,
        bool?
            improvementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
        DateTime? dateTemplatesAndIndicativeFundingBandSent)
    {
        IndicativeFundingBandCalculated = indicativeFundingBandCalculated;
        ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody =
            improvementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody;
        IndicativeFundingBand = indicativeFundingBand;
        DateTemplatesAndIndicativeFundingBandSent = dateTemplatesAndIndicativeFundingBandSent;
    }

    public void SetSendAgreedImprovementPlanForApproval(bool? hasSavedImprovementPlanInSharePoint,
        bool? hasEmailedAgreedPlanToRegionalDirectorForApproval)
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

    public void SetRequestImprovementGrantOfferLetter(
        DateTime? dateTeamContactedForRequestingImprovementGrantOfferLetter,
        bool? includeContactDetails,
        bool? attachSchoolImprovementPlan,
        bool? copyInRegionalDirector,
        bool? sendEmailToGrantTeam)
    {
        DateTeamContactedForRequestingImprovementGrantOfferLetter =
            dateTeamContactedForRequestingImprovementGrantOfferLetter;
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
        _fundingHistories.Add(new FundingHistory(id, supportProjectId, fundingType, fundingAmount, financialYear,
            fundingRounds, comments));
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

    public void SetEngagementConcernEscalation(EngagementConcernId engagementConcernId, bool? confirmStepsTaken,
        string? primaryReason,
        string? escalationDetails, DateTime? dateOfDecision, string? warningNotice)
    {
        var engagementConcern = _engagementConcerns.SingleOrDefault(x => x.Id == engagementConcernId);
        if (engagementConcern != null)
        {
            engagementConcern.SetEngagementConcernEscalationDetails(confirmStepsTaken, primaryReason, escalationDetails,
                dateOfDecision, warningNotice);
        }
    }

    public void AddEngagementConcern(EngagementConcernId engagementConcernId, SupportProjectId supportProjectId,
        EngagementConcernDetails? engagementConcernDetails)
    {
        if (engagementConcernDetails != null)
            _engagementConcerns.Add(new EngagementConcern(engagementConcernId, supportProjectId,
                engagementConcernDetails
            ));
    }

    public void EditEngagementConcern(EngagementConcernId engagementConcernId,
        string? engagementConcernDetails, string? engagementConcernSummary, DateTime? engagementConcernRaisedDate)
    {
        var engagementConcern = _engagementConcerns.SingleOrDefault(x => x.Id == engagementConcernId);
        if (engagementConcern != null)
        {
            engagementConcern.SetEngagementConcernDetails(engagementConcernDetails, engagementConcernSummary,
                engagementConcernRaisedDate);
        }
    }

    public void SetInformationPowersDetails(EngagementConcernId engagementConcernId,
        bool? informationPowersInUse, string? informationPowersDetails,
        DateTime? powersUsedDate)
    {
        var engagementConcern = _engagementConcerns.SingleOrDefault(x => x.Id == engagementConcernId);
        if (engagementConcern == null)
        {
            throw new InvalidOperationException($"Engagement concern with id {engagementConcernId} not found.");
        }

        engagementConcern.SetInformationPowersDetails(informationPowersInUse, informationPowersDetails,
            powersUsedDate);
    }

    public void AddImprovementPlan(ImprovementPlanId improvementPlanId, SupportProjectId supportProjectId)
    {
        if (!_improvementPlans.Any(x => x.Id == improvementPlanId))
        {
            _improvementPlans.Add(new ImprovementPlan(improvementPlanId, supportProjectId));
        }
    }

    public void AddImprovementPlanObjective(ImprovementPlanObjectiveId improvementPlanObjectiveId,
        ImprovementPlanId improvementPlanId, string areaOfImprovement, string details)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        var order = improvementPlan.ImprovementPlanObjectives
            .Where(x => x.AreaOfImprovement == areaOfImprovement)
            .Select(x => x.Order)
            .DefaultIfEmpty(0)
            .Max() + 1;

        improvementPlan.AddObjective(
            improvementPlanObjectiveId,
            improvementPlanId,
            areaOfImprovement,
            details,
            order
        );
    }

    public void SetImprovementPlanObjectivesComplete(ImprovementPlanId improvementPlanId,
        bool objectivesSectionComplete)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        improvementPlan.SetObjectivesComplete(objectivesSectionComplete);
    }

    public void SetImprovementPlanObjectiveDetails(ImprovementPlanObjectiveId improvementPlanObjectiveId,
        ImprovementPlanId improvementPlanId, string details)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        improvementPlan.SetObjectiveDetails(
            improvementPlanObjectiveId,
            details
        );
    }

    public void AddImprovementPlanReview(
        ImprovementPlanReviewId improvementPlanReviewId,
        ImprovementPlanId improvementPlanId,
        string reviewer,
        DateTime reviewDate)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        improvementPlan.AddReview(improvementPlanReviewId, reviewer, reviewDate);
    }

    public void AddImprovementPlanObjectiveProgress(
        ImprovementPlanObjectiveProgressId improvementPlanObjectiveProgressId,
        ImprovementPlanId improvementPlanId,
        ImprovementPlanReviewId improvementPlanReviewId,
        ImprovementPlanObjectiveId improvementPlanObjectiveId,
        string progressStatus,
        string progressDetails)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        improvementPlan.AddImprovementPlanObjectiveProgress(improvementPlanReviewId,
            improvementPlanObjectiveProgressId, improvementPlanObjectiveId, progressStatus, progressDetails);
    }

    public void SetImprovementPlanObjectiveProgressDetails(ImprovementPlanId improvementPlanId,
        ImprovementPlanReviewId improvementPlanReviewId,
        ImprovementPlanObjectiveProgressId improvementPlanObjectiveProgressId, string progressStatus,
        string progressDetails)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        improvementPlan.SetImprovementPlanObjectiveProgressDetails(improvementPlanReviewId,
            improvementPlanObjectiveProgressId, progressStatus, progressDetails);
    }

    public void SetImprovementPlanReviewNextReviewDate(ImprovementPlanId improvementPlanId,
        ImprovementPlanReviewId improvementPlanReviewId, DateTime? nextReviewDate)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        improvementPlan.SetNextReviewDate(improvementPlanReviewId, nextReviewDate);
    }

    public void SetImprovementPlanReviewDetails(ImprovementPlanId improvementPlanId,
        ImprovementPlanReviewId improvementPlanReviewId, string reviewer, DateTime reviewDate)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        improvementPlan.SetImprovementPlanReviewDetails(improvementPlanReviewId, reviewer, reviewDate);
    }

    public void SetOverallProgress(ImprovementPlanId improvementPlanId,
        ImprovementPlanReviewId improvementPlanReviewId,
        string howIsTheSchoolProgressingOverall,
        string overallProgressDetails)
    {
        var improvementPlan = _improvementPlans.SingleOrDefault(x => x.Id == improvementPlanId);

        if (improvementPlan == null)
        {
            throw new InvalidOperationException($"Improvement plan with id {improvementPlanId} not found.");
        }

        improvementPlan.SetOverallProgress(improvementPlanReviewId,
            howIsTheSchoolProgressingOverall,
            overallProgressDetails);
    }

    public void SetInterimExecutiveBoardCreated(EngagementConcernId engagementConcernId,
        bool? interimExecutiveBoardCreated,
        string? interimExecutiveBoardCreatedDetails, DateTime? interimExecutiveBoardCreatedDate)
    {
        var engagementConcern = _engagementConcerns.SingleOrDefault(x => x.Id == engagementConcernId);

        if (engagementConcern == null)
        {
            throw new InvalidOperationException($"Engagement concern with id {engagementConcernId} not found.");
        }

        engagementConcern.SetInterimExecutiveBoardCreated(interimExecutiveBoardCreated,
            interimExecutiveBoardCreatedDetails, interimExecutiveBoardCreatedDate);
    }

    public void SetEngagementConcernResolvedDetails(EngagementConcernId engagementConcernId,
        bool? engagementConcernResolved, string? engagementConcernResolvedDetails,
        DateTime? engagementConcernResolvedDate)
    {
        var engagementConcern = _engagementConcerns.SingleOrDefault(x => x.Id == engagementConcernId);
        if (engagementConcern != null)
        {
            engagementConcern.SetEngagementConcernResolvedDetails(engagementConcernResolved,
                engagementConcernResolvedDetails, engagementConcernResolvedDate);
        }
    }

    #endregion
}