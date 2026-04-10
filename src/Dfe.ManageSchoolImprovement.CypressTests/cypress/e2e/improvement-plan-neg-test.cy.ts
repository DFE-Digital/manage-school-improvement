import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import improvementPlan from '../pages/improvementPlan';

describe('User navigate to the Improvement Plan, and  navigate to Add Review page', () => {
    beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .selectProjectFilter("Plymouth Grove Primary School")
            .applyFilters()
            .hasFilterSuccessNotification()
            .selectSchoolName("Plymouth Grove Primary School");
        taskList
            .navigateToTab('Improvement plan')

        improvementPlan    
            .improvementPlanPageLoads()
    });

      it('should get the validation errors when form is submitted with invalid data', () => {
        Logger.log("Validate the review form");
        improvementPlan
            .clickRecordOrViewProgress()
            .clickAddReview()
            .validateReviewForm();
    });
});
