import engagementConcern from "cypress/pages/engagementConcern";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";

describe("User navigates to the Engagement Concern tab to record use of interim executive board for non-academy school type", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Engagement concern')

        cy.executeAccessibilityTests()
    });

    it("should be able to cancel recording the use of interim executive board", () => {
        Logger.log("check that Canel link works");
        engagementConcern.hasRecordUseOfInterimExecutiveBoardButton();
        engagementConcern.clickRecordUseOfInterimExecutiveBoard()
        engagementConcern.clickCancel();
        engagementConcern.engagementConcernPageDisplayed();

        cy.executeAccessibilityTests()
    });


    it("should get validation error message for empty fields on 'Record use of interim executive board' page", () => {
        Logger.log("check that validation error message is displayed");
        engagementConcern.clickRecordUseOfInterimExecutiveBoard()
        engagementConcern.hasTitle('Record use of interim executive board - Manage school improvement')

        //save without entering details and validate error message
        engagementConcern.clickButton('Save and return');
        engagementConcern.errorMessage('ieb-created-details-error-link', 'Enter details')
        engagementConcern.errorMessage('ieb-created-date-error-link', 'Enter a date')
    });

    it("should be able to expand 'How to create interim executive boards' section", () => {
        Logger.log("check that How to create IEB section expands with text");
        engagementConcern.hasRecordUseOfInterimExecutiveBoardButton();
        engagementConcern.clickRecordUseOfInterimExecutiveBoard()
        engagementConcern.showHowToCreateIEBSection();

        cy.executeAccessibilityTests()
    });

    it("should be able to record use of interim executive board", () => {
        Logger.log("record use of interim executive board");
        engagementConcern.clickRecordUseOfInterimExecutiveBoard()
        engagementConcern.hasTitle('Record use of interim executive board - Manage school improvement')
        engagementConcern.enterText("ieb-created-details", "Recording new use of interim executive board")
        engagementConcern.enterDate("ieb-created-date", "5", "10", "2024");

        cy.executeAccessibilityTests()

        engagementConcern.clickButton('Save and return');
        engagementConcern.hasSuccessNotification("Use of interim executive board recorded");
        engagementConcern.hasIEBAssignToDifferentConcernLink();
        engagementConcern.hasFieldsNotEmpty()

        cy.executeAccessibilityTests()
    });
});