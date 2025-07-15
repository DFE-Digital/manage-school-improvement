class ContactDetails {
    public enterRandomContactDetails(): this {
        const timestamp = Date.now().toString(36);
        const firstNameSuffix = timestamp.substring(0, 6);
        const orgSuffix = timestamp.substring(0, 8);
        const emailSuffix = timestamp.substring(0, 7);

        const randomFirstName = 'First' + firstNameSuffix;
        const randomOrganisation = 'Org' + orgSuffix;
        const randomEmail = `test.${emailSuffix}@example.com`;
        const randomPhone = '079'+ timestamp.substring(0, 9).replace(/[a-z]/g, '5');

        cy.getByName('name').type(randomFirstName);
        cy.getByName('organisation').type(randomOrganisation);
        cy.getByName('email-address').type(randomEmail);
        cy.getByName('phone').type(randomPhone);

        return this;
    }

    public clickAddContactButton(): this {
        cy.getById('add-contact-button').contains('Add contact').click();

        return this;
    }

    public errorMessageForField(fieldName: string, errorMessage: string): this {
        cy.url().should('include', '/add-contact-details');
        cy.getById(fieldName).parent().find('.govuk-error-message').should('contain', errorMessage);

        return this;
    }

    public enterInvalidPhoneNumber(): this {
        cy.getByName('phone').clear().type('23232323');
        this.clickAddContactButton();

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
