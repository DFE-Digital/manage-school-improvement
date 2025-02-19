import { Logger } from "cypress/common/logger";
import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import riseHomePage from "cypress/pages/riseHomePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import aboutTheSchool from "cypress/pages/aboutTheSchool";
import path from "path";


describe("User navigates to the rise landing page", () => {
    let school = 'Coom'

    beforeEach(() => {
        cy.login();
        cy.url().should('contains', 'schools-requiring-improvement')   
    });

    it("Should be able to see Add a school option and school list", () => {
       riseHomePage
         .AddSchool()

       cy.executeAccessibilityTests()

       whichSchoolNeedsHelp.withSchoolName(school)

       whichSchoolNeedsHelp.clickContinue()

       cy.executeAccessibilityTests()

       checkSchoolDetails.clickContinue()

       riseHomePage.selectSchool(school)

      // aboutTheSchool.deleteSchool()

    });
});
