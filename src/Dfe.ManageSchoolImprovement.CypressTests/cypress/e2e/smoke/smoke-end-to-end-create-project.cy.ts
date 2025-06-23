import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import taskList from "cypress/pages/taskList";
import fundingHistory from "cypress/pages/tasks/fundingHistory";
import taskListActions from "cypress/pages/tasks/taskListActions";
import * as schoolData from "cypress/fixtures/school-data.json";

describe("User completes their newly created project", () => {
  const {
    schoolShort,
    schoolLong,
    lastInspectionDate,
    qualityOfEducation,
    leadershipAndManagement,
    assignedTo,
    advisedBy,
    urn,
    localAuthority,
    region,
    schoolType,
    faithSchool,
    ofstedRating,
    pfi
  } = schoolData;

  const today = new Date();
  const dateAdded = today.toLocaleDateString("en-GB", {
    day: "numeric",
    month: "long",
    year: "numeric",
  });

  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-identified-for-targeted-intervention");
  });

  it("Should be able to add a school and add it to the list", { tags: ['smoke'] }, () => {
    homePage.AddSchool();

    cy.executeAccessibilityTests();

    whichSchoolNeedsHelp
      .hasHeader("Select school")
      .withShortSchoolName(schoolShort)
      .withLongSchoolName("Plymouth Grove Primary")
      .clickContinue();

    cy.executeAccessibilityTests();

    checkSchoolDetails
      .hasHeader("Check school details")
      .hasSchoolName("Plymouth Grove Primary")
      .hasURN(urn)
      .hasLocalAuthority(localAuthority)
      .hasSchoolType(schoolType)
      .hasFaithSchool(faithSchool)
      .hasOfstedRating(ofstedRating)
      .hasLastInspection(lastInspectionDate)
      .hasPFI(pfi);

    checkSchoolDetails.clickContinue();

    homePage
      .hasSchoolName("Plymouth Grove Primary")
      .hasURN(urn)
      .hasLocalAuthority(localAuthority)
      .hasRegion(region)
      .hasAddSchoolSuccessNotification();

    Logger.log("Seleting previously created project");
    homePage.selectSchoolName(schoolLong);

    cy.executeAccessibilityTests();

    taskList
      .hasHeader("Plymouth Grove Primary")
      .hasDateAdded(dateAdded)
      .hasInspectionDate(lastInspectionDate)
      .hasQualityOfEducation(qualityOfEducation)
      .hasLeadershipAndManagement(leadershipAndManagement)
      .hasAssignedTo(assignedTo)
      .hasAdvisedBy(advisedBy)

      .hasChangeLinks()

      .hasNav()

      .hasTasks()
      .hasTasksNotStartedElementsPresent();

    Logger.log("Selecting 'Confirm eligiblity' task");
    taskList.selectTask("Confirm eligibility");
    //cy.executeAccessibilityTests(); COMMENTED OUT AS AXE FALSE POSITIVE ARIA-EXPANDED RADIO BUTTON THING
    taskListActions.hasHeader("Is this school still eligible for targeted intervention?");
    taskListActions.selectYesAndContinue();
    cy.executeAccessibilityTests();
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-eligibility-status");
    
    Logger.log("Selecting 'Funding history' task");
    taskList.selectTask("Enter the funding history");
    taskListActions.hasHeader("Has the school received any funding in the last 2 financial years?");
    taskListActions.selectNoAndContinue();
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("funding_history_status");
    taskList.selectTask("Enter the funding history");
    taskListActions.selectYesAndContinue();
    fundingHistory.enterFundingHistory();
    fundingHistory.confirmFundingHistory();
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("funding_history_status");
    
    Logger.log("Selecting 'Contact the responsible body' task");
    taskList.selectTask("Contact the responsible body");
    taskListActions.hasHeader("Contact the responsible body");
    taskListActions.selectButtonOrCheckbox("discuss-best-approach");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("confirm_responsible_body_status");
    taskList.selectTask("Contact the responsible body");
    taskListActions.selectButtonOrCheckbox("email-responsible-body");
    taskListActions.enterDate("responsible-body-contacted-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm_responsible_body_status");
    
    Logger.log("Selecting 'Record the school's response' task");
    taskList.selectTask("Record the school's response");
    taskListActions.hasHeader("Record the responsible body's response");
    taskListActions.selectButtonOrCheckbox("acknowledged");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("record-school-response_status");
    taskList.selectTask("Record the school's response");
    taskListActions.selectButtonOrCheckbox("has-saved-school-response-in-sharepoint");
    taskListActions.enterDate("school-response-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-school-response_status");
    
    Logger.log("Selecting 'Check potential adviser conflicts of interest' task");
    taskList.selectTask("Check potential adviser conflicts of interest");
    taskListActions.hasHeader("Check potential adviser conflicts of interest");
    taskListActions.selectButtonOrCheckbox("send-conflict-of-interest-form-to-proposed-adviser-and-the-school");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("CheckPotentialAdviserConflictsOfInterest_status");
    taskList.selectTask("Check potential adviser conflicts of interest");
    taskListActions.selectButtonOrCheckbox("receive-completed-conflict-of-interest-form");
    taskListActions.selectButtonOrCheckbox("save-completed-conflict-of-interest-form-in-sharepoint");
    taskListActions.enterDate("date-conflicts-of-interest-were-checked", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("CheckPotentialAdviserConflictsOfInterest_status");
    
    Logger.log("Selecting 'Allocate an adviser' task");
    taskList.selectTask("Allocate an adviser");
    taskListActions.hasHeader("Allocate an adviser");
    taskListActions.enterText("adviser-email-address", "adviser.email-rise@education.gov.uk");
    taskListActions.enterDate("date-adviser-allocated", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("AllocateAdviser_status");
    
    Logger.log("Selecting 'Send introductory email' task");
    taskList.selectTask("Send introductory email");
    taskListActions.hasHeader("Send introductory email");
    taskListActions.selectButtonOrCheckbox("share-email-template-with-adviser");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("send-introductory-email-request-improvement-plan_status");
    taskList.selectTask("Send introductory email");
    taskListActions.selectButtonOrCheckbox("remind-adviser-to-copy-in-rise-team-on-email-sent");
    taskListActions.enterDate("introductory-email-sent-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("send-introductory-email-request-improvement-plan_status");

    Logger.log("Selecting 'Arrange adviser's initial visit' task");
    taskList.selectTask("Arrange adviser's initial visit");
    taskListActions.hasHeader("Arrange adviser's initial visit");
    taskListActions.enterDate("adviser-visit-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("adviser-school-visit_status");

    Logger.log("Selecting 'Record date of initial visit' task");
    taskList.selectTask("Record date of initial visit");
    taskListActions.hasHeader("Record date of initial visit");
    taskListActions.enterDate("school-visit-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-school-visit-date_status");

    Logger.log("Selecting 'Write and save the Note of Visit' task");
    taskList.selectTask("Write and save the Note of Visit");
    taskListActions.hasHeader("Write and save the Note of Visit");
    taskListActions.selectButtonOrCheckbox("give-the-adviser-the-note-of-visit-template");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("note-of-visit_status");
    taskList.selectTask("Write and save the Note of Visit");
    taskListActions.selectButtonOrCheckbox("ask-the-adviser-to-send-you-their-notes");
    taskListActions.enterDate("enter-date-note-of-visit-saved-in-sharepoint", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("note-of-visit_status");

    Logger.log("Selecting 'Complete and save the assessment template' task");
    taskList.selectTask("Complete and save the assessment template");
    taskListActions.hasHeader("Complete and save the assessment template");
    taskListActions.selectButtonOrCheckbox("has-talk-to-adviser");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("complate-save-assessment-template_status");
    taskList.selectTask("Complete and save the assessment template");
    taskListActions.selectButtonOrCheckbox("complete-assessment-template");
    taskListActions.enterDate("saved-assessemnt-template-in-sharepoint-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("complate-save-assessment-template_status");

    Logger.log("Selecting 'Record matching decision' task");
    taskList.selectTask("Record matching decision");
    taskListActions.hasHeader("Record matching decision");
    taskListActions.selectButtonOrCheckbox("yes");
    taskListActions.enterDate("decision-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-support-decision_status");

    Logger.log("Selecting 'Choose preferred supporting organisation' task");
    taskList.selectTask("Choose preferred supporting organisation");
    taskListActions.hasHeader("Choose preferred supporting organisation");
    taskListActions.enterText("organisation-name", "Pudsey Grammar School");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("choose-preferred-supporting-organisation-status");
    taskList.selectTask("Choose preferred supporting organisation");
    taskListActions.enterText("id-number", "108079");
    taskListActions.enterDate("date-support-organisation-chosen", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("choose-preferred-supporting-organisation-status");

    Logger.log("Selecting 'Carry out due diligence on preferred supporting organisation' task");
    taskList.selectTask("Carry out due diligence on preferred supporting organisation");
    taskListActions.hasHeader("Carry out due diligence on preferred supporting organisation");
    taskListActions.selectButtonOrCheckbox("check-organisation-has-capacity-and-willing-to-provide-support");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("due-diligence-on-preferred-supporting-organisation-status");
    taskList.selectTask("Carry out due diligence on preferred supporting organisation");
    taskListActions.selectButtonOrCheckbox("speak-to-trust-relationship-manager-or-local-authority-lead-to-check-choice");
    taskListActions.selectButtonOrCheckbox("contact-sfso-for-financial-check");
    taskListActions.selectButtonOrCheckbox("check-the-organisation-has-a-vendor-account");
    taskListActions.enterDate("due-diligence-completed-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("due-diligence-on-preferred-supporting-organisation-status");

    Logger.log("Selecting 'Record supporting organisation appointment' task");
    taskList.selectTask("Record supporting organisation appointment");
    taskListActions.hasHeader("Record supporting organisation appointment");
    taskListActions.selectButtonOrCheckbox("no");
    taskListActions.enterText("DisapprovingSupportingOrganisationAppointmentNotes", "details");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("record-supporting-organisation-appointment-status");
    taskList.selectTask("Record supporting organisation appointment");
    taskListActions.selectButtonOrCheckbox("yes");
    taskListActions.enterDate("appointment-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-supporting-organisation-appointment-status");

    Logger.log("Selecting 'Add supporting organisation contact details' task");
    taskList.selectTask("Add supporting organisation contact details");
    taskListActions.hasHeader("Add supporting organisation contact details");
    taskListActions.enterText("name", "Joe Bloggs");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("add-supporting-organisation-contact-details-status");
    taskList.selectTask("Add supporting organisation contact details");
    taskListActions.enterText("email-address", "joe.bloggs@email.com");
    taskListActions.enterDate("date-supporting-organisation-details-added", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("add-supporting-organisation-contact-details-status");

    Logger.log("Selecting 'Request planning grant offer letter' task");
    taskList.selectTask("Request planning grant offer letter");
    taskListActions.hasHeader("Request planning grant offer letter");
    taskListActions.selectButtonOrCheckbox("include-contact-details");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("request-planning-grant-offer-letter_status");
    taskList.selectTask("Request planning grant offer letter");
    taskListActions.selectButtonOrCheckbox("confirm-amount-funding");
    taskListActions.selectButtonOrCheckbox("copy-regional-director");
    taskListActions.selectButtonOrCheckbox("email-rise-grant-team");
    taskListActions.enterDate("date-grant-team-contacted", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("request-planning-grant-offer-letter_status");

    Logger.log("Selecting 'Confirm planning grant offer letter sent' task");
    taskList.selectTask("Confirm planning grant offer letter sent");
    taskListActions.hasHeader("Confirm planning grant offer letter sent");
    taskListActions.enterDate("planning-grant-offer-letter-sent-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-planning-grant-offer-letter_status");

    Logger.log("Selecting 'Share the improvement plan template' task");
    taskList.selectTask("Share the improvement plan template");
    taskListActions.hasHeader("Share the improvement plan template");
    taskListActions.selectButtonOrCheckbox("send-the-template-to-the-supporting-organisation");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("share-the-improvement-plan-template_status");
    taskList.selectTask("Share the improvement plan template");
    taskListActions.selectButtonOrCheckbox("send-the-template-to-the-schools-responsible-body");
    taskListActions.enterDate("date-templates-sent", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("share-the-improvement-plan-template_status");

    Logger.log("Selecting 'Review the improvement plan' task");
    taskList.selectTask("Review the improvement plan");
    taskListActions.hasHeader("Review the improvement plan");
    taskListActions.enterDate("date-improvement-plan-received", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("review-the-improvement-plan_status");
    taskList.selectTask("Review the improvement plan");
    taskListActions.selectButtonOrCheckbox("review-improvement-plan");
    taskListActions.selectButtonOrCheckbox("confirm-plan-cleared-by-rise")
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("review-the-improvement-plan_status");

    Logger.log("Selecting 'Send the agreed improvement plan for approval' task");
    taskList.selectTask("Send the agreed improvement plan for approval");
    taskListActions.hasHeader("Send the agreed improvement plan for approval");
    taskListActions.selectButtonOrCheckbox("save-agreed-improvement-plan-in-sp");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("send-agreed-improvement-plan_status");
    taskList.selectTask("Send the agreed improvement plan for approval");
    taskListActions.selectButtonOrCheckbox("email-agreed-plan-to-rg");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("send-agreed-improvement-plan_status");

    Logger.log("Selecting 'Record improvement plan decision' task");
    taskList.selectTask("Record improvement plan decision");
    taskListActions.hasHeader("Record improvement plan decision");
    taskListActions.selectButtonOrCheckbox("no");
    taskListActions.enterText("DisapprovingImprovementPlanDecisionNotes", "details");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("record-improvement-plan-decision_status");
    taskList.selectTask("Record improvement plan decision");
    taskListActions.selectButtonOrCheckbox("yes");
    taskListActions.enterDate("improvement-plan-decision-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-improvement-plan-decision_status");

    Logger.log("Selecting 'Request improvement grant offer letter' task");
    taskList.selectTask("Request improvement grant offer letter");
    taskListActions.hasHeader("Request improvement grant offer letter");
    taskListActions.enterDate("grant-team-contacted-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("request-improvement-grant-offer-letter_status");
    taskList.selectTask("Request improvement grant offer letter");
    taskListActions.selectButtonOrCheckbox("include-contact-details");
    taskListActions.selectButtonOrCheckbox("attach-school-improvement-plan");
    taskListActions.selectButtonOrCheckbox("copy-in-regional-director");
    taskListActions.selectButtonOrCheckbox("send-email-to-grant-team");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("request-improvement-grant-offer-letter_status");

    Logger.log("Selecting 'Confirm improvement grant offer letter sent' task");
    taskList.selectTask("Confirm improvement grant offer letter sent");
    taskListActions.hasHeader("Confirm improvement grant offer letter sent");
    taskListActions.enterDate("date-improvement-grant-offer-letter-sent", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-improvement-grant-offer-letter_status");

  });
});