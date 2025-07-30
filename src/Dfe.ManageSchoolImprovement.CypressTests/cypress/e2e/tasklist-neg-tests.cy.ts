import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import taskList from "cypress/pages/taskList";
import taskListActions from "cypress/pages/tasks/taskListActions";

describe("Tasklist negative tests", () => {
    
    beforeEach(() => {
        cy.login();
        cy.url().should("contains", "schools-identified-for-targeted-intervention");
    });
    
    it("Should be able to validate Tasklist", () => {

        Logger.log("Selecting project");
        homePage
            .selectProjectFilter("Plymtree Church of England Primary School")
            .applyFilters()
            .selectSchoolName("Plymtree Church of England Primary School");
        
        Logger.log("Validating date input field");
        taskList.selectTask("Contact the responsible body");
        taskListActions.clearDateInput("responsible-body-contacted-date");

        taskListActions.enterDate("responsible-body-contacted-date", "po", "ta", "to");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Enter a date in the correct format", "responsible-body-contacted-date-error-link");
        taskListActions.clearDateInput("responsible-body-contacted-date");

        taskListActions.enterDate("responsible-body-contacted-date", "5", "1", "1978");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Year must be between 2000 and 2050", "responsible-body-contacted-date-error-link");
        taskListActions.clearDateInput("responsible-body-contacted-date");

        taskListActions.enterDate("responsible-body-contacted-date", "5", "1", "2078");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Year must be between 2000 and 2050", "responsible-body-contacted-date-error-link");
        taskListActions.clearDateInput("responsible-body-contacted-date");

        taskListActions.enterDate("responsible-body-contacted-date", "5", "10", "2049");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("You must enter today's date or a date in the past", "responsible-body-contacted-date-error-link");
        taskListActions.clearDateInput("responsible-body-contacted-date");

        taskListActions.enterDate("responsible-body-contacted-date", "56", "10", "2024");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Day must be between 1 and 31", "responsible-body-contacted-date-error-link");
        taskListActions.clearDateInput("responsible-body-contacted-date");

        taskListActions.enterDate("responsible-body-contacted-date", "5", "13", "2024");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskListActions.hasValidation("Month must be between 1 and 12", "responsible-body-contacted-date-error-link");
        taskListActions.clearDateInput("responsible-body-contacted-date");

        whichSchoolNeedsHelp.clickBack();

        Logger.log("Validating text input field");
        taskList.selectTask("Record improvement plan decision");
        taskListActions.selectButtonOrCheckbox("no");
        taskListActions.enterText("DisapprovingImprovementPlanDecisionNotes", "' OR 1=1--'");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskList.hasFilterSuccessNotification();

        taskList.selectTask("Record improvement plan decision");
        taskListActions.selectButtonOrCheckbox("no");
        taskListActions.clearInput("DisapprovingImprovementPlanDecisionNotes");
        taskListActions.enterText("DisapprovingImprovementPlanDecisionNotes", "<script>window.alert('Hello World')</script>");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskList.hasFilterSuccessNotification();

        taskList.selectTask("Record improvement plan decision");
        taskListActions.selectButtonOrCheckbox("no");
        taskListActions.clearInput("DisapprovingImprovementPlanDecisionNotes");
        taskListActions.enterText("DisapprovingImprovementPlanDecisionNotes", "echo ${username}");
        taskListActions.selectButtonOrCheckbox("save-and-continue-button");
        taskList.hasFilterSuccessNotification();
    });
});
