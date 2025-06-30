class AboutTheSchool {

  public schoolOverivewSectionVisible(): this {
        cy.url().should('include', '/about-the-school');
    cy.contains('Page not found').should('not.exist');

    return this;
  }
}

const aboutTheSchool = new AboutTheSchool();

export default aboutTheSchool;
