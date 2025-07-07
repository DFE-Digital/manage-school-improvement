import notes from "cypress/pages/notes";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";

describe("User navigates to the Notes Tab", () => {
    const noteText = "This is a test note.";
    const updatedNoteText = "This is an updated test note.";

    beforeEach(() => {
        cy.login();
        homePage
            .hasAddSchool()
            .selectFirstSchoolFromList()
        taskList
            .navigateToTab('Notes');

        cy.executeAccessibilityTests()
    });

    it("should add a new note", () => {
        notes.clickAddNote();
        notes.enterNote(noteText);
        notes.saveNote();
        notes.hasSuccessNotification();
    });

    it("should edit the first note", () => {
        notes.editFirstNote(updatedNoteText);
    });
});
