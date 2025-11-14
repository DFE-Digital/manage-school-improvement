import contactDetails from "cypress/pages/contactDetails";
import contacts from "cypress/pages/contacts";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";

describe("User navigates to the Contacts Tab", () => {
    const nameErrorMessage = "Enter a name";
    const organisationErrorMessage = "Enter an organisation";
    const emailErrorMessage = "Enter an email address";
    const phoneErrorMessage = "Enter a valid phone number";

    beforeEach(() => {
        cy.login();
        homePage
            .hasAddSchool()
            .selectFirstSchoolFromList()

        taskList
            .navigateToTab('Contacts');

        cy.executeAccessibilityTests()
    });

    it("should be able to add new Contact", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .hasRolesVisible()
            .selectRole('other-role')
            .clickContinue()

        contactDetails
            .enterRandomContactDetails()
            .clickAddContactButton()

        contacts.hasContactAddedSuccessMessage('Contact added')

        cy.executeAccessibilityTests()
    });

    it("should show validation message for mandatory fields: Name, Organisation and Email ", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .selectRole('other-role')
            .clickContinue()

        contactDetails
            .clickAddContactButton()
            .errorMessageForField('name', nameErrorMessage)
            .errorMessageForField('organisation', organisationErrorMessage)
            .errorMessageForField('email-address', emailErrorMessage);

        cy.executeAccessibilityTests()
    });

    it("should show validation message for invalid phone number ", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .selectRole('other-role')
            .clickContinue()

        contactDetails
            .enterInvalidPhoneNumber()
            .clickAddContactButton()
            .errorMessageForField('phone', phoneErrorMessage);

        cy.executeAccessibilityTests()
    });

    it("should be able to change the added contact", () => {
        contacts
            .clickChangeButton()
            .clickContinue()

        contactDetails
            .editOrganisation('Edited Organisation')

        contacts
            .hasContactAddedSuccessMessage('Contact updated');

        cy.executeAccessibilityTests()
    })
});
