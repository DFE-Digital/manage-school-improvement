class Pagination {

    public hasPagination(): this {
        cy.get('#pagination-label').should('be.visible');

        return this;
    }

    public navigateToTheNextPage(): this {
        cy.get('.moj-pagination__item--next').should('be.visible')
            .click();
        cy.url().should('contains', 'currentPage=2')
        return this;
    }

    public navigateToThePreviousPage(): this {
        cy.get('.moj-pagination__item--prev').should('be.visible')
            .click();
        cy.url().should('contains', 'currentPage=1')
        return this;
    }
}

const pagination = new Pagination();

export default pagination;
