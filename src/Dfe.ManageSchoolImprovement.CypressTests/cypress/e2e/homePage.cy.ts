import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import paginationComponent from "cypress/pages/pagination";

describe("User navigates to the MSI landing page", () => {
    beforeEach(() => {
        cy.login();
    });

    it("should have cookies banner and user should navigate to the cookies page", () => {
        Logger.log("Cookies banner")
        homePage
            .hasCookiesBanner()
            .viewCookiesPage()

        cy.executeAccessibilityTests()
    });

    it("should have 'Add a school' button, school list and filter section", () => {
        homePage
            .hasAddSchool()
            .hasProjectCount()
            .hasProjectFilter()
        Logger.log("Filters on the homepage")

        cy.executeAccessibilityTests()
    });


    it('should display school records', () => {
        Logger.log("Homepage shows results")
        homePage
            .resultCountNotZero()
    });


    it("should navigate to the next and previous pages", () => {
        paginationComponent
            .hasPagination()
            .navigateToTheNextPage()
            .navigateToThePreviousPage()
        Logger.log("Moved to the previous page, which is page 1")

        cy.executeAccessibilityTests()
    });
});
