import watchlistHomepage from "cypress/pages/watchlistHomepage";
import { Logger } from "cypress/common/logger";
import { access } from "fs";

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

       it("should be able to add a school to their watchlist", () => {
        Logger.log("Add school to watchlist and validate the school is added to the list");
        watchlistHomepage
            .clickAddSchoolToWatchlistButton()

        cy.executeAccessibilityTests()

        watchlistHomepage
            .selectAndContinueAddSchoolToWatchlist('Plymouth Grove Primary School')
            .confirmSchoolDetails()

        cy.executeAccessibilityTests()

        watchlistHomepage    
            .clickSaveAndCompleteButton()
            .successfullyAddedMessage()
            .hasSchoolList()
            .hasResultsWithDetails()
    });


    it("should be able to find the watchlist table with details and helpful links", () => {
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


    it("should be able to remove the school from their watchlist", () => {
        Logger.log("User navigated to Task list");
        watchlistHomepage
            .hasSchoolList()
            .clickRemoveLinkForSchool()

        cy.executeAccessibilityTests()

        watchlistHomepage    
            .removeSchoolFromWatchlist()
            .successfullyRemovedMessage()

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
