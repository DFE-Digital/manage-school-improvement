import projectStatus from "cypress/pages/projectStatus";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import { Logger } from "cypress/common/logger";
import improvementPlan from "cypress/pages/improvementPlan";
import taskListActions from "cypress/pages/tasks/taskListActions";

describe("User navigates to the Status and Eligibility Tab", () => {

    beforeEach(() => {
        cy.login();
        homePage
            .rejectCookies()
            .hasAddSchool()
            .selectProjectFilter("Plymouth Grove Primary")
            .applyFilters()
            .selectFirstSchoolFromList()
        Logger.log("User navigates to the Status and Eligibility tab")
        taskList
            .navigateToTab('Status and eligibility');

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
            .clickChangeStatusAndEligibilityButton()
            .clickContinueButton()
        
            cy.executeAccessibilityTests()

        projectStatus    
            .projectEligibleForIntervention('Yes')
            .enterDateSupportIsDueToEnd('01', '01', '2048')
             .clickContinueButton()

            cy.executeAccessibilityTests()

            projectStatus
                .hasPageHeading('Confirm the change')
                .clickContinueButton()
                .hasValidation('Enter a date', 'status-eligibility-change-date-error-link')
                .enterDateProjectStatusOrEligibilityChange('01', '01', '2025')

            .clickContinueButton()

               Logger.log("check validation message for details field");
             
            projectStatus   
                .hasPageHeading('Enter details about the change')
                .clickContinueButton()
                .hasValidation('Enter details', 'change-details-error-link')
                .enterDetails('change-details', 'The change is required')
                .clickContinueButton()
        
        projectStatus
           .hasCheckYourAnswersPageWithDetails()

        cy.executeAccessibilityTests()
    });

    it("user should update the details for Eligible and In progress status", () => {
        projectStatus
            .clickChangeStatusAndEligibilityButton()
            .hasHeading('Change project status')
            .checkedStatusBydefault('in-progress')
            .clickContinueButton()

        cy.executeAccessibilityTests()
        Logger.log("update details for In progress status");
       projectStatus    
            .projectEligibleForIntervention('Yes')
            .enterDateSupportIsDueToEnd('01', '01', '2048')
             .clickContinueButton()

            cy.executeAccessibilityTests()

            projectStatus
                .hasPageHeading('Confirm the change')
                .enterDateProjectStatusOrEligibilityChange('01', '01', '2025')
                .clickContinueButton()

               Logger.log("check validation message for details field");
             
            projectStatus   
                .hasPageHeading('Enter details about the change')
                .enterDetails('change-details', 'The change is required')
                .clickContinueButton()
        
        projectStatus
           .hasCheckYourAnswersPageWithDetails()

        cy.executeAccessibilityTests()
        projectStatus
            .clickSaveAndContinueButton()
            .getUpdatedStatusWithDetails('In progress')

        cy.executeAccessibilityTests()
    });

    it("user should change the project status to Paused, eligibility Yes and see the change history", () => {
        projectStatus
            .clickChangeStatusAndEligibilityButton()
            .hasHeading('Change project status')
            .selectStatusPaused()

        cy.executeAccessibilityTests()

        Logger.log("update details for Paused status");
        projectStatus
            .clickContinueButton()
          .projectEligibleForIntervention('Yes')
            .enterDateSupportIsDueToEnd('01', '01', '2048')
             .clickContinueButton()

            cy.executeAccessibilityTests()

            projectStatus
                .hasPageHeading('Confirm the change')
                .enterDateProjectStatusOrEligibilityChange('01', '01', '2025')
                .clickContinueButton()

               Logger.log("check validation message for details field");
             
            projectStatus   
                .hasPageHeading('Enter details about the change')
                .enterDetails('change-details', 'The change is required')
                .clickContinueButton()
        
        projectStatus
           .hasCheckYourAnswersPageWithDetails()
        cy.executeAccessibilityTests()
        projectStatus
            .clickSaveAndContinueButton()
            .getUpdatedStatusWithDetails('Paused')
            .getProjectStatusChangeHistory('Paused')

        Logger.log("Important banner should display when status is Paused");    
        projectStatus
            .hasCurrentStatus('Paused')
            .bannerDisplayedForPausedOrStoppedStatus('Paused')

        cy.executeAccessibilityTests()    

    });

    it("tasks in task list should be ready only when the project status is Paused ", () => {
        Logger.log("cannot progress the task when the project status is Paused");
        projectStatus
            .hasCurrentStatus('Paused')
        taskList.navigateToTab('Task list')    

        Logger.log("Phase 1 task cannot progress");
        taskList.hasTaskStatusCannotProgress("confirm_responsible_body_status");

        taskList.selectTask("Make initial contact with the responsible body");
        taskListActions
            .hasDisplayedImportantBanner('paused')

        cy.executeAccessibilityTests()

        Logger.log("Phase 2 task cannot progress");
        taskList.clickBackLink();
        taskList.hasTaskStatusCannotProgress("record-school-visit-date_status");
        taskList.selectTask("Record date of initial visit");
        taskListActions
            .hasDisplayedImportantBanner('paused')

        cy.executeAccessibilityTests()

        Logger.log("Phase 3 task cannot progress");
        taskList.clickBackLink();
        taskList.hasTaskStatusCannotProgress("enter-improvement-plan-objectives_status");
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions
            .hasDisplayedImportantBanner('paused')

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

    it("user could change the project status to Stopped, with eligibility No and see the change status history timeline", () => {
        projectStatus
            .clickChangeStatusAndEligibilityButton()
            .hasHeading('Change project status')
            .selectStatusStopped()

        cy.executeAccessibilityTests()

        Logger.log("update details for Stopped status");
        projectStatus
            .clickContinueButton()
            .projectEligibleForIntervention('No')
            .enterDateSupportIsDueToEnd('01', '01', '2048')
             .clickContinueButton()

            cy.executeAccessibilityTests()

            projectStatus
                .hasPageHeading('Confirm the change')
                .enterDateProjectStatusOrEligibilityChange('01', '01', '2025')
                .clickContinueButton()

               Logger.log("check validation message for details field");
             
            projectStatus   
                .hasPageHeading('Enter details about the change')
                .enterDetails('change-details', 'The change is required')
                .clickContinueButton()
        
        projectStatus
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

    it("tasks in task list should be ready only when the project status is Stopped ", () => {
        Logger.log("cannot progress the task when the project status is Stopped");
        projectStatus
            .hasCurrentStatus('Stopped')
        taskList.navigateToTab('Task list')    

        Logger.log("Phase 1 task cannot progress");
        taskList.hasTaskStatusCannotProgress("confirm_responsible_body_status");

        taskList.selectTask("Make initial contact with the responsible body");
        taskListActions
            .hasDisplayedImportantBanner('stopped')

        cy.executeAccessibilityTests()

        Logger.log("Phase 2 task cannot progress");
        taskList.clickBackLink();
        taskList.hasTaskStatusCannotProgress("record-school-visit-date_status");
        taskList.selectTask("Record date of initial visit");
        taskListActions
            .hasDisplayedImportantBanner('stopped')

        cy.executeAccessibilityTests()

        Logger.log("Phase 3 task cannot progress");
        taskList.clickBackLink();
        taskList.hasTaskStatusCannotProgress("enter-improvement-plan-objectives_status");
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions
            .hasDisplayedImportantBanner('stopped')

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
            .clickChangeStatusAndEligibilityButton()
            .hasHeading('Change project status')
            .selectStatusInProgress()

        cy.executeAccessibilityTests()

        projectStatus
           .clickContinueButton()
          .projectEligibleForIntervention('Yes')
            .enterDateSupportIsDueToEnd('01', '01', '2048')
             .clickContinueButton()

            cy.executeAccessibilityTests()

            projectStatus
                .hasPageHeading('Confirm the change')
                .enterDateProjectStatusOrEligibilityChange('01', '01', '2025')
                .clickContinueButton()

               Logger.log("check validation message for details field");
             
            projectStatus   
                .hasPageHeading('Enter details about the change')
                .enterDetails('change-details', 'The change is required')
                .clickContinueButton()
        
        projectStatus
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
