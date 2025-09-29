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


    it("should show validation error for incomplete Escalation checklist", () => {
        engagementConcern.hasEscalateLink('Escalate');
        engagementConcern.clickEscalateLink();
        engagementConcern.hasTitle('Escalate an engagement concern - Manage school improvement')
        Logger.log("check that Escalation checklist validation works");
        engagementConcern.checkEscalationCheckbox('use-information-powers')
        engagementConcern.checkEscalationCheckbox('send-notice')
        engagementConcern.clickButton('Confirm and continue');
        engagementConcern.errorMessage('before-using-notice-error-link', 'You must complete all actions in Before you consider using a notice')
        engagementConcern.errorMessage('get-approval-error-link', 'You must complete all actions in Get approval to use a notice')
        engagementConcern.errorMessage('issuing-notice-error-link', 'You must complete all actions in Issuing a notice')

    });


    it("should be able to Escalate an engagement concern", () => {
        engagementConcern.hasEscalateLink('Escalate');
        engagementConcern.clickEscalateLink();
        engagementConcern.hasTitle('Escalate an engagement concern - Manage school improvement')
        Logger.log("Confirm all steps taken");
        engagementConcern.checkEscalationCheckbox('use-existing-relationships')
        engagementConcern.checkEscalationCheckbox('follow-mediation-process')
        engagementConcern.checkEscalationCheckbox('use-information-powers')
        engagementConcern.checkEscalationCheckbox('gather-evidence')
        engagementConcern.checkEscalationCheckbox('get-approval')
        engagementConcern.checkEscalationCheckbox('complete-moderation')
        engagementConcern.checkEscalationCheckbox('draft-notice')
        engagementConcern.checkEscalationCheckbox('consult-la')
        engagementConcern.checkEscalationCheckbox('carry-out-assessment')
        engagementConcern.checkEscalationCheckbox('submit-notice')
        engagementConcern.checkEscalationCheckbox('notify-responsible-body')
        engagementConcern.checkEscalationCheckbox('send-notice')
        engagementConcern.checkEscalationCheckbox('complete-form')

        engagementConcern.clickButton('Confirm and continue')

        Logger.log("Reason for escalation")
        engagementConcern.reasonForEscalationPageDisplayed()
        engagementConcern.checkCheckbox('communication')
        engagementConcern.enterText("escalation-details", "Escalating engagement concern for testing")
        engagementConcern.clickButton('Continue')

        Logger.log("check that Date validation works")
        engagementConcern.hasTitle('Enter date of regional director\'s approval to mandate - Manage school improvement')
        engagementConcern.clickButton('Save')
        engagementConcern.errorMessage('escalate-decision-date-error', 'You must enter a date')

        Logger.log("Enter valid date")
        engagementConcern.enterDate("escalate-decision-date", "5", "10", "2024")
        engagementConcern.clickButton('Save')
        engagementConcern.hasRecordedNotification("Mandation recorded")
        engagementConcern.clickViewEngagementConcern()
        engagementConcern.hasTitle('Engagement concern - Manage school improvement')
        engagementConcern.hasFieldsNotEmpty()
    });

});