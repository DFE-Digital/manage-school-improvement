class AboutTheSchool {
  public deleteSchool(): this {
    cy.contains("Delete school").click({ force: true });

    return this;
  }
}

const aboutTheSchool = new AboutTheSchool();

export default aboutTheSchool;
