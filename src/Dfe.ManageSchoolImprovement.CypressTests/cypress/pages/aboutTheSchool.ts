class AboutTheSchool {

  public schoolOverivewSectionVisible(): this {
    cy.url().should('include', '/about-the-school');
    cy.contains('Page not found').should('not.exist');

    return this;
  }

  public hasSchoolOverviewSection(): this {
    cy.get('h2').should('contain', 'School overview');

    return this;
  }
  public checkAllFieldsNotEmpty(): this {
    const expectedFields = [
      'Previous URN',
      'Region',
      'Local authority',
      'School type',
      'School phase',
      'Religious character',
      'Diocese',
      'Number on roll (NOR)'
    ];

    for(const fieldName of expectedFields) {
      cy.get('dl.govuk-summary-list')
        .find('dt.govuk-summary-list__key')
        .contains(fieldName)
        .closest('.govuk-summary-list__row')
        .find('dd.govuk-summary-list__value')
        .should('exist')
        .and('not.be.empty');
    }

    cy.get('.govuk-summary-card__content').within(() => {
      cy.get('dd.govuk-summary-list__value').each($el => {
        cy.wrap($el)
          .should('exist')
          .and('not.be.empty');
      });
    });
    
    return this;
  }

  public expandSectionAndCheckText(): this {
    cy.get('.govuk-details').click();
    cy.get('.govuk-details__text')
      .should('be.visible')
      .and('not.be.empty');

    return this;
  }
}

const aboutTheSchool = new AboutTheSchool();

export default aboutTheSchool;
