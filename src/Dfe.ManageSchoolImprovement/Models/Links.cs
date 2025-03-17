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
        public static readonly LinkItem WhichSchoolNeedsHelp = AddLinkItem(backText: "Back", page: "/AddSchool/WhichSchoolNeedsHelp");
        public static readonly LinkItem Summary = AddLinkItem(page: "/AddSchool/Summary");
    }

    public static class SchoolList
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/SchoolList/Index");
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
        public static readonly LinkItem ContactTheSchool = AddLinkItem(backText: "Back", page: "/TaskList/ContactTheSchool/ContactTheSchool");
        public static readonly LinkItem RecordTheSchoolResponse = AddLinkItem(backText: "Back", page: "/TaskList/RecordTheSchoolResponse/Index");
        public static readonly LinkItem CheckPotentialAdviserConflictsOfInterest = AddLinkItem(backText: "Back", page: "/TaskList/AdviserConflictOfInterest/AdviserConflictOfInterest");
        public static readonly LinkItem AllocateAdviser = AddLinkItem(backText: "Back", page: "/TaskList/AllocateAdviser/AllocateAdviser");
        public static readonly LinkItem SendIntroductoryEmail = AddLinkItem(backText: "Back", page: "/TaskList/SendIntroductoryEmail/Index");
        public static readonly LinkItem ArrangeAdviserVisitToSchool = AddLinkItem(backText: "Back", page: "/TaskList/ArrangeAdviserVisitToSchool/ArrangeAdviserVisitToSchool");
        public static readonly LinkItem CompleteAndSaveAssessmentTemplate = AddLinkItem(backText: "Back", page: "/TaskList/CompleteAndSaveAssessmentTemplate/Index");
        public static readonly LinkItem NoteOfVisit = AddLinkItem(backText: "Back", page: "/TaskList/NoteOfVisit/NoteOfVisit");
        public static readonly LinkItem RecordVisitDateToVisitSchool = AddLinkItem(backText: "Back", page: "/TaskList/RecordVisitDateToVisitSchool/Index");
        public static readonly LinkItem ChoosePreferredSupportingOrganisation = AddLinkItem(backText: "Back", page: "/TaskList/ChoosePreferredSupportingOrganisation/Index");
        public static readonly LinkItem RecordMatchingDecision = AddLinkItem(backText: "Back", page: "/TaskList/RecordMatchingDecision/Index");
        public static readonly LinkItem AddSupportingOrganisationContactDetails = AddLinkItem(backText: "Back", page: "/TaskList/AddSupportingOrganisationContactDetails/Index");
        public static readonly LinkItem DueDiligenceOnPreferredSupportingOrganisation = AddLinkItem(backText: "Back", page: "/TaskList/DueDiligenceOnPreferredSupportingOrganisation/Index");
        public static readonly LinkItem RecordSupportingOrganisationAppointment = AddLinkItem(backText: "Back", page: "/TaskList/RecordSupportingOrganisationAppointment/Index");
        public static readonly LinkItem ShareTheImprovementPlanTemplate = AddLinkItem(backText: "Back", page: "/TaskList/ShareTheImprovementPlanTemplate/Index");
        public static readonly LinkItem RecordImprovementPlanDecision = AddLinkItem(backText: "Back", page: "/TaskList/RecordImprovementPlanDecision/Index");
        public static readonly LinkItem SendAgreedImprovementPlanForApproval = AddLinkItem(backText: "Back", page: "/TaskList/SendAgreedImprovementPlanForApproval/Index");
        public static readonly LinkItem RequestPlanningGrantOfferLetter = AddLinkItem(backText: "Back", page: "/TaskList/RequestPlanningGrantOfferLetter/Index");
        public static readonly LinkItem ConfirmPlanningGrantOfferLetter = AddLinkItem(backText: "Back", page: "/TaskList/ConfirmPlanningGrantOfferLetterSent/Index");
        public static readonly LinkItem ReviewTheImprovementPlan = AddLinkItem(backText: "Back", page: "/TaskList/ReviewTheImprovementPlan/Index");
        public static readonly LinkItem RequestImprovementGrantOfferLetter = AddLinkItem(backText: "Back", page: "/TaskList/RequestImprovementGrantOfferLetter/Index");
        public static readonly LinkItem ConfirmImprovementGrantOfferLetterSent = AddLinkItem(backText: "Back", page: "/TaskList/ConfirmImprovementGrantOfferLetterSent/Index");
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

    public static class AssignDeliveryOfficer
    {
        public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/AssignDeliveryOfficer/Index");

    }
    public static class Public
    {
        public static readonly LinkItem Accessibility = AddLinkItem(page: "/Public/AccessibilityStatement");
        public static readonly LinkItem CookiePreferences = AddLinkItem(page: "/Public/CookiePreferences");
        public static readonly LinkItem CookiePreferencesURL = AddLinkItem(page: "/public/cookie-Preferences");
    }
}

public class LinkItem
{
    public string Page { get; set; } = null!;
    public string BackText { get; set; } = "Back";
}
