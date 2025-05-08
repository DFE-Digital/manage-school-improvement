class CheckSchoolDetails {
  public hasHeader(header: string = "Check school details"): this {

    cy.get("h1").contains(header);

    return this;
  }

  public hasSchoolName(schoolLong: string): this {

    cy.get('[data-cy="school-name"]').contains(schoolLong);

    return this;
  }

  public hasURN(URN: string): this {

    cy.get(".govuk-summary-list__value").eq(1).contains(URN);

    return this;
  }

  public hasLocalAuthority(localAuthority: string): this {

    cy.get(".govuk-summary-list__value").eq(3).contains(localAuthority);

    return this;
  }

  public hasSchoolType(schoolType: string): this {

    cy.get(".govuk-summary-list__value").eq(5).contains(schoolType);

    return this;
  }

  public hasFaithSchool(faithSchool: string): this {

    cy.get(".govuk-summary-list__value").eq(7).contains(faithSchool);

    return this;
  }

  public hasOfstedRating(ofstedRating: string): this {

    cy.get(".govuk-summary-list__value").eq(9).contains(ofstedRating);

    return this;
  }

  public hasLastInspection(lastInspection: string): this {

    cy.get(".govuk-summary-list__value").eq(11).contains(lastInspection);

    return this;
  }

  public hasPFI(pfi: string): this {

    cy.get(".govuk-summary-list__value").eq(13).contains(pfi);

    return this;
  }

  public clickContinue(): this {
    cy.contains("Continue").click();

    return this;
  }
}

const checkSchoolDetails = new CheckSchoolDetails();

export default checkSchoolDetails;
