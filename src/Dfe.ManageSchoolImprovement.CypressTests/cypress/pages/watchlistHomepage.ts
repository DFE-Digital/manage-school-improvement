import { contains } from "cypress/types/jquery";


class WatchlistHomepage {


    public navigateToWatchlistHomepage() {
        cy.visit('/watchlist');
        return this;
    }

    public hasSchoolList() {
        cy.get('.govuk-table').should('exist');

        return this;
    }

    hasSchoolTableWithColumns(columns: string[]) {
        cy.get('.govuk-table').should('exist');
        columns.forEach(column => {
            cy.get('.govuk-table').contains(column).should('exist');
        });

        return this;

    }

    public hasTitle(title: string) {
        cy.get('h1').contains(title).should('exist');

        return this;
    }

    public clickAddSchoolToWatchlistButton() {
        cy.get('a').contains('Add school to watchlist').should('exist').click();

        return this;
    }


    public selectAndContinueAddSchoolToWatchlist(schoolName: string) {
        cy.get('h1').should('contain.text', 'Select a school');
        cy.getById('SearchQuery').first().type(schoolName);
        cy.getById("SearchQuery__listbox").contains(schoolName, { timeout: 5000 }).click({ force: true });
        cy.getByCyData('select-common-submitbutton').should('exist').click();

        return this;
    }

    public confirmSchoolDetails() {
        cy.get('h1').should('contain.text', 'Confirm school details');
        const keys = [
            'Name',
            'Date added',
            'Assigned to',
            'Supporting organisation',
            //  'Milestone',
            'Status'
        ];

        cy.get('.govuk-summary-list').should('exist');
        keys.forEach(key => {
            cy.get('.govuk-summary-list__key').contains(key).should('exist').parent().within(() => {
                cy.get('.govuk-summary-list__value').invoke('text').should('not.be.empty');
            });
        });

        return this;
    }

    public hasChangeSchoolLink() {
        cy.get('a').contains('Change school').should('exist');
        return this;
    }

    public clickSaveAndAddAnotherButton() {
        cy.get('button.govuk-button').contains('Save and add another').click();
        return this;
    }

    public clickSaveAndCompleteButton() {
        cy.getByCyData('save-and-finish-button').contains('Save and complete').click();


        return this;
    }

    public addedSchoolToWatchlistSuccessfully(schoolName: string = 'Plymouth Grove Primary School') {
        cy.get('table.govuk-table tbody tr').should('exist').contains('td', schoolName).parent('tr').within(() => {
            cy.get('td').each(($cell) => {
                cy.wrap($cell).invoke('text').should('not.be.empty');
            });
        });
        return this;
    }

    public hasResultsWithDetails() {

        cy.get('table.govuk-table tbody tr').should('have.length.greaterThan', 0);

        cy.get('table.govuk-table tbody tr').each(($row) => {
            cy.wrap($row).find('td').each(($cell) => {
                cy.wrap($cell).invoke('text').should('not.be.empty');
            });
        });
        return this;
    }

    public hasHelpfulLinks() {
        cy.get('h2').contains('Helpful links (links open in a new tab)');

        cy.get('a').then($links => {
            cy.contains('a', 'RISE delivery dashboard').should('exist');
            cy.contains('a', 'RISE data tables').should('exist');
            cy.contains('a', 'RISE monitoring report').should('exist');
            cy.contains('a', 'See inspection reports at Ofsted').should('exist');
            cy.contains('a', 'Get information about schools').should('exist');
            cy.contains('a', 'Find information about schools and trusts').should('exist').click();

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
        cy.get('.govuk-table tbody tr').first().find('td').first().find('a').click();

        cy.url().should('include', '/task-list')

        return this;
    }

    public clickRemoveLinkForSchool(schoolName: string = 'Plymouth Grove Primary School') {
        cy.getByCyData(`remove-link-${schoolName}`).should('exist').click();

        return this;
    }

    public removeSchoolFromWatchlist(schoolName: string = 'Plymouth Grove Primary School') {
        cy.get('h1').should('contain.text', 'Confirm the details below to remove the school from your watchlist.The school can still be found within the all schools list.');

        cy.get('a').contains('Cancel').should('exist');
        cy.getByCyData('remove-from-watchlist-button').should('exist').click();

        cy.get('h1').should('contain.text', 'Your school watchlist');
        cy.url().should('include', '/watchlist')

        return this;
    }

    public successfullyAddedMessage() {
        cy.get('.govuk-notification-banner').should('exist').should('contain.text', 'School added to watchlist');

        return this;
    }

    public successfullyRemovedMessage() {
        cy.get('.govuk-notification-banner').should('exist').should('contain.text', 'School removed from watchlist');

        return this;
    }


}
const watchlistHomepage = new WatchlistHomepage();

export default watchlistHomepage;