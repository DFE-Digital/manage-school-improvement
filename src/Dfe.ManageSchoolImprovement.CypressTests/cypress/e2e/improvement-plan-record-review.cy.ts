import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";
import improvementPlan from '../pages/improvementPlan';

describe('User navigate to the Improvement Plan, and record progress review', () => {
    beforeEach(() => {
      cy.login();
        homePage
            .acceptCookies()
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Improvement plan')
    
    });

     it('should be able to Record first review successfully', () => {
        improvementPlan
            .clickRecordOrViewProgress()
            .clickRecordProgressLink()
            .validateOverallProgress()
            .recordOverallProgress()
            .recordProgressForObjective()
            .recordProgressForObjective()

        cy.executeAccessibilityTests()    

        improvementPlan
            .hasStatusTag('Progress recorded');   
    });
});
