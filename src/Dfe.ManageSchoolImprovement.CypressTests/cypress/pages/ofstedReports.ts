class OfstedReports {


    public hasOfstedReport(value: string): this {
        cy.get('h2').should("be.visible");

        return this;
    }

    public ofstedReportsPageVisible(): this {
        cy.url().should('include', '/ofsted-reports');
        cy.contains('Page not found').should('not.exist');

        return this;
    }

}

const ofstedReports = new OfstedReports();

export default ofstedReports;