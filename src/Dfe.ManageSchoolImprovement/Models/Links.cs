namespace Dfe.ManageSchoolImprovement.Frontend.Models;

public static class Links
{
    private static readonly List<LinkItem> _links = [];

    private static bool _isApplicationDocumentsEnabled;
    public static bool IsApplicationDocumentsEnabled => _isApplicationDocumentsEnabled;

    public static LinkItem AddLinkItem(string page, string backText = "Back")
    {
        LinkItem item = new() { Page = page, BackText = backText };
        _links.Add(item);
        return item;
    }

    public static void InializeProjectDocumentsEnabled(bool isApplicationDocumentsEnabled)
    {
        _isApplicationDocumentsEnabled = isApplicationDocumentsEnabled;
    }
    public static LinkItem ByPage(string page)
    {
        return _links.Find(x => string.Equals(page, x.Page, StringComparison.InvariantCultureIgnoreCase));
    }


    public static class AddSchool
    {
        public static readonly LinkItem SelectSchool = AddLinkItem(backText: "Back", page: "/AddSchool/SelectSchool");
        public static readonly LinkItem Summary = AddLinkItem(page: "/AddSchool/Summary");
    }

    public static class SchoolList
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/SchoolList/Index");
    }

    public static class Contacts
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/Contacts/Index");
        public static readonly LinkItem AddContact = AddLinkItem(backText: "Back", page: "/Contacts/AddContact");
        public static readonly LinkItem AddContactDetail = AddLinkItem(backText: "Back", page: "/Contacts/AddContactDetail");
        public static readonly LinkItem EditContact = AddLinkItem(backText: "Back", page: "/Contacts/EditContact");
        public static readonly LinkItem EditContactDetail = AddLinkItem(backText: "Back", page: "/Contacts/EditContactDetail");
    }

    public static class AboutTheSchool
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/AboutTheSchool/Index");
    }
    public static class OfstedReports
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/OfstedReports/Index");
    }
    
    public static class TaskList
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/TaskList/Index");
        public static readonly LinkItem CheckEligibility = AddLinkItem(backText: "Back", page: "/TaskList/ConfirmEligibility/Index");
        public static readonly LinkItem ContactTheResponsibleBody = AddLinkItem(backText: "Back", page: "/TaskList/ContactTheResponsibleBody/index");
        public static readonly LinkItem RecordTheResponsibleBodyResponse = AddLinkItem(backText: "Back", page: "/TaskList/RecordTheResponsibleBodyResponse/Index");
        public static readonly LinkItem CheckPotentialAdviserConflictsOfInterest = AddLinkItem(backText: "Back", page: "/TaskList/AdviserConflictOfInterest/AdviserConflictOfInterest");
        public static readonly LinkItem AllocateAdviser = AddLinkItem(backText: "Back", page: "/TaskList/AllocateAdviser/AllocateAdviser");
        public static readonly LinkItem SendIntroductoryEmail = AddLinkItem(backText: "Back", page: "/TaskList/SendIntroductoryEmail/Index");
        public static readonly LinkItem ArrangeAdvisersFirstFaceToFaceVisit = AddLinkItem(backText: "Back", page: "/TaskList/ArrangeAdvisersFirstFaceToFaceVisit/ArrangeAdvisersFirstFaceToFaceVisit");
        public static readonly LinkItem CompleteAndSaveInitialDiagnosisAssessment = AddLinkItem(backText: "Back", page: "/TaskList/CompleteAndSaveInitialDiagnosisAssessment/Index");
        public static readonly LinkItem RecordVisitDateToVisitSchool = AddLinkItem(backText: "Back", page: "/TaskList/RecordVisitDateToVisitSchool/Index");
        public static readonly LinkItem ChoosePreferredSupportingOrganisation = AddLinkItem(backText: "Back", page: "/TaskList/ChoosePreferredSupportingOrganisation/Index");
        public static readonly LinkItem RecordInitialDiagnosisDecision = AddLinkItem(backText: "Back", page: "/TaskList/RecordInitialDiagnosisDecision/Index");
        public static readonly LinkItem AddSupportingOrganisationContactDetails = AddLinkItem(backText: "Back", page: "/TaskList/AddSupportingOrganisationContactDetails/Index");
        public static readonly LinkItem DueDiligenceOnPreferredSupportingOrganisation = AddLinkItem(backText: "Back", page: "/TaskList/DueDiligenceOnPreferredSupportingOrganisation/Index");
        public static readonly LinkItem RecordSupportingOrganisationAppointment = AddLinkItem(backText: "Back", page: "/TaskList/RecordSupportingOrganisationAppointment/Index");
        public static readonly LinkItem ShareTheIndicativeFundingBandAndTheImprovementPlanTemplate = AddLinkItem(backText: "Back", page: "/TaskList/ShareTheIndicativeFundingBandAndTheImprovementPlanTemplate/Index");
        public static readonly LinkItem RecordImprovementPlanDecision = AddLinkItem(backText: "Back", page: "/TaskList/RecordImprovementPlanDecision/Index");
        public static readonly LinkItem SendAgreedImprovementPlanForApproval = AddLinkItem(backText: "Back", page: "/TaskList/SendAgreedImprovementPlanForApproval/Index");
        public static readonly LinkItem RequestPlanningGrantOfferLetter = AddLinkItem(backText: "Back", page: "/TaskList/RequestPlanningGrantOfferLetter/Index");
        public static readonly LinkItem ConfirmPlanningGrantOfferLetter = AddLinkItem(backText: "Back", page: "/TaskList/ConfirmPlanningGrantOfferLetterSent/Index");
        public static readonly LinkItem ReviewTheImprovementPlan = AddLinkItem(backText: "Back", page: "/TaskList/ReviewTheImprovementPlan/Index");
        public static readonly LinkItem RequestImprovementGrantOfferLetter = AddLinkItem(backText: "Back", page: "/TaskList/RequestImprovementGrantOfferLetter/Index");
        public static readonly LinkItem ConfirmImprovementGrantOfferLetterSent = AddLinkItem(backText: "Back", page: "/TaskList/ConfirmImprovementGrantOfferLetterSent/Index");
        public static readonly LinkItem FundingHistory = AddLinkItem(backText: "Back", page: "/TaskList/FundingHistory/Index");
        public static readonly LinkItem FundingHistoryAdd = AddLinkItem(backText: "Back", page: "/TaskList/FundingHistory/AddFundingHistory");
        public static readonly LinkItem FundingHistoryEdit = AddLinkItem(backText: "Back", page: "/TaskList/FundingHistory/EditFundingHistory");
        public static readonly LinkItem FundingHistoryDetails = AddLinkItem(backText: "Back", page: "/TaskList/FundingHistory/FundingHistoryDetails");
    }

    public static class DeleteProject
    {
        public static readonly LinkItem ConfirmToDeleteProject = AddLinkItem(page: "/DeleteSupportProject/Index");
    }
    public static class Notes
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/Notes/Index");
        public static readonly LinkItem NewNote = AddLinkItem(backText: "Back", page: "/Notes/NewNote");
        public static readonly LinkItem EditNote = AddLinkItem(backText: "Back", page: "/Notes/EditNote");
    }

    public static class EngagementConcern
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/EngagementConcern/Index");
        public static readonly LinkItem RecordEngagementConcern = AddLinkItem(backText: "Back", page: "/EngagementConcern/RecordEngagementConcern");
        public static readonly LinkItem EscalateEngagementConcern = AddLinkItem(backText: "Back", page: "/EngagementConcern/EscalateEngagementConcern/EscalateEngagementConcern");
        public static readonly LinkItem ReasonForEscalation = AddLinkItem(backText: "Back", page: "/EngagementConcern/EscalateEngagementConcern/ReasonForEscalation");
        public static readonly LinkItem DateOfDecision = AddLinkItem(backText: "Back", page: "/EngagementConcern/EscalateEngagementConcern/DateOfDecision");
        public static readonly LinkItem EscalationConfirmation = AddLinkItem(backText: "Back", page: "/EngagementConcern/EscalateEngagementConcern/EscalationConfirmation");
        public static readonly LinkItem RecordUseOfInformationPowers = AddLinkItem(backText: "Back", page: "/EngagementConcern/RecordUseOfInformationPowers");
    }

    public static class CaseStudy
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/CaseStudy/Index");
        public static readonly LinkItem SetDetails = AddLinkItem(backText: "Back", page: "/CaseStudy/SetDetails");
    }

    public static class AssignDeliveryOfficer
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/AssignDeliveryOfficer/Index");

    }

    public static class ImprovementPlan
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/ImprovementPlan/Index");
        public static readonly LinkItem SelectAnAreaOfImprovement = AddLinkItem(backText: "Back", page: "/ImprovementPlan/SelectAnAreaOfImprovement");
        public static readonly LinkItem AddAnObjective = AddLinkItem(backText: "Back", page: "/ImprovementPlan/AddAnObjective");
        public static readonly LinkItem EditAnObjective = AddLinkItem(backText: "Back", page: "/ImprovementPlan/EditAnObjective");
        public static readonly LinkItem ImprovementPlanTab = AddLinkItem(backText: "Back", page: "/ImprovementPlan/ImprovementPlanTab");

    }

    public static class Public
    {
        public static readonly LinkItem Accessibility = AddLinkItem(page: "/Public/AccessibilityStatement");
        public static readonly LinkItem CookiePreferences = AddLinkItem(page: "/Public/CookiePreferences");
        public static readonly LinkItem Privacy = AddLinkItem(page: "/Public/Privacy");
        public static readonly LinkItem CookiePreferencesURL = AddLinkItem(page: "/public/cookie-Preferences");
    }
}

public class LinkItem
{
    public string Page { get; set; } = null!;
    public string BackText { get; set; } = "Back";
}
