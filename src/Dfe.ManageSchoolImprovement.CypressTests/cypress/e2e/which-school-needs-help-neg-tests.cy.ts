import homePage from "cypress/pages/homePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import { Logger } from "cypress/common/logger";

describe("Which school needs help negative tests", () => {
  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-identified-for-targeted-intervention");
    homePage.AddSchool();
  });

  it("Should validate null/empty school name", () => {
     Logger.log("Testing with null/empty school name");

    cy.executeAccessibilityTests();

    whichSchoolNeedsHelp
      .withShortSchoolName(" ")
      .clickContinue()
      .hasValidation("Enter the school name or URN");

    cy.executeAccessibilityTests();
  });

  it("Should validate invalid school name", () => {
    Logger.log("Testing with invalid school name");
    whichSchoolNeedsHelp
      .withShortSchoolName("POTATO")
      .clickContinue()
      .hasValidation("We could not find any schools matching your search criteria");

    cy.executeAccessibilityTests();

     whichSchoolNeedsHelp.clickBack();
     homePage.hasAddSchool();

  });

  it("Should validate already existing school", () => {
    Logger.log("Testing with already existing school");
     whichSchoolNeedsHelp
      .withShortSchoolName("Inta")
      .withLongSchoolName("Intake Primary School")
      .clickContinue()
      .hasValidation("This school is already receiving targeted intervention. Select a different school");

    cy.executeAccessibilityTests();
  });

  it("Should validate SQL injection attempt", () => {
    Logger.log("Testing with SQL injection attempt");
    whichSchoolNeedsHelp
      .withShortSchoolName("' OR 1=1--'")
      .clickContinue()
      .hasValidation("We could not find any schools matching your search criteria");

    cy.executeAccessibilityTests();
  });

  it("Should validate cross-site scripting attempt", () => {
    Logger.log("Testing with cross-site scripting attempt");
    whichSchoolNeedsHelp
      .withShortSchoolName("<script>window.alert('Hello World')</script>")
      .clickContinue()
      .hasValidation("We could not find a school matching your search criteria");

    cy.executeAccessibilityTests();

    // Reload page or go back to get past missing textfield after XSS blows it up!
    whichSchoolNeedsHelp.clickBack();

  });

  it("Should validate bash scripting attempt", () => {
    Logger.log("Testing with bash scripting attempt");
    whichSchoolNeedsHelp
      .withShortSchoolName('echo ${username}')
      .clickContinue()
      .hasValidation("We could not find any schools matching your search criteria");
      
      cy.executeAccessibilityTests();
  });
});
