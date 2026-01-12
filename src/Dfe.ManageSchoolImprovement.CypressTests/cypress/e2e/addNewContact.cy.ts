import contactDetails from "cypress/pages/contactDetails";
import contacts from "cypress/pages/contacts";
import homePage from "cypress/pages/homePage";
import taskList from "cypress/pages/taskList";

describe("User navigates to the Contacts Tab", () => {
    const nameErrorMessage = "Enter a name";
    const sOTypeErrorMessage = "Select an organisation type";
    const emailErrorMessage = "Enter an email address";
    const roleErrorMessage = "Select a role";
    const jobTitleErrorMessage = "Enter a job title";
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

    it("should show contacts details associated with the school ", () => {
        contacts
            .hasAddContactButton()
            .hasDefaultContactCardsVisible();

        cy.executeAccessibilityTests()
    });

    it("should show validation message for not selecting Organisation ", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .clickContinue()

            .orgErrorMessage(sOTypeErrorMessage);

        cy.executeAccessibilityTests()
    });

    it("should show validation message for not selecting Role and job title ", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .selectOrganisation('organisation-type-school')
            .clickContinue()
            .clickContinue()

            .roleErrorMessage(roleErrorMessage)
            .selectRoleandEnterDetails('other-job-title')
            .clickContinue()
            .roleTextFieldMissingErrorMessage(jobTitleErrorMessage);

        cy.executeAccessibilityTests()
    });

    it("should show validation message for mandatory fields: Name, Email and Phone number ", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .selectOrganisation('organisation-type-supporting-organisation')
            .clickContinue()
            .selectSoOrganisation('headteacher')
            .clickContinue()

        contactDetails
            .clickSaveAndReturnButton()
            .errorMessageForField('name', nameErrorMessage)
            .errorMessageForField('email-address', emailErrorMessage)
            .enterInvalidPhoneNumber()
            .clickSaveAndReturnButton()
            .errorMessageForField('phone', phoneErrorMessage);

        cy.executeAccessibilityTests()
    });

    it("should be able to add new Contact for organisation- School", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .hasOrganisationVisible()
            .selectOrganisation('organisation-type-school')
            .clickContinue()
            .hasSchoolRolesVisible()
            .selectRoleandEnterDetails('headteacher-(interim)')
            .clickContinue()

        contactDetails
            .enterRandomContactDetails()
            .clickSaveAndReturnButton()

        contacts
          .hasContactAddedSuccessMessage('Contact added')
          .detailsVisibleInContactCard();

        cy.executeAccessibilityTests()
    });

    it("should be able to add new Contact for organisation- Supporting organisation", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .hasOrganisationVisible()
            .selectOrganisation('organisation-type-supporting-organisation')
            .clickContinue()
            .hasSORolesVisible()
            .selectRoleandEnterDetails('accounting-officer')
            .clickContinue()

        contactDetails
            .enterRandomContactDetails()
            .clickSaveAndReturnButton()

        contacts.hasContactAddedSuccessMessage('Contact added')

        cy.executeAccessibilityTests()
    });

    it("should be able to add new Contact for organisation- Governance bodies", () => {
        contacts
            .hasAddContactButton()
            .clickAddContact()
            .hasOrganisationVisible()
            .selectOrganisation('organisation-type-governance-bodies')
            .clickContinue()
            .hasGovBodiesRolesVisible()
            .selectRoleandEnterDetails('other-body')
            .clickContinue()

        contactDetails
            .enterRandomContactDetails()
            .enterJobTitleForGovBody('JobTitle')
            .clickSaveAndReturnButton()

        contacts.hasContactAddedSuccessMessage('Contact added')

        cy.executeAccessibilityTests()
    });


    it.only("should be able to change the added contact", () => {
        contacts
            .clickChangeButton()
            .editContactPageVisible()
            .clickContinue()
            .clickContinue()

        contactDetails
            .editFullName('TestName Edited')

        contacts
            .hasContactAddedSuccessMessage('Contact updated');

        cy.executeAccessibilityTests()
    })
});
