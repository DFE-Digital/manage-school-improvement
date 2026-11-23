class RecordProgress {

    public recordProgressPageLoads(): this {
        cy.get('body').should('be.visible');
        cy.get('h2').should('contain.text', 'Record progress');
        cy.url().should('contain', '/record-progress')
        return this;
    }

    public hasSchoolsMatchedWithSOMessage(expectedMessage: string): this {
        cy.get('h3').should('contain.text', expectedMessage);
        return this;
    }

    public hasMessage(expectedMessage: string): this {
        cy.get('p').should('contain.text', expectedMessage);

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
        cy.get('h1').should('contain.text', 'Progress reviews');
        cy.contains('.moj-ticket-panel__content--blue a', 'Record progress')
            .should('exist')
            .click();
        cy.url().should('include', '/record-progress');
        return this;
    }

    public selectNextSteps(id: string): this {
        cy.get('h1').should('contain.text', 'Record progress');
        cy.get('.govuk-radios__label').first().should('contain', "Continue to review progress")
        cy.get('.govuk-radios__label').should('contain', "Match with a supporting organisation")
        cy.get(`#${id}`).check();

        return this;
    }

    public addAdditionalComments(id: string): this {
        cy.get(`#${id}`).clear();
        cy.get(`#${id}`).type('Additional comments for the review');

        return this;
    }

    public clickSaveButton(): this {
        cy.get('body').then($body => {
            if ($body.find('[data-cy="save-button"]').length > 0) {
                cy.getByCyData('save-button').click();
            } else {
                cy.getByCyData('save-and-return-button').click();
            }
        });

        return this;
    }

    public hasReviewProgressSchoolsMessage(expectedMessage: string): this {
        cy.get('h3').should('contain.text', expectedMessage);
        return this;
    }

    public hasObjectiveTitle(expectedTitle: string): this {
        cy.get('h2, h3, h4').should('contain.text', expectedTitle);
        return this;
    }

    public hasMatchingDecision(): this {
        cy.get('.moj-ticket-panel__content').first().find('p').invoke('text').should((text) => {
            const normalizedText = text.replace(/\s+/g, ' ').trim();
            expect(
                normalizedText.includes('Matching decision: Matched with a supporting organisation') ||
                normalizedText.includes('Matching decision: Review progress'),
                `expected a matching decision in "${normalizedText}"`
            ).to.equal(true);
        });
        return this;
    }

    public hasRecordProgressButton(): this {
        cy.url().should('include', '/record-progress');
        cy.get('h2').should('contain.text', 'Record progress');
        cy.getByCyData('record-or-view-progress-button')
            .should('exist')
            .and('be.visible')
            .and('contain.text', 'Record progress');

        return this;
    }

    public hasViewProgressButton(): this {
        cy.getByCyData('record-or-view-progress-button').contains('View progress').should('exist');

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
        cy.url().should('include', '/record-progress');
        cy.contains('a.govuk-link', 'Delete')
            .should('exist');
        return this;
    }

    public clickFirstChangeObjective(): this {
        cy.get('.govuk-grid-column-two-thirds > :nth-child(4)')
            .within(() => {
                cy.contains('a.govuk-link', 'Change').click()
            })
        cy.url().should('include', '/change-an-objective');

        return this;
    }

    public clickFirstDeleteObjective(): this {
        cy.get('.govuk-grid-column-two-thirds > :nth-child(4)')
            .within(() => {
                cy.contains('a.govuk-link', 'Delete').click()
            })
        cy.get('h1').should('contain.text', 'Are you sure you want to delete this objective?');

        return this;
    }

    public updateObjectiveDetails(text: string): this {
        cy.url().should('include', '/change-an-objective');
        cy.get('#ObjectiveDetails').clear();
        cy.get('#ObjectiveDetails').type(text)
        cy.get('[data-cy="save-and-return-button"]').click();
        cy.get('h2').should('contain.text', 'Record progress');

        return this;
    }

    public clickRecordProgress(): this {
        this.hasRecordProgressButton();
        cy.getByCyData('record-or-view-progress-button').click();
        cy.url().should('include', '/progress-reviews');

        return this;
    }

    public clickChangeOverallProgressLink(): this {
        cy.getByCyData('change-overall-progress-link').click();
        cy.url().should('include', '/overall-progress');

        return this;
    }

    public clickRecordProgressButton(): this {
        cy.getByCyData('record-or-view-progress-button').click();
        cy.url().should('include', '/progress-reviews');
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
        this.hasReturnToRecordProgressLink();

        return this;
    }

    public clickAddReview(): this {
        cy.getByCyData('add-review-button').click();
        cy.getByCyData('select-radio-adviser').should('exist');

        return this;
    }
    public enterDate(dateFieldId: string, day: string, month: string, year: string): this {
        cy.getById(`${dateFieldId}-day`).clear();
        cy.getById(`${dateFieldId}-day`).type(day);
        cy.getById(`${dateFieldId}-month`).clear();
        cy.getById(`${dateFieldId}-month`).type(month);
        cy.getById(`${dateFieldId}-year`).clear();
        cy.getById(`${dateFieldId}-year`).type(year);

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
        cy.url().should('include', '/next-review');

        return this;
    }

    public anotherReviewNeeded(): this {
        cy.url().should('include', '/next-review');
        cy.getById('yes').check();
        this.enterDate('NextReviewDate', '12', '12', '2027');
        cy.get('[type="submit"]').contains('Save and return').click();
        // Saving returns to the review list - anchor there before callers assert on it.
        this.hasProgressReviewsPageLoaded();

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
        cy.contains('.moj-ticket-panel__content--blue a', 'Edit review details').should('exist');

        return this;
    }

    public clickEditReviewDetailsLink(): this {
        cy.url().should('include', '/progress-reviews');
        cy.get('a').contains('Edit review details').should('exist');

        return this;
    }

    public reviewSummaryCardVisible(): this {
        cy.get('.moj-ticket-panel__content')
            .first()
            .should('exist')
            .and('be.visible');

        return this;
    }

    public clickReturnToRecordProgress(): this {
        cy.get('body').then($body => {
            if ($body.find('[data-cy="return-to-progress-reviews-button"]').length > 0) {
                cy.getByCyData('return-to-progress-reviews-button').click();
            } else if ($body.find('[data-cy="return-to-improvement-plan-link"]').length > 0) {
                cy.getByCyData('return-to-improvement-plan-link').click();
            } else {
                cy.contains('a', 'Return to record progress').click();
            }
        });

        return this;
    }

    public clickReturnToRecordProgressTab(): this {
        cy.get('a').contains('Return to record progress').click();
        cy.url().should('include', '/record-progress');

        return this;
    }
    

    public hasSummaryCard(): this {
        cy.get(String.raw`.govuk-summary-card.govuk-\!-margin-bottom-6`).should('exist');

        return this;
    }

    public hasChangeLink(): this {
        cy.get('[data-cy="change-objective-progress-link"], [data-cy="change-overall-progress-link"]')
            .should('exist');

        return this;
    }

    public hasDeleteLink(): this {
        cy.getByCyData('objective-summary-change-link').should('exist');

        return this;
    }

    public clickEditOrDeleteReviewLink(): this {
        cy.get('a').contains('Edit or delete review').click();
        cy.get('h1').should('contain.text', 'Edit progress review');
        cy.getByCyData('delete-review-link').contains('Delete').click();
        cy.get('h1').should('contain.text', 'Are you sure you want to delete this progress review?');
        cy.url().should('include', '/delete-review');
        cy.getByCyData('save-and-return-button').contains('Delete review')
        cy.get('a').contains('Cancel').should('exist');

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

    public hasReturnToRecordProgressLink(): this {
        cy.get('a').contains('Return to record progress').should('exist');

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

        const maxObjectives = 20;

        const recordOne = (remaining: number): void => {
            cy.url().then((url) => {
                if (url.includes('/overall-progress')) {
                    return;
                }

                if (remaining === 0) {
                    throw new Error(
                        `Still not on /overall-progress after ${maxObjectives} objectives (currently ${url})`
                    );
                }

                cy.get('.govuk-fieldset__legend.govuk-fieldset__legend--m').should('contain.text', 'Select progress rating')
                cy.getByCyData('select-radio-red').check();
                cy.getByCyData('progress-details-textarea').type('Test progress comments');
                cy.getByCyData('skip-objective-link').should('exist');
                cy.getByCyData('save-button').click();

                recordOne(remaining - 1);
            });
        };

        recordOne(maxObjectives);

        return this;
    }

    public hasFirstReviewStatusAndLinksForSO(): this {
        cy.contains('.moj-ticket-panel__content', 'Progress not recorded').first().within(() => {
            cy.get('.govuk-tag').first().should('contain.text', 'Progress not recorded');
            cy.contains('a', 'Edit or delete review').should('exist');
            cy.contains('a', 'Record progress').should('exist');
            cy.root().invoke('text').should((text) => {
                const normalizedText = text.replace(/\s+/g, ' ').trim();
                expect(normalizedText).to.contain('Matching decision: Match with a supporting organisation');
            });
        });

        return this;
    }

    public hasStatusTagWIthDeleteOption(status: string): this {
        cy.contains('.moj-ticket-panel__content', status).first().within(() => {
            cy.get('.govuk-tag').should('contain.text', status);
            cy.contains('a', 'Edit or delete review').should('exist');
            cy.contains('a', 'Record progress').should('exist');

        });

        return this;
    }

    public hasStatusTagWIthoutDeleteOption(status: string): this {
        cy.contains('.moj-ticket-panel__content', status).first().within(() => {
            cy.get('.govuk-tag').should('contain.text', status);
            cy.contains('a', 'Edit review details').should('exist');
            cy.contains('a', 'View progress review').should('exist');

        });

        return this;
    }

    public clickRecordOrViewProgressForce(): this {
        cy.get('.govuk-button').contains('Record or view progress').click({ force: true });

        return this;
    }

    public ifObjectiveExist(): this {
        cy.url().should('include', '/record-progress');
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

    public hasStatusAndLinksForReviewProgress(): this {
        cy.contains('.moj-ticket-panel__content', 'Progress not recorded').first().within(() => {
            cy.get('.govuk-tag').first().should('contain.text', 'Progress not recorded');
            cy.contains('a', 'Edit or delete review').should('exist');
            cy.contains('a', 'Record progress').should('exist');
            cy.contains(/Matching\s+decision:\s+Review progress/).should('exist');
        });

        return this;
    }

}
const recordProgress = new RecordProgress();
export default recordProgress;
