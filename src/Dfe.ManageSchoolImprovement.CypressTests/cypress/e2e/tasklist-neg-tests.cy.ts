import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import taskList from "cypress/pages/taskList";
import taskListActions from "cypress/pages/tasks/taskListActions";
import improvementPlan from "cypress/pages/improvementPlan";

describe("Tasklist negative tests", () => {
    
    beforeEach(() => {
        cy.login();
        cy.url().should("contains", "schools-identified-for-targeted-intervention");
        homePage
            .selectProjectFilter("Plymtree Church of England Primary School")
            .applyFilters()
            .selectSchoolName("Plymtree Church of England Primary School");
    });
    
    it("should validate date format", () => {
        Logger.log("Validating date format input");
        taskList.selectTask("Make initial contact with the responsible body");
        taskListActions.clearDateInput("responsible-body-initial-contact-date");
        taskListActions.enterDate("responsible-body-initial-contact-date", "po", "ta", "to");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Enter a date in the correct format", "responsible-body-initial-contact-date-error-link");
    });

    it("should validate year range (too early)", () => {
        Logger.log("Validating year range: too early");
        taskList.selectTask("Make initial contact with the responsible body");
        taskListActions.clearDateInput("responsible-body-initial-contact-date");
        taskListActions.enterDate("responsible-body-initial-contact-date", "5", "1", "1978");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Year must be between 2000 and 2050", "responsible-body-initial-contact-date-error-link");
    });

    it("should validate year range (too late)", () => {
        Logger.log("Validating year range: too late");
        taskList.selectTask("Make initial contact with the responsible body");
        taskListActions.clearDateInput("responsible-body-initial-contact-date");
        taskListActions.enterDate("responsible-body-initial-contact-date", "5", "1", "2078");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Year must be between 2000 and 2050", "responsible-body-initial-contact-date-error-link");
    });

    it("should validate date is not in the future", () => {
        Logger.log("Validating date is not in the future");
        taskList.selectTask("Make initial contact with the responsible body");
        taskListActions.clearDateInput("responsible-body-initial-contact-date");
        taskListActions.enterDate("responsible-body-initial-contact-date", "5", "10", "2049");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("You must enter today's date or a date in the past", "responsible-body-initial-contact-date-error-link");
    });

    it("should validate day range", () => {
        Logger.log("Validating day range");
        taskList.selectTask("Make initial contact with the responsible body");
        taskListActions.clearDateInput("responsible-body-initial-contact-date");
        taskListActions.enterDate("responsible-body-initial-contact-date", "56", "10", "2024");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Day must be between 1 and 31", "responsible-body-initial-contact-date-error-link");
    });

    it("should validate month range", () => {
        Logger.log("Validating month range");
        taskList.selectTask("Make initial contact with the responsible body");
        taskListActions.clearDateInput("responsible-body-initial-contact-date");
        taskListActions.enterDate("responsible-body-initial-contact-date", "5", "13", "2024");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Month must be between 1 and 12", "responsible-body-initial-contact-date-error-link");
    });

    it("should validate improvement plan decision notes for SQL injection", () => {
        Logger.log("Validating improvement plan decision notes for SQL injection");
        taskList.selectTask("Record improvement plan decision");
        taskListActions.selectButtonOrCheckbox("no");
        taskListActions.enterText("DisapprovingImprovementPlanDecisionNotes", "' OR 1=1--'");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskList.hasFilterSuccessNotification();
    });

    it("should validate improvement plan decision notes for XSS", () => {
        Logger.log("Validating improvement plan decision notes for XSS");
        taskList.selectTask("Record improvement plan decision");
        taskListActions.selectButtonOrCheckbox("no");
        taskListActions.clearInput("DisapprovingImprovementPlanDecisionNotes");
        taskListActions.enterText("DisapprovingImprovementPlanDecisionNotes", "<script>window.alert('Hello World')</script>");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskList.hasFilterSuccessNotification();
    });

    it("should validate improvement plan decision notes for shell injection", () => {
        Logger.log("Validating improvement plan decision notes for shell injection");
        taskList.selectTask("Record improvement plan decision");
        taskListActions.selectButtonOrCheckbox("no");
        taskListActions.clearInput("DisapprovingImprovementPlanDecisionNotes");
        taskListActions.enterText("DisapprovingImprovementPlanDecisionNotes", "echo ${username}");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskList.hasFilterSuccessNotification();
    });

    it("should validate improvement plan objectives selection", () => {
        Logger.log("Validating improvement plan objectives selection");
        taskList.selectTask("Enter improvement plan objectives");
        improvementPlan.selectAddAnotherObjective("Add another objective");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Select an area of improvement", "quality-of-education-error-link");
        taskListActions.selectButtonOrCheckbox("quality-of-education");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.selectSaveAndAddAnotherObjectiveButton();
        taskListActions.hasValidation("Enter details of the objective", "ObjectiveDetails-error-link");
    });
});
