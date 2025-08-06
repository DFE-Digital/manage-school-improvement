## Cypress Testing

### Test Setup

The Cypress tests are designed to run against the front-end of the application. To set up the tests, you need to provide a configuration file named `cypress.env.json` with the following information:

```javascript
{
    "url": "<enter frontend URL>",
    "username": "<enter the user you want to run the tests with>",
    "api": "<enter backend URL>",
    "apiKey": "<enter API key for backend>",
    "authKey": "<enter key set for the CypressTestSecret>"
}
```

While it is possible to pass these configurations through commands, it is easier to store them in the configuration file.

#### Authentication

The authentication is invoked in every test using the `login` command:

```javascript
beforeEach(() => {
    cy.login();
});
```

Intercepts all browser requests and adds a special auth header using the `authKey`. Make sure you set the `CypressTestSecret` in your app, and it matches the `authKey` in the `cypress.env.json` file.

### Test Execution

If you have a `cypress.env.json` file, the `cy:open` and `cy:run` commands will automatically pick up the configuration.

Navigate to the `Dfe.ManageSchoolImprovement.CypressTests` directory:

```
cd Dfe.ManageSchoolImprovement.CypressTests/
```

To open the Cypress Test Runner, run the following command:

```
npm run cy:open
```

To execute the tests in headless mode, use the following command (the output will log to the console):

```
npm run cy:run
```

### Accessibility Testing

The `executeAccessibilityTests` command is implemented in Cypress and is used to perform accessibility tests on a web application. It utilises the Axe accessibility testing library to check for accessibility issues based on the specified criteria.

#### Usage

To use this command, simply call `executeAccessibilityTests()` in your Cypress test code. Here's an example:

```javascript
it("should perform accessibility tests", () => {
    // Perform actions and assertions on your web application
    // ...

    // Execute accessibility tests
    cy.executeAccessibilityTests();

    // Continue with other test logic
    // ...
});
```

#### Command Details

The `executeAccessibilityTests` command under "support/commands.ts"

This will run all accessibility rules provided by the framework

