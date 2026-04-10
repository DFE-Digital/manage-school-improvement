class TaskListActions {
    public hasHeader(header: string): this {
        cy.get("h1").contains(header);

        return this;
    }

    public selectNoAndContinue(): this {
        cy.get('[data-cy="select-radio-no"]').click();
        cy.get('[data-cy="select-common-submitbutton"]').click();

        return this;
    }

    public selectYesAndContinue(): this {
        cy.get('[data-cy="select-radio-yes"]').click();
        cy.get('[data-cy="select-common-submitbutton"]').click();

        return this;
    }

    public selectButtonOrCheckbox(id: string): this {
        cy.getById(id).click();

        return this;
    }

    public checkCheckbox(id: string): this {
        cy.getById(id).check();

        return this;
    }

    public clickButton(value: string): this {
        cy.get(`[value="${value}"]`).click();

        return this;
    }

    public linkExists(linkText: string): this {
        cy.get('a').contains(linkText).should('exist');

        return this;
    }

    public enterDate(id: string, day: string, month: string, year: string): this {
        cy.getById(`${id}-day`).type(day);
        cy.getById(`${id}-month`).type(month);
        cy.getById(`${id}-year`).type(year);

        return this;
    }

    public enterText(id: string, text: string): this {
        cy.getById(id).clear();
        cy.getById(id).type(text, { parseSpecialCharSequences: false });

        return this;
    }

    public hasValidation(valText: string, id: string): this {
        cy.getById(id).contains(valText);

        return this;
    }

    public clearDateInput(id: string): this {
        cy.getById(`${id}-day`).clear();
        cy.getById(`${id}-month`).clear();
        cy.getById(`${id}-year`).clear();

        return this;
    }

    public clearInput(id: string): this {
        cy.getById(id).clear();

        return this;
    }

    public selectSaveAndAddAnotherObjectiveButton(): this {
        cy.get('[value="add-another"]').click();

        return this;

    }

    public confirmSupportingOrganisationDetails(): this {
        cy.title().should('eq', 'Confirm supporting organisation details - Manage school improvement');
        cy.get('.govuk-summary-card').should('exist');
        cy.get('.govuk-summary-card__actions a').contains('Change').should('exist');
        cy.get('.govuk-summary-card__content .govuk-summary-list__row').each(($row) => {
            cy.wrap($row).find('.govuk-summary-list__value').should('not.be.empty');
        });
        return this;
    }

    public peferredSupportingOrganisationDetails(): this {
        cy.title().should('eq', 'Choose preferred supporting organisation - Manage school improvement');
        cy.get('.govuk-summary-card').should('exist');
        cy.get('.govuk-summary-card__content .govuk-summary-list__row').each(($row) => {
            cy.wrap($row).find('.govuk-summary-list__value').should('not.be.empty');
        });
        cy.getByCyData('return-btn').should('contains', 'Return to task list');
        cy.getByCyData('change-supporting-org-btn').should('contains', 'Change supporting rganisation');
        cy.getByCyData('view-contacts-link').should('contains', 'Return to task list');

        return this;
    }

    public hasSOSummeryList(): this {
        cy.get('.govuk-summary-list').should('exist');
        cy.get('.govuk-summary-list__row').each(($row) => {
            cy.wrap($row).find('.govuk-summary-list__value').should('not.be.empty');
        });

        return this;
    }

    public hasDisplayedImportantBanner(infoText: string): this {
        cy.get('.govuk-notification-banner__content').invoke('text').then((text) => {
            const normalizedText = text.replaceAll(/\s+/g, ' ').trim();
            expect(normalizedText).to.contain(infoText);
        });

        return this;
    }

    public hasDisplayedImportantInfo(infoText: string): this {
        cy.get('.govuk-inset-text').invoke('text').then((text) => {
            const normalizedText = text.replaceAll(/\s+/g, ' ').trim();
            expect(normalizedText).to.contain(infoText);
        });

        return this;
    }

    public hasDeleteObjectiveLink(): this {
        cy.get('h1').should('contain.text', 'Review objectives ');
        cy.get('a.govuk-link').contains('Delete')
            .should('exist');

        return this;
    }

    public deleteObjectiveSuccessfully(): this {
        cy.get('a.govuk-link').contains('Delete').click();
        cy.get('h1').should('contain.text', 'Are you sure you want to delete this objective?');
        cy.get('.govuk-button.govuk-button--warning').contains('Delete objective').click();
        cy.get('.govuk-notification-banner__content').should('contain.text', 'Objective deleted');

        return this;
    }

    public addAndDeleteObjective(): this {
        cy.getByCyData('page-heading').invoke('text').then((headingText) => {
            if (headingText.trim() === 'Review objectives') {
                this.deleteObjectiveSuccessfully();

                cy.getByCyData('add-another-objective-button').click();
            } else {
                this.addObjective();
                cy.getByCyData('objective-summary-change-link').should('contain.text', 'Delete');
            }
        });
        return this;
    }

    public addObjective(): this {
        cy.url().should('include', '/improvement-plan/select-an-area-of-improvement');        
        cy.getById('quality-of-education').click();
        cy.getById('save-and-continue-button').click();
        cy.getByCyData('objective-details-textarea').type('New objective details');
        cy.getByCyData('save-and-finish-button').click();

        return this;
    }

    public eligibilityBannerExists(): this {
        cy.get('.govuk-notification-banner').should('exist');
        cy.get('.govuk-notification-banner__heading').should('contain.text', 'If the eligibility of the school has changed');

        return this;
    }

}

const taskListActions = new TaskListActions();
export default taskListActions;
