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

  before(() => {
    cy.removeProjectIfItExists(urn);
  });

  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-identified-for-targeted-intervention");
  });

  // Initial project creation
  it("Should be able to add a new school to the system", () => {
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
  it("Should complete the 'Confirm eligibility' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Confirm eligibility' task");
    taskList.selectTask("Confirm eligibility");
    //cy.executeAccessibilityTests(); COMMENTED OUT AS AXE FALSE POSITIVE ARIA-EXPANDED RADIO BUTTON THING
    taskListActions.hasHeader("Is this school still eligible for targeted intervention?");
    taskListActions.selectYesAndContinue();
    cy.executeAccessibilityTests();
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-eligibility-status");

    cy.executeAccessibilityTests();  
  });

  // Task 2: Funding history
  it("Should complete the 'Enter the funding history' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Funding history' task");
    taskList.selectTask("Enter the funding history");

    cy.executeAccessibilityTests();

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

    cy.executeAccessibilityTests();  
  });

  // Task 3: Contact the responsible body
  it("Should complete the 'Make initial contact with the responsible body' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Contact the responsible body' task");
    taskList.selectTask("Make initial contact with the responsible body");

    cy.executeAccessibilityTests();

    taskListActions.hasHeader("Make initial contact with the responsible body");
    taskListActions.selectButtonOrCheckbox("initial-contact-responsible-body");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskListActions.enterDate("responsible-body-initial-contact-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm_responsible_body_status");
  });

  // Task 4: Check potential adviser conflicts of interest
  it("Should complete the 'Check potential adviser conflicts of interest' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Check potential adviser conflicts of interest' task");
    taskList.selectTask("Check potential adviser conflicts of interest");

    cy.executeAccessibilityTests();

    taskListActions.hasHeader("Check potential adviser conflicts of interest");
    taskListActions.selectButtonOrCheckbox("review-advisers-conflict-of-interest-form");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("CheckPotentialAdviserConflictsOfInterest_status");
    taskList.selectTask("Check potential adviser conflicts of interest");
    taskListActions.enterDate("date-conflict-of-interest-declaration-checked", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("CheckPotentialAdviserConflictsOfInterest_status");
  });

  // Task 5: Send the formal notification
  it("Should complete the 'Send the formal notification' task", () => {
    homePage.selectSchoolName(schoolLong);

    Logger.log("Selecting 'Send the formal notification' task");
    taskList.selectTask("Send the formal notification");
    taskListActions.hasHeader("Send the formal notification");
    taskListActions.selectButtonOrCheckbox("use-enrolment-letter-template-to-draft-email");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("send-formal-notification_status");
    taskList.selectTask("Send the formal notification");
    taskListActions.selectButtonOrCheckbox("attach-targeted-intervention-information-sheet");
    taskListActions.selectButtonOrCheckbox("add-recipients");
    taskListActions.selectButtonOrCheckbox("send-email");
    taskListActions.enterDate("date-of-formal-contact", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("send-formal-notification_status");
  });


  // Task 6: Record the responsible body's response to the conflict of interest request
  it("Should complete the 'Record the responsible body's response to the conflict of interest request", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Record the responsible body's response to the conflict of interest request' task");
    taskList.selectTask("Record the responsible body's response to the conflict of interest request");

    cy.executeAccessibilityTests();

    taskListActions.hasHeader("Record the responsible body's response to the conflict of interest request");
    taskListActions.selectButtonOrCheckbox("ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("record-school-response_status");
    taskList.selectTask("Record the responsible body's response to the conflict of interest request");
    taskListActions.enterDate("ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-school-response_status");
  });

  // Task 7: Allocate an adviser
  it("Should complete the 'Allocate an adviser' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Allocate an adviser' task");
    taskList.selectTask("Allocate an adviser");

    cy.executeAccessibilityTests();

    taskListActions.hasHeader("Allocate an adviser");
    taskListActions.enterText("adviser", "TestFirstName TestSurname");
    taskListActions.enterDate("date-adviser-allocated", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("AllocateAdviser_status");
  });

  // Task 8: Send introductory email
  it("Should complete the 'Send introductory email' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Send introductory email' task");
    taskList.selectTask("Send introductory email");

    cy.executeAccessibilityTests();

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

  // Task 9: Arrange adviser's initial visit
  it("Should complete the 'Arrange adviser's initial visit' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Arrange adviser's initial visit' task");
    taskList.selectTask("Arrange adviser's initial visit");

    cy.executeAccessibilityTests();

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

  // Task 10: Record date of initial visit
  it("Should complete the 'Record date of initial visit' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Record date of initial visit' task");
    taskList.selectTask("Record date of initial visit");

    cy.executeAccessibilityTests();

    taskListActions.hasHeader("Record date of initial visit");
    taskListActions.enterDate("school-visit-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("record-school-visit-date_status");
  });

  // Task 11: Complete and save the initial diagnosis assessment
  it("Should complete the 'Complete and save the initial diagnosis assessment' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Complete and save the initial diagnosis assessment' task");
    taskList.selectTask("Complete and save the initial diagnosis assessment");

    cy.executeAccessibilityTests();

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

  // Task 12: Record initial diagnosis decision
  it("Should complete the 'Record initial diagnosis decision' task", () => {
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

  // Task 13: Choose preferred supporting organisation
  it("Should complete the 'Choose preferred supporting organisation' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Choose preferred supporting organisation' task");
    taskList.selectTask("Choose preferred supporting organisation");

    cy.executeAccessibilityTests();

    taskListActions.hasHeader("Choose preferred supporting organisation");
    taskListActions.selectButtonOrCheckbox("support-organisation-type-trust");
    taskListActions.selectButtonOrCheckbox("continue-button");

    taskListActions.enterText("organisation-name", "North West Schools Partnership");
    taskListActions.enterText("trust-ukprn", "10058689");
    taskListActions.enterDate("date-support-organisation-confirmed", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-return-button");
    taskListActions.confirmSupportingOrganisationDetails();
    taskListActions.selectButtonOrCheckbox("save-and-return-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusInProgress("choose-preferred-supporting-organisation-status");
   
    taskList.selectTask("Choose preferred supporting organisation");
    taskListActions.hasHeader("Choose preferred supporting organisation");
    taskListActions.selectButtonOrCheckbox("complete-assessment-tool");
    taskListActions.selectButtonOrCheckbox("continue-button");
    taskListActions.selectButtonOrCheckbox("save-and-return-button");
    taskListActions.selectButtonOrCheckbox("save-and-return-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("choose-preferred-supporting-organisation-status");
  });

  // Task 14: Carry out due diligence on preferred supporting organisation
  it("Should complete the 'Carry out due diligence on preferred supporting organisation' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Carry out due diligence on preferred supporting organisation' task");
    taskList.selectTask("Carry out due diligence on preferred supporting organisation");

    cy.executeAccessibilityTests();

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

  // Task 15: Record supporting organisation appointment
  it("Should complete the 'Record supporting organisation appointment' task", () => {
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

    cy.executeAccessibilityTests();  
  });

  // Task 16: Add supporting organisation contact details
  it("Should complete the 'Add supporting organisation contact details' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Add supporting organisation contact details' task");
    taskList.selectTask("Add supporting organisation contact details");

    cy.executeAccessibilityTests();

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

  // Task 17: Request planning grant offer letter
  it("Should complete the 'Request planning grant offer letter' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Request planning grant offer letter' task");
    taskList.selectTask("Request planning grant offer letter");

    cy.executeAccessibilityTests();

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

  // Task 18: Confirm planning grant offer letter sent
  it("Should complete the 'Confirm planning grant offer letter sent' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Confirm planning grant offer letter sent' task");
    taskList.selectTask("Confirm planning grant offer letter sent");

    cy.executeAccessibilityTests();

    taskListActions.hasHeader("Confirm planning grant offer letter sent");
    taskListActions.enterDate("planning-grant-offer-letter-sent-date", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-planning-grant-offer-letter_status");
  });

  // Task 19: Share indicative funding band and the improvement plan template
  it("Should complete the 'Share indicative funding band and the improvement plan template' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Share indicative funding band and the improvement plan template' task");
    taskList.selectTask("Share indicative funding band and the improvement plan template");

    cy.executeAccessibilityTests();

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

  // Task 20: Review the improvement plan and confirm the funding band
  it("Should complete the 'Review the improvement plan and confirm the funding band' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Review the improvement plan and confirm the funding band' task");
    taskList.selectTask("Review the improvement plan and confirm the funding band");

    cy.executeAccessibilityTests();

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

  // Task 21: Send the agreed improvement plan for approval
  it("Should complete the 'Send the agreed improvement plan for approval' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Send the agreed improvement plan for approval' task");
    taskList.selectTask("Send the agreed improvement plan for approval");

    cy.executeAccessibilityTests();

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

  // Task 22: Record improvement plan decision
  it("Should complete the 'Record improvement plan decision' task", () => {
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

    cy.executeAccessibilityTests();  
  });

  // Task 23: Enter improvement plan objectives //Not visible in Test environment
  it("Should complete the 'Enter improvement plan objectives' task", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Enter improvement plan objectives' task");
    taskList.selectTask("Enter improvement plan objectives");

    cy.executeAccessibilityTests();

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

  // Task 24: Request improvement grant offer letter
  it("Should complete the final grant tasks", () => {
    homePage.selectSchoolName(schoolLong);
    
    Logger.log("Selecting 'Request improvement grant offer letter' task");
    taskList.selectTask("Request improvement grant offer letter");

    cy.executeAccessibilityTests();

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

  // Task 25: Confirm improvement grant offer letter 
  it("Should complete the 'Confirm improvement grant offer letter sent' task", () => {
    homePage.selectSchoolName(schoolLong);

    Logger.log("Selecting 'Confirm improvement grant offer letter sent' task");
    taskList.selectTask("Confirm improvement grant offer letter sent");

    cy.executeAccessibilityTests();

    taskListActions.hasHeader("Confirm improvement grant offer letter sent");
    taskListActions.enterDate("date-improvement-grant-offer-letter-sent", "01", "01", "2024");
    taskListActions.selectButtonOrCheckbox("save-and-continue-button");
    taskList.hasFilterSuccessNotification()
      .hasTaskStatusCompleted("confirm-improvement-grant-offer-letter_status");

    cy.executeAccessibilityTests();

  });
  
});
