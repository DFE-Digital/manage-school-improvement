import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import riseHomePage from "cypress/pages/riseHomePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";

describe("User creates a basic project", () => {
  let school: string;
  let urn: string;
  let localAuthority: string;
  let region: string;
  let schoolType: string;
  let faithSchool: string;
  let ofstedRating: string;
  let lastInspectionCheckDetails: string;
  let pfi: string;

  before(() => {
    cy.fixture('school-data').then((data) => {
      school = data.school;
      urn = data.urn;
      localAuthority = data.localAuthority;
      region = data.region;
      schoolType = data.schoolType;
      faithSchool = data.faithSchool;
      ofstedRating = data.ofstedRating;
      lastInspectionCheckDetails = data.lastInspectionCheckDetails;
      pfi = data.pfi;
    });
  });

  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-identified-for-targeted-intervention");
  });

  it("Should be able to add a school and add it to the list", () => {
    riseHomePage.AddSchool();

    cy.executeAccessibilityTests();

    whichSchoolNeedsHelp
      .hasHeader("Select school")
      .withSchoolName("Plymouth Grove Primary")
      .clickContinue();

    cy.executeAccessibilityTests();

    checkSchoolDetails
      .hasHeader("Check school details")
      .hasSchoolName("Plymouth Grove Primary")
      .hasURN(urn)
      .hasLocalAuthority(localAuthority)
      .hasSchoolType(schoolType)
      .hasFaithSchool(faithSchool)
      .hasOfstedRating(ofstedRating)
      .hasLastInspection(lastInspectionCheckDetails)
      .hasPFI(pfi);

    checkSchoolDetails.clickContinue();

    riseHomePage
      .hasSchoolName(school)
      .hasURN(urn)
      .hasLocalAuthority(localAuthority)
      .hasRegion(region)
      .hasAddSchoolSuccessNotification();
  });
});
