import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import whichSchoolNeedsHelp from "cypress/pages/whichSchoolNeedsHelp";
import checkSchoolDetails from "cypress/pages/checkSchoolDetails";
import taskList from "cypress/pages/taskList";
import fundingHistory from "cypress/pages/tasks/fundingHistory";
import taskListActions from "cypress/pages/tasks/taskListActions";
import * as schoolData from "cypress/fixtures/school-data.json";

describe("Add a school which requires an improvement and complete it's tasks", () => {
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

  // Cleanup after all tests
  after(() => {
    cy.removeProjectIfItExists(urn);
  });

  // Initial project creation
  it("Should be able to add a school to the system", { tags: ['smoke'] }, () => {
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

    Logger.log("Selecting previously created project");
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
  });

  // Task 1: Confirm eligibility
  it("Should complete the 'Confirm eligibility' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Confirm eligibility' task");
    taskList.selectTask("Confirm eligibility");
    //cy.executeAccessibilityTests(); COMMENTED OUT AS AXE FALSE POSITIVE ARIA-EXPANDED RADIO BUTTON THING
    taskListActions.hasHeader("Is this school still eligible for targeted intervention?");
    taskListActions.selectYesAndContinue();
    cy.executeAccessibilityTests();
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-eligibility-status");
  });

  // Task 2: Funding history
  it("Should complete the 'Enter the funding history' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 3: Contact the responsible body
  it("Should complete the 'Contact the responsible body' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });
    
  // Task 4: Record the responsible body's response
  it("Should complete the 'Record the responsible body's response' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Record the responsible body's response' task");
    taskList.selectTask("Record the responsible body's response");
    taskListActions.hasHeader("Record the responsible body's response");
    taskListActions.selectButtonOrCheckbox("acknowledged");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("record-school-response_status");
    taskList.selectTask("Record the responsible body's response");
    taskListActions.selectButtonOrCheckbox("has-saved-school-response-in-sharepoint");
    taskListActions.enterDate("school-response-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-school-response_status");
  });

  // Task 5: Check potential adviser conflicts of interest
  it("Should complete the 'Check potential adviser conflicts of interest' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 6: Allocate an adviser
  it("Should complete the 'Allocate an adviser' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Allocate an adviser' task");
    taskList.selectTask("Allocate an adviser");
    taskListActions.hasHeader("Allocate an adviser");
    taskListActions.enterText("adviser", "TestFirstName TestSurname");
    taskListActions.enterDate("date-adviser-allocated", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("AllocateAdviser_status");
  });

  // Task 7: Send introductory email
  it("Should complete the 'Send introductory email' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 8: Arrange adviser's initial visit
  it("Should complete the 'Arrange adviser's initial visit' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Arrange adviser's initial visit' task");
    taskList.selectTask("Arrange adviser's initial visit");
    taskListActions.hasHeader("Arrange adviser's initial visit");
    taskListActions.selectButtonOrCheckbox("confirm-adviser-has-note-of-visit-template");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("adviser-school-visit_status");
    taskList.selectTask("Arrange adviser's initial visit");
    taskListActions.enterDate("adviser-visit-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("adviser-school-visit_status");
  });

  // Task 9: Record date of initial visit
  it("Should complete the 'Record date of initial visit' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Record date of initial visit' task");
    taskList.selectTask("Record date of initial visit");
    taskListActions.hasHeader("Record date of initial visit");
    taskListActions.enterDate("school-visit-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-school-visit-date_status");
  });

  // Task 10: Complete and save the initial diagnosis assessment
  it("Should complete the 'Complete and save the initial diagnosis assessment' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Complete and save the initial diagnosis assessment' task");
    taskList.selectTask("Complete and save the initial diagnosis assessment");
    taskListActions.hasHeader("Complete and save the initial diagnosis assessment");
    taskListActions.selectButtonOrCheckbox("has-talk-to-adviser");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("complate-save-assessment-template_status");
    taskList.selectTask("Complete and save the initial diagnosis assessment");
    taskListActions.selectButtonOrCheckbox("complete-assessment-template");
    taskListActions.enterDate("saved-assessemnt-template-in-sharepoint-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("complate-save-assessment-template_status");
  });

  // Task 11: Record initial diagnosis decision
  it("Should complete the 'Record initial diagnosis decision' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Record initial diagnosis decision' task");
    taskList.selectTask("Record initial diagnosis decision");
    taskListActions.hasHeader("Record initial diagnosis decision");
    taskListActions.selectButtonOrCheckbox("review-school-progress");
    taskListActions.enterText("NotMatchingNotes", "Review notes");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("record-support-decision_status");
    taskList.selectTask("Record initial diagnosis decision");
    taskListActions.enterDate("decision-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-support-decision_status");
  });

  // Task 12: Choose preferred supporting organisation
  it("Should complete the 'Choose preferred supporting organisation' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Choose preferred supporting organisation' task");
    taskList.selectTask("Choose preferred supporting organisation");
    taskListActions.hasHeader("Choose preferred supporting organisation");
    taskListActions.selectButtonOrCheckbox("complete-assessment-tool");
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
  });

  // Task 13: Carry out due diligence on preferred supporting organisation
  it("Should complete the 'Carry out due diligence on preferred supporting organisation' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 14: Record supporting organisation appointment
  it("Should complete the 'Record supporting organisation appointment' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 15: Add supporting organisation contact details
  it("Should complete the 'Add supporting organisation contact details' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 16: Request planning grant offer letter
  it("Should complete the 'Request planning grant offer letter' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 17: Confirm planning grant offer letter sent
  it("Should complete the 'Confirm planning grant offer letter sent' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Confirm planning grant offer letter sent' task");
    taskList.selectTask("Confirm planning grant offer letter sent");
    taskListActions.hasHeader("Confirm planning grant offer letter sent");
    taskListActions.enterDate("planning-grant-offer-letter-sent-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-planning-grant-offer-letter_status");
  });

  // Task 18: Share indicative funding band and the improvement plan template
  it("Should complete the 'Share indicative funding band and the improvement plan template' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Share indicative funding band and the improvement plan template' task");
    taskList.selectTask("Share indicative funding band and the improvement plan template");
    taskListActions.hasHeader("Share indicative funding band and the improvement plan template");
    taskListActions.selectButtonOrCheckbox("calculate-funding-band");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("share-the-improvement-plan-template_status");
    taskList.selectTask("Share indicative funding band and the improvement plan template");
    taskListActions.selectButtonOrCheckbox("funding-band-40000");
     taskListActions.selectButtonOrCheckbox("send-template");
    taskListActions.enterDate("date-templates-sent", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("share-the-improvement-plan-template_status");
  });

  // Task 19: Review the improvement plan and confirm the funding band
  it("Should complete the 'Review the improvement plan and confirm the funding band' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Review the improvement plan and confirm the funding band' task");
    taskList.selectTask("Review the improvement plan and confirm the funding band");
    taskListActions.hasHeader("Review the improvement plan and confirm the funding band");
    taskListActions.enterDate("date-improvement-plan-received", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("review-the-improvement-plan_status");
    taskList.selectTask("Review the improvement plan and confirm the funding band");
    taskListActions.selectButtonOrCheckbox("review-improvement-plan");
    taskListActions.selectButtonOrCheckbox("confirm-plan-cleared-by-rise")
    taskListActions.selectButtonOrCheckbox("confirm-funding-band")
    taskListActions.selectButtonOrCheckbox("funding-band-no-funding-required")
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("review-the-improvement-plan_status")
  });

  // Task 20: Send the agreed improvement plan for approval
  it("Should complete the 'Send the agreed improvement plan for approval' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 21: Record improvement plan decision
  it("Should complete the 'Record improvement plan decision' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 22: Enter improvement plan objectives
  it("Should complete the 'Enter improvement plan objectives' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Enter improvement plan objectives' task");
    taskList.selectTask("Enter improvement plan objectives");
    taskListActions.hasHeader("Select an area of improvement");
    taskListActions.selectButtonOrCheckbox("quality-of-education");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");     
    taskListActions.enterText("ObjectiveDetails", "Quality of education details");
    taskListActions.clickButton("finish");
    taskListActions.clickButton("save");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("enter-improvement-plan-objectives_status");
    taskList.selectTask("Enter improvement plan objectives");
    taskListActions.clickButton("add-another"); 
    taskListActions.selectButtonOrCheckbox("leadership-and-management");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");     
    taskListActions.enterText("ObjectiveDetails", "Leadership-and-management details");
    taskListActions.clickButton("finish");
    taskListActions.linkExists("Change")
    taskListActions.selectButtonOrCheckbox("MarkAsComplete")
    taskListActions.clickButton("save");    
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("enter-improvement-plan-objectives_status");
  });

  // Task 23: Request improvement grant offer letter
  it("Should complete the final grant tasks", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);
    
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
  });

  // Task 24: Confirm improvement grant offer letter 
  it("Should complete the 'Confirm improvement grant offer letter sent' task", { tags: ['smoke'] }, () => {
    homePage.selectSchoolName(schoolLong);

    Logger.log("Selecting 'Confirm improvement grant offer letter sent' task");
    taskList.selectTask("Confirm improvement grant offer letter sent");
    taskListActions.hasHeader("Confirm improvement grant offer letter sent");
    taskListActions.enterDate("date-improvement-grant-offer-letter-sent", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-improvement-grant-offer-letter_status");
  });
  
});
