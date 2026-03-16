import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import taskListActions from "cypress/pages/tasks/taskListActions";
import improvementPlan from '../pages/improvementPlan'

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
            .deleteObjectiveSuccessfully();

        cy.executeAccessibilityTests()
    });

    it("should be able to delete improvement plan objectives from Improvement plan tab", () => {
        Logger.log("delete objective details from Improvement plan tab");
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions.addObjective();
        taskListActions.selectButtonOrCheckbox("MarkAsComplete")
        taskListActions.clickButton('save');
        taskList
            .navigateToTab('Improvement plan')

        cy.executeAccessibilityTests();

        improvementPlan
            .hasDeleteObjectiveLink()
            .deleteObjectiveSuccessfully();

        cy.executeAccessibilityTests();
    });

    it("should be able to delete progress reviews when no review has been recorded- Progress not recorded", () => {
        taskList
            .navigateToTab('Improvement plan')
        improvementPlan
            .ifObjectiveExist()

        taskList
            .navigateToTab('Task list')
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions.addObjective();
        taskListActions.selectButtonOrCheckbox("MarkAsComplete")
        taskListActions.clickButton('save');

        Logger.log("delete progress review");
        taskList
            .navigateToTab('Improvement plan')

        improvementPlan
            .clickRecordOrViewProgress()
            .clickAddReview()
            .fillReviewDetails()
            .anotherReviewNeeded()
            .hasStatusTag('Progress not recorded')
            .hasEditOrDeleteReview()
            .deleteReview()

        cy.executeAccessibilityTests()
    });

    it.skip("should not be able to delete progress reviews when review has been recorded -Progress recorded", () => {
        taskList
            .navigateToTab('Improvement plan')
        improvementPlan
            .ifObjectiveExist()

        taskList
            .navigateToTab('Task list')
        taskList.selectTask("Enter improvement plan objectives");
        taskListActions.addObjective();
        taskListActions.selectButtonOrCheckbox("MarkAsComplete")
        taskListActions.clickButton('save');

        Logger.log("delete progress review");
        taskList
            .navigateToTab('Improvement plan')

        improvementPlan
            .clickRecordOrViewProgress()
            .clickAddReview()
            .fillReviewDetails()
            .anotherReviewNeeded()
            .hasStatusTag('Progress partly recorded')
            .hasEditReviewDetailsLink()
            .clickRecordProgressLink()
            .recordProgressForObjective()

        cy.executeAccessibilityTests()

        Logger.log("progress review cannot be deleted once recorded");
        improvementPlan
            .recordOverallProgress()
            .returnToProgressReviews()
            .hasEditReviewDetailsLink()
            .clickEditReviewDetailsLink()
            .noDeleteButtonExist()

        cy.executeAccessibilityTests()
    });
});