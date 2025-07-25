import ofstedReports from "cypress/pages/ofstedReports";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";

describe("User navigates to the Ofsted Reports Page", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Ofsted reports')

        cy.executeAccessibilityTests()
    });

    it("should display all the fields in the Most recent and Previous Ofsted report section", () => {
        Logger.log("check most recent and previous ofsted report section");
        ofstedReports.hasMostRecentOfstedReportSection()
        ofstedReports.hasPreviousOfstedReportSection()
        ofstedReports.checkAllFieldsNotEmpty();

        cy.executeAccessibilityTests()
    });

      it("should expand the information link", () => {
        Logger.log("check that information link expands");
        ofstedReports.expandSectionAndCheckText();

        cy.executeAccessibilityTests()
    });

});