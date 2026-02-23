import projectStatus from "cypress/pages/projectStatus";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";
import improvementPlan from "cypress/pages/improvementPlan";

describe("User navigates to the Project Status Tab", () => {

    beforeEach(() => {
        cy.login();
        homePage
            .rejectCookies()
            .hasAddSchool()
            .selectFirstSchoolFromList()
        Logger.log("User navigates to the Project Status tab")
        taskList
            .navigateToTab('Project status');

        cy.executeAccessibilityTests()
    });

    it("newly added school should show the default status as In progress and no history shown ", () => {
        Logger.log("Check the default status for newly added school");
        projectStatus
            .hasCurrentStatus('In progress')
            .hasNoStatusHistory()

        cy.executeAccessibilityTests()
    });

    it("user should not see an important banner when the project has In Progress status", () => {
        projectStatus
            .hasCurrentStatus('In progress')
            .hasNoBannerForInProgressStatus()

        cy.executeAccessibilityTests()
    })

    it("validation error messages should show for invalid dates and blank details", () => {
        Logger.log("check validation message for date field");
        projectStatus
            .hasCurrentStatus('In progress')
            .clickChangeProjectStatusButton()
            .clickContinueButton()
            .hasHeading('Enter date the project continued')
            .clickContinueButton()
            .hasValidation('Enter a date', 'project-status-in-progress-date-error-link')
            .hasValidation('Enter a date', 'project-status-in-progress-date-error')
            .enterDate('project-status-in-progress-date', '01', '01', '2049')
            .clickContinueButton()
            .hasValidation('Enter today\'s date or a date in the past', 'project-status-in-progress-date-error')
            .enterDate('project-status-in-progress-date', '02', '01', '2024')
            .clickContinueButton()

        Logger.log("check validation message for details field");
        projectStatus
            .clickContinueButton()
            .hasValidation('Enter details', 'project-status-in-progress-details-error-link')
            .hasValidation('Enter details', 'more-detail-error')

        cy.executeAccessibilityTests()
    });

    it("user could update the details for In progress status", () => {
        projectStatus
            .clickChangeProjectStatusButton()
            .hasHeading('Change project status')
            .checkedStatusBydefault('in-progress')

        cy.executeAccessibilityTests()
        Logger.log("update details for In progress status");
        projectStatus
            .clickContinueButton()
            .hasHeading('Enter date the project continued')
            .enterDate('project-status-in-progress-date', '01', '01', '2024')

        cy.executeAccessibilityTests()

        projectStatus
            .clickContinueButton()
            .hasPageHeading('Enter details')
            .enterDetails('project-status-in-progress-details', 'Project is progressing as expected.')

        cy.executeAccessibilityTests()
        projectStatus
            .clickContinueButton()
            .hasPageHeading('Check your answers')
            .hasCheckYourAnswersPageWithDetails()

        cy.executeAccessibilityTests()
        projectStatus
            .clickSaveAndContinueButton()
            .hasSuccessNotification()
            .getUpdatedStatusWithDetails('In progress')

        cy.executeAccessibilityTests()
    });

    it("user could change the project status to Paused and see the change history", () => {
        projectStatus
            .clickChangeProjectStatusButton()
            .hasHeading('Change project status')
            .selectStatusPaused()

        cy.executeAccessibilityTests()

        Logger.log("update details for Paused status");
        projectStatus
            .clickContinueButton()
            .hasHeading('Enter date the intervention paused')
            .enterDate('project-status-paused-date', '03', '12', '2024')

        cy.executeAccessibilityTests()

        projectStatus
            .clickContinueButton()
            .hasPageHeading('Enter details')
            .enterDetails('project-status-paused-details', 'Project is paused due to unforeseen circumstances.')

        cy.executeAccessibilityTests()
        projectStatus
            .clickContinueButton()
            .hasPageHeading('Check your answers')
            .hasCheckYourAnswersPageWithDetails()

        cy.executeAccessibilityTests()
        projectStatus
            .clickSaveAndContinueButton()
            .hasSuccessNotification()
            .getUpdatedStatusWithDetails('Paused')
            .getProjectStatusChangeHistory('Paused')

        cy.executeAccessibilityTests()
    });

    it("user should only View Progress of Improvement Plan when project status is Paused ", () => {
        projectStatus
            .hasCurrentStatus('Paused')
        taskList
            .navigateToTab('Improvement plan')

        Logger.log("User could only see View Progress when project status is Paused");
        improvementPlan
            .hasViewProgressButton()

        cy.executeAccessibilityTests()
    });

    it("user should see an important banner when the project is Paused or Stopped", () => {
        projectStatus
            .hasCurrentStatus('Paused')
            .bannerDisplayedForPausedOrStoppedStatus('Paused')

        cy.executeAccessibilityTests()
    })

    it("user could change the project status to Stopped and see the change status history timeline", () => {
        projectStatus
            .clickChangeProjectStatusButton()
            .hasHeading('Change project status')
            .selectStatusStopped()

        cy.executeAccessibilityTests()

        Logger.log("update details for Stopped status");
        projectStatus
            .clickContinueButton()
            .enterDate('project-status-stopped-date', '02', '02', '2025')

        cy.executeAccessibilityTests()

        projectStatus
            .clickContinueButton()
            .hasPageHeading('Enter details')
            .enterDetails('project-status-stopped-details', 'Project is stopped due to unforeseen circumstances.')

        cy.executeAccessibilityTests()
        projectStatus
            .clickContinueButton()
            .hasPageHeading('Check your answers')
            .hasCheckYourAnswersPageWithDetails()

        cy.executeAccessibilityTests()
        projectStatus
            .clickSaveAndContinueButton()
            .hasSuccessNotification()
            .getUpdatedStatusWithDetails('Stopped')

            Logger.log("Check the project status change history timeline for Stopped status");
        projectStatus    
            .getProjectStatusChangeHistory('Stopped')

        cy.executeAccessibilityTests()
    });

    it("user should not be able to record the Improvement plan if the project status is Stopped", () => {
        projectStatus
            .hasCurrentStatus('Stopped')
        taskList
            .navigateToTab('Improvement plan')

        Logger.log("User could only see View Progresswhen project status is Stopped");
        improvementPlan
            .hasViewProgressButton()

        cy.executeAccessibilityTests()
    });

    it("user could change the project status back to In progress from Stopped status", () => {
        projectStatus
            .hasCurrentStatus('Stopped')
            .clickChangeProjectStatusButton()
            .hasHeading('Change project status')
            .selectStatusInProgress()

        cy.executeAccessibilityTests()

        projectStatus
            .clickContinueButton()
            .hasHeading('Enter date the project continued')
            .enterDate('project-status-in-progress-date', '15', '01', '2026')

        cy.executeAccessibilityTests()

        projectStatus
            .clickContinueButton()
            .hasPageHeading('Enter details')
            .enterDetails('project-status-in-progress-details', 'Project is progressing as expected.')

        cy.executeAccessibilityTests()
        projectStatus
            .clickContinueButton()
            .hasPageHeading('Check your answers')
            .hasCheckYourAnswersPageWithDetails()

        cy.executeAccessibilityTests()
        Logger.log("save and return after updating In progress status and check saved details");
        projectStatus
            .clickSaveAndContinueButton()
            .hasSuccessNotification()
            .getUpdatedStatusWithDetails('In progress')

        cy.executeAccessibilityTests()
    });
});
