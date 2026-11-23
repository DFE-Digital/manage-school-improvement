class SupportingOrgType {

  public hasOrganisationTypeOptions(): this {
    cy.getById("support-organisation-type-school").should('exist');
    cy.getById("support-organisation-type-trust").should('exist');
    cy.getById("support-organisation-type-local-authority").should('exist');
    cy.getById("support-organisation-type-local-authority-traded-service").should('exist');
    cy.getById("support-organisation-type-federation-education-partnership").should('exist');

    return this;
  }

  public withShortSOName(schoolShort: string): this {
    cy.get("#search input[id='SearchQuery']")
      .should("be.visible")
      .type(schoolShort, { parseSpecialCharSequences: false });

    return this;
  }

  public withLongSOName(schoolLong: string): this {
    cy.get("#SearchQuery__listbox").should("be.visible");
    cy.contains("#SearchQuery__listbox li", schoolLong)
      .should("be.visible")
      .click();

    return this;
  }
  
  
}

const supportingOrgType = new SupportingOrgType();
  
export default supportingOrgType;