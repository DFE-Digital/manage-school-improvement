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

      it('should get the validation errors when form is submitted with invalid data', () => {
        improvementPlan
            .clickRecordOrViewProgress()
            .clickAddReview()
            .validateReviewForm();
    });
});
