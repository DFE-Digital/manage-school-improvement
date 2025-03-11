import { Logger } from "cypress/common/logger";
import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import riseHomePage from "cypress/pages/riseHomePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import aboutTheSchool from "cypress/pages/aboutTheSchool";
import path from "path";


describe("User navigates to the rise landing page", () => {
  let header = Cypress.env('header')
  let school = Cypress.env('school')
  let urn = Cypress.env('urn')
  let localAuthority = Cypress.env('localAuthority')
  let region = Cypress.env('region')
  let schoolType = Cypress.env('schoolType')
  let faithSchool = Cypress.env('faithSchool')
  let ofstedRating = Cypress.env('ofstedRating')
  let lastInspection = Cypress.env('lastInspection')
  let pfi = Cypress.env('pfi')

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
                         .hasURN(urn)
                         .hasLocalAuthority(localAuthority)
                         .hasSchoolType(schoolType)
                         .hasFaithSchool(faithSchool)
                         .hasOfstedRating(ofstedRating)
                         .hasLastInspection(lastInspection)
                         .hasPFI(pfi)

       checkSchoolDetails.clickContinue()

       riseHomePage.hasSchoolName(school)
                   .hasURN(urn)
                   .hasLocalAuthority(localAuthority)
                   .hasRegion(region)

    });
});
