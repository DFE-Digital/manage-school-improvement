import watchlistHomepage from "cypress/pages/watchlistHomepage";
import { Logger } from "cypress/common/logger";

describe("User lands in the watchlist homepage when loggedn in ", () => {
    beforeEach(() => {
        cy.login();
         watchlistHomepage
           .hasTitle('Your school watchlist')
       
        cy.executeAccessibilityTests()

    });

    it("should be able to see the schools assigned to the user", () => {
        Logger.log("User navigated to Task list");
        watchlistHomepage
           .hasSchoolList()
           .hasSchoolTableWithColumns(['School', 'Date added to MSI', 'Assigned to', 'Supporting organisation' , 'Milestone', 'Status']) 
           .hasResultsWithDetails()
           .hasHelpfulLinks()

        cy.executeAccessibilityTests()
    });

});    
