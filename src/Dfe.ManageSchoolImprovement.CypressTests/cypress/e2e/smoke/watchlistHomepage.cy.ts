import watchlistHomepage from "cypress/pages/watchlistHomepage";
import { Logger } from "cypress/common/logger";

describe("User lands in the watchlist homepage when loggedn in ", () => {
    beforeEach(() => {
        cy.login();
        watchlistHomepage
            .navigateToWatchlistHomepage()
            .hasTitle('Your school watchlist')

        cy.executeAccessibilityTests()

    });

    it("should show a message when there are no schools assigned to the user", () => {
        Logger.log("User friendly message show when there are no schools assigned to the user");
        watchlistHomepage
            .hasMessageWhenNoSchools('Your watchlist is currently empty.')

        cy.executeAccessibilityTests()
    });


    it("should show only those schools which are assigned to the loggedin user", () => {
        Logger.log("User friendly message show when there are no schools assigned to the user");
        watchlistHomepage
            .hasAssignedToValue()

    });

    it("should be able to find the schools list with details,assigned to the user", () => {
        Logger.log("User navigated to Task list");
        watchlistHomepage
            .hasSchoolList()
            .hasSchoolTableWithColumns(['School', 'Date added to MSI', 'Assigned to', 'Supporting organisation', 'Milestone', 'Status'])
            .hasResultsWithDetails()
            .hasHelpfulLinks()

        cy.executeAccessibilityTests()
    });


    it("should be able to navigate to the school when selected", () => {
        Logger.log("User navigated to Task list");
        watchlistHomepage
            .hasSchoolList()
            .clickFirstSchoolInList()

        cy.executeAccessibilityTests()
    });


    it("should be able to navigate to the school when selected", () => {
        Logger.log("User navigated to Task list");
        watchlistHomepage
            .hasSchoolList()
            .removeFirstSchoolInList()

        cy.executeAccessibilityTests()
    });

    it("should be able to sort the list, based on columns ", () => {
        Logger.log("User navigated to Task list");
        watchlistHomepage
            .hasSchoolList()
            .sortByColumn('School')
            .sortByColumn('Date added to MSI')
            .sortByColumn('Assigned to')
            .sortByColumn('Supporting organisation')
            .sortByColumn('Milestone')
            .sortByColumn('Status')

        cy.executeAccessibilityTests()
    });
});    
