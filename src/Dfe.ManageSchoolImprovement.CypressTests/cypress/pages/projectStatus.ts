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

    public hasChangeProjectStatusButton(): this {
        cy.get('[role="button"]').should("be.visible");

        return this;
    }
    public clickChangeProjectStatusButton(): this {
        cy.get('[role="button"]').contains('Change project status').click();

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
        cy.getById('continue-button').contains('Continue').click();

        return this;
    }

    public hasValidation(valText: string, id: string): this {
        cy.getById(id).contains(valText);

        return this;
    }

    public enterDate(dateFieldId: string, day: string, month: string, year: string): this {
        cy.getById(`${dateFieldId}-day`).clear().type(day);
        cy.getById(`${dateFieldId}-month`).clear().type(month);
        cy.getById(`${dateFieldId}-year`).clear().type(year);

        return this;
    }

    public enterDetails(id: string, details: string): this {
        cy.getById(id).clear().type(details);

        return this;
    }

    public hasErrorMessage(id: string, message: string): this {
        cy.getById(id).should('contain', message);

        return this;
    }

    public clickSaveAndContinueButton(): this {
        cy.getById('continue-button').contains('Save and continue').click();

        return this;
    }

    public hasCheckYourAnswersPageWithDetails(): this {
        cy.url().should('include', '/project-status-answers');
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
        cy.get('.govuk-notification-banner__content').contains('Status updated successfully').should("be.visible");

        return this;
    }

    public getUpdatedStatusWithDetails(status: string): this {
        cy.get('.govuk-summary-list__value').should('contain', status);
        cy.get('dl.govuk-summary-list').first().within(() => {
            cy.get('.govuk-summary-list__row').each($row => {
               // cy.wrap($row).within(() => {
                    cy.get('.govuk-summary-list__key').invoke('text').then((keyText) => {
                        const trimmedKeyText = keyText.trim();
                        const keysToCheck = ['Status', 'Date of decision', 'Details'];
                        if (keysToCheck.includes(trimmedKeyText)) {
                            cy.get('.govuk-summary-list__value').should('not.be.empty');
                        }
                    });
               // });
            });
        });

        return this;
    }
}

const projectStatus = new ProjectStatus();

export default projectStatus;
