class FundingHistory {
    public enterFundingHistory(): this {
        cy.getById("funding-type").type("Funding type");
        cy.getById("funding-amount").type("1000");
        cy.getById("financial-year-input").type("2023/24");
        cy.getById("funding-rounds").type("1");
        cy.contains("Save and continue").click();

        return this;
    }

    public confirmFundingHistory(): this { 
        cy.getById("funding-history-details-complete").click();
        cy.getById("save-and-continue-button").click();

        return this;
    }
}

const fundingHistory = new FundingHistory();
  
export default fundingHistory;
