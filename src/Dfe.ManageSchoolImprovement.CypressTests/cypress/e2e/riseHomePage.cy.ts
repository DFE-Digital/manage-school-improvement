import { Logger } from "cypress/common/logger";
import riseHomePage from "cypress/pages/riseHomePage";
import path from "path";


describe("User navigates to the rise landing page", () => {
    beforeEach(() => {
        cy.login();   
    });

    it("Should be able to see Add a school option and school list", () => {
       riseHomePage
         .hasAddSchool()
         .hasProjectCount()
         .hasProjectFilter()
         
         cy.executeAccessibilityTests()
    });

    it("Should show the success notification banner when a filter is applied", () => {
        riseHomePage
            .hasProjectFilter()
            .selectEastMidlandsRegionFilter()
            .applyFilters()
            .hasSuccessNotification()
    });
});
