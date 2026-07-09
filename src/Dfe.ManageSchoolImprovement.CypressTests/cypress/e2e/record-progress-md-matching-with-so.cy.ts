import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import recordProgress from '../pages/recordProgress';

describe('Matching decision-Match with SO, User navigate to the Record progress to Add review and record progress', () => {
    beforeEach(() => {
        cy.login();
        homePage
            .rejectCookies()
            .selectProjectFilter("Plymouth Grove Primary School")
            .applyFilters()
            .hasFilterSuccessNotification()
            .selectSchoolName("Plymouth Grove Primary School");
        taskList
            .navigateToTab('Termly reviews')

        cy.executeAccessibilityTests()

    });

    it.skip('should display user friendly message if no adviser allocated and no objectives recorded', () => { //seperate data needed for this test
        recordProgress
            .hasSchoolsMatchedWithSOMessage('Schools matched with a supporting organisation')
            .hasReviewProgressSchoolsMessage('Review progress schools');
    })

    it('should display Record Progress button when Improvement task is COMPLETED and Adviser is assigned', () => {
        Logger.log("Record Progress button, with Change and Delete links for objectives is visible");
        recordProgress
            .hasMessage('Record progress on the improvement plan objectives after each review.')
            .hasRecordProgressButton()
            .hasChangeObjectiveLink()
            .hasDeleteObjectiveLink()

        cy.executeAccessibilityTests()
    });

    it('should allow change objectives from record progress tab', () => {
        Logger.log("change objective details");
        recordProgress
            .hasChangeObjectiveLink()
            .clickFirstChangeObjective()

        cy.executeAccessibilityTests()

        recordProgress
            .updateObjectiveDetails('Updated objective details');

        cy.executeAccessibilityTests()
    });

    it('should show initial state when no reviews exist', () => {
        recordProgress
            .clickRecordProgress()

        cy.executeAccessibilityTests()

        recordProgress
            .hasTitle('Progress reviews')
            .hasNoReviewsMessage()

        cy.executeAccessibilityTests()
    });

    it('should be able to Add first review successfully when Matching decision is Match with a SO', () => {
        Logger.log("add First review");
        recordProgress
            .clickRecordProgress()
            .clickAddReview()

        cy.executeAccessibilityTests()

        recordProgress
            .fillReviewDetails()
            .anotherReviewNeeded()
            .hasStatusTag('Progress not recorded')
            .hasFirstReviewStatusAndLinksForSO()

        cy.executeAccessibilityTests()

        recordProgress
            .hasAddReviewButton()
            .hasChangeNextReviewDateLink()
            .hasReturnToRecordProgressLink()

        cy.executeAccessibilityTests()

    });

    it('check validation on overall progress page', () => {
        Logger.log("check validation on overall progress page");
        recordProgress
            .clickChangeOverallProgressLink()

        cy.executeAccessibilityTests()

        recordProgress
            .validateOverallProgressPage();

        cy.executeAccessibilityTests()
    });

    it.skip('should be able to Record the first review successfully for matching SO', () => { //skip due to a bug #243624
        Logger.log("record First review");

        recordProgress
            .clickRecordProgress()
            .clickRecordProgressLink()
            .recordProgressForObjective()
            .recordOverallProgress()
            .hasStatusTag('Progress recorded');

        cy.executeAccessibilityTests()
    });
});
