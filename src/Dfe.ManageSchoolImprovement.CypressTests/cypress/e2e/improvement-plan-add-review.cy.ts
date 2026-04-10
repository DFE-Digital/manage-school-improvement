import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import improvementPlan from '../pages/improvementPlan';

describe('User navigate to the Improvement Plan, and  Add progress review', () => {
    beforeEach(() => {
        cy.login();
        homePage
            .rejectCookies()
            .selectProjectFilter("Plymouth Grove Primary School")
            .applyFilters()
            .hasFilterSuccessNotification()
            .selectSchoolName("Plymouth Grove Primary School");
        taskList
            .navigateToTab('Improvement plan')

    });

    it.skip('should display user friendly message if no adviser allocated and no objectives recorded', () => {
        improvementPlan
            .hasNoAdviserMessage('an adviser has been allocated')
            .hasNoObjectivesMessage('all objectives have been added');
    })

    it('should display Record or View Progress button when Improvement task is COMPLETED and Adviser is assigned', () => {
        Logger.log("Record or View Progress button, with Change and Delete links for objectives is visible");
        improvementPlan
            .hasRecordOrViewProgressButton()
            .hasChangeObjectiveLink()
            .hasDeleteObjectiveLink()

        cy.executeAccessibilityTests()
    });

    it('should allow change objectives from improvement plan tab', () => {
        Logger.log("change objective details");
        improvementPlan
            .hasChangeObjectiveLink()
            .clickFirstChangeObjective()

        cy.executeAccessibilityTests()

        improvementPlan
          .updateObjectiveDetails('Updated objective details');
    
        cy.executeAccessibilityTests()
    });

    it('should show initial state when no reviews exist', () => {
        improvementPlan
            .clickRecordOrViewProgress()
            .hasTitle('Progress reviews')
            .hasNoReviewsMessage()

        cy.executeAccessibilityTests()
    });

    it('should be able to Add first review successfully', () => {
        Logger.log("add First review");
        improvementPlan
            .clickRecordOrViewProgress()
            .clickAddReview()
            .fillReviewDetails()
            .anotherReviewNeeded()
            .hasStatusTag('Progress not recorded')
            .hasFirstReviewStatusAndLinks()

        cy.executeAccessibilityTests()

        improvementPlan
            .hasAddReviewButton()
            .hasChangeNextReviewDateLink()
            .hasReturnToImprovementPlanLink()

        cy.executeAccessibilityTests()

    });

    it('check validation on overall progress page', () => {
        Logger.log("check validation on overall progress page");
        improvementPlan
            .clickChangeOverallProgressLink()
            .validateOverallProgressPage();

        cy.executeAccessibilityTests()
    });

    it.skip('should be able to Record the first review successfully', () => {
        Logger.log("record First review");
  
        improvementPlan
            .clickRecordOrViewProgress()
            .clickRecordProgressLink()
            .recordProgressForObjective()
            .recordOverallProgress()
            .hasStatusTag('Progress recorded');

        cy.executeAccessibilityTests()
    });

});
