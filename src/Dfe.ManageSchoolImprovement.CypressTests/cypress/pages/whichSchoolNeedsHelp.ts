class WhichSchoolNeedsHelp {
  public hasHeader(header: string = "Which school needs help?"): this {

    cy.get("h1").contains(header);

    return this;
  }

  public withSchoolName(school: string = "Plymouth Grove Primary School"): this {
    cy.getById("SearchQuery").typeFast(school);
    cy.contains(school).click();

    return this;
  }

  public clickContinue(): this {
    cy.contains("Continue").click();

    return this;
  }
}

const whichSchoolNeedsHelp = new WhichSchoolNeedsHelp();

export default whichSchoolNeedsHelp;