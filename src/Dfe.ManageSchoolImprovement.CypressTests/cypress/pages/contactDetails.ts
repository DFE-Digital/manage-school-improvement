class ContactDetails {
    public enterRandomContactDetails(): this {
        const timestamp = Date.now().toString(36);
        const firstNameSuffix = timestamp.substring(0, 6);
        const emailSuffix = timestamp.substring(0, 7);

        const randomFirstName = 'First' + firstNameSuffix;
        const randomEmail = `test.${emailSuffix}@example.com`;
        const randomPhone = '079'+ timestamp.substring(0, 9).replaceAll(/[a-z]/g, '5');

        cy.getByName('name').type(randomFirstName);
        cy.getByName('email-address').type(randomEmail);
        cy.getByName('phone').type(randomPhone);

        return this;
    }

    public enterJobTitleForGovBody(jobTitle: string): this {
        cy.getById('JobTitle').should('be.visible')
            .type(jobTitle);

        return this;
    }

    public clickSaveAndReturnButton(): this {
        cy.url().should('include', '/add-contact-details');
        cy.getById('add-contact-button').contains('Save and return').click();

        return this;
    }

    public errorMessageForField(fieldName: string, errorMessage: string): this {
        cy.url().should('include', '/add-contact-details');
        cy.getById(fieldName).parent().find('.govuk-error-message').should('contain', errorMessage);

        return this;
    }

    public enterInvalidPhoneNumber(): this {
        cy.getByName('phone').clear().type('23232323');
       
        return this;
    }

    public editFullName(fullName: string): this {
        cy.getByName('name').clear().type(fullName);
         cy.getById('add-contact-button').contains('Save and return').click();

        return this;
    }
}

const contactDetails = new ContactDetails();

export default contactDetails;
