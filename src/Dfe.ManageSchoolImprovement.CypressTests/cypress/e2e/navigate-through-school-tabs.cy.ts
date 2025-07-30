import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import aboutTheSchool from "cypress/pages/aboutTheSchool";
import ofstedReports from "cypress/pages/ofstedReports";
import contacts from "cypress/pages/contacts";
import caseStudy from "cypress/pages/caseStudy";
import engagementConcern from "cypress/pages/engagementConcern";
import notes from "cypress/pages/notes";
import improvementPlan from "cypress/pages/improvementPlan";

describe("User select the school", () => {
    beforeEach(() => {
        cy.login();
        homePage
            .hasAddSchool()
            .selectFirstSchoolFromList()
        taskList
            .hasTabs()

        cy.executeAccessibilityTests()

    });

    it("should be able to navigate to the Task list tab by default", () => {
        Logger.log("User navigated to Task list");
        taskList
            .hasTaskListHeading('Phase 1: Identifying')
            .hasTaskListHeading('Phase 2: Initial diagnosis and matching')
            .hasTaskListHeading('Phase 3: Diagnosis and planning')

        cy.executeAccessibilityTests()
    });

    it("should be able to navigate to the About the school tab", () => {
        Logger.log("User navigates to the About the school tab");
        taskList
            .navigateToTab('About the school');
        aboutTheSchool
            .schoolOverivewSectionVisible()

        cy.executeAccessibilityTests()
    });

    it("should be able to navigate to the Ofsted reports tab", () => {
        Logger.log("User navigates to the Ofsted reports tab");
        taskList
            .navigateToTab('Ofsted reports');
        ofstedReports
            .hasOfstedReport('Most recent Ofsted report')
            .hasOfstedReport('Previous Ofsted report')
            .ofstedReportsPageVisible()

        cy.executeAccessibilityTests()
    });

    it.only("should be able to navigate to the Improvement plan tab", () => {
        Logger.log("User navigates to the Improvement plan tab");
        taskList
            .navigateToTab('Improvement plan');
        improvementPlan
            .hasHeading('Improvement plan')

        cy.executeAccessibilityTests()
    });


    it("should be able to navigate to the Contacts tab", () => {
        Logger.log("User navigates to the Contacts tab");
        taskList
            .navigateToTab('Contacts');
        contacts
            .hasAddContactButton()

        cy.executeAccessibilityTests()
    });

    it("should be able to navigate to the Case Study tab", () => {
        Logger.log("User navigates to the Case Study tab");
        taskList
            .navigateToTab('Case Study');
        caseStudy
            .CaseStudyPageVisible()

        cy.executeAccessibilityTests()
    });

    it("should be able to navigate to the Engagement concern tab", () => {
        Logger.log("User navigates to the Engagement concern tab");
        taskList
            .navigateToTab('Engagement concern');
        engagementConcern
            .engagementConcernPageDisplayed()

        cy.executeAccessibilityTests()
    });

    it("should be able to navigate to the Notes tab", () => {
        Logger.log("User navigates to the Notes tab");
        taskList
            .navigateToTab('Notes');
        notes
            .hasHeading('Project notes')
            .hasAddNoteButton()

        cy.executeAccessibilityTests()
    });

});   
