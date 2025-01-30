import { Logger } from "cypress/common/logger";
import riseHomePage from "cypress/pages/riseHomepage";
import path from "path";


describe("User navigates to the rise landing page", () => {
    beforeEach(() => {
        cy.login();
        cy.url().should('contains', 'schools-requiring-improvement')   
    });

    it("Should be able to see Add a school option and school list", () => {
       riseHomePage
         .hasAddSchool()
         .hasProjectCount()
         .hasProjectFilter()
    });
});
