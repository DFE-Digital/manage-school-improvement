class ImprovementPlan {

    public improvementPlanPageLoads(): this {
        cy.url().should('contain', '/improvement-plan')
        return this;
    }

    public hasNoAdviserMessage(expectedMessage: string): this {
        cy.get('.govuk-list > :nth-child(1)').should('contain.text', expectedMessage);
        return this;
    }

    public hasTitle(expectedTitle: string): this {
        cy.get('h1').should('contain.text', expectedTitle);
        return this;
    }

    public hasAddReviewButton(): this {
        cy.get('a').contains('Add review').should('exist');
        return this;
    }

    public selectAddAnotherObjective(buttonText: string): this {
        cy.get('.govuk-button--secondary').contains(buttonText).click();
        return this;
    }

    public clickRecordProgressLink(): this {
        cy.get('a').contains('Record progress').click()
        cy.url().should('include', '/progress-reviews');
        
        return this;
    }

    public hasNoObjectivesMessage(expectedMessage: string): this {
        cy.get('.govuk-list > :nth-child(2)').should('contain.text', expectedMessage);
        return this;
    }

    public hasRecordOrViewProgressButton(): this {
        cy.get('[role="button"]')
            .should('exist')
            .and('contain.text', 'Record or view progress');
        return this;
    }

    public recordOrViewProgressButtonNotVisible(): this {
        cy.get('[data-testid="record-view-progress-btn"]').should('not.exist');
        return this;
    }

    public hasChangeObjectiveLinks(): this {
        cy.contains('a.govuk-link', 'Change')
            .should('exist');
        return this;
    }

    public clickFirstChangeObjective(): this {
        cy.get('.govuk-grid-column-two-thirds > :nth-child(4)')
            .within(() => {
                cy.contains('a.govuk-link', 'Change').click()
            })

        return this;
    }

    public updateObjectiveDetails(text: string): this {
        cy.get('#ObjectiveDetails')
            .clear()
            .type(text)
        cy.get('.govuk-button').click();
        cy.url().should('include', '/improvement-plan');

        return this;
    }

    public clickRecordOrViewProgress(): this {
        cy.get('a').contains('Record or view progress').click();
        cy.url().should('include', '/progress-reviews');

        return this;
    }

    public hasNoReviewsMessage(): this {
        cy.get('h2')
            .should('contain.text', 'No progress reviews recorded');
        cy.get('a').contains('Add review').should('exist');
        cy.get('a').contains('Return to improvement plan').should('exist');

        return this;
    }

    public clickAddReview(): this {
        cy.get('a').contains('Add review').click();
        
        return this;
    }
    public enterDate(dateFieldId: string, day: string, month: string, year: string): this {
        cy.getById(`${dateFieldId}-day`).clear().type(day);
        cy.getById(`${dateFieldId}-month`).clear().type(month);
        cy.getById(`${dateFieldId}-year`).clear().type(year);

        return this;
    }

    public validateReviewForm(): this {
        cy.get('[type="submit"]').contains('Save').click();
        cy.getByCyData('error-summary').should('be.visible');
        cy.getByCyData('error-summary').should('contain.text', 'Select who carried out the review');
        cy.getByCyData('error-summary').should('contain.text', 'Enter the date of the review');

        //invalid date - date should be today's or a past date
        cy.get('[data-cy="select-radio-adviser"]').check()
        this.enterDate('ReviewDate', '12', '11', '2040')
        cy.get('[type="submit"]').contains('Save').click();
        cy.getByCyData('error-summary').should('contain.text', "You must enter today's date or a date in the past");

        return this;
    }

    public fillReviewDetails(): this {
        this.enterDate('ReviewDate', '02', '08', '2025');
        cy.getByCyData('select-radio-someone-else').check();
        cy.getById('CustomReviewerName').type('Richika Test');
        cy.get('[type="submit"]').contains('Save').click();

        return this;
    }

    public anotherReviewNeeded(): this {
        cy.url().should('include', '/next-review');
        cy.getById('yes').check();
        this.enterDate('NextReviewDate', '12', '12', '2026');
        cy.get('[type="submit"]').contains('Save and return').click();

        return this;
    }

    public validateOverallProgress(): this {
        cy.url().should('include', '/overall-progress')
        cy.get('[type="submit"]').contains('Save').click();
        cy.getByCyData('error-summary').should('be.visible');
        cy.getByCyData('error-summary').should('contain.text', 'Select how the school is progressing overall');
        cy.getByCyData('error-summary').should('contain.text', 'Enter details about how the school is progressing overall');

        return this;
    }

    public recordOverallProgress(): this {
        cy.url().should('include', '/overall-progress')
        cy.getByCyData('select-radio-school-not-improving').check()
        cy.getById('OverallProgressDetails').type('Overall progress details')
        cy.get('[type="submit"]').contains('Save').click();

        return this;

    }

    public hasEditReviewDetailsLink(): this {
        cy.get('a').contains('Edit review details').should('exist');

        return this;
    }

    public hasChangeNextReviewDateLink(): this {
        cy.get('a').contains('Change next review date').should('exist');

        return this;
    }

    public hasReturnToImprovementPlanLink(): this {
        cy.get('a').contains('Return to improvement plan').should('exist');

        return this;
    }

    public recordProgressForObjective(): this {
        cy.url().should('include', '/record-progress')
        do {
            cy.get('.govuk-fieldset__legend').should('contain.text', 'How is the school progressing with this objective?')
            cy.getByCyData('select-radio-on-schedule').check();
            cy.getById('ProgressDetails').type('Test progress comments');
            cy.get('a').contains('Skip this objective').should('exist');
            cy.get('[type="submit"]').contains('Save').click();
        } while (cy.url().should('not.include', '/progress-summary'));

        return this;
    }

    public hasFirstReviewStatusAndLinks(): this {
        cy.get('.moj-ticket-panel__content').within(() => {
            cy.get('.govuk-tag').first().should('contain.text', 'Progress not recorded');
            cy.get('a').contains('Edit review details').should('exist');
            cy.get('a').contains('Record progress').should('exist');
        });

        return this;
    }

    public hasStatusTag(status: string): this {
        cy.get('.moj-ticket-panel__content').within(() => {
            cy.get('.govuk-tag').should('contain.text', status);

        });

        return this;
    }
}
const improvementPlan = new ImprovementPlan();
export default improvementPlan;