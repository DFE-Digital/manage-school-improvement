

class WatchlistHomepage {


    public navigateToWatchlistHomepage() {
        cy.visit('/watchlist');
        return this;
    }

    public hasSchoolList() {
        cy.get('[data-testid="school-list"]').should('exist');

        return this;
    }

    hasSchoolTableWithColumns(columns: string[]) {
        // Checks if the school table exists and has the specified columns
        cy.get('[data-testid="school-table"]').should('exist');
        columns.forEach(column => {
            cy.get('[data-testid="school-table"] th').contains(column).should('exist');
        });

        return this;

    }


    public hasTitle(title: string) {
        cy.get('h1').contains(title).should('exist');

        return this;
    }

    public hasResultsWithDetails() {

        cy.get('[data-testid="school-table"] tbody tr').should('have.length.greaterThan', 0);

        cy.get('[data-testid="school-table"] tbody tr').each(($row) => {
            cy.wrap($row).find('td').each(($cell) => {
                cy.wrap($cell).invoke('text').should('not.be.empty');
            });
        });
        return this;
    }

    public hasHelpfulLinks() {
        cy.get('[data-testid="helpful-links"]').should('exist').should('have.text', 'Helpful links (links open in a new tab)');

        cy.get('[data-testid="helpful-links"]').parent().within(() => {
            cy.contains('a', 'RISE delivery dashboard').should('exist');
            cy.contains('a', 'RISE data tables').should('exist');
            cy.contains('a', 'RISE monitoring report').should('exist');
            cy.contains('a', 'See inspection reports at Ofsted').should('exist');
            cy.contains('a', 'Get information about schools').should('exist');
            cy.contains('a', 'Find information about schools and trusts').should('exist');

        });
        return this;
    }


    public hasMessageWhenNoSchools(message: string) {
        cy.get('body').then($body => {
            if ($body.find('table[data-testid="school-table"]').length > 0) {
                cy.log('Schools are assigned to the user');
            } else {
                cy.get('p').should('exist').should('contain.text', message);
            }
        });
        return this;
    }

    public hasAssignedToValue() {
        cy.get('[data-testid="school-table"] tbody tr').each(($row) => {
            cy.wrap($row).find('td').eq(2).invoke('text').then((text) => {
                const trimmed = text.trim();
                expect(trimmed).to.eq('TestFirstName TestLastName');
            });
        });
        return this;

    }

    public sortByColumn(columnName: string) {
        cy.get('[data-testid="school-table"] th').contains(columnName).click();

        return this;
    }


    public clickFirstSchoolInList() {
        cy.get('[data-testid="school-table"] tbody tr').first().find('td').first().find('a').click();
        return this;
    }

    public removeFirstSchoolInList() {
        cy.get('[data-testid="school-table"] tbody tr').first().find('td').last().find('button').click();
        return this;
    }

}
const watchlistHomepage = new WatchlistHomepage();

export default watchlistHomepage;