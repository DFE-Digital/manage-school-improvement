class CaseStudy {


    public CaseStudyPageVisible(): this {
        cy.url().should('include', '/case-study');
        cy.contains('Page not found').should('not.exist');

        return this;
    }

}

const caseStudy = new CaseStudy();

export default caseStudy;