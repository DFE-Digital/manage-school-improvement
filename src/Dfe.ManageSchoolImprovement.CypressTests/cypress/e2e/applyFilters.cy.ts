import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";

describe("User search results by appliying filters", () => {
    beforeEach(() => {
        cy.login();
    });

    it("should show the success notification banner when a region filter is applied", () => {
        homePage
            .hasProjectFilter()
            .selectEastMidlandsRegionFilter()
            .applyFilters()
            .hasFilterSuccessNotification()
    });

    it("should filter projects by region", () => {
        cy.executeAccessibilityTests()

        Logger.log("Testing we can filter projects by region...");
        homePage
          .withFilterRegions()
           .hasFilterRegions()

        Logger.log("Clearing Filters...");
        homePage.clearFilters()
    });

    it("should filter projects by school name", () => {
        cy.executeAccessibilityTests()

        Logger.log("Testing we can filter projects by inputting schoolname...");
        homePage
          .withProjectFilter("Outwood Academy Shafton")
          .hasSchoolName("Outwood Academy Shafton")

        Logger.log("Clearing Filters...");
        homePage.clearFilters()
    });
});
