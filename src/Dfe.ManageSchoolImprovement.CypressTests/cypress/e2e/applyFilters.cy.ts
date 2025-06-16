import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";

describe("User search results by appliying filters", () => {
    beforeEach(() => {
        cy.login();
    });

    afterEach(() => {
        Logger.log("Clear Filters");
        homePage.clearFilters()
    });

    it("should show results successfully on selecting multiple projects", () => {
        homePage
            .hasProjectFilter()
            .selectEastMidlandsRegionFilter()
            .applyFilters()
            .hasFilterSuccessNotification()

        //  cy.executeAccessibilityTests() commented accessibility due to bug #209930
    });

    it("should show the expected filtered results", () => {
        Logger.log("Testing-filter projects by region");
        homePage
            .selectFilterRegions()
            .applyFilters()
            .hasFilterSuccessNotification()
            .hasFilterRegions()

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by school name", () => {
        Logger.log("Testing- filter projects by providing schoolname");
        homePage
            .selectProjectFilter("Outwood Academy Shafton")
            .applyFilters()
            .hasFilterSuccessNotification()
            .hasSchoolName("Outwood Academy Shafton")

        //  cy.executeAccessibilityTests()
    });

       it("should filter projects by URN", () => {
        Logger.log("Testing- filter projects by providing URN");
        homePage
            .selectProjectFilter("105443")
            .applyFilters()
            .hasFilterSuccessNotification()
            .hasURN("105443")

        //  cy.executeAccessibilityTests()
    }); 

    it("should filter projects by Assigned to", () => {
        Logger.log("Testing - filter projects by Assigned to");
        homePage
            .selectAssignedToFilter("Richika Dogra")
            .applyFilters()
            .hasFilterSuccessNotification()

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by Not Assigned to", () => {
        Logger.log("Testing - filter projects by Assigned to");
        homePage
            .selectNotAssignedToFilter()
            .applyFilters()
            .hasFilterSuccessNotification()

        //  cy.executeAccessibilityTests()
    });

    it("should show/hide all the filter sections", () => {
        Logger.log("Testing - filter projects by Assigned to");
        homePage
            .showAllFilterSections()
            .hideAllFilterSections()

        //  cy.executeAccessibilityTests()
    });

    it("should show all the results when no filter is selected", () => {
        Logger.log("Testing - filter projects by Assigned to");
        homePage
            .applyFilters()
            .noFiltersSelected()
            .resultCountNotZero()

        //  cy.executeAccessibilityTests()
    });
});
