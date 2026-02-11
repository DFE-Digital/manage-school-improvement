import aboutTheSchool from "cypress/pages/aboutTheSchool";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";

describe("User navigates to About the school tab", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('About the school')

        cy.executeAccessibilityTests()
    });

    it("should see the School overview section", () => {
        aboutTheSchool.hasSchoolOverviewSection()
        aboutTheSchool.checkAllFieldsNotEmpty();

        cy.executeAccessibilityTests()
    });

    it("should expand the information link", () => {
        aboutTheSchool.expandSectionAndCheckText();

        cy.executeAccessibilityTests()
    });

    it("should show the other services information links", () => {
        aboutTheSchool
          .hasSeeInspectionReportsAtOfstedLink()
          .hasGetInformationAboutSchoolsLink()
          .hasFindInformationAboutSchoolsAndTrustLink();

        cy.executeAccessibilityTests()

    });
});
