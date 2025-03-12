import { Logger } from "cypress/common/logger";
import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import riseHomePage from "cypress/pages/riseHomePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import aboutTheSchool from "cypress/pages/aboutTheSchool";
import path from "path";


describe("User navigates to the rise landing page", () => {
  let header;
  let school;
  let urn;
  let localAuthority;
  let region;
  let schoolType;
  let faithSchool;
  let ofstedRating;
  let lastInspection;
  let pfi;

  before(() => {
    cy.fixture('school-data').then((data) => {
      header = data.header;
      school = data.school;
      urn = data.urn;
      localAuthority = data.localAuthority;
      region = data.region;
      schoolType = data.schoolType;
      faithSchool = data.faithSchool;
      ofstedRating = data.ofstedRating;
      lastInspection = data.lastInspection;
      pfi = data.pfi;
    });
  });

  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-requiring-improvement");
  });

  it("Should be able to add a school and add it to the list", () => {
    riseHomePage.AddSchool();

    cy.executeAccessibilityTests();

    whichSchoolNeedsHelp
      .hasHeader(header)
      .withSchoolName(school)
      .clickContinue();

    cy.executeAccessibilityTests();

    checkSchoolDetails
      .hasHeader(header)
      .hasSchoolName(school)
      .hasURN(urn)
      .hasLocalAuthority(localAuthority)
      .hasSchoolType(schoolType)
      .hasFaithSchool(faithSchool)
      .hasOfstedRating(ofstedRating)
      .hasLastInspection(lastInspection)
      .hasPFI(pfi);

    checkSchoolDetails.clickContinue();

    riseHomePage
      .hasSchoolName(school)
      .hasURN(urn)
      .hasLocalAuthority(localAuthority)
      .hasRegion(region);
  });
});
