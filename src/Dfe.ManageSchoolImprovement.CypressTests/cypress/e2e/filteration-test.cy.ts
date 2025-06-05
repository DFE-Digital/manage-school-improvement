import homePage from "cypress/pages/homePage";
import { Logger } from "cypress/common/logger";

describe("Filteration Tests", { tags: ["@dev", "@stage"] }, () => {
  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-identified-for-targeted-intervention");
  });

  it("Should filter projects by region", () => {
    cy.executeAccessibilityTests()
    Logger.log("Testing we can filter projects by region...");
    homePage.withFilterRegions()
    homePage.hasFilterRegions()
    Logger.log("Clearing Filters...");
    homePage.clearFilters()
  });

  it("Should filter projects by school name", () => {
    cy.executeAccessibilityTests()
    Logger.log("Testing we can filter projects by inputting schoolname...");
    homePage.withProjectFilter("Outwood Academy Shafton")
    homePage.hasSchoolName("Outwood Academy Shafton")
    Logger.log("Clearing Filters...");
    homePage.clearFilters()
  });
});
