import { Logger } from "cypress/common/logger";
import riseHomePage from "cypress/pages/homePage";
import path from "path";


describe("User navigates to the MSI landing page", () => {
    beforeEach(() => {
        cy.login();   
    });

    it("should have 'Add a school' button, school list and filter section", () => {
       riseHomePage
         .hasAddSchool()
         .hasProjectCount()
         .hasProjectFilter()
         
         cy.executeAccessibilityTests()
    });

    it("should have cookies banner and user should navigate to the cookies page", () => {
       riseHomePage
         .hasCookiesBanner()
         .viewCookiesPage()
    });    

    it("Should show the success notification banner when a filter is applied", () => {
        riseHomePage
            .hasProjectFilter()
            .selectEastMidlandsRegionFilter()
            .applyFilters()
            .hasFilterSuccessNotification()
    });
});
