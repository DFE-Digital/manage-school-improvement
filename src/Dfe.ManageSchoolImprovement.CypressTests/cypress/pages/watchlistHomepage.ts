

class WatchlistHomepage {


	public hasSchoolList() {
		// Checks if the school list element exists on the page
		cy.get('[data-testid="school-list"]').should('exist');

        return this; // Allows for method chaining
	}

	hasSchoolTableWithColumns(columns: string[]) {
		// Checks if the school table exists and has the specified columns
		cy.get('[data-testid="school-table"]').should('exist');
		columns.forEach(column => {
			cy.get('[data-testid="school-table"] th').contains(column).should('exist');
		});

        return this; // Allows for method chaining

	}

       
	public hasTitle(title: string) {
		// Checks if the page contains the specified title
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
}
const watchlistHomepage = new WatchlistHomepage();

export default watchlistHomepage;