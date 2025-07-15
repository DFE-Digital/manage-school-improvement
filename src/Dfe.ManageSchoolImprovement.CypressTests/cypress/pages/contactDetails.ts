class ContactDetails {

    public enterRandomContactDetails(): this {
        const randomFirstName = 'First' + Math.random().toString(36).substring(2, 8);
        const randomOrganisation = 'Org' + Math.random().toString(36).substring(2, 10);
        const randomEmail = `test.${Math.random().toString(36).substring(2, 8)}@example.com`;
        const randomPhone = '07' + Math.floor(Math.random() * 900000000 + 100000000);

        cy.getByName('name').type(randomFirstName);
        cy.getByName('organisation').type(randomOrganisation);
        cy.getByName('email-address').type(randomEmail);
        cy.getByName('phone').type(randomPhone);

        cy.getById('add-contact-button').click

        return this;
    }

    public clickAddContactButton(): this {
        cy.getById('add-contact-button').contains('Add contact').click();

        return this;
    }

    public errorMessageForField(fieldName: string, errorMessage: string): this {
        cy.url().should('include', '/add-contact-details');
        cy.getById(fieldName).parent().find('.govuk-error-message').should('contain', errorMessage)

        return this;
    }

    public enterInvalidPhoneNumber(): this {
        cy.getByName('phone').clear().type('23232323');
        this.clickAddContactButton()

        return this;
    }

    public editOrganisation(organisation: string): this {
        cy.getByName('organisation').clear().type(organisation);
        this.clickAddContactButton();

        return this;
    }
}

const contactDetails = new ContactDetails();

export default contactDetails;
