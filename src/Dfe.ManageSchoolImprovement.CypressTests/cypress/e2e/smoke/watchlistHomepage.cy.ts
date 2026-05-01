import watchlistHomepage from "cypress/pages/watchlistHomepage";
import { Logger } from "cypress/common/logger";

describe("User lands in the watchlist homepage when loggedn in ", () => {
    beforeEach(() => {
        cy.login();
         watchlistHomepage
           .hasTitle('Your school watchlist')
       
        cy.executeAccessibilityTests()

    });

    it("should be able to find the schools list with details,assigned to the user", () => {
        Logger.log("User navigated to Task list");
        watchlistHomepage
           .hasSchoolList()
           .hasSchoolTableWithColumns(['School', 'Date added to MSI', 'Assigned to', 'Supporting organisation' , 'Milestone', 'Status']) 
           .hasResultsWithDetails()
           .hasHelpfulLinks()

        cy.executeAccessibilityTests()
    });

    it("should show a message when there are no schools assigned to the user", () => {
        Logger.log("User navigated to Task list");
        watchlistHomepage
           .hasSchoolList()
           .hasMessageWhenNoSchools('Your watchlist is currently empty.')

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
           .removeFirstSchoolInList()

        cy.executeAccessibilityTests()
    });  


});    
