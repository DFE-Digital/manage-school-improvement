class AboutTheSchool {
  public clickContinue(): this {
    cy.contains("Continue").click();

    return this;
  }

  public deleteSchool(): this {
    cy.contains("Delete school").click({ force: true });

    return this;
  }
}

const aboutTheSchool = new AboutTheSchool();

export default aboutTheSchool;
