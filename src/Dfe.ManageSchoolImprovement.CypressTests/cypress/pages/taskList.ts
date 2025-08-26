class TaskList {
  public hasHeader(header: string): this {
    cy.get("h1").contains(header);

    return this;
  }

  public hasTaskListHeading(value: string): this {
    cy.get("h2").contains(value);
    return this
  }

  public hasDateAdded(date: string): this {
    cy.get('.govuk-summary-list__value').eq(0).should("contain.text", date);
    return this;
  }

  public hasInspectionDate(date: string): this {
    cy.get('.govuk-summary-list__value').eq(1).should("contain.text", date);
    return this;
  }

  public hasQualityOfEducation(value: string): this {
    cy.get('.govuk-summary-list__value').eq(2).should("contain.text", value);
    return this;
  }

  public hasLeadershipAndManagement(value: string): this {
    cy.get('.govuk-summary-list__value').eq(3).should("contain.text", value);
    return this;
  }

  public hasAssignedTo(value: string): this {
    cy.get('.govuk-summary-list__value').eq(4).should("contain.text", value);
    return this;
  }

  public hasAdvisedBy(value: string): this {
    cy.get('.govuk-summary-list__value').eq(5).should("contain.text", value);
    return this;
  }

  public hasChangeLinks(): this {
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

  public hasNav(): this {
    cy.contains("Task list").first().should('have.attr', 'aria-current')
    cy.contains("About the school")
    cy.contains("Ofsted reports")
    cy.contains("Contacts")
    cy.contains("Case Study")
    cy.contains("Engagement concern")
    cy.contains("Notes")

    return this;
  }

  public hasTasks(): this {
    cy.contains('Confirm eligibility');
    cy.contains('Enter the funding history');
    cy.contains('Make initial contact with the responsible body');
    cy.contains('Record the responsible body\'s response');
    cy.contains('Check potential adviser conflicts of interest');
    cy.contains('Allocate an adviser');
    cy.contains('Send introductory email');
    cy.contains("Arrange adviser's initial visit");
    cy.contains('Record date of initial visit');
    cy.contains('Complete and save the initial diagnosis assessment');
    cy.contains('Record initial diagnosis decision');
    cy.contains('Choose preferred supporting organisation');
    cy.contains('Carry out due diligence on preferred supporting organisation');
    cy.contains('Record supporting organisation appointment');
    cy.contains('Add supporting organisation contact details');
    cy.contains('Request planning grant offer letter');
    cy.contains('Share indicative funding band and the improvement plan template');
    cy.contains('Review the improvement plan and confirm the funding band');
    cy.contains('Send the agreed improvement plan for approval');
    cy.contains('Record improvement plan decision');
    cy.contains('Request improvement grant offer letter');
    cy.contains('Confirm improvement grant offer letter sent');

    return this;
  }

  public hasTasksNotStartedElementsPresent(): this {
    cy.get("#confirm-eligibility-status").contains('Not Started');
    cy.get('#funding_history_status').contains("Not Started");
    cy.get('#confirm_responsible_body_status').contains('Not Started');
    cy.get('#record-school-response_status').contains("Not Started");
    cy.get('#CheckPotentialAdviserConflictsOfInterest_status').contains("Not Started");
    cy.get('#AllocateAdviser_status').contains("Not Started");
    cy.get('#send-introductory-email-request-improvement-plan_status').contains("Not Started");
    cy.get('#adviser-school-visit_status').contains("Not Started");
    cy.get('#record-school-visit-date_status').contains("Not Started");
    cy.get('#complate-save-assessment-template_status').contains("Not Started");
    cy.get('#record-support-decision_status').contains('Not Started');
    cy.get('#choose-preferred-supporting-organisation-status').contains('Not Started');
    cy.get('#due-diligence-on-preferred-supporting-organisation-status').contains('Not Started');
    cy.get('#record-supporting-organisation-appointment-status').contains("Not Started");
    cy.get('#add-supporting-organisation-contact-details-status').contains("Not Started");
    cy.get('#request-planning-grant-offer-letter_status').contains("Not Started");
    cy.get('#confirm-planning-grant-offer-letter_status').contains("Not Started");
    cy.get('#share-the-improvement-plan-template_status').contains("Not Started");
    cy.get('#review-the-improvement-plan_status').contains('Not Started');
    cy.get("#send-agreed-improvement-plan_status").contains('Not Started');
    cy.get("#record-improvement-plan-decision_status").contains('Not Started');
    cy.get('#request-improvement-grant-offer-letter_status').contains("Not Started");
    cy.get("#confirm-improvement-grant-offer-letter_status").contains('Not Started');

    return this;
  }

  public hasTabs(): this {
    cy.contains('Task list');
    cy.contains('About the school');
    cy.contains('Ofsted reports');
    cy.contains('Contacts');
    cy.contains('Case Study');
    cy.contains('Engagement concern');
    cy.contains('Notes');

    return this;

  }

  public navigateToTab(tabName: string): this {
    cy.get('.moj-sub-navigation__link')
      .contains(tabName).click();

    return this;
  }


  public hasFilterSuccessNotification(): this {
    cy.get('[cy-data="task-updated-success-notification"]').should("be.visible");

    return this;
  }

  public hasTaskStatusCompleted(id: string) {
    cy.get(`#${id}`).contains("Completed");

    return this;
  }

  public hasTaskStatusInProgress(id: string) {
    cy.get(`#${id}`).contains("In Progress");
  }

  public selectTask(taskName: string) {
    cy.contains(taskName).click();

    return this;
  }

  public deleteSchool(): this {
    cy.contains("Delete school").click({ force: true });

    return this;
  }

}

const taskList = new TaskList();

export default taskList;
