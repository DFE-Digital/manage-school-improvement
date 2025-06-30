// ***********************************************************
// This example support/e2e.ts is processed and
// loaded automatically before your test files.
//
// This is a great place to put global configuration and
// behavior that modifies Cypress.
//
// You can change the location of this file or turn off
// automatically serving support files with the
// 'supportFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/configuration
// ***********************************************************

// Import commands.js using ES2015 syntax:
import { AuthenticationInterceptorParams } from "cypress/auth/authenticationInterceptor";
import "./commands";
import 'cypress-axe'
import { RuleObject } from "axe-core";
import {EnvAuthKey} from "../constants/cypressConstants";
import registerCypressGrep from '@cypress/grep';
registerCypressGrep(); 

Cypress.on('uncaught:exception', (err, runnable) => {
    if (err.message.includes('Invalid or unexpected token')) {
      return false; // prevent Cypress from failing the test
    }
  });

declare global {
    namespace Cypress {
        interface Chainable {
            getByTestId(id: string): Chainable<Element>;
            containsByTestId(id: string): Chainable<Element>;
            getById(id: string): Chainable<Element>;
            getByCyData(id: string): Chainable<Element>;
            getByClass(className: string): Chainable<Element>;
            getByName(name: string): Chainable<Element>;
            getByRole(role: string): Chainable<Element>;
            getByLabelFor(labelFor: string): Chainable<Element>;
            getByRadioOption(radioText: string): Chainable<Element>;
            login(params?: AuthenticationInterceptorParams): Chainable<Element>;
            loginWithCredentials(): Chainable<Element>;
            assertChildList(selector: string, values: string[]): Chainable<Element>;
            executeAccessibilityTests(ruleExclusions?: RuleObject): Chainable<Element>;
            typeFast(text: string): Chainable<Element>;
        }
    }
}
