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
        string assignedAdviserFullName,
        string assignedAdviserEmailAddress)
    {
        Id = id;
        SchoolName = schoolName;
        SchoolUrn = schoolUrn;
        LocalAuthority = localAuthority;
        Region = region;
        AssignedAdviserFullName = assignedAdviserFullName;
        AssignedAdviserEmailAddress = assignedAdviserEmailAddress;
    }
    #region Properties
    public SupportProjectId Id { get; private set; }

    public string SchoolName { get; private set; }

    public string SchoolUrn { get; private set; }

    public string Region { get; private set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public string LocalAuthority { get; private set; }

    public DateTime? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? AssignedAdviserFullName { get; private set; }

    public string? AssignedAdviserEmailAddress { get; private set; }
    
    public string? AssignedDeliveryOfficerFullName { get; private set; }
    

    public string? AssignedDeliveryOfficerEmailAddress { get; private set; }

    public IEnumerable<SupportProjectNote> Notes => _notes.AsReadOnly();

    private readonly List<SupportProjectNote> _notes = new();

    public bool? FindSchoolEmailAddress { get; private set; }

    public bool? UseTheNotificationLetterToCreateEmail { get; private set; }

    public bool? AttachRiseInfoToEmail { get; private set; }

    public DateTime? ContactedTheSchoolDate { get; private set; }

    public bool? SendConflictOfInterestFormToProposedAdviserAndTheSchool { get; private set; }

    public bool? ReceiveCompletedConflictOfInterestForm { get; private set; }

    public bool? SaveCompletedConflictOfinterestFormInSharePoint { get; private set; }

    public DateTime? DateConflictsOfInterestWereChecked { get; private set; }

    public DateTime? SchoolResponseDate { get; private set; }

    public bool? HasAcceeptedTargetedSupport { get; private set; }

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

    public bool? AskTheAdviserToSendYouTheirNotes { get; private set; }

    public DateTime? DateNoteOfVisitSavedInSharePoint { get; private set; }

    public DateTime? SchoolVisitDate { get; private set; }

    public DateTime? DateSupportOrganisationChosen { get; private set; }

    public string? SupportOrganisationName { get; private set; }

    public string? SupportOrganisationIdNumber { get; private set; }

    public DateTime? RegionalDirectorDecisionDate { get; private set; }

    public bool? HasSchoolMatchedWithSupportingOrganisation { get; private set; }

    public string? NotMatchingSchoolWithSupportingOrgNotes { get; private set; }

    public DateTime? DateSupportingOrganisationContactDetailsAdded { get; private set; }

    public string? SupportingOrganisationContactName { get; private set; }

    public string? SupportingOrganisationContactEmailAddress { get; private set; }


    public bool? CheckOrganisationHasCapacityAndWillingToProvideSupport { get; set; }

    public bool? CheckChoiceWithTrustRelationshipManagerOrLaLead { get; set; }

    public bool? DiscussChoiceWithSfso { get; set; }
    public bool? CheckFinancialConcernsAtSupportingOrganisation { get; set; }
    public bool? CheckTheOrganisationHasAVendorAccount { get; set; }
    public DateTime? DateDueDiligenceCompleted { get; set; }

    public DateTime? RegionalDirectorAppointmentDate { get; private set; }
    public bool? HasConfirmedSupportingOrgnaisationAppointment { get; private set; }
    public string? DisapprovingSupportingOrgnaisationAppointmentNotes { get; private set; }

    public bool? SendTheTemplateToTheSupportingOrganisation { get; private set; }
    public bool? SendTheTemplateToTheSchoolsResponsibleBody { get; private set; }
    public DateTime? DateTemplatesSent { get; private set; }

    public DateTime? RegionalDirectorImprovementPlanDecisionDate { get; private set; }
    public bool? HasApprovedImprovementPlanDecision { get; private set; }
    public string? DisapprovingImprovementPlanDecisionNotes { get; private set; }
    public bool? HasSavedImprovementPlanInSharePoint { get; private set; }
    public bool? HasEmailedAgreedPlanToRegionalDirectorForApproval { get; private set; }

    public DateTime? DateTeamContactedForRequestingPlanningGrantOfferLetter { get; private set; }

    public DateTime? ImprovementPlanReceivedDate { get; private set; }

    public bool? ReviewImprovementPlanWithTeam { get; private set; }

    public DateTime? DateTeamContactedForRequestingImprovementGrantOfferLetter { get; private set; }

    public DateTime? DateTeamContactedForConfirmingPlanningGrantOfferLetter { get; private set; }

    public DateTime? DateImprovementGrantOfferLetterSent { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    #endregion

    public static SupportProject Create(
        string schoolName,
        string schoolUrn,
        string localAuthority,
        string region,
        DateTime? deletedAt = null)
    {

        return new SupportProject()
        {
            SchoolName = schoolName,
            SchoolUrn = schoolUrn,
            LocalAuthority = localAuthority,
            Region = region,
            DeletedAt = deletedAt
        };
    }

    #region Methods
    public void SetAdviser(string assignedAdviserFullName, string assignedAdviserEmailAddress)
    {
        AssignedAdviserFullName = assignedAdviserFullName;
        AssignedAdviserEmailAddress = assignedAdviserEmailAddress;
    }
    
    public void SetDeliveryOfficer(string assignedDeliveryOfficerFullName, string assignedDeliveryOfficerEmailAddress)
    {
        AssignedDeliveryOfficerFullName = assignedDeliveryOfficerFullName;
        AssignedDeliveryOfficerEmailAddress = assignedDeliveryOfficerEmailAddress;
    }

    public void AddNote(SupportProjectNoteId id, string note, string author, DateTime date, SupportProjectId supportProjectId)
    {
        _notes.Add(new SupportProjectNote(id, note, author, date, supportProjectId));
    }

    public void EditSupportProjectNote(SupportProjectNoteId id, string note, string author, DateTime date)
    {
        var noteToUpdate = _notes.SingleOrDefault(x => x.Id == id);
        if (noteToUpdate != null)
        {
            noteToUpdate.SetNote(note, author, date);
        }
    }
    public void SetContactTheSchoolDetails(bool? findSchoolEmailAddress, bool? useTheNotificationLetterToCreateEmail, bool? attachRiseInfoToEmail, DateTime? schoolContactedDate)
    {
        FindSchoolEmailAddress = findSchoolEmailAddress;
        UseTheNotificationLetterToCreateEmail = useTheNotificationLetterToCreateEmail;
        AttachRiseInfoToEmail = attachRiseInfoToEmail;
        ContactedTheSchoolDate = schoolContactedDate;
    }

    public void SetAdviserConflictOfInterestDetails(bool? sendConflictOfInterestFormToProposedAdviserAndTheSchool, bool? receiveCompletedConflictOfInterestForm, bool? saveCompletedConflictOfinterestFormInSharePoint, DateTime? dateConflictsOfInterestWereChecked)
    {
        SendConflictOfInterestFormToProposedAdviserAndTheSchool = sendConflictOfInterestFormToProposedAdviserAndTheSchool;
        ReceiveCompletedConflictOfInterestForm = receiveCompletedConflictOfInterestForm;
        SaveCompletedConflictOfinterestFormInSharePoint = saveCompletedConflictOfinterestFormInSharePoint;
        DateConflictsOfInterestWereChecked = dateConflictsOfInterestWereChecked;
    }
    public void SetSchoolResponse(DateTime? schoolResponseDate, bool? hasAcceeptedTargetedSupport, bool? hasSavedSchoolResponseinSharePoint)
    {
        SchoolResponseDate = schoolResponseDate;
        HasAcceeptedTargetedSupport = hasAcceeptedTargetedSupport;
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

    public void SetAdviserVisitDate(DateTime? adviserVisitDate)
    {
        AdviserVisitDate = adviserVisitDate;
    }

    public void SetCompleteAndSaveAssessmentTemplate(DateTime? savedAssessmentTemplateInSharePointDate, bool? hasTalkToAdviserAboutFindings, bool? hasCompleteAssessmentTemplate)
    {
        SavedAssessmentTemplateInSharePointDate = savedAssessmentTemplateInSharePointDate;
        HasTalkToAdviserAboutFindings = hasTalkToAdviserAboutFindings;
        HasCompleteAssessmentTemplate = hasCompleteAssessmentTemplate;
    }

    public void SetSchoolVisitDate(DateTime? schoolVisitDate)
    {
        SchoolVisitDate = schoolVisitDate;
    }

    public void SetNoteOfVisitDetails(bool? giveTheAdviserTheNoteOfVisitTemplate,
                                      bool? askTheAdviserToSendYouTheirNotes,
                                      DateTime? dateNoteOfVisitSavedInSharePoint)
    {
        GiveTheAdviserTheNoteOfVisitTemplate = giveTheAdviserTheNoteOfVisitTemplate;
        AskTheAdviserToSendYouTheirNotes = askTheAdviserToSendYouTheirNotes;
        DateNoteOfVisitSavedInSharePoint = dateNoteOfVisitSavedInSharePoint;
    }

    public void SetChoosePreferredSupportOrganisation(DateTime? dateSupportOrganisationChosen,
        string? supportOrganisationName,
        string? supportOrganisationIdNumber)
    {
        DateSupportOrganisationChosen = dateSupportOrganisationChosen;
        SupportOrganisationName = supportOrganisationName;
        SupportOrganisationIdNumber = supportOrganisationIdNumber;
    }

    public void SetRecordMatchingDecision(DateTime? regionalDirectorDecisionDate, bool? hasSchoolMatchedWithSupportingOrganisation, string? notMatchingSchoolWithSupportingOrgNotes)
    {
        RegionalDirectorDecisionDate = regionalDirectorDecisionDate;
        HasSchoolMatchedWithSupportingOrganisation = hasSchoolMatchedWithSupportingOrganisation;
        NotMatchingSchoolWithSupportingOrgNotes = notMatchingSchoolWithSupportingOrgNotes;
    }

    public void SetSupportingOrganisationContactDetails(DateTime? dateSupportingOrganisationContactDetailsAdded, string? supportingOrganisationContactName, string? supportingOrganisationContactEmailAddress)
    {
        DateSupportingOrganisationContactDetailsAdded = dateSupportingOrganisationContactDetailsAdded;
        SupportingOrganisationContactName = supportingOrganisationContactName;
        SupportingOrganisationContactEmailAddress = supportingOrganisationContactEmailAddress;
    }

    public void SetDueDiligenceOnPreferredSupportingOrganisationDetails(bool? checkOrganisationHasCapacityAndWillingToProvideSupport, bool? checkChoiceWithTrustRelationshipManagerOrLaLead, bool? discussChoiceWithSfso, bool? checkFinancialConcernsAtSupportingOrganisation, bool? checkTheOrganisationHasAVendorAccount, DateTime? dateDueDiligenceCompleted)
    {
        CheckOrganisationHasCapacityAndWillingToProvideSupport = checkOrganisationHasCapacityAndWillingToProvideSupport;
        CheckChoiceWithTrustRelationshipManagerOrLaLead = checkChoiceWithTrustRelationshipManagerOrLaLead;
        DiscussChoiceWithSfso = discussChoiceWithSfso;
        CheckFinancialConcernsAtSupportingOrganisation = checkFinancialConcernsAtSupportingOrganisation;
        CheckTheOrganisationHasAVendorAccount = checkTheOrganisationHasAVendorAccount;
        DateDueDiligenceCompleted = dateDueDiligenceCompleted;
    }

    public void SetRecordSupportingOrganisationAppointment(DateTime? regionalDirectorAppointmentDate, bool? hasConfirmedSupportingOrgnaisationAppointment, string? disapprovingSupportingOrgnaisationAppointmentNotes)
    {
        RegionalDirectorAppointmentDate = regionalDirectorAppointmentDate;
        HasConfirmedSupportingOrgnaisationAppointment = hasConfirmedSupportingOrgnaisationAppointment;
        DisapprovingSupportingOrgnaisationAppointmentNotes = disapprovingSupportingOrgnaisationAppointmentNotes;
    }

    public void SetRecordImprovementPlanDecision(DateTime? regionalDirectorImprovementPlanDecisionDate, bool? hasApprovedImprovementPlanDecision, string? disapprovingImprovementPlanDecisionNotes)
    {
        RegionalDirectorImprovementPlanDecisionDate = regionalDirectorImprovementPlanDecisionDate;
        HasApprovedImprovementPlanDecision = hasApprovedImprovementPlanDecision;
        DisapprovingImprovementPlanDecisionNotes = disapprovingImprovementPlanDecisionNotes;
    }

    public void SetImprovementPlanTemplateDetails(bool? sendTheTemplateToTheSupportingOrganisation, bool? sendTheTemplateToTheSchoolsResponsibleBody, DateTime? dateTemplatesSent)
    {
        SendTheTemplateToTheSupportingOrganisation = sendTheTemplateToTheSupportingOrganisation;
        SendTheTemplateToTheSchoolsResponsibleBody = sendTheTemplateToTheSchoolsResponsibleBody;
        DateTemplatesSent = dateTemplatesSent;
    }

    public void SetSendAgreedImprovementPlanForApproval(bool? hasSavedImprovementPlanInSharePoint, bool? hasEmailedAgreedPlanToRegionalDirectorForApproval)
    {
        HasSavedImprovementPlanInSharePoint = hasSavedImprovementPlanInSharePoint;
        HasEmailedAgreedPlanToRegionalDirectorForApproval = hasEmailedAgreedPlanToRegionalDirectorForApproval;
    }

    public void SetRequestPlanningGrantOfferLetterDetails(DateTime? dateTeamContactedForRequestingPlanningGrantOfferLetter)
    {
        DateTeamContactedForRequestingPlanningGrantOfferLetter = dateTeamContactedForRequestingPlanningGrantOfferLetter;
    }

    public void SetReviewTheImprovementPlan(DateTime? improvementPlanReceivedDate, bool? reviewImprovementPlanWithTeam)
    {
        ImprovementPlanReceivedDate = improvementPlanReceivedDate;
        ReviewImprovementPlanWithTeam = reviewImprovementPlanWithTeam;
    }

    public void SetRequestImprovementGrantOfferLetter(DateTime? dateTeamContactedForRequestingImprovementGrantOfferLetter)
    {
        DateTeamContactedForRequestingImprovementGrantOfferLetter = dateTeamContactedForRequestingImprovementGrantOfferLetter;
    }

    public void SetConfirmPlanningGrantOfferLetterDate(DateTime? dateTeamContactedForConfirmingPlanningGrantOfferLetter)
    {
        DateTeamContactedForConfirmingPlanningGrantOfferLetter = dateTeamContactedForConfirmingPlanningGrantOfferLetter;
    }

    public void SetConfirmImprovementGrantOfferLetterDetails(DateTime? dateImprovementGrantOfferLetterSent)
    {
        DateImprovementGrantOfferLetterSent = dateImprovementGrantOfferLetterSent;
    }

    public void SetSoftDeleted(string deletedBy)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
    #endregion
}
