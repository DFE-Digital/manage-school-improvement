import riseHomePage from "cypress/pages/riseHomePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";

describe("Which school needs help negative tests", () => {
  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-identified-for-targeted-intervention");
  });

  it("Should be able to validate Which school needs help page", () => {
    riseHomePage.AddSchool();

    cy.executeAccessibilityTests();

    // ADD NEG-TEST FOR ADDING A NULLSTRING SCHOOL
    whichSchoolNeedsHelp
      .withShortSchoolName(" ")
      .clickContinue()
      .hasValidation("Enter the school name or URN");

    cy.executeAccessibilityTests();

    // ADD NEG-TEST FOR ADDING INVALID SCHOOLNAME
    whichSchoolNeedsHelp
      .withShortSchoolName("POTATO")
      .clickContinue()
      .hasValidation("We could not find any schools matching your search criteria");
  
      cy.executeAccessibilityTests();

    //GO BACK AND COME BACK IN AGAIN TO GET ROUND AUTOCOMPLETE FAILING TO APPEAR
    whichSchoolNeedsHelp.clickBack();
    riseHomePage.AddSchool();

    // ADD NEG-TEST FOR ADDING ALREADY EXISTING SCHOOL
    whichSchoolNeedsHelp
      .withShortSchoolName("Inta")
      .withLongSchoolName("Intake Primary School")
      .clickContinue()
      .hasValidation("This school is already getting support, choose a different school");

   whichSchoolNeedsHelp.clickBack();
   riseHomePage.AddSchool();

    // ADD NEG-TEST FOR SQL INJECTION ATTEMPT
    whichSchoolNeedsHelp
      .withShortSchoolName("' OR 1=1--'")
      .clickContinue()
      .hasValidation("We could not find any schools matching your search criteria");

    cy.executeAccessibilityTests();

    // ADD NEG-TEST FOR CROSS-SITE SCRIPTING ATTEMPT
    whichSchoolNeedsHelp
      .withShortSchoolName("<script>window.alert('Hello World')</script>")
      .clickContinue()
      .hasValidation("We could not find a school matching your search criteria");
  
      cy.executeAccessibilityTests();

    // RELOAD PAGE OR GO BACK TO GET PAST MISSING TEXTFIELD AFTER XSS BLOWS IT UP!
    whichSchoolNeedsHelp.clickBack();
    riseHomePage.AddSchool();

    // ADD NEG-TEST FOR BASH SCRIPTING ATTEMPT
    whichSchoolNeedsHelp
      .withShortSchoolName('echo ${username}')
      .clickContinue()
      .hasValidation("We could not find any schools matching your search criteria");

    cy.executeAccessibilityTests();

  });
});