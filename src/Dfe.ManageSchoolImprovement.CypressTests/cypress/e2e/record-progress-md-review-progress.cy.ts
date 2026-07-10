import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import recordProgress from '../pages/recordProgress';

describe('Matching Decision-Review Progress: User navigate to the Record progress to Add review and record progress', () => {
    beforeEach(() => {
        cy.login();
        homePage
            .rejectCookies()
            .selectProjectFilter("Sunning Hill Primary School")
            .applyFilters()
            .hasFilterSuccessNotification()
            .selectSchoolName("Sunning Hill Primary School");
        taskList
            .navigateToTab('Termly reviews')

        cy.executeAccessibilityTests()

    });

    it('should be able to Add first review successfully when Matching decision is Review progress', () => {
        Logger.log("add First review");
        recordProgress
            .hasMessage('Record progress after each review.')
            .clickRecordProgressButton()
            .clickAddReview()

        cy.executeAccessibilityTests()

        recordProgress
            .fillReviewDetails()
            .anotherReviewNeeded()
            .hasStatusTag('Progress not recorded')
            .hasStatusAndLinksForReviewProgress()
            .hasAddReviewButton()
            .hasChangeNextReviewDateLink()
            .hasReturnToRecordProgressLink()

        cy.executeAccessibilityTests()

    });

    it('should be able to delete the review when the review status is Progress not recorded', () => {
        Logger.log("Delete confirmation page should display on deleting the review");
        recordProgress
            .clickRecordProgressButton()

        cy.executeAccessibilityTests()

        recordProgress
            .hasStatusTag('Progress not recorded')
            .clickEditOrDeleteReviewLink()

        cy.executeAccessibilityTests()
    });

    it('should be able to Record the first review successfully for Review progress', () => {
        Logger.log("record First review");

        recordProgress
            .clickRecordProgressButton()
            .clickRecordProgressLink()

        cy.executeAccessibilityTests()

        recordProgress
            .selectNextSteps('review-school-progress')
            .addAdditionalComments('AdditionalDetails')
            .clickSaveButton()

        cy.executeAccessibilityTests()

        recordProgress
            .hasStatusTag('Progress recorded')
            .reviewSummaryCardVisible()
            .hasMatchingDecision()
            .clickReturnToRecordProgress()

        cy.executeAccessibilityTests()

        recordProgress
            .hasStatusTag('Progress recorded')
            .clickReturnToRecordProgressTab()
            .hasSummaryCard()

        cy.executeAccessibilityTests()
    });

});
