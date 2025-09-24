import engagementConcern from "cypress/pages/engagementConcern";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";

describe("User navigates to the Engagement Concern tab to record Engagement concern and information powers", () => {
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
       
        //save without entering details and validate error message
        engagementConcern.clickButton('Save and return');
        engagementConcern.errorMessage('engagement-concern-details-error-link', 'You must enter concern details')

        cy.executeAccessibilityTests()

        engagementConcern.enterText("engagement-concern-details", "Recording new engagement concern")

        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Engagement concern recorded");
        engagementConcern.hasFieldsNotEmpty();
        engagementConcern.hasEngagementConcernChangeLink("Change");
        engagementConcern.hasEscalateLink("Escalate");

        cy.executeAccessibilityTests()
    });

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

    it("should be able to cancel recording the Use of Information powers", () => {
        Logger.log("check that Canel link works for Information powers");
        engagementConcern.clickRecordUserOfInformationPowersButton()
        engagementConcern.clickCancel();
        engagementConcern.engagementConcernPageDisplayed();

        cy.executeAccessibilityTests()
    });


    it("should be able to Record use of information powers ", () => {
        Logger.log("record information powers in use");
        engagementConcern.clickRecordUserOfInformationPowersButton()
        engagementConcern.checkCheckbox('information-powers-in-use');
        engagementConcern.enterText("information-powers-details", "Details about why and how the powers have been used");
        engagementConcern.enterDate("powers-used-date", "5", "10", "2024");

        cy.executeAccessibilityTests()

        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Use of information powers recorded");
        engagementConcern.hasFieldsNotEmpty();
        engagementConcern.hasInformationPowersChangeLink("Change");

        cy.executeAccessibilityTests();
    })

    it("should be able to Change Engagement Concern ", () => {
        Logger.log("change engagement concern");
        engagementConcern.hasEngagementConcernChangeLink("Change")
        engagementConcern.clickEngagementConcernChangeLink();
        engagementConcern.enterText("engagement-concern-details", "Add new details");
        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Engagement concern updated");

        cy.executeAccessibilityTests();
    })

    it.only("should be able to Change Information Powers ", () => {
        Logger.log("change information powers");
        engagementConcern.hasInformationPowersChangeLink("Change")
        engagementConcern.clickInformationPowersChangeLink();
        engagementConcern.enterText("information-powers-details", "Add new details");
        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Information powers updated");

        cy.executeAccessibilityTests();
    })
});