import { Logger } from "cypress/common/logger";
describe("Navigate through footer links", () => {
    beforeEach(() => {
        cy.login();
        cy.visit("/");
    });

    it('user should be able to navigate to the footer links', () => {
        cy.get('.govuk-footer').should('be.visible');

        Logger.log("Unified Intake section");
        cy.get('h2').should('contain', 'Problems and feedback');
        cy.contains('a', 'Report a problem or give feedback').should('be.visible')

        Logger.log("Get accessibility and cookies link");
        cy.contains('a', 'Accessibility statement').should('be.visible').click()
        Logger.log("Acessibility statement link opened in another tab");
        
        Logger.log("Cookies preferences opens in the same tab");
        cy.contains('a', 'Cookies').should('be.visible').click()
        // Get cookies header to allow url chance to update in Cypress
        cy.get('h1').should('contain', 'Cookie preferences')
        cy.url().then(href => {
            expect(href).includes('cookie')
        });

        Logger.log("Get license and copyright link");
        cy.contains('a', 'Open Government Licence v3.0').should('be.visible')
        cy.contains('a', '© Crown copyright').should('be.visible')
    });
});
