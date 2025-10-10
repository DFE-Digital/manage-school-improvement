import engagementConcern from "cypress/pages/engagementConcern";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";

describe("User navigates to the Engagement Concern tab to record Engagement concern", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Engagement concern')

        cy.executeAccessibilityTests()
    });

    it("should be able to cancel recording the Engagement concern", () => {
        Logger.log("check that Canel link works");
        engagementConcern.clickRecordEngagementConcern()
        engagementConcern.clickCancel();
        engagementConcern.engagementConcernPageDisplayed();
        cy.executeAccessibilityTests()
    });


    it("should be able to record the Engagement concern", () => {
        Logger.log("record engagement concern");
        engagementConcern.clickRecordEngagementConcern()
        engagementConcern.hasTitle('Record engagement concern - Manage school improvement')
       engagementConcern.enterText("engagement-concern-summary", "Engagement concern summary")
        
        engagementConcern.enterText("engagement-concern-details", "Recording new engagement concern")

        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Engagement concern recorded");
        engagementConcern.hasFieldsNotEmpty();
        engagementConcern.hasEngagementConcernChangeLink("Change");
        engagementConcern.hasEscalateLink("Escalate");

        cy.executeAccessibilityTests()
    });

    it("should be able to Change Engagement Concern ", () => {
        Logger.log("change engagement concern");
        engagementConcern.hasEngagementConcernChangeLink("Change")
        engagementConcern.clickEngagementConcernChangeLink();
        engagementConcern.enterText("engagement-concern-details", "Add new details");
        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Engagement concern updated");

        cy.executeAccessibilityTests();
    })

    it("should be able to resolve the Engagement concern", () => {
        Logger.log("resolve engagement concern");
        engagementConcern.hasEngagementConcernChangeLink("Change");
        engagementConcern.clickEngagementConcernChangeLink();
        engagementConcern.checkResolveConcernCheckbox();
        engagementConcern.enterText("resolution-details", "Add new details");
        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Engagement concern updated");

        cy.executeAccessibilityTests();
    });  

    it("should be able to record multiple Engagement concern", () => {

        Logger.log("record another engagement concern");
        engagementConcern.clickRecordEngagementConcern()
        engagementConcern.hasTitle('Record engagement concern - Manage school improvement')
        engagementConcern.enterText("engagement-concern-summary", "Engagement concern summary")
        engagementConcern.enterText("engagement-concern-details", "Recording new engagement concern")
        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Engagement concern recorded");
        engagementConcern.hasFieldsNotEmpty();
        engagementConcern.hasRecordEngagementConcernButton();
    });
});