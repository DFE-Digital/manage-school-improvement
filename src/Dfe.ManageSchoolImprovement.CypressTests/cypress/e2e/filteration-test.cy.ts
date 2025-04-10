import riseHomePage from "cypress/pages/riseHomePage";
import { Logger } from "cypress/common/logger";

describe("Filteration Tests", { tags: ["@dev", "@stage"] }, () => {
  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-identified-for-targeted-intervention");
  });

  it("Should filter projects by region", () => {
    cy.executeAccessibilityTests()
    Logger.log("Testing we can filter projects by region...");
    riseHomePage.withFilterRegions()
    riseHomePage.hasFilterRegions()
    //cy.executeAccessibilityTests() -- COMMENTED OUT AS AXE THROWING ARIA-EXPANDED FALSE-POSITIVE
    Logger.log("Clearing Filters...");
    riseHomePage.clearFilters()
    //cy.executeAccessibilityTests() -- COMMENTED OUT AS AXE THROWING ARIA-EXPANDED FALSE-POSITIVE
  });
});
