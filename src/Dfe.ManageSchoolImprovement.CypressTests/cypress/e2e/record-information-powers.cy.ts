import engagementConcern from "cypress/pages/engagementConcern";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";

describe("User navigates to the Engagement Concern tab to Record  use of Information powers", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Engagement concern')

        cy.executeAccessibilityTests()
    });

    it("should be able to cancel recording the Use of Information powers", () => {
        Logger.log("check that Canel link works for Information powers");
        engagementConcern.clickRecordUserOfInformationPowersButton()
        engagementConcern.clickCancel();
        engagementConcern.engagementConcernPageDisplayed();

        cy.executeAccessibilityTests()
    });

    it("should get validation error message for empty fields on 'Record use of information powers' page", () => {
        Logger.log("check that validation error message is displayed");
        engagementConcern.clickRecordUserOfInformationPowersButton();

        //save without entering any details and validate error message
        engagementConcern.clickButton('Save and return');
        engagementConcern.errorMessage('information-powers-details-error-link', 'Enter details')
        engagementConcern.errorMessage('powers-used-date-error-link', 'Enter a date')

        cy.executeAccessibilityTests()

    })


    it("should be able to Record use of information powers ", () => {
        Logger.log("record information powers in use");
        engagementConcern.clickRecordUserOfInformationPowersButton()
        engagementConcern.enterText("information-powers-details", "Details about why and how the powers have been used");
        engagementConcern.enterDate("powers-used-date", "5", "10", "2024");

        cy.executeAccessibilityTests()

        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Use of information powers recorded");
        engagementConcern.hasFieldsNotEmpty();
        engagementConcern.hasInformationPowersChangeLink("Change");

        cy.executeAccessibilityTests();
    })

    it("should be able to Change Information Powers ", () => {
        Logger.log("change information powers");
        engagementConcern.hasInformationPowersChangeLink("Change")
        engagementConcern.clickInformationPowersChangeLink();
        engagementConcern.enterText("information-powers-details", "Add new details");
        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Information powers updated");

        cy.executeAccessibilityTests();
    });
});
