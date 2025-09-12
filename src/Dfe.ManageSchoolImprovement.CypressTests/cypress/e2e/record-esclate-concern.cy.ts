import engagementConcern from "cypress/pages/engagementConcern";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";

describe("User navigates to the Engagement Concern tab to Escalate an engagement concern", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Engagement concern')

        cy.executeAccessibilityTests()
    });

    it("should Escalate an engagement concern", () => {
        Logger.log("check that Escalate link works");
        engagementConcern.hasEscalateLink('Escalate');
        engagementConcern.clickEscalateLink();
        engagementConcern.hasTitle('Escalate engagement concern - Manage school improvement')
        engagementConcern.checkCheckbox('confirm-steps-taken')
        engagementConcern.clickButton('Confirm and continue');

        Logger.log("Reason for escalation");
        engagementConcern.hasTitle('Reason for escalation - Manage school improvement');
        engagementConcern.checkCheckbox('communication');
        engagementConcern.enterText("escalation-details", "Escalating engagement concern for testing");
        engagementConcern.clickButton('Continue');
        engagementConcern.hasTitle('Date of decision to escalate - Manage school improvement');

       
        Logger.log("check that Date validation works");
        engagementConcern.clickButton('Save');
        engagementConcern.errorMessage('escalate-decision-date-error', 'You must enter a date')
        engagementConcern.enterDate("escalate-decision-date", "5", "10", "2024");
        engagementConcern.clickButton('Save');
        engagementConcern.hasTitle('Escalation confirmation - Manage school improvement');
        engagementConcern.clickViewEngagementConcern();
        engagementConcern.hasTitle('Escalate engagement concern - Manage school improvement')
        engagementConcern.hasFieldsNotEmpty();
    });

});