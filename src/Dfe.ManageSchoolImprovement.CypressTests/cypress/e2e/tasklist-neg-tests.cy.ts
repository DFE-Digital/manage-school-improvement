import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import taskListActions from "cypress/pages/tasks/taskListActions";
import improvementPlan from "cypress/pages/improvementPlan";
import supportingOrgType from "cypress/pages/tasks/supportingOrgType";

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
        taskListActions.hasValidation("Enter today's date or a date in the past", "responsible-body-initial-contact-date-error-link");
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

    it("should validate preferred supporting organisation type - school", () => {
        Logger.log("Validating preferred supporting organisation");
        taskList.selectTask("Choose preferred supporting organisation");
        taskListActions.hasHeader("Choose preferred supporting organisation");
        taskListActions.selectButtonOrCheckbox("support-organisation-type-school");
        taskListActions.selectButtonOrCheckbox("continue-button");
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("Enter the school name or URN", "SearchQuery-error-link");
        supportingOrgType.withShortSOName('111111');
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("We could not find any schools matching your search criteria", "SearchQuery-error-link");

        cy.executeAccessibilityTests();
    });

    it("should validate preferred supporting organisation type - trust", () => {
        Logger.log("Validating preferred supporting organisation");
        taskList.selectTask("Choose preferred supporting organisation");
        taskListActions.hasHeader("Choose preferred supporting organisation");
        taskListActions.selectButtonOrCheckbox("support-organisation-type-trust");
        taskListActions.selectButtonOrCheckbox("continue-button");
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("Enter the trust name or UKPRN", "SearchQuery-error-link");
        supportingOrgType.withShortSOName('123abc');
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("We could not find any trusts matching your search criteria", "SearchQuery-error-link");

        cy.executeAccessibilityTests();
    });

    it("should validate preferred supporting organisation type - LA", () => {
        Logger.log("Validating preferred supporting organisation");
        taskList.selectTask("Choose preferred supporting organisation");
        taskListActions.hasHeader("Choose preferred supporting organisation");
        taskListActions.selectButtonOrCheckbox("support-organisation-type-local-authority");
        taskListActions.selectButtonOrCheckbox("continue-button");
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("Enter the local authority name or code", "SearchQuery-error-link");
        supportingOrgType.withShortSOName('xxx');
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("We could not find any local authorities matching your search criteria", "SearchQuery-error-link");

        cy.executeAccessibilityTests();
    });

    it("should validate preferred supporting organisation type - LA traded service", () => {
        Logger.log("Validating preferred supporting organisation");
        taskList.selectTask("Choose preferred supporting organisation");
        taskListActions.hasHeader("Choose preferred supporting organisation");
        taskListActions.selectButtonOrCheckbox("support-organisation-type-local-authority-traded-service");
        taskListActions.selectButtonOrCheckbox("continue-button");
        taskListActions.hasHeader("Enter supporting organisation details");
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("Enter the supporting organisation's name", "organisation-name-error-link");
        taskListActions.hasValidation("Enter the supporting organisation's companies house number", "companies-house-number-error-link");

        cy.executeAccessibilityTests();
    });

    it("should validate preferred supporting organisation type - Federation or Education partneship", () => {
        Logger.log("Validating preferred supporting organisation");
        taskList.selectTask("Choose preferred supporting organisation");
        taskListActions.hasHeader("Choose preferred supporting organisation");
        taskListActions.selectButtonOrCheckbox("support-organisation-type-federation-education-partnership");
        taskListActions.selectButtonOrCheckbox("continue-button");
        taskListActions.hasHeader("Enter supporting organisation details");
        taskListActions.clearInput("organisation-name");
        taskListActions.clearInput("identifying-number");
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("Enter the supporting organisation's name", "organisation-name-error-link");
        taskListActions.hasValidation("Enter the supporting organisation's identifying number", "identifying-number-error-link");

        cy.executeAccessibilityTests();
    });

    it("should validate date for preferred supporting organisation type - Federation or Education partneship", () => {
        Logger.log("Validating preferred supporting organisation");
        taskList.selectTask("Choose preferred supporting organisation");
        taskListActions.hasHeader("Choose preferred supporting organisation");
        taskListActions.selectButtonOrCheckbox("support-organisation-type-federation-education-partnership");
        taskListActions.selectButtonOrCheckbox("continue-button");
        taskListActions.hasHeader("Enter supporting organisation details");
        taskListActions.enterText("organisation-name", 'Federation Name');
        taskListActions.enterText("identifying-number", "123456789");
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasHeader("Confirm supporting organisation details");
        taskListActions.selectButtonOrCheckbox("save-and-return-button");
        taskListActions.hasValidation("Enter a date", "date-support-organisation-confirmed-error-link");

        cy.executeAccessibilityTests();
    });

    it("should validate Confirm supporting organisation contact details", () => {
        Logger.log("Validating text input fields for 'Confirm supporting organisation contact details' task");
        taskList.selectTask("Confirm supporting organisation contact details");
        taskListActions.hasHeader("Confirm supporting organisation contact details");
        taskListActions.clearInput("name");
        taskListActions.clearInput("email-address");
        taskListActions.selectButtonOrCheckbox("continue-button");
        taskListActions.hasValidation("Enter a name", "name-error-link");
        taskListActions.hasValidation("Enter an email address", "email-address-error-link");

        cy.executeAccessibilityTests();

        taskListActions.enterText("name", "Test Test");
        taskListActions.enterText("email-address", "test.bloggsx@email.com");
        taskListActions.selectButtonOrCheckbox("continue-button");

        Logger.log("Validating text input fields for 'Confirm supporting organisation address details' page");
        taskListActions.hasHeader("Confirm supporting organisation address details");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Enter a street address", "address-1-error-link");
        taskListActions.hasValidation("Enter a town or city", "town-error-link");
        taskListActions.hasValidation("Enter a postcode", "postcode-error-link");

        cy.executeAccessibilityTests();
    });
});
