class Notes {

    public hasHeading(value: string): this {
        cy.get('h2').should('contain', value);

        return this;
    }
    public hasAddNoteButton(): this {
        cy.get('[role="button"]').should("be.visible");

        return this;
    }
}

const notes = new Notes();

export default notes;