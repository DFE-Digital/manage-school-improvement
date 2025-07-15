class Contacts {


    public hasAddContactButton(): this {
        cy.get('[role="button"]').should("be.visible");
        return this;
    }

    public clickAddContact(): this {
        cy.get('[role="button"]').contains('Add contact').click();
        cy.url().should('include', '/add-contact');

        return this;
    }

    public hasRolesVisible(): this {
        cy.contains('label', 'Director of Education').should('be.visible');
        cy.contains('label', 'Headteacher').should('be.visible');
        cy.contains('label', 'Chair of governors').should('be.visible');
        cy.contains('label', 'Trust relationship manager').should('be.visible');
        cy.contains('label', 'Trust CEO').should('be.visible');
        cy.contains('label', 'Trust accounting officer').should('be.visible');
        cy.contains('label', 'Other role').should('be.visible');

        return this;
    }

    public selectRole(role: string): this {
        cy.getById(role).check();
        if (role === 'other-role') {
            cy.get('#OtherRole').should('be.visible')
                .type('Tester');
        }

        return this;
    }

    public clickContinue(): this {
        cy.get('[type="submit"]').contains('Continue').click();

        return this;
    }

    public clickChangeButton(): this {
        cy.url().should('include', '/contacts')
        cy.get('[href^="/edit-contact/"]').first().click()

        return this;
    }

    public hasContactAddedSuccessMessage(string): this {
        cy.url().should('include', '/contacts');
        cy.get('.govuk-notification-banner--success').contains(string).should('be.visible');

        return this;
    }

}

const contacts = new Contacts();

export default contacts;
