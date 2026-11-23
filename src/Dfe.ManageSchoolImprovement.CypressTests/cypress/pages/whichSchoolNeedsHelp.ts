class WhichSchoolNeedsHelp {
  public hasHeader(header: string = "Select school"): this {

    cy.get("h1").contains(header);

    return this;
  }

  public withShortSchoolName(schoolShort: string): this {
    cy.get("#search input[id='SearchQuery']")
      .should("be.visible")
      .type(schoolShort, { parseSpecialCharSequences: false });

    return this;
  }

  public withLongSchoolName(schoolLong: string): this {
    cy.get("#SearchQuery__listbox").should("be.visible");
    cy.contains("#SearchQuery__listbox li", schoolLong)
      .should("be.visible")
      .click();

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
