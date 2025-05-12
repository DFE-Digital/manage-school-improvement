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
}

const taskListActions = new TaskListActions();
export default taskListActions;
