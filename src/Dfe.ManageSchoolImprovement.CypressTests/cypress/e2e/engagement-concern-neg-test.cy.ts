import engagementConcern from "cypress/pages/engagementConcern";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";

describe("User navigates to the Engagement Concern tab and saves with incorrect details", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Engagement concern')

        cy.executeAccessibilityTests()
    });

    it("should get validation error message for empty fields on Record Engagement Concern page", () => {
        Logger.log("check that validation error message is displayed");
        engagementConcern.clickRecordEngagementConcern()
         engagementConcern.checkResolveConcernCheckbox();

        //save without entering any details and validate error message
        engagementConcern.clickButton('Save and return');

        engagementConcern.errorMessage('engagement-concern-summary-error-link', 'Enter a summary')
        engagementConcern.errorMessage('engagement-concern-details-error-link', 'Enter details')
        engagementConcern.errorMessage('resolution-details-error-link', 'Enter details')

        cy.executeAccessibilityTests()
    });
});
   