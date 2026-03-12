import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import taskListActions from "cypress/pages/tasks/taskListActions";
import improvementPlan from '../pages/improvementPlan'

describe('Delete improvement plan objective', () => {
    beforeEach(() => {
        cy.login();
        homePage
            .rejectCookies()
            .selectProjectFilter("Boston Nursery School")
            .applyFilters()
            .selectSchoolName("Boston Nursery School");

    });

    it("should be able to delete improvement plan objectives from Task list", () => {
        Logger.log("delete objective details from Task list");
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions
            .addAndDeleteObjective()
            .deleteObjectiveSuccessfully();

        cy.executeAccessibilityTests()
    });

    it("should be able to delete improvement plan objectives from Improvement plan tab", () => {
        Logger.log("delete objective details from Improvement plan tab");
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions.addObjective();
        taskListActions.selectButtonOrCheckbox("MarkAsComplete")
        taskListActions.clickButton('save');
        taskList
            .navigateToTab('Improvement plan')
        
        cy.executeAccessibilityTests();
            
        improvementPlan
            .hasDeleteObjectiveLink()
            .deleteObjectiveSuccessfully();

        cy.executeAccessibilityTests();
    });
});
