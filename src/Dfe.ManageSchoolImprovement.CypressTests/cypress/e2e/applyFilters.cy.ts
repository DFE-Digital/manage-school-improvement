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
});