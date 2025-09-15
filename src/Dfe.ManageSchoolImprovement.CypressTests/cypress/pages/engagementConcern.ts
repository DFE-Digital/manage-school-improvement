class EngagementConcern {

    public engagementConcernPageDisplayed(): this {
        cy.url().should('include', '/engagement-concern');
        cy.contains('Page not found').should('not.exist');

        return this;
    }

    public clickRecordEngagementConcern(): this {
        cy.get('body').then($body => {
            const buttonExists = $body.find('[role="button"]:contains("Record engagement concern")').length > 0;

            if (buttonExists) {
                cy.get('[role="button"]').contains('Record engagement concern').click({ force: true });
                cy.log('Clicked Record engagement concern button');
            } else {
                expect(buttonExists, 'Engagement concern already existing').to.be.true;
            }
        });

        return this;
    }

    public hasTitle(expectedTitle: string): this {
        cy.title().should('eq', expectedTitle);

        return this;
    }

    public clickChangeLinkForIEB(linkText: string): this {
        cy.getByCyData('ieb-created-change-link').should('contain', linkText).click();

        return this;
    }

    public clickEscalateLink(): this {
        cy.getByCyData('engagement-concern-escalate-link').click();
        cy.url().should('include', '/escalate-engagement-concern');

        return this;
    }

    public recordConcernPageIsVisible(): this {
        cy.url().should('include', '/record-engagement-concern');
        cy.title().should('eq', 'Record engagement concern - Manage school improvement');

        return this;
    }

    public clickRecordUserOfInformationPowersButton(): this {
        cy.get('body').then($body => {
            const buttonExists = $body.find('[role="button"]:contains("Record use of information powers")').length > 0;

            if (buttonExists) {
                cy.get('[role="button"]').contains('Record use of information powers').click({ force: true });
                cy.log('Clicked Record use of information powers');
            } else {
                expect(buttonExists, 'Use of Information powers already existing').to.be.true;
            }
        });

        return this;
    }

    public clickCancel(): this {
        cy.get('a').contains('Cancel').click();

        return this;
    }

    public hasRecordUseOfInterimExecutiveBoardButton(): this {
        cy.get('[role="button"]').should("be.visible");

        return this;
    }

    public clickRecordUseOfInterimExecutiveBoard(): this {
        cy.get('body').then($body => {
            const buttonExists = $body.find('[role="button"]:contains("Record use of interim executive board")').length > 0;

            if (buttonExists) {
                cy.get('[role="button"]').contains('Record use of interim executive board').click({ force: true });
                cy.log('Clicked Record use of interim executive board button');
            } else {
                expect(buttonExists, 'Use of interim executive board already existing').to.be.true;
            }
        });

        return this;
    }   

    public showHowToCreateIEBSection(): this {
        cy.get('.govuk-details__summary-text').contains('How to create interim executive boards').click();
        cy.get('.govuk-details__text')
            .should('be.visible')
            .and('not.be.empty');

        return this;
    }

    public checkCheckbox(id: string): this {
        cy.getById(id).check();

        return this;
    }


    public enterText(id: string, text: string): this {
        cy.getById(id).clear().type(text);

        return this;
    }

    public enterDate(dateFieldId: string, day: string, month: string, year: string): this {
        cy.getById(`${dateFieldId}-day`).clear().type(day);
        cy.getById(`${dateFieldId}-month`).clear().type(month);
        cy.getById(`${dateFieldId}-year`).clear().type(year);

        return this;
    }

    public clickButton(text): this {
        cy.get('[type="submit"]').should('contain.text', text)
            .click();

        return this;
    }

    public errorMessage(id: string, message: string): this {
        cy.getById(id).should('contain', message);

        return this;
    }

    public hasSuccessNotification(expectedMessage: string): this {
        cy.get('.govuk-notification-banner--success')
            .should('be.visible')
            .find('.govuk-notification-banner__content')
            .should('contain', expectedMessage);

        return this;
    }

    public hasFieldsNotEmpty(): this {
        cy.get('.govuk-summary-list__value').each(($el) => {
            cy.wrap($el).should('not.be.empty');
        });

        return this;
    }

    public clickViewEngagementConcern(): this {
        cy.getByCyData('escalate-confirmation-btn').click();

        return this;
    }

    public hasEngagementConcernChangeLink(linkText: string): this {
        cy.get('[data-cy="engagement-concern-change-link"]')
            .should('be.visible')
            .contains(linkText);

        return this;
    }
     public hasRecordedNotification(expectedMessage: string): this {
        cy.get('.govuk-panel')
            .should('be.visible')
            .should('contain', expectedMessage);

        return this;
    }

    public clickEngagementConcernChangeLink(): this {
        cy.getByCyData('engagement-concern-change-link').click();
        cy.url().should('include', '/change-engagement-concern');

        return this;

    }

    public hasInformationPowersChangeLink(linkText: string): this {
        cy.get('[data-cy="information-powers-change-link"]')
            .should('be.visible')
            .contains(linkText);

        return this;
    }

    public clickInformationPowersChangeLink(): this {
        cy.getByCyData('information-powers-change-link').click();
        cy.url().should('include', '/change-use-of-information-powers');

        return this;

    }

    public hasEscalateLink(linkText: string): this {
        cy.getByCyData('engagement-concern-escalate-link')
            .should('be.visible')
            .contains(linkText)

        return this;
    }

    public unCheckCheckbox(id: string): this {
        cy.getById(id).uncheck();

        return this;
    }
}

const engagementConcern = new EngagementConcern();
export default engagementConcern;
