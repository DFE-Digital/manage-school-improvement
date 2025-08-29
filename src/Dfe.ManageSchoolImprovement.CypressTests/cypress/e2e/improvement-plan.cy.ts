import improvementPlan from '../pages/improvementPlan';
import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";

describe('Improvement Plan Progress Recording', () => {
     beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Improvement plan')

        cy.executeAccessibilityTests()
    });

it('should display user friendly message if no adviser allocated and no objectives recorded', () => {
    improvementPlan
        .hasNoAdviserMessage('an adviser has been allocated')
        .hasNoObjectivesMessage('all objectives have been added');
})

        it('should display Record or View Progress button when no reviews exist', () => {
            Logger.log("should display Record or View Progress button");
            improvementPlan
              .hasRecordOrViewProgressButton();

            cy.exec
        });

        it('should not show Record or View Progress button when no adviser is assigned', () => {
            improvementPlan
                 .interceptNoAdviser()
                .recordOrViewProgressButtonNotVisible();
        });



        it('should allow changing objectives from improvement plan tab', () => {
            improvementPlan
            .hasChangeObjectiveLinks()
                .clickFirstChangeObjective()
                .updateObjective('Updated objective text');
        });



        it('should show initial state when no reviews exist', () => {
            improvementPlan
                .clickRecordOrViewProgress()
                .hasNoReviewsMessage();
        });

        it('should validate Add Progress Review form', () => {
            improvementPlan
                .clickRecordOrViewProgress()
                .clickAddReview()
                .validateReviewForm();
        });

        it('should handle recording progress against objectives', () => {
            improvementPlan
                .clickRecordOrViewProgress()
                .clickAddReview()
                .fillReviewDetails()
                .recordProgressForObjective();
        });


  
        it('should display correct status and links based on progress state', () => {
            improvementPlan
                .hasReviewStatusAndLinks();
        });

});