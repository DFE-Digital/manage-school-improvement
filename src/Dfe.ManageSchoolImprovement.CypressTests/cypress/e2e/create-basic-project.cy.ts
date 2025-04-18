import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import riseHomePage from "cypress/pages/riseHomePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";


describe("User navigates to the rise landing page", () => {
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
      .hasLastInspection(lastInspection)
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
