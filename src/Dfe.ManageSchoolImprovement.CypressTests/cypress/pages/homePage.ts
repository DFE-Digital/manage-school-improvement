class HomePage {
  public AddSchool(): this {
    cy.contains("Add a school").click();
    //   cy.login({role: ProjectRecordCreator})
    return this;
  }

  public hasAddSchool(): this {
    cy.get('[data-cy="select-heading"]').should(
      "contain.text",
      "Schools identified for targeted intervention"
    );
    cy.contains("Add a school").should("be.visible");
    return this;
  }
  public selectFirstSchoolFromList(): this {
    cy.get('[data-cy="trust-name-0"]').first().click(); // Select the first school in the list    
    cy.url().should('include', '/task-list/')
    return this;

  }

  public hasCookiesBanner(): this {
    cy.get('.govuk-cookie-banner__heading').contains('Cookies on Manage school improvement')
    cy.get('[data-test="cookie-banner-accept"]').contains('Accept analytics cookies')
    cy.get('[data-test="cookie-banner-reject"]').contains('Reject analytics cookies')
    cy.get('[data-test="cookie-banner-link-1"]').contains('View cookies')
    return this;
  }

  public viewCookiesPage(): this {
    cy.get('[data-test="cookie-banner-link-1"]').click()
    cy.url().should('contains', '/cookie-preferences')
    cy.get('[data-qa="submit"]').should('be.visible')
    return this;
  }

  public hasProjectCount(): this {
    cy.get('[data-cy="select-projectlist-filter-count"]').should(
      "contain.text",
      "schools found"
    );
    return this;
  }

  public selectProjectFilter(project: string): this {
    cy.get('[data-cy="select-projectlist-filter-title"]').typeFast(project);
    cy.get('[data-cy="select-projectlist-filter-apply"]').click();

    return this;
  }

  public selectAssignedToFilter(assignedTo: string): this {
    // cy.get('[data-cy="select-projectlist-filter-assignedTo"]') //need to apply in code
    cy.get('#accordion-officers-heading').should('contain', 'Assigned to').click()
    cy.get('#filter-delivery-officer-not-assigned')
    cy.get('.govuk-accordion__section-content .govuk-checkboxes')
      .contains(assignedTo.trim())
      .click();
    return this;
  }

  public selectNotAssignedToFilter(): this {
    cy.get('#accordion-officers-heading').should('contain', 'Assigned to').click()
    cy.get('#filter-delivery-officer-not-assigned')
    cy.get('[data-cy="select-projectlist-filter-officer-not-assigned"]').click()
    return this;
  }

  public hasProjectFilter(): this {
    cy.get(".moj-filter__options").should("be.visible");

    return this;
  }

  public hasRegionFilter(region: string): this {
    cy.getByTestId(`${region}-option`).should("be.checked");

    return this;
  }

  public hasLocalAuthorityFilter(localAuthority: string): this {
    cy.getByTestId(`${localAuthority}-option`).click();

    return this;
  }

  public selectEastMidlandsRegionFilter(): this {
    cy.get('[data-cy="select-projectlist-filter-region"]').click();
    cy.get('[data-cy="select-projectlist-filter-region-East Midlands"]').click();

    return this;
  }

  public applyFilters(): this {
    cy.get('[data-cy="select-projectlist-filter-apply"]').click();

    return this;
  }

  public noFiltersSelected(): this {
    cy.get('.moj-filter__selected').should('contain.text', 'No filters selected')

    return this
  }

  public selectSchool(schoolLong: string): this {
    cy.contains(schoolLong).click();

    return this;
  }

  public hasSchoolName(schoolLong: string): this {
    cy.get('[data-cy="trust-name-0"]').contains(schoolLong);

    return this;
  }

  public hasAssignedToOption(assignedTo: string): this {
    cy.get('[data-cy="assigned-to-0"]').contains(assignedTo);

    return this;
  }

  public selectSchoolName(schoolLong: string): this {
    cy.get('[data-cy^="trust-name-"]').contains(schoolLong).click();

    return this;
  }

  public hasURN(URN: string): this {
    cy.get("#urn-0").contains(URN);

    return this;
  }

  public hasLocalAuthority(localAuthority: string): this {
    cy.get('[id^="localauthority-"]').eq(0).contains(localAuthority);

    return this;
  }

  public hasRegion(region: string): this {
    cy.get('[id^="region-"]').eq(0).contains(region);

    return this;
  }

  public hasFilterSuccessNotification(): this {
    cy.get('[data-cy="filter-success-notification"]').should("be.visible")
    cy.get('#govuk-notification-banner-title').should('contain', 'Success')

    return this;
  }

  public hasAddSchoolSuccessNotification(): this {
    cy.get('[data-cy="add-school-success-notification"]').should("be.visible");

    return this;
  }

  public showAllFilterSections(): this {
    cy.get('.govuk-accordion__show-all-text').should('contain.text', 'Show all sections')
      .click()
    cy.get('.govuk-accordion__show-all-text').should('contain.text', 'Hide all sections')

    return this;
  }

  public hideAllFilterSections(): this {
    cy.get('.govuk-accordion__show-all-text').should('contain.text', 'Hide all sections')
      .click()
    cy.get('.govuk-accordion__show-all-text').should('contain.text', 'Show all sections')

    return this;
  }

  public selectFilterRegions(): this {
    // Check if the Region accordion section is not expanded
    cy.get('[data-cy="select-projectlist-filter-region"]')
      .should("have.attr", "aria-expanded", "false")
      .then(($accordion) => {
        if ($accordion.attr("aria-expanded") === "false") {
          cy.get('[data-cy="select-projectlist-filter-region"]').click();
        }
      });

    // Select the region checkboxes
    cy.get("#filter-project-region-east-midlands").check();
    cy.get("#filter-project-region-east-of-england").check();
    cy.get("#filter-project-region-london").check();
    cy.get("#filter-project-region-north-east").check();

    return this;
  }

  public hasFilterRegions(): this {
    // Assert that the results show projects from selected regions
    const validRegions = [
      "East Midlands",
      "East of England",
      "London",
      "North East",
    ];

    // Grab all elements whose ID starts with 'region-' and check the first 10
    cy.get("[id^='region-']").each(($el, index) => {
      if (index < 10) {
        const regionText = $el.text();
        expect(validRegions.some((r) => regionText.includes(r))).to.be.true;
      }
    });
    return this;
  }

  public clearFilters(): this {
    // Clear the filters
    cy.get('[data-cy="clear-filter"]').should('be.visible')
      .click();
    cy.url().should('contain', 'clear')

    return this;
  }

  public resultCountNotZero(): this {
    cy.get('[data-cy="trust-name-0"]').should('be.visible')
    cy.contains(/schools found/i)
      .invoke('text')
      .then((text: string) => {
        // Use regex to extract the number from the string
        const regex = /^(\d+)\s+schools found$/i;
        const result = regex.exec(text)
        expect(result).to.not.be.null;

        const schoolCount = parseInt(result![1], 10);

        // Assert that the school count is not zero
        expect(schoolCount).to.be.greaterThan(0);

      });
    return this;
  }
}
const homePage = new HomePage();

export default homePage;
