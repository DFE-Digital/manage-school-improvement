class ImprovementPlan {

    hasHeading(expectedHeading: string): this {
        cy.get('h2').should('contain.text', expectedHeading);
        return this;
    }
}

const improvementPlan = new ImprovementPlan();

export default improvementPlan;
