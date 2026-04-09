class ProjectStatus {
    public hasHeading(value: string): this {
        cy.get('h2').should('contain', value);

        return this;
    }

    public hasPageHeading(heading: string): this {
        cy.get('h1').should('contain', heading);

        return this;
    }

    public hasCurrentStatus(value: string): this {
        cy.get('.govuk-summary-list__value').should('contain', value);

        return this;
    }

    public bannerDisplayedForPausedOrStoppedStatus(status: string): this {
        if (status === 'Paused') {
            cy.get('.govuk-notification-banner').should('contain', 'Project paused');
            cy.get('.govuk-notification-banner').within(() => {
                cy.get('a').contains('Change the project status and eligibility').should('exist');
            });
        } else if (status === 'Stopped') {
            cy.get('.govuk-notification-banner').should('contain', 'Project stopped');
            cy.get('.govuk-notification-banner').within(() => {
                cy.get('a').contains('Change the project status and eligibility').should('exist');
            });
        }

        return this;
    }

    public hasNoBannerForInProgressStatus(): this {
        cy.get('.govuk-notification-banner').should('not.exist');

        return this;
    }

    public hasChangeStatusAndEligibilityButton(): this {
        cy.getByCyData('change-project-status-button').should("be.visible");

        return this;
    }
    public clickChangeStatusAndEligibilityButton(): this {
        cy.getByCyData('change-project-status-button').click();

        return this;
    }
    public selectStatusInProgress(): this {
        cy.getById('in-progress').check()
        return this;
    }

    public selectStatusPaused(): this {
        cy.getById('paused').check()

        return this;
    }

    public selectStatusStopped(): this {
        cy.getById('stopped').check()

        return this;
    }

    public checkedStatusBydefault(statusId: string): this {
        cy.getById(statusId).should('be.checked');

        return this;
    }

    public clickContinueButton(): this {
        cy.get('[type="submit"]').contains('Continue').click();
        return this;
    }

    public projectEligibleForIntervention(eligibility: string): this {
        if (eligibility === 'Yes') {
            cy.getById('yes').click();
        } else if (eligibility === 'No') {
            cy.getById('no').click();
        }
          return this;    
    }

    public enterDateSupportIsDueToEnd(day: string, month: string, year: string): this {
        cy.getById('support-is-due-to-end-date-day').type(day);
        cy.getById('support-is-due-to-end-date-month').type(month);
        cy.getById('support-is-due-to-end-date-year').type(year);

       return this;
    }

    public hasValidation(valText: string, id: string): this {
        cy.getById(id).contains(valText);

        return this;
    }

    public enterDateProjectStatusOrEligibilityChange(day: string, month: string, year: string): this {
        cy.getById('status-eligibility-change-date-day').type(day);
        cy.getById('status-eligibility-change-date-month').type(month);
        cy.getById('status-eligibility-change-date-year').type(year); 

        return this;
    }

    public enterDate(dateFieldId: string, day: string, month: string, year: string): this {
        cy.getById(`${dateFieldId}-day`).clear().type(day);
        cy.getById(`${dateFieldId}-month`).clear().type(month);
        cy.getById(`${dateFieldId}-year`).clear().type(year);

        return this;
    }

    public enterDetails(id: string, details: string): this {
        cy.getByName(id).type(details);

        return this;
    }

    public hasErrorMessage(id: string, message: string): this {
        cy.getById(id).should('contain', message);

        return this;
    }

    public clickSaveAndContinueButton(): this {
        cy.get('[type="submit"]').click();
        return this;
    }

    public hasCheckYourAnswersPageWithDetails(): this {
        cy.url().should('include', '/check-your-answers');
        cy.get('.govuk-summary-list__row').each($row => {
            cy.wrap($row).within(() => {
                cy.get('.govuk-summary-list__key').should('not.be.empty');
                cy.get('.govuk-summary-list__value').should('not.be.empty');
                if ($row.index() === 0) {
                    cy.get('a').contains('Change').should('exist');
                } else {
                    cy.log('No Change link required here');
                }
            });
        });
        return this;
    }

    public clickSaveAndReturnButton(): this {
        cy.getByCyData('return-to-project-status-button').contains('Save and return').click();

        return this;
    }

    public hasSuccessNotification(): this {
        cy.get('.govuk-notification-banner.govuk-notification-banner--success').contains('Status and eligibility updated successfully').should("be.visible");

        return this;
    }

    public hasNoStatusHistory(): this {
        cy.get('.moj-timeline').should('not.be.visible')

        return this;
    }

    public getUpdatedStatusWithDetails(status: string): this {
        cy.get('.govuk-summary-list__value').should('contain', status);
        cy.get('dl.govuk-summary-list').each($summaryList => {
            cy.wrap($summaryList).within(() => {
                cy.get('.govuk-summary-list__row').each($row => {
                    this.checkSummaryListRow($row);
                });
            });
        });

        return this;
    }

    private checkSummaryListRow($row: JQuery<HTMLElement>): void {
        cy.wrap($row).within(() => {
            cy.get('.govuk-summary-list__key').invoke('text').then((keyText) => {
                const trimmedKeyText = keyText.trim();
                const keysToCheck = ['Status', 'Date of decision', 'Details'];
                if (keysToCheck.includes(trimmedKeyText)) {
                    cy.get('.govuk-summary-list__value').should('not.be.empty');
                }
            });
        });
    }

    public getProjectStatusChangeHistory(status: string): this {
        cy.get('.moj-timeline').should('exist');
        cy.get('.moj-timeline__item').first().within(() => {
                if (status == 'Stopped') {
                    cy.get('.govuk-tag.govuk-tag--red').should('contain', status);
                  
                } 
                else if (status == 'Paused') {
                    cy.get('.govuk-tag.govuk-tag--yellow').should('contain', status);
                }
                else if (status == 'In progress') {
                    cy.get('.govuk-tag.govuk-tag--green').should('contain', status);
                }
                cy.get('.moj-timeline__date').should('not.be.empty');
            });


        return this;
    }
}

const projectStatus = new ProjectStatus();

export default projectStatus;


