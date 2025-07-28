import "cypress-axe";
import { AuthenticationInterceptor } from "../auth/authenticationInterceptor";
import { Logger } from "../common/logger";
import { RuleObject } from "axe-core";

Cypress.Commands.add("getByTestId", (id) => {
    cy.get(`[data-testid="${id}"]`);
});

Cypress.Commands.add("containsByTestId", (id) => {
    cy.get(`[data-testid*="${id}"]`);
});

Cypress.Commands.add("getById", (id) => {
    cy.get(`[id="${id}"]`);
});

Cypress.Commands.add("getByCyData", (id) => {
    cy.get(`[data-cy='${id}']`);
});

Cypress.Commands.add("getByClass", (className) => {
    cy.get(`[class="${className}"]`);
});

Cypress.Commands.add("getByName", (name) => {
    cy.get(`[name="${name}"]`);
});

Cypress.Commands.add("getByRole", (role) => {
    cy.get(`[role="${role}"]`);
});

Cypress.Commands.add("getByLabelFor", (labelFor) => {
    cy.get(`[for="${labelFor}"]`);
})

Cypress.Commands.add("getByRadioOption", (radioText: string) => {
    cy.contains(radioText)
        .invoke('attr', 'for')
        .then((id) => {
            cy.get('#' + id);
        });
})

Cypress.Commands.add("assertChildList", (selector: string, values: string[]) => {
    cy.getByTestId(selector)
        .children()
        .should("have.length", values.length)
        .each((el, i) => {
            expect(el.text()).to.equal(values[i]);
        });
});

Cypress.Commands.add("login", (params) => {
    cy.clearCookies();
    cy.clearLocalStorage();

    // Intercept all browser requests and add our special auth header
    // Means we don't have to use azure to authenticate
    new AuthenticationInterceptor().register(params);
    cy.visit("/");
    cy.url().should('contains', 'schools-identified-for-targeted-intervention')
});

Cypress.Commands.add("executeAccessibilityTests", (ruleOverride?: RuleObject) => {
    Logger.log("Executing the command");
    const continueOnFail = false;

    let ruleConfiguration: RuleObject = {
        region: { enabled: false }
    };

    if (ruleOverride) {
        ruleConfiguration = { ...ruleConfiguration, ...ruleOverride };
    }

    // Ensure that the axe dependency is available in the browser
    Logger.log("Inject Axe");
    cy.injectAxe();

    Logger.log("Checking accessibility");
    cy.checkA11y(undefined, {
        rules: ruleConfiguration,
    }, undefined, continueOnFail);
});

Cypress.Commands.add('typeFast', { prevSubject: 'element' }, (subject: JQuery<HTMLElement>, text: string) => {
    cy.wrap(subject).invoke('val', text);
  });

Cypress.Commands.add('removeProjectIfItExists', (urn: string) => {
    cy.request({
      method: 'DELETE',
      url: `${Cypress.env('msiApi')}/api/SupportProjectApi/support-project/${urn}`,
      headers: {
        'Authorization': `Bearer ${Cypress.env('authKey')}`
      }
    });
});
