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

    public hasMostRecentOfstedReportSection(): this {
        cy.get('h2').contains('Most recent Ofsted report')
            .should('be.visible');

        return this;
    }

    public hasPreviousOfstedReportSection(): this {
        cy.get('h2').contains('Previous Ofsted report')
            .should('be.visible');

        return this;
    }

    public checkAllFieldsNotEmpty(): this {
        const expectedFields = [
            'Date of last inspection',
            'Quality of education',
            'Behaviour and attitudes',
            'Personal development',
            'Leadership and management',
            'Date of last inspection',
            'Quality of education',
            'Behaviour and attitudes',
            'Personal development',
            'Leadership and management',
        ];

        expectedFields.forEach(fieldName => {
            cy.get('dl.govuk-summary-list')
                .find('dt.govuk-summary-list__key')
                .contains(fieldName)
                .closest('.govuk-summary-list__row')
                .find('dd.govuk-summary-list__value')
                .should('exist')
                .and('not.be.empty');
        });

        cy.get('.govuk-summary-card__content').each(($el, index) => {
            cy.wrap($el).within(() => {
                cy.get('dd.govuk-summary-list__value').each($el => {
                    cy.wrap($el)
                        .should('exist')
                        .and('not.be.empty');
                });
            });
        })
        return this;
    }

    public expandSectionAndCheckText(): this {
        cy.get('.govuk-details').click();
        cy.get('.govuk-details__text')
            .should('be.visible')
            .and('not.be.empty');

        return this;
    }

}

const ofstedReports = new OfstedReports();

export default ofstedReports;