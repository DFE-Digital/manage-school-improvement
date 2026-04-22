import { Logger } from "cypress/common/logger";

class TaskList {
  public hasHeader(header: string): this {
    cy.get("h1").contains(header);

    return this;
  }

  public hasTaskListHeading(value: string): this {
    cy.get("h2").contains(value);
    return this
  }

  public hasCurrentStatus(status: string): this {
    cy.get('.govuk-summary-list__value').eq(0).should("contain.text", status);
    return this;
  }

  public hasEligibility(eligibility: string): this {
    cy.get('.govuk-summary-list__value').eq(1).should("contain.text", eligibility);
    return this;
  }

  public hasDateAdded(date: string): this {
    cy.get('.govuk-summary-list__value').eq(2).should("contain.text", date);
    return this;
  }

  public hasAssignedTo(value: string): this {
    cy.get('.govuk-summary-list__value').eq(3).should("contain.text", value);
    return this;
  }

  public hasAdvisedBy(value: string): this {
    cy.get('.govuk-summary-list__value').eq(4).should("contain.text", value);
    return this;
  }

  public hasEngagementConcern(value: string): this {
    cy.get('.govuk-summary-list__value').eq(5).should("contain.text", value);
    return this;
  }

  public hasChangeLinks(): this {
    cy.get('a')
    cy.get('.govuk-summary-list').within(() => {
      cy.get('.govuk-summary-list__row').eq(0).find('.govuk-link').contains('Change').should('have.attr', 'href').and('include', 'status');
      cy.get('.govuk-summary-list__row').eq(3).find('.govuk-link').contains('Assign a delivery officer')
      cy.get('.govuk-summary-list__row').eq(5).find('.govuk-link').contains('Change').should('have.attr', 'href').and('include', 'record-engagement-concern');
    });

    return this;
  }

  public hasNav(): this {
    cy.contains("Task list").first().should('have.attr', 'aria-current')
    cy.contains("About the school")
    cy.contains("Improvement plan")
    cy.contains("Contacts")
    cy.contains("Case Study")
    cy.contains("Engagement concern")
    cy.contains("Notes")
    cy.contains("Status and eligibility");

    return this;
  }

  public hasTasks(): this {
    cy.contains('Confirm starting eligibility');
    cy.contains('Enter the funding history');
    cy.contains('Make initial contact with the responsible body');
    cy.contains('Check potential adviser conflicts of interest');
    cy.contains('Send the formal notification');
    cy.contains('Record the responsible body\'s response');
    cy.contains('Allocate an adviser');
    cy.contains('Send introductory email');
    cy.contains("Arrange adviser's initial visit");
    cy.contains('Record date of initial visit');
    cy.contains('Complete and save the initial diagnosis assessment');
    cy.contains('Record initial diagnosis decision');
    cy.contains('Choose preferred supporting organisation');
    cy.contains('Carry out due diligence on preferred supporting organisation');
    cy.contains('Record supporting organisation appointment');
    cy.contains('Confirm supporting organisation contact details');
    cy.contains('Request planning grant offer letter');
    cy.contains('Confirm planning grant offer letter sent');
    cy.contains('Share indicative funding band and the improvement plan template');
    cy.contains('Review the improvement plan and confirm the funding band');
    cy.contains('Send the agreed improvement plan for approval');
    cy.contains('Record improvement plan decision');
    cy.contains('Enter improvement plan objectives');
    cy.contains('Request improvement grant offer letter');
    cy.contains('Confirm improvement grant offer letter sent');

    return this;
  }

  public hasTasksCannotStartYetElementsPresent(): this {
    cy.get("#confirm-eligibility-status").contains('Completed');
    cy.get('#funding_history_status').contains("Cannot start yet");
    cy.get('#confirm_responsible_body_status').contains('Cannot start yet');
    cy.get('#record-school-response_status').contains("Cannot start yet");
    cy.get('#CheckPotentialAdviserConflictsOfInterest_status').contains("Cannot start yet");
    cy.get('#send-formal-notification_status').contains("Cannot start yet");
    cy.get('#AllocateAdviser_status').contains("Cannot start yet");
    cy.get('#send-introductory-email-request-improvement-plan_status').contains("Cannot start yet");
    cy.get('#adviser-school-visit_status').contains("Cannot start yet");
    cy.get('#record-school-visit-date_status').contains("Cannot start yet");
    cy.get('#complate-save-assessment-template_status').contains("Cannot start yet");
    cy.get('#record-support-decision_status').contains('Cannot start yet');
    cy.get('#choose-preferred-supporting-organisation-status').contains('Cannot start yet');
    cy.get('#due-diligence-on-preferred-supporting-organisation-status').contains('Cannot start yet');
    cy.get('#record-supporting-organisation-appointment-status').contains("Cannot start yet");
    cy.get('#add-supporting-organisation-contact-details-status').contains("Cannot start yet");
    cy.get('#request-planning-grant-offer-letter_status').contains("Cannot start yet");
    cy.get('#confirm-planning-grant-offer-letter_status').contains("Cannot start yet");
    cy.get('#share-the-improvement-plan-template_status').contains("Cannot start yet");
    cy.get('#review-the-improvement-plan_status').contains('Cannot start yet');
    cy.get("#send-agreed-improvement-plan_status").contains('Cannot start yet');
    cy.get("#record-improvement-plan-decision_status").contains('Cannot start yet');
    cy.get("#enter-improvement-plan-objectives_status").contains('Cannot start yet');
    cy.get('#request-improvement-grant-offer-letter_status').contains("Cannot start yet");
    cy.get("#confirm-improvement-grant-offer-letter_status").contains('Cannot start yet');

    return this;
  }

