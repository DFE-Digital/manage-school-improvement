class SupportingOrgType {

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