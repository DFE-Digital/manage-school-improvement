import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import recordProgress from '../pages/recordProgress';

describe('User navigate to the Record Progress, and  navigate to Add Review page', () => {
    beforeEach(() => {
        cy.login();
        homePage
            .acceptCookies()
            .selectProjectFilter("Plymouth Grove Primary School")
            .applyFilters()
            .hasFilterSuccessNotification()
            .selectSchoolName("Plymouth Grove Primary School");
        taskList
            .navigateToTab('Record progress')

        recordProgress
            .recordProgressPageLoads()
    });

    it('should get the validation errors when form is submitted with invalid data', () => {
        Logger.log("Validate the review form");
        recordProgress
            .clickRecordProgress()
            .clickAddReview()
            .validateReviewForm();
    });
});
