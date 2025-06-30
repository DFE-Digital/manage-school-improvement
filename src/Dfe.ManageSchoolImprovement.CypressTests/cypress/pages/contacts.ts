class Contacts {


    public hasAddContactButton(): this {
         cy.get('[role="button"]').should("be.visible");
        return this;
    }

}

const contacts = new Contacts();

export default contacts;