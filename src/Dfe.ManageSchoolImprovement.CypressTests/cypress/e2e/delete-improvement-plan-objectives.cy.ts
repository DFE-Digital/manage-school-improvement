import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import taskListActions from "cypress/pages/tasks/taskListActions";
import recordProgress from '../pages/recordProgress'

describe('Delete improvement plan objective', () => {
    beforeEach(() => {
        cy.login();
        homePage
            .rejectCookies()
            .selectProjectFilter("Boston Nursery School")
            .applyFilters()
            .selectSchoolName("Boston Nursery School");

    });

    it("should be able to delete improvement plan objectives from Task list", () => {
        Logger.log("delete objective details from Task list");
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions
            .addAndDeleteObjective()

        cy.executeAccessibilityTests()
    });

    it("should be able to delete improvement plan objectives from Record progress tab", () => {
        Logger.log("delete objective details from Record progress tab");
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions.addObjective();
        taskListActions.checkCheckbox("MarkAsComplete")
        taskListActions.clickButton('save');
        taskList
            .navigateToTab('Termly reviews')

        cy.executeAccessibilityTests();

        recordProgress
            .hasDeleteObjectiveLink()
            .deleteObjectiveSuccessfully();

        cy.executeAccessibilityTests();
    });

    it("should be able to delete progress reviews when no review has been recorded- Progress not recorded", () => {
        taskList
            .navigateToTab('Termly reviews')
        recordProgress
            .ifObjectiveExist()

        taskList
            .navigateToTab('Task list')
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions.addObjective();
        taskListActions.checkCheckbox("MarkAsComplete")
        taskListActions.clickButton('save');

        Logger.log("delete progress review");
        taskList
            .navigateToTab('Termly reviews')

        recordProgress
            .clickRecordProgress()
            .clickAddReview()
            .fillReviewDetails()
            .anotherReviewNeeded()
            .hasStatusTag('Progress not recorded')
            .hasEditOrDeleteReview()
            .deleteReview()

        cy.executeAccessibilityTests()
    });

    it("should not be able to delete progress reviews when review has been recorded -Progress recorded", () => { //skipped due to bug #243624
        taskList
            .navigateToTab('Termly reviews')
        recordProgress
            .ifObjectiveExist()

        taskList
            .navigateToTab('Task list')
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions.addObjective();
        taskListActions.checkCheckbox("MarkAsComplete")
        taskListActions.clickButton('save');

        Logger.log("delete progress review");
        taskList
            .navigateToTab('Termly reviews')

        recordProgress
            .clickRecordProgress()
            .clickAddReview()
            .fillReviewDetails()
            .anotherReviewNeeded()
            .hasStatusTag('Progress partly recorded')
            .hasEditReviewDetailsLink()
            .clickRecordProgressLink()
            .recordProgressForObjective()

        cy.executeAccessibilityTests()

        Logger.log("progress review cannot be deleted once recorded");
        recordProgress
            .recordOverallProgress()
            .returnToProgressReviews()
            .hasEditReviewDetailsLink()
            .clickEditReviewDetailsLink()
            .noDeleteButtonExist()

        cy.executeAccessibilityTests()
    });
});