  public hasTabs(): this {
    cy.contains('Task list');
    cy.contains('About the school');
    cy.contains('Improvement plan');
    cy.contains('Contacts');
    cy.contains('Case Study');
    cy.contains('Engagement concern');
    cy.contains('Notes');
    cy.contains('Status and eligibility');

    return this;

  }

  public hasSchoolSummaryList(): this {
    cy.get('.govuk-summary-list').should('exist');
    cy.get('.govuk-summary-list__row').each(($row) => {
      cy.wrap($row).find('.govuk-summary-list__value').should('not.be.empty');
    });

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

  public hasTaskStatusCannotProgress(id: string) {
    cy.get(`#${id}`).contains("Cannot progress");

    return this;
  }

  public hasTaskStatusCannotStartYet(id: string) {
    cy.get(`#${id}`).contains("Cannot start yet");

    return this;
  }

  public hasTaskStatusNotStarted(id: string) {
    cy.get(`#${id}`).contains("Not Started");

    return this;
  }


  public selectTask(taskName: string) {
    cy.contains(taskName).click();

    return this;
  }

  public clickBackLink(): this {
    cy.get('.govuk-back-link').click();

    return this;
  }

  public hasProjectMustBeAssignedBanner(): this {
    cy.get('.govuk-notification-banner__content').should('contain.text', 'Project must be assigned');
    return this;
  }

  public assignUserToSchoolIfNotAssigned(): this {
    if (cy.get('.govuk-summary-list__value').eq(3).contains('a')) {
      Logger.log("Assigning user to school");
      this.assignUserToSchool()
      this.userAssignedSuccessMessage();
    }
    else {
      Logger.log("User is already assigned to the school");
    }

    return this;
  }

  public allocateAdviserIfNotAllocated(): this {
    if (cy.get('.govuk-summary-list__value').eq(4).contains('text', 'Cannot allocate an adviser yet')) {
      Logger.log("Allocating adviser to school");
      cy.get('.govuk-summary-list__row')
        .contains('Advised by')
        .parent()
        .find('.govuk-link')
        .contains('Change')
        .click();

      cy.url().should('include', '/allocate-adviser');
      cy.get('h1').should('contain.text', 'Allocate adviser to school');
      cy.getById('adviser').
        should('exist');
      cy.getById('adviser').click().type('TestFirstName TestSurname');

      cy.getById('adviser').should('not.have.value', '');
      cy.getByCyData('continue-Btn').click();
    }
    else {
      Logger.log("Adviser is already allocated to the school");

    }

    return this;
  }

  public assignUserToSchool(): this {
    cy.get('.govuk-summary-list__row')
      .contains('Assigned to')
      .parent()
      .find('.govuk-link')
      .contains('Assign a delivery officer')
      .click();

    cy.url().should('include', '/assign-delivery-officer');
    cy.get('h1').should('contain.text', 'Who will work with this school?');
    cy.getById('delivery-officer').should('exist');
    cy.getById('delivery-officer').click().type('TestFirstName TestSurname');
    cy.getById('delivery-officer').should('not.have.value', '');
    cy.getByCyData('continue-Btn').click();

    return this;
  }

  public userAssignedSuccessMessage(): this {
    cy.get('.govuk-notification-banner.govuk-notification-banner--success').should('contain.text', 'Person is assigned');

    return this;
  }

  public navigateToChangeCurrentStatusPage(): this {
    cy.get('.govuk-summary-list__row')
      .contains('Current status')
      .parent()
      .find('.govuk-link')
      .contains('Change')
      .click();

    cy.url().should('include', '/change-project-status');
    cy.get('h2').should('contain.text', 'Change project status');

    return this;

  }

  public navigateToAssignDeliveryOfficerPage(): this {
    cy.get('.govuk-summary-list__row')
      .contains('Assigned to')
      .parent()
      .find('.govuk-link')
      .contains('Change')
      .click();

    cy.url().should('include', '/assign-delivery-officer');
    cy.get('h1').should('contain.text', 'Who will work with this school?');

    return this;
  }

  public navigateToRecordEngagementConcernPage(): this {
    cy.get('.govuk-summary-list__row')
      .contains('Engagement concern')
      .parent()
      .find('.govuk-link')
      .contains('Change')
      .click();

    cy.url().should('include', '/record-engagement-concern');
    cy.get('h1').should('contain.text', 'Record engagement concern');

    return this;
  }

  public navigateToAllocateAdviserPage(): this {
    cy.get('.govuk-summary-list__row')
      .contains('Advised by')
      .parent()
      .find('.govuk-link')
      .contains('Change')
      .click();

    cy.url().should('include', '/allocate-adviser');
    cy.get('h1').should('contain.text', 'Allocate an adviser');

    return this;
  }

}

const taskList = new TaskList();

export default taskList;
