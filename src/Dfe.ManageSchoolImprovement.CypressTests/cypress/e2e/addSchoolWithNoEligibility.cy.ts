import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import confirmStartingEligibility from "cypress/pages/confirmStartingEligibility";

describe("User try to add a school which is not eligible for intervention", () => {
    const {
        schoolShort = "Ashbury",
        schoolLong = "Ashbury Meadow Primary School",
        urn = "133770",
    } = Cypress.env();

   before(() => {
        cy.removeProjectIfItExists(urn);
    });


    // Initial project creation
    it("navigated to the 'School is not eligible' page", () => {
        cy.login();
        cy.url().should("contains", "schools-identified-for-targeted-intervention");

        homePage.AddSchool();

        cy.executeAccessibilityTests();

        whichSchoolNeedsHelp
            .hasHeader("Select school")
            .withShortSchoolName(schoolShort)
            .withLongSchoolName(schoolLong)
            .clickContinue();

        cy.executeAccessibilityTests();

        checkSchoolDetails
            .hasHeader("Check school details")
            .hasSchoolName(schoolLong)
            .hasURN(urn)
            .hasLocalAuthority("Manchester")
            .clickContinue();

        cy.executeAccessibilityTests();

        Logger.log("Selecting that the school is not eligible to start intervention");
        confirmStartingEligibility
            .hasHeader("Is this school eligible to begin targeted intervention?")
            .selectEligibility(false)
            .clickContinue()

        cy.executeAccessibilityTests();

        confirmStartingEligibility
            .hasHeader("When did the school's eligibility change?")
            .eligibilityChangeDate("30/03/2026")
            .clickContinue()

        cy.executeAccessibilityTests();

        confirmStartingEligibility
            .hasHeader("Explain the reasons for the eligibility change")
            .eligibilityChangeReason("The school has improved and is no longer eligible for intervention.")
            .clickContinue()

        cy.executeAccessibilityTests();

        confirmStartingEligibility
            .checkYourAnswers()
            .clickSaveAndComplete();

        cy.executeAccessibilityTests();

        Logger.log("School not eligible page should display with correct information");
        confirmStartingEligibility
            .schoolIsNotEligiblePage()
            .clickBackToProjectListButton()
    });
});
