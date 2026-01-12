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

    public hasDefaultContactCardsVisible(): this {
        cy.get('h3').should('contain', 'School contacts');
        cy.get('.govuk-summary-card').should('have.length.greaterThan', 0);
        cy.get('.govuk-summary-card__title').first().should('not.be.empty');

        return this;
    }

    public detailsVisibleInContactCard(): this {
        cy.get('.govuk-summary-card__content dt').each(($el) => {
            cy.wrap($el).next('dd').should('not.be.empty');
        });

        return this;
    }

    public hasOrganisationVisible(): this {
        cy.contains('label', 'School').should('be.visible');
        cy.contains('label', 'Supporting organisation').should('be.visible');
        cy.contains('label', 'Governance bodies').should('be.visible');

        return this;
    }

    public selectOrganisation(organisation: string): this {
        cy.getById(organisation).check();

        return this;
    }

    public selectSoOrganisation(sOorganisation: string): this {
        cy.getById(sOorganisation).check();

        return this;
    }

    public hasSchoolRolesVisible(): this {
        cy.contains('label', 'Headteacher (permanent)').should('be.visible');
        cy.contains('label', 'Headteacher (interim)').should('be.visible');
        cy.contains('label', 'Chair of governors').should('be.visible');
        cy.contains('label', 'Other job title').should('be.visible');

        return this;
    }

    public hasSORolesVisible(): this {
        cy.contains('label', 'Accounting officer').should('be.visible');
        cy.contains('label', 'Headteacher').should('be.visible');
        cy.contains('label', 'Other job title').should('be.visible');

        return this;
    }
    public hasGovBodiesRolesVisible(): this {
        cy.contains('label', 'Trust').should('be.visible');
        cy.contains('label', 'Local authority').should('be.visible');
        cy.contains('label', 'Diocese').should('be.visible');
        cy.contains('label', 'Foundation').should('be.visible');
        cy.contains('label', 'Federation').should('be.visible');
        cy.contains('label', 'Other body').should('be.visible');

        return this;
    }

    public selectRoleandEnterDetails(role: string): this {
        cy.getById(role).check();
        if (role === 'other-body') {
            cy.getById('organisationTypeSubCategoryOther').should('be.visible')
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
        cy.get('.govuk-summary-card').filter((index, card) => {
            return Cypress.$(card).find('a:contains("Change")').length > 0;
        }).first().within(() => {
            cy.contains('a', 'Change').click();
        });

        return this;
    }

    public hasContactAddedSuccessMessage(string): this {
        cy.url().should('include', '/contacts');
        cy.get('.govuk-notification-banner--success').contains(string).should('be.visible');

        return this;
    }

    public orgErrorMessage(errorMessage: string): this {
        cy.url().should('include', '/add-contact-organisation');
        cy.getByCyData('error-summary').should('contain', errorMessage);

        return this;
    }

    public roleErrorMessage(errorMessage: string): this {
        cy.url().should('include', '/add-contact');
        cy.getById('-hint-error-link').should('contain', errorMessage);

        return this;
    }


    public roleTextFieldMissingErrorMessage(errorMessage: string): this {
        cy.url().should('include', '/add-contact');
        cy.getById('organisationTypeSubCategoryOther-error-link').should('contain', errorMessage);

        return this;
    }

    public editContactPageVisible(): this {
        cy.get('h1').should('contain', 'Edit contact');

        return this;
    }

}

const contacts = new Contacts();

export default contacts;
