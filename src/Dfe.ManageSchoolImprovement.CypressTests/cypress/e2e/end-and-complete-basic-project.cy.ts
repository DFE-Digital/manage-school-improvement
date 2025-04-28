import { Logger } from "cypress/common/logger";
import riseHomePage from "cypress/pages/riseHomePage";
import aboutTheSchool from "cypress/pages/aboutTheSchool";
import taskList from "cypress/pages/taskList";
import confirmEligibility from "cypress/pages/tasks/confirmEligibility";

describe("User completes their newly created project", () => {
  let school: string;
  let urn: string;
  let localAuthority: string;
  let region: string;
  let schoolType: string;
  let faithSchool: string;
  let ofstedRating: string;
  let lastInspectionAboutSchool: string;
  let pfi: string;
  let dateAdded: string;
  let qualityOfEducation: string;
  let leadershipAndManagement: string;
  let assignedTo: string;
  let advisedBy: string;

  before(() => {
    cy.fixture("school-data").then((data) => {
      school = data.school;
      urn = data.urn;
      localAuthority = data.localAuthority;
      region = data.region;
      schoolType = data.schoolType;
      faithSchool = data.faithSchool;
      ofstedRating = data.ofstedRating;
      lastInspectionAboutSchool = data.lastInspectionAboutSchool;
      pfi = data.pfi;

      // 👇 Generate today's date
      const today = new Date();
      dateAdded = today.toLocaleDateString("en-GB", {
        day: "2-digit",
        month: "long",
        year: "numeric",
      });

      qualityOfEducation = data.qualityOfEducation;
      leadershipAndManagement = data.leadershipAndManagement;
      assignedTo = data.assignedTo;
      advisedBy = data.advisedBy;
    });
  });

  beforeEach(() => {
    cy.login();
    cy.url().should("contains", "schools-identified-for-targeted-intervention");
  });

  it("Should complete end-to-end complete project", () => {
    Logger.log("Seleting previously created project");
    riseHomePage.selectSchoolName(school);

    cy.executeAccessibilityTests();

    taskList
      .hasHeader(school)

      .hasDateAdded(dateAdded)
      .hasInspectionDate(lastInspectionAboutSchool)
      .hasQualityOfEducation(qualityOfEducation)
      .hasLeadershipAndManagement(leadershipAndManagement)
      .hasAssignedTo(assignedTo)
      .hasAdvisedBy(advisedBy)

      .hasChangeLinks()

      .hasNav()

      .hasTasks()
      .hasTasksNotStartedElementsPresent();

    Logger.log("Selecting 'Confirm eligiblity' task");
    taskList.selectConfirmEligibility();

    //cy.executeAccessibilityTests(); COMMENTED OUT AS AXE FALSE POSITIVE ARIA-EXPANDED RADIO BUTTON THING

    confirmEligibility.hasHeader("Is this school still eligible for targeted intervention?");
    
    confirmEligibility.selectYesAndContinue();
  });
});
