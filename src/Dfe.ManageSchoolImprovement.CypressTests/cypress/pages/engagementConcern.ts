class EngagementConcern {


    public engagementConcernPageDisplayed(): this {
        cy.url().should('include', '/engagement-concern');
        cy.contains('Page not found').should('not.exist');

        return this;
    }
}

const engagementConcern = new EngagementConcern();

export default engagementConcern;