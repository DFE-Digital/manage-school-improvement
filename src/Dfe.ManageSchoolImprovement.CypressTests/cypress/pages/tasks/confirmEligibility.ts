class ConfirmEligibility {
    public hasHeader(header: string): this {
        cy.get("h1").contains(header);
    
        return this;
      }

    public selectYesAndContinue(): this {
        cy.get('[data-cy="select-radio-yes"]').click();
        cy.get('[data-cy="select-common-submitbutton"]').click();

        return this;
    }
  }
  
  const confirmEligibility = new ConfirmEligibility();
  
  export default confirmEligibility;