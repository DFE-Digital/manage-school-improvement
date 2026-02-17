import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";

describe("User search results by applying filters", () => {
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
            .selectNottinghamLAFilter()
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero();

        // cy.executeAccessibilityTests() - commented accessibility due to bug #209930
    });

    it("should show the expected filtered results", () => {
        Logger.log("Testing - filter projects by region");
        homePage
            .selectFilterRegions()
            .applyFilters()
            .hasFilterSuccessNotification()
            .hasFilterRegions();

        // cy.executeAccessibilityTests() - commented due to bug #209930
    });

    it("should filter projects by school name", () => {
        Logger.log("Testing - filter projects by providing school name");
        homePage
            .selectProjectFilter("Outwood Academy Shafton")
            .applyFilters()
            .hasFilterSuccessNotification()
            .hasSchoolName("Outwood Academy Shafton")

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by URN", () => {
        Logger.log("Testing - filter projects by providing URN");
        homePage
            .selectProjectFilter("106135")
            .applyFilters()
            .hasFilterSuccessNotification()
            .hasURN("106135")

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by Assigned to", () => {
        const assignedTo = "Richika Dogra";
        Logger.log(`Testing - filter projects by Assigned to: ${assignedTo}`);
        homePage
            .selectFilter("Assigned to", assignedTo)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by Not Assigned to", () => {
        Logger.log("Testing - filter projects by Not Assigned to");
        homePage
            .selectFilter("Not Assigned", "")
            .applyFilters()
            .hasFilterSuccessNotification();

        //  cy.executeAccessibilityTests()
    });

    it("should show/hide all the filter sections", () => {
        Logger.log("Testing - show/hide all the filter sections");
        homePage
            .showAllFilterSections()
            .hideAllFilterSections()

        //  cy.executeAccessibilityTests()
    });

    it("should show all the results when no filter is selected", () => {
        Logger.log("Testing - show all results with no filters");
        homePage
            .applyFilters()
            .noFiltersSelected()
            .resultCountNotZero()

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by specific Region", () => {
        const region = "North West";
        Logger.log(`Testing - filter projects by specific Region: ${region}`);

        homePage
            .selectFilter("Region", region)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()
            .verifyFilterApplied("Region", region);

        Logger.log(`Successfully filtered projects by Region: ${region}`);

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by Local authority", () => {
        const localAuthority = "Birmingham";
        Logger.log(`Testing - filter projects by Local authority: ${localAuthority}`);

        homePage
            .selectFilter("Local authority", localAuthority)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()
            .verifyFilterApplied("Local authority", localAuthority);

        Logger.log(`Successfully filtered projects by Local authority: ${localAuthority}`);

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by Advised by", () => {
        const advisedBy = "TestFirstName TestSurname";

        Logger.log(`Testing - filter projects by Advised by: ${advisedBy}`);
        homePage
            .selectFilter("Advised by", advisedBy)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()

        Logger.log(`Successfully filtered projects by Advised by: ${advisedBy}`);

        //  cy.executeAccessibilityTests()
    });

    it("filters should retain their value when navigated back from the result pages", () => {
        const region = "London";

        homePage
            .selectFilter("Region", region)
            .applyFilters()
            .hasFilterSuccessNotification()
            .selectFirstSchoolFromList()
            .clickBacklink();

        Logger.log("Navigated back to the homepage after viewing a school");

        homePage
            .verifyFilterChecked("Region", region)

        Logger.log("Successfully verified that filters retain their values after navigation");

        //  cy.executeAccessibilityTests()

    });

    it("should filter projects by Trust", () => {
        Logger.log("Trust Filters");
        const trust = "Anthem Schools Trust";

        Logger.log(`Testing - filter projects by Trust: ${trust}`);
        homePage
            .selectFilter("Trust", trust)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()

        Logger.log(`Successfully filtered projects by Trust: ${trust}`);

        //  cy.executeAccessibilityTests()
    });


    it("should filter projects by Date school added to MSI- Year and month", () => {
        Logger.log("Date Filter");
        const dateSchoolAddedToMsi = "2025 April";

        Logger.log(`Testing - filter projects by Date: ${dateSchoolAddedToMsi}`);
        homePage
            .selectFilter("Date school added to MSI", dateSchoolAddedToMsi)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()
            .checkResultsContainDateSchoolAddedToMsi(dateSchoolAddedToMsi);

        Logger.log(`Successfully filtered projects by Year: ${dateSchoolAddedToMsi}`);

    });

    it("should filter projects by Date school added to MSI-Only Year", () => {
        Logger.log("Only year Filters");
        const dateSchoolAddedToMsi = "2025";

        Logger.log(`Testing - filter projects by Year: ${dateSchoolAddedToMsi}`);
        homePage
            .selectFilter("Date school added to MSI", dateSchoolAddedToMsi)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()
            .checkResultsContainDateSchoolAddedToMsi(dateSchoolAddedToMsi);

        Logger.log(`Successfully filtered projects by Year: ${dateSchoolAddedToMsi}`);

    });

    it("should filter projects by multiple Dates school added to MSI", () => {
        Logger.log("Only year Filters");
        const dateSchoolAddedToMsi1 = "2025 March";
        const dateSchoolAddedToMsi2 = "2020 September";

        homePage
            .selectFilter("Date school added to MSI", dateSchoolAddedToMsi1)
            .showDateFilterOptions()
            .selectFilter("Date school added to MSI", dateSchoolAddedToMsi2)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()

        Logger.log(`Successfully filtered projects by Year: ${dateSchoolAddedToMsi1}`);
        Logger.log(`Successfully filtered projects by Year: ${dateSchoolAddedToMsi2}`);

    });

    it("should filter projects by Current status- In progress", () => {
        Logger.log("Search records with project status filter");
        const currentStatus = "In progress";

        Logger.log(`Testing - filter projects by Current status: ${currentStatus}`);
        homePage
            .selectFilter("Current Status", currentStatus)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()

        Logger.log(`Successfully filtered projects by Current status: ${currentStatus}`);

        //  cy.executeAccessibilityTests()
    });

    it("should filter projects by Current status- In progress", () => {
        Logger.log("Project status filter");
        const currentStatus = "Paused";

        Logger.log(`Testing - filter projects by Current status: ${currentStatus}`);
        homePage
            .selectFilter("Current Status", currentStatus)
            .applyFilters()
            .hasFilterSuccessNotification()
            .resultCountNotZero()

        Logger.log(`Successfully filtered projects by Current status: ${currentStatus}`);

        //  cy.executeAccessibilityTests()
    });
});
