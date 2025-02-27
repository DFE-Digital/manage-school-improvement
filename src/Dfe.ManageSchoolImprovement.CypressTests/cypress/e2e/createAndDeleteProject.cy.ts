import { Logger } from "cypress/common/logger";
import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import riseHomePage from "cypress/pages/riseHomePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import aboutTheSchool from "cypress/pages/aboutTheSchool";
import path from "path";


describe("User navigates to the rise landing page", () => {
    let school = 'Coom'
    let header = 'Which school needs help?'
    let URN = '105443'
    let localAuthority = 'Manchester'
    let region = 'North West'
    let schoolType = 'Community school'
    let faithSchool = 'Does not apply'
    let ofstedRating = 'Good'
    let lastInspection = '16 January 2011'
    let pfi = 'Does not apply'

    beforeEach(() => {
        cy.login()
        cy.url().should('contains', 'schools-requiring-improvement')   
    });

    it("Should be able to add a school and add it to the list", () => {
       riseHomePage
         .AddSchool()

       cy.executeAccessibilityTests()

       whichSchoolNeedsHelp.hasHeader(header)
                           .withSchoolName(school)
                           .clickContinue()

       cy.executeAccessibilityTests()

       checkSchoolDetails.hasHeader(header)
                         .hasSchoolName(school)
                         .hasURN(URN)
                         .hasLocalAuthority(localAuthority)
                         .hasSchoolType(schoolType)
                         .hasFaithSchool(faithSchool)
                         .hasOfstedRating(ofstedRating)
                         .hasLastInspection(lastInspection)
                         .hasPFI(pfi)

       checkSchoolDetails.clickContinue()

       riseHomePage.hasSchoolName(school)
                   .hasURN(URN)
                   .hasLocalAuthority(localAuthority)
                   .hasRegion(region)

    });
});
