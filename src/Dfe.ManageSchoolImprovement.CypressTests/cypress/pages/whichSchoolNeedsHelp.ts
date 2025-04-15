class WhichSchoolNeedsHelp {
  public hasHeader(header: string = "Select school"): this {

    cy.get("h1").contains(header);

    return this;
  }

  public withSchoolName(school: string = "Plym"): this {
    cy.getById("SearchQuery").typeFast(school);
    cy.contains("Plymouth Grove Primary School").click();

    return this;
  }

  public clickContinue(): this {
    cy.contains("Continue").click();

    return this;
  }
}

const whichSchoolNeedsHelp = new WhichSchoolNeedsHelp();

export default whichSchoolNeedsHelp;
