import engagementConcern from "cypress/pages/engagementConcern";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";

describe("User navigates to the Engagement Concern tab to record use of interim executive board", () => {
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
        engagementConcern.checkCheckbox('ieb-created')

        //save without entering details and validate error message
        engagementConcern.clickButton('Confirm and continue');
        engagementConcern.errorMessage('more-detail-error', 'You must enter details')

        cy.executeAccessibilityTests()

        engagementConcern.enterText("ieb-created-details", "Recording new use of interim executive board")
        engagementConcern.clickButton('Confirm and continue');
        engagementConcern.hasTitle('Enter date of regional director\'s decision to create an interim executive board - Manage school improvement')

        Logger.log("validate ieb date");
        engagementConcern.clickButton('Save');
        engagementConcern.errorMessage('ieb-created-date-error', 'You must enter a date')
        engagementConcern.enterDate("ieb-created-date", "5", "10", "2024");

        cy.executeAccessibilityTests()

        engagementConcern.clickButton('Save');
        engagementConcern.hasSuccessNotification("Use of interim executive board recorded");
        engagementConcern.hasFieldsNotEmpty()

        cy.executeAccessibilityTests()
    });

it("should be able to make changes to interim executive board when ieb already recorded", () => {
    Logger.log("Change the details of interim executive board");
    engagementConcern.clickChangeLinkForIEB('Change');
    engagementConcern.hasTitle('Change use of interim executive board - Manage school improvement')
    engagementConcern.unCheckCheckbox('ieb-created')
    engagementConcern.clickButton('Confirm and continue');
    engagementConcern.hasSuccessNotification("Interim executive board removed");
  });   

});