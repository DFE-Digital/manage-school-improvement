import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";   

describe("User navigates to any Tab listed for school", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .hasAddSchool()
            .selectProjectFilter("Plymouth Grove Primary")
            .applyFilters()
            .selectFirstSchoolFromList()

        cy.executeAccessibilityTests()
    });

    it("should show school summary list on the top of the page", () => {
        taskList
          .navigateToTab('Task list')
          .hasSchoolSummaryList()
    });

    it("should be able navigate to Change Current Status ", () => {
        Logger.log("User navigates to Change Current Status page using Change link in the summery list");
        taskList
            .navigateToChangeCurrentStatusPage()

        cy.executeAccessibilityTests()
    });

    it("should be able navigate to  Change Assigned to", () => {
        Logger.log("User navigates to Change Assigned person page using Change link in the summery list");

        taskList
            .navigateToAssignDeliveryOfficerPage()

        cy.executeAccessibilityTests()
    });

     it("should be able navigate to Allocate an adviser page", () => {
        Logger.log("User navigates to Allocate an adviser page using Change link in the summery list");

        taskList
            .navigateToAllocateAdviserPage()

        cy.executeAccessibilityTests()
    });

    it("should be able navigate to  Change Engagement concern", () => {
         Logger.log("User navigates to Change Engagement concern page using Change link in the summery list");
        taskList
            .navigateToRecordEngagementConcernPage()

        cy.executeAccessibilityTests()
    });
});
