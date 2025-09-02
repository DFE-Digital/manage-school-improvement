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
}

const taskListActions = new TaskListActions();
export default taskListActions;
