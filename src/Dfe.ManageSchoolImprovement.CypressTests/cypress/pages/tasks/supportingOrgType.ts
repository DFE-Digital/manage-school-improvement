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
    cy.getById("SearchQuery").eq(0).type(schoolShort, {parseSpecialCharSequences: false });

    return this;
  }

    public withLongSOName(schoolLong: string): this {
    cy.getById("SearchQuery__listbox").contains(schoolLong, { timeout: 5000 }).click();

    return this;
  }
  
  
}

const supportingOrgType = new SupportingOrgType();
  
export default supportingOrgType;