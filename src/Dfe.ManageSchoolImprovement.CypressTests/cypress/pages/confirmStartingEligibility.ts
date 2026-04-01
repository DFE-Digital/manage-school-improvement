class ConfirmStartingEligibilityPage {
    public hasHeader(header: string = "Is this school eligible to begin targeted intervention?"): this {

        cy.get("h1").contains(header);

        return this;
    }

    public selectEligibility(eligible: boolean): this {
        const option = eligible ? "Yes" : "No";
        cy.getByRadioOption(option).check();

        return this;
    }

    public clickContinue(): this {
        cy.getById('save-and-continue-button').contains('Continue').click();

        return this;
    }

    public eligibilityChangeDate(date: string): this {
        cy.get('h1').should('contain', 'When did the school\'s eligibility change?');
        cy.getByName('eligibility-check-date-day').clear();
        cy.getByName('eligibility-check-date-month').clear();
        cy.getByName('eligibility-check-date-year').clear();
        const [day, month, year] = date.split('/');
        cy.getByName('eligibility-check-date-day').type(day);
        cy.getByName('eligibility-check-date-month').type(month);
        cy.getByName('eligibility-check-date-year').type(year);

        return this;
    }

    public eligibilityChangeReason(reason: string): this {
        cy.get('h1').should('contain', 'Explain the reasons for the eligibility change');
        cy.getByName('eligibility-check-details').type(reason);

        return this;
    }

    public checkYourAnswers(): this {
        cy.url().should('include', '/eligibility-check-answers');
        cy.get('h1').should('contain', 'Check your answers');
        cy.get('dl').within(() => {
            cy.get('dt').contains('Eligibility').should('exist')
                .siblings('dd').find('a').contains('Change').should('exist');
            cy.get('dt').contains('Date of change').should('exist')
                .siblings('dd').find('a').contains('Change').should('exist');
            cy.get('dt').contains('Details').should('exist')
                .siblings('dd').find('a').contains('Change').should('exist');
        });

        cy.contains('Page not found').should('not.exist');

        return this;
    }

    public clickSaveAndComplete(): this {
        cy.getById('save-and-continue-button').contains('Save and complete').click();

        return this;
    }

    public schoolIsNotEligiblePage(): this {
        cy.url().should('include', '/eligibility-not-eligible-guidance');
        cy.get('h1').should('contain', 'School is not eligible');
        cy.contains('Page not found').should('not.exist');

        return this;
    }

    public clickBackToProjectListButton(): this {
        cy.url().should('include', '/eligibility-not-eligible-guidance');
        cy.getById('save-and-continue-button').contains('Back to project list').click();
        cy.url().should('include', '/schools-identified-for-targeted-intervention');

        return this;
    }

    public hasValidation(valText: string, id: string): this {
        cy.getById(id).contains(valText);

        return this;
    }

    public clickChangeLink(fieldName: string): this {
        cy.get('dl').within(() => {
            cy.get('dt').contains(fieldName).should('exist')
                .siblings('dd').find('a').contains('Change').click();
        });

        return this;
    }
}

const confirmStartingEligibility = new ConfirmStartingEligibilityPage();

export default confirmStartingEligibility;  