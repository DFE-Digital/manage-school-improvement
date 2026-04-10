class ImprovementPlan {

    public improvementPlanPageLoads(): this {
        cy.get('body').should('be.visible');
        cy.get('h2').should('contain.text', 'Improvement plan');
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

    public hasAddNextReviewDateLink(): this {
        cy.url().should('include', '/progress-reviews');
        cy.get('a').contains('Add next review date').should('exist');

        return this;
    }

    public hasChangeNextReviewDateLink(): this {
        cy.url().should('include', '/progress-reviews');

        cy.get('a').contains('Change next review date').should('exist');
        return this;
    }

    public selectAddAnotherObjective(buttonText: string): this {
        cy.get('.govuk-button--secondary').contains(buttonText).click();
        return this;
    }

    public clickRecordProgressLink(): this {
        cy.getByCyData('record-progress-link').click()
        cy.url().should('include', '/progress-reviews');

        return this;
    }

    public hasNoObjectivesMessage(expectedMessage: string): this {
        cy.get('.govuk-list > :nth-child(2)').should('contain.text', expectedMessage);
        return this;
    }

    public hasRecordOrViewProgressButton(): this {
        cy.url().should('include', '/improvement-plan');
        cy.get('h2').should('contain.text', 'Improvement plan');
        cy.get('[data-cy="record-or-view-progress-button"]')
            .should('exist')
            .and('be.visible')
            .and('contain.text', 'Record or view progress');

        return this;
    }

    public hasViewProgressButton(): this {
        cy.getByCyData('record-or-view-progress-button').should('contain.text', 'View Progress');

        return this;
    }

    public recordOrViewProgressButtonNotVisible(): this {
        cy.getByCyData('record-or-view-progress-button').should('not.exist');

        return this;
    }

    public hasChangeObjectiveLink(): this {
        cy.contains('a.govuk-link', 'Change')
            .should('exist');
        return this;
    }

    public hasDeleteObjectiveLink(): this {
        cy.url().should('include', '/improvement-plan');
        cy.contains('a.govuk-link', 'Delete')
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

    public clickFirstDeleteObjective (): this {
        cy.get('.govuk-grid-column-two-thirds > :nth-child(4)')
            .within(() => {
                cy.contains('a.govuk-link', 'Delete').click()
            })
        return this;
    }

    public updateObjectiveDetails(text: string): this {
        cy.url().should('include', '/change-an-objective');
        cy.get('#ObjectiveDetails')
            .clear()
            .type(text)
        cy.get('.govuk-button').click();
        cy.get('h2').should('contain.text', 'Improvement plan');

        return this;
    }

    public clickRecordOrViewProgress(): this {
        this.hasRecordOrViewProgressButton();
        cy.getByCyData('record-or-view-progress-button').click();
        cy.url().should('include', '/progress-reviews');

        return this;
    }

    public clickChangeOverallProgressLink(): this {
        cy.getByCyData('change-overall-progress-link').click();
        cy.url().should('include', '/overall-progress');

        return this;
    }

    public hasProgressReviewsPageLoaded(): this {
        cy.url().should('include', '/progress-reviews');
        cy.get('h1').should('contain.text', 'Progress reviews');
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
        cy.getByCyData('add-review-button').click();

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
        cy.getByCyData('error-summary').should('contain.text', 'Enter a date');
        cy.get('[data-cy="select-radio-adviser"]').check()
        this.enterDate('ReviewDate', '12', '11', '2055')
        cy.get('[type="submit"]').contains('Save').click();
        cy.getByCyData('error-summary').should('contain.text', "Year must be between 2000 and 2050");

        return this;
    }

    public fillReviewDetails(): this {
        this.enterDate('ReviewDate', '19', '03', '2026');
        cy.getByCyData('select-radio-someone-else').check();
        cy.getById('CustomReviewerName').type('Richika Test');
        cy.get('[type="submit"]').contains('Save').click();

        return this;
    }

    public anotherReviewNeeded(): this {
        cy.url().should('include', '/next-review');
        cy.getById('yes').check();
        this.enterDate('NextReviewDate', '12', '12', '2027');
        cy.get('[type="submit"]').contains('Save and return').click();

        return this;
    }

    public validateOverallProgressPage(): this {
        cy.getByCyData('overall-progress-details-textarea').clear();
        cy.get('[type="submit"]').contains('Save and return').click();
        cy.getByCyData('error-summary').should('be.visible');
        cy.getByCyData('error-summary').should('contain.text', 'Enter the next steps');

        return this;
    }

    public recordOverallProgress(): this {
        cy.url().should('include', '/overall-progress')
        cy.getByCyData('overall-progress-details-textarea').type('Overall progress details')
        cy.getByCyData('save-button').click();

        return this;

    }

    public returnToProgressReviews(): this {
        cy.getByCyData('return-to-progress-reviews-button').click();
        cy.url().should('include', '/progress-reviews');

        return this;
    }

    public hasEditReviewDetailsLink(): this {
        cy.get('a').contains('Edit review details').should('exist');

        return this;
    }

    public clickEditReviewDetailsLink(): this {
        cy.url().should('include', '/progress-reviews');
        cy.get('a').contains('Edit review details').should('exist');

        return this;
    }

    public noDeleteButtonExist(): this {
        cy.get('[type = "button"]').should('not.contain.text', 'Delete review');
        return this;
    }


    public clickEditReviewDetails(): this {
        cy.get('a').contains('Edit review details').click();
        cy.get('h1').should('contain.text', 'Edit progress review');
        return this;
    }

    public hasEditOrDeleteReview(): this {
        cy.get('a').contains('Edit or delete review').should('exist');

        return this;
    }

    public deleteReview(): this {
        cy.get('a').contains('Edit or delete review').click();
        cy.get('h1').should('contain.text', 'Edit progress review');        
        cy.getByCyData('delete-review-link').contains('Delete').click(); //button name should be Delete review
        cy.get('h1').should('contain.text', 'Are you sure you want to delete this progress review?');

        cy.executeAccessibilityTests();

        cy.getByCyData('save-and-return-button').contains('Delete review').click();
        cy.get('.govuk-notification-banner__content').should('contain.text', 'Progress review deleted');

        return this;
    }

    public hasReturnToImprovementPlanLink(): this {
        cy.get('a').contains('Return to improvement plan').should('exist');

        return this;
    }

    public hasOverallProgressChangeLink(): this {
        cy.getByCyData('change-overall-progress-link').should('exist');

        return this;
    }

    public hasRecordObjectiveLink(): this {
        cy.getByCyData('record-objective-link').first().should('exist');
        
        return this;
    }

    public recordProgressForObjective(): this {
        cy.url().should('include', '/record-progress')
        do {
            cy.get('.govuk-fieldset__legend.govuk-fieldset__legend--m').should('contain.text', 'Select progress rating')
            cy.getByCyData('select-radio-red').check();
            cy.getByCyData('progress-details-textarea').type('Test progress comments');
            cy.getByCyData('skip-objective-link').should('exist');
            cy.getByCyData('save-button').click();
        } while (cy.url().should('not.include', '/overall-progress'));

        return this;
    }

    public hasFirstReviewStatusAndLinks(): this {
        cy.get('.moj-ticket-panel__content').within(() => {
            cy.get('.govuk-tag').first().should('contain.text', 'Progress not recorded');
            cy.get('a').contains('Edit or delete review').should('exist');
            cy.get('a').contains('Record progress').should('exist');
        });

        return this;
    }

    public hasStatusTag(status: string): this {
        cy.get('.moj-ticket-panel__content').first().within(() => {
            cy.get('.govuk-tag').should('contain.text', status);

        });

        return this;
    }

    public clickRecordOrViewProgressForce(): this {
        cy.get('.govuk-button').contains('Record or view progress').click({ force: true });

        return this;
    }

    public ifObjectiveExist(): this {
        cy.url().should('include', '/improvement-plan');
        cy.get('body').then($body => {
            if ($body.find('a.govuk-link:contains("Delete")').length > 0) {
                this.deleteObjectiveSuccessfully();
            } else {
                cy.log('There is no objective to delete');
            }
        });
        return this;
    }

    public deleteObjectiveSuccessfully(): this {
        cy.get('a.govuk-link').contains('Delete').click();
        cy.get('h1').should('contain.text', 'Are you sure you want to delete this objective?');
        cy.get('.govuk-button.govuk-button--warning').contains('Delete objective').click();
        cy.get('.govuk-notification-banner__content').should('contain.text', 'Objective deleted');

        return this;
    }
}
const improvementPlan = new ImprovementPlan();
export default improvementPlan;
