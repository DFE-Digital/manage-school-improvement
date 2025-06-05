export class PaginationComponent {
    constructor(private prefix: string = "") {}

    
    public hasPagination(): this {
     cy.get('#pagination-label').should('be.visible');
    
    return this;
  }

    public navigateToTheNextPage(): this {
       cy.get('.moj-pagination__item--next').should('be.visible')
         .click();
        cy.url().should('contains','currentPage=2')
        return this;
    }

    public navigateToThePreviousPage(): this {
  cy.get('.moj-pagination__item--prev').should('be.visible')
         .click();
        cy.url().should('contains','currentPage=1')
        return this;
    }

    public goToPage(pageNumber: string) {
        cy.getByTestId(`${this.prefix}page-${pageNumber}`).click();

        return this;
    }

    public isCurrentPage(pageNumber: string): this {
        // Used to check that we have navigated to the next page with ajax
        cy.getByTestId(`${this.prefix}page-${pageNumber}`)
            .parent()
            .should("have.class", "govuk-pagination__item--current");

        return this;
    }
}

const paginationComponent = new PaginationComponent();

export default paginationComponent;
