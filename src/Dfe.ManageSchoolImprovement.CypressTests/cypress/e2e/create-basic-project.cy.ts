import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import riseHomePage from "cypress/pages/riseHomePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import * as schoolData from "cypress/fixtures/school-data.json";

describe("User creates a basic project", () => {
  const {
    school,
    urn,
    localAuthority,
    region,
    schoolType,
    faithSchool,
    ofstedRating,
    lastInspectionCheckDetails,
    pfi
   } = schoolData;

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
