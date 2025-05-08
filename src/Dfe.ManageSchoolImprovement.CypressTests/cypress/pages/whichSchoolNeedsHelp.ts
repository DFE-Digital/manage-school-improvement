class WhichSchoolNeedsHelp {
  public hasHeader(header: string = "Select school"): this {

    cy.get("h1").contains(header);

    return this;
  }

  public withShortSchoolName(schoolShort: string): this {
    cy.getById("SearchQuery").eq(0).type(schoolShort, {parseSpecialCharSequences: false });

    return this;
  }

  public withLongSchoolName(schoolLong: string): this {
    cy.getById("SearchQuery__listbox").contains(schoolLong, { timeout: 5000 }).click();

    return this;
  }

  public clickContinue(): this {
    cy.contains("Continue").click();

    return this;
  }

  public clickBack(): this {
    cy.contains('Back').click();

    return this;
  }

  public hasValidation(valText: string): this {
    cy.get('#SearchQuery-error-link').contains(valText);
    cy.get('#SearchQuery-error').contains(valText);

    return this;
  }
}

const whichSchoolNeedsHelp = new WhichSchoolNeedsHelp();

export default whichSchoolNeedsHelp;
