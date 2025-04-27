class TaskList {
  public hasHeader(header: string): this {
    cy.get("h1").contains(header);

    return this;
  }

  // Assert 'Date Added' matches the expected value
  public hasDateAdded(date: string): this {
    cy.get('.govuk-summary-list__value').eq(0).should("contain.text", date);
    return this; // Return this to allow method chaining
  }

  // Assert 'Date of last OFSTED inspection' matches the expected value
  public hasInspectionDate(date: string): this {
    cy.get('.govuk-summary-list__value').eq(1).should("contain.text", date);
    return this; // Return this to allow method chaining
  }

  // Assert 'Quality of education' matches the expected value
  public hasQualityOfEducation(value: string): this {
    cy.get('.govuk-summary-list__value').eq(2).should("contain.text", value);
    return this; // Return this to allow method chaining
  }

  // Assert 'Leadership and management' matches the expected value
  public hasLeadershipAndManagement(value: string): this {
    cy.get('.govuk-summary-list__value').eq(3).should("contain.text", value);
    return this; // Return this to allow method chaining
  }

  // Assert 'Assigned to' matches the expected value
  public hasAssignedTo(value: string): this {
    cy.get('.govuk-summary-list__value').eq(4).should("contain.text", value);
    return this; // Return this to allow method chaining
  }

  // Assert 'Advised by' matches the expected value
  public hasAdvisedBy(value: string): this {
    cy.get('.govuk-summary-list__value').eq(5).should("contain.text", value);
    return this; // Return this to allow method chaining
  }

public hasChangeLinks() : this {
    // Get all links that have the visible text 'Change'
    cy.get('a')
    .filter(':visible')
    .contains('Change')
    .eq(0)
    .should('have.attr', 'href')
    .and('include', 'delivery-officer');

  cy.get(':nth-child(6) > .govuk-summary-list__actions > .govuk-link')
    .filter(':visible')
    .contains('Change')
    .should('have.attr', 'href')
    .and('include', 'adviser');

  return this;
}

public hasNav() : this {
  cy.contains("Task list").first().should('have.attr', 'aria-current')
  cy.contains("About the school")
  cy.contains("Ofsted reports")
  cy.contains("Contacts")
  cy.contains("Case Study")
  cy.contains("Notes")

  return this;
}

public hasTasks() : this {
  cy.contains('Confirm eligibility');
  cy.contains('Enter the funding history');
  cy.contains('Contact the responsible body');
  cy.contains('Record the school\'s response');
  cy.contains('Check potential adviser conflicts of interest');
  cy.contains('Allocate an adviser');
  cy.contains('Send introductory email');
  cy.contains('Arrange adviser visit to school');
  cy.contains('Record date of visit to school');
  cy.contains('Write and save the Note of Visit');
  cy.contains('Complete and save the assessment template');
  cy.contains('Record matching decision');
  cy.contains('Choose preferred supporting organisation');
  cy.contains('Carry out due diligence on preferred supporting organisation');
  cy.contains('Record supporting organisation appointment');
  cy.contains('Add supporting organisation contact details');
  cy.contains('Request planning grant offer letter');
  cy.contains('Confirm planning grant offer letter sent');
  cy.contains('Share the improvement plan template');
  cy.contains('Review the improvement plan');
  cy.contains('Send the agreed improvement plan for approval');
  cy.contains('Record improvement plan decision');
  cy.contains('Request improvement grant offer letter');
  cy.contains('Confirm improvement grant offer letter sent');

  return this;
}

public tasksNotStartedElementsPresent() : this {
  cy.contains("Not Started").eq(0);
  cy.contains("Not Started").eq(1);
  cy.contains("Not Started").eq(2);
  cy.contains("Not Started").eq(3);
  cy.contains("Not Started").eq(4);
  cy.contains("Not Started").eq(5);
  cy.contains("Not Started").eq(6);
  cy.contains("Not Started").eq(7);
  cy.contains("Not Started").eq(8);
  cy.contains("Not Started").eq(9);
  cy.contains("Not Started").eq(10);
  cy.contains("Not Started").eq(11);
  cy.contains("Not Started").eq(12);
  cy.contains("Not Started").eq(13);
  cy.contains("Not Started").eq(14);
  cy.contains("Not Started").eq(15);
  cy.contains("Not Started").eq(16);
  cy.contains("Not Started").eq(17);
  cy.contains("Not Started").eq(18);
  cy.contains("Not Started").eq(19);
  cy.contains("Not Started").eq(20);
  cy.contains("Not Started").eq(21);
  cy.contains("Not Started").eq(22);
  cy.contains("Not Started").eq(23);


  return this;
}

  public deleteSchool(): this {
    cy.contains("Delete school").click({ force: true });

    return this;
  }
}

const taskList = new TaskList();

export default taskList;
