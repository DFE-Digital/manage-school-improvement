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

    beforeEach(() => {
        cy.removeProjectIfItExists(urn);
        cy.login();
        cy.url().should("contains", "schools-identified-for-targeted-intervention");

        Logger.log("Navigating to add school page and add non eligible school");
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
    });

    it("should navigate to the 'School is not eligible' page", () => {
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

        Logger.log("Not Eligible sholl should not display in the UI");
        homePage
            .selectProjectFilter(schoolLong)
            .applyFilters()
            .hasFilterSuccessNotification()
    });


    it("should get validation on eligibility change date and details page", () => {
        confirmStartingEligibility
            .hasHeader("When did the school's eligibility change?")
            .clickContinue()
            .hasValidation("Enter a date", "eligibility-check-date-error-link")
            .eligibilityChangeDate("30/03/2049")
            .clickContinue()
            .hasValidation("Enter today's date or a date in the past", "eligibility-check-date-error-link")

        cy.executeAccessibilityTests();

        Logger.log("Validation should work on eligibility change reason");
        confirmStartingEligibility
            .eligibilityChangeDate("01/04/2026")
            .clickContinue()
            .clickContinue()
            .hasHeader("Explain the reasons for the eligibility change")
            .clickContinue()
            .hasValidation("Enter details", "eligibility-check-details-error-link")
            .eligibilityChangeReason("The conference hall was already busy before the first session began. People gathered around the stands, picking up brochures, chatting over coffee, and comparing notes from earlier talks. One area focused on cloud security, another on AI infrastructure, while a smaller corner displayed a simple experiment showing seeds growing inside a small box, like astronauts use in space. It was surprisingly memorable and reminded everyone that even the smallest idea can grow with the right conditions nearby.!")
            .clickContinue()
            .hasValidation("Details must be 500 characters or less", "eligibility-check-details-error-link")

        cy.executeAccessibilityTests();

    });

    it("user should be able to Change the answers", () => {
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

        Logger.log("Changes date updated");
        confirmStartingEligibility
            .checkYourAnswers()
            .clickChangeLink("Date of change")
            .eligibilityChangeDate("15/03/2026")
            .clickContinue()
            .checkYourAnswers()
    });
});
