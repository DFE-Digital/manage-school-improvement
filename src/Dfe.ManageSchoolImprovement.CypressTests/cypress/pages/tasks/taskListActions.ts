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
        cy.getById(id).type(text, {parseSpecialCharSequences: false });

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

    public hasDisplayedImportantInfo(infoText: string): this {
         cy.get('.govuk-notification-banner__content').invoke('text').then((text) => {
            const normalizedText = text.replace(/\s+/g, ' ').trim();
            expect(normalizedText).to.contain(infoText);
        });

        return this;
    }

}

const taskListActions = new TaskListActions();
export default taskListActions;
