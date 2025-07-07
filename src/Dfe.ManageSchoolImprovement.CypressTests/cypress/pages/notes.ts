import { log } from "console";
import { Logger } from "cypress/common/logger";

class Notes {
    public hasHeading(value: string): this {
        cy.get('h2').should('contain', value);

        return this;
    }
    public hasAddNoteButton(): this {
        cy.get('[role="button"]').should("be.visible");

        return this;
    }
    public clickAddNote(): this {
        cy.get('[role="button"]').contains('Add note').click();

        return this;
    }
    public enterNote(note: string): this {
        cy.get('[name="project-note-body"]').clear().type(note);

        return this;
    }
    public saveNote(): this {
        cy.get('[type="submit"]').contains('Save note').click();

        return this;
    }
    public editFirstNote(newNote: string): this {
        cy.get('.app-notes__note').first().then($section => {
            if ($section.find('a:contains("Edit note")').length > 0) {
                cy.wrap($section).within(() => {
                    cy.get('a').contains('Edit note').click();
                    cy.get('[name="project-note-body"]')
                        .should('be.visible')
                        .clear()
                        .type(newNote);
                    cy.get('[type="submit"]')
                        .contains('Save note')
                        .should('be.visible')
                        .click();                   
                })
                this.hasSuccessNotification();
            } else {
                Logger.log('Edit note link is not available in the first note. The project may not be assigned to the user.');
            }
        });

        return this;
    }

    public hasSuccessNotification(): this {
        cy.get('.govuk-notification-banner__content').contains('Note added').should("be.visible");

        return this;
    }

    public getFirstNote() {
        return cy.get('app-notes').first();
    }
}

const notes = new Notes();

export default notes;