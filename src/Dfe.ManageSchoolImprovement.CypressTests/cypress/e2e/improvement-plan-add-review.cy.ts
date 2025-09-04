import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import improvementPlan from '../pages/improvementPlan';

describe('User navigate to the Improvement Plan, and  Add progress review', () => {
    beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Improvement plan')

        improvementPlan    
            .improvementPlanPageLoads()
    });

    it.skip('should display user friendly message if no adviser allocated and no objectives recorded', () => {
        improvementPlan
            .hasNoAdviserMessage('an adviser has been allocated')
            .hasNoObjectivesMessage('all objectives have been added');
    })

    it('should display Record or View Progress button when Improvement task is COMPLETED and Adviser is assigned', () => {
        Logger.log("should display Record or View Progress button");
        improvementPlan
            .hasRecordOrViewProgressButton()

        cy.executeAccessibilityTests()
    });

    it('should allow changing objectives from improvement plan tab', () => {
        improvementPlan
            .hasChangeObjectiveLinks()
            .clickFirstChangeObjective()
            .updateObjectiveDetails('Updated details text')

        cy.executeAccessibilityTests()
    });

    it('should show initial state when no reviews exist', () => {
        improvementPlan
            .clickRecordOrViewProgress()
            .hasTitle('Progress reviews')
            .hasNoReviewsMessage()

        cy.executeAccessibilityTests()
    });

    it('should get the validation errors when form is submitted with invalid data', () => {
        improvementPlan
            .clickRecordOrViewProgress()
            .clickAddReview()
            .validateReviewForm();
    });
    
    it('should be able to Add first review successfully', () => {
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

});
