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

  public clickBacklink(): this {
    cy.get('.govuk-back-link').should('contain', 'Back').click();
    cy.url().should('include', '/schools-identified-for-targeted-intervention');

    return this;
  }

  public hasCookiesBanner(): this {
    cy.get('.govuk-cookie-banner__heading').contains('Cookies on Manage school improvement')
    cy.get('[data-test="cookie-banner-accept"]').contains('Accept analytics cookies')
    cy.get('[data-test="cookie-banner-reject"]').contains('Reject analytics cookies')
    cy.get('[data-test="cookie-banner-link-1"]').contains('View cookies')
    return this;
  }

  public acceptCookies(): this {
    cy.get('[data-test="cookie-banner-accept"]').contains('Accept analytics cookies').click()
    cy.get('.govuk-cookie-banner__heading').should('not.be.visible')

    return this;
  }

  public rejectCookies(): this {
    cy.get('[data-test="cookie-banner-reject"]').contains('Reject analytics cookies').click()
    cy.get('.govuk-cookie-banner__heading').should('not.be.visible')

    return this;
  }

  public viewCookiesPage(): this {
    cy.get('[data-test="cookie-banner-link-1"]').click()
    cy.url().should('include', '/cookie-preferences')
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
    cy.get('#accordion-officers-heading').should('contain', 'Assigned to').click()
    cy.get('#filter-delivery-officer-not-assigned')
    cy.get('.govuk-accordion__section-content .govuk-checkboxes')
      .contains(assignedTo.trim())
      .click();
    return this;
  }

  public verifyFilterChecked(filterType: string, filterValue: string): this {
    cy.get('.moj-filter__selected')
      .should('contain', filterType);

    cy.get('.moj-filter__selected')
      .contains(new RegExp(filterValue, 'i'))
      .should('exist');

    return this;
  }

  public selectNotAssignedToFilter(): this {
    cy.get('#accordion-officers-heading').should('contain', 'Assigned to').click()
    cy.get('#filter-delivery-officer-not-assigned')
    cy.get('[data-cy="select-projectlist-filter-officer-not-assigned"]').click()
    return this;
  }

  public selectTrustFilter(trustName: string): this {
    cy.get('[data-cy="select-projectlist-filter-trust"]')
      .should("exist")
      .click()

    cy.get('#filter-searches').should('be.visible').type(trustName, { force: true });

    cy.get('.govuk-checkboxes')
      .contains(trustName)
      .closest('.govuk-checkboxes__item')
      .find('input[type="checkbox"]')
      .check();

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
    cy.getByCyData('select-projectlist-filter-local-authority').click();
    cy.getByCyData('select-projectlist-filter-local-authority-Nottingham').click();

    return this;
  }

  public selectNottinghamLAFilter(): this {
    cy.get('[data-cy="select-projectlist-filter-region"]').click();
    cy.get('[data-cy="select-projectlist-filter-region-East Midlands"]').click();

    return this;
  }

  public applyFilters(): this {
    cy.getByCyData('select-projectlist-filter-apply').should('contain', 'Apply filters')
      .click();

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
  public hasURN(urn: string): this {
    cy.get("#urn-0").contains(urn);

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
    cy.get('[data-cy="select-projectlist-filter-region"]')
      .should("have.attr", "aria-expanded", "false")
      .then(($accordion) => {
        if ($accordion.attr("aria-expanded") === "false") {
          cy.get('[data-cy="select-projectlist-filter-region"]').click();
        }
      });

    cy.get("#filter-project-region-east-midlands").check();
    cy.get("#filter-project-region-east-of-england").check();

    return this;
  }

  public hasFilterRegions(): this {
    const validRegions = [
      "East Midlands",
      "East of England",
      "London",
      "North East",
    ];

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
    cy.get('.govuk-table__cell').should('exist')
    cy.get('[data-cy="select-projectlist-filter-count"]').should('not.have.text', '0 schools found')
    cy.get('[data-cy="select-projectlist-filter-count"]').invoke('text').then((text) => {
      cy.log(`Project list filter count text: ${text}`);
    });
    return this;
  }

  public hasAssignedToFilter(assignedTo: string): this {
    cy.get('.moj-filter__selected').should('contain', 'Assigned to')
    cy.get('[data-cy="select-projectlist-filter-officer-*"]')
      .contains(assignedTo.trim())
      .parent()
      .find('input[type="checkbox"]')
      .should('be.checked');

    return this;
  }

  public selectAdvisedByFilter(searchText: string): this {
    cy.get('[data-cy="select-projectlist-filter-adviser"]')
      .should("exist")
      .click()
    cy.get('#filter-searches').should('be.visible')
    cy.get(`[data-cy="select-projectlist-filter-adviser-${searchText}"]`)
      .check();

    return this;
  }

  public selectSpecificRegionFilter(region: string): this {
    cy.get('[data-cy="select-projectlist-filter-region"]')
      .should("exist")
      .then(($accordion) => {
        if ($accordion.attr("aria-expanded") === "false") {
          cy.wrap($accordion).click();
        }
      });

    cy.get('.govuk-checkboxes')
      .contains(region)
      .parent()
      .find('input[type="checkbox"]')
      .check();

    return this;
  }

  public selectLocalAuthorityFilter(localAuthority: string): this {
    cy.get('[data-cy="select-projectlist-filter-local-authority"]')
      .should("exist")
      .then(($accordion) => {
        if ($accordion.attr("aria-expanded") === "false") {
          cy.wrap($accordion).click();
        }
      });

    cy.get('.govuk-checkboxes')
      .contains(localAuthority)
      .parent()
      .find('input[type="checkbox"]')
      .check();

    return this;
  }

  public hasSelectedRegionFilter(region: string): this {
    cy.get(':nth-child(1) > .govuk-table__cell').should('contain', region);

    return this;
  }

  public hasAdvisedByFilter(advisedBy: string): this {
    cy.get(':nth-child(2) > .govuk-table__cell').should('contain', advisedBy);
    return this;
  }

  public hasSelectedLocalAuthorityFilter(localAuthority: string): this {
    cy.get(':nth-child(1) > .govuk-table__cell').should('contain', localAuthority);

    return this;
  }

  public hasSelectedTrustFilter(trust: string): this {
    cy.get('.govuk-table__row').should('contain', trust);
    return this;
  }

  public selectFilter(filterType: string, filterValue: string): this {
    switch (filterType.toLowerCase()) {
      case 'assigned to':
        this.selectAssignedToFilter(filterValue);
        break;
      case 'advised by':
        this.selectAdvisedByFilter(filterValue);
        break;
      case 'region':
        this.selectSpecificRegionFilter(filterValue);
        break;
      case 'local authority':
        this.selectLocalAuthorityFilter(filterValue);
        break;
      case 'not assigned':
        this.selectNotAssignedToFilter();
        break;
      case 'trust':
        this.selectTrustFilter(filterValue);
        break;
      case 'date school added to msi':
        if (filterValue) {
          // filterValue should be in format "YYYY-MM" or "YYYY"
          const [year, month] = filterValue.split(' ');
          cy.log(`Year: ${year}, Month: ${month}`);
          this.selectYearMonthFilter(year, month);
        }
        break;
      default:
        throw new Error(`Filter type '${filterType}' is not supported.`);
    }
    return this;
  }

  public showDateFilterOptions(): this {
    cy.getByCyData('select-projectlist-filter-date')
      .should("exist")
      .click();

    return this;
  }

  public firstRecordInListShowsDetails(): this {
     cy.get('.govuk-table__cell').first().each(($row) => {
      const rowText = $row.text().replace(/\s+/g, ' ').trim();
      expect(rowText).to.not.be.empty;
      expect(rowText).to.include('In progress');
      expect(rowText).to.include('URN');
      expect(rowText).to.include('Region');
      expect(rowText).to.include('Date added to MSI');
      expect(rowText).to.include('Local authority');
     
    });
    return this;   
  }

  public selectYearMonthFilter(year: string, month?: string): this {
    this.showDateFilterOptions();

    cy.getByCyData(`select-projectlist-filter-year-${year}`)
      .should("exist")
      .click();

    cy.getByCyData(`select-projectlist-filter-year-${year}`).should('be.checked');

    if (month) {
      cy.getByCyData(`select-projectlist-filter-month-${year}-${month}`)
        .should("exist")
        .click();

      cy.getByCyData(`select-projectlist-filter-month-${year}-${month}`).should('be.checked');

    } else {
      cy.log('Month is not provided');
    }

    return this;
  }

  public selectYearFilter(year: string): this {
    cy.getByCyData(`select-projectlist-filter-year-${year}`)
      .should("exist")
      .click();
    cy.get('.govuk-checkboxes')
      .contains(year)
      .closest('.govuk-checkboxes__item')
      .find('input[type="checkbox"]')
      .check();
    return this;
  }

  public selectMonthFilter(month: string): this {
    cy.getByCyData(`select-projectlist-filter-month-2025-${month}`)
      .should("exist")
      .click();
    cy.get('.govuk-checkboxes')
      .contains(month)
      .closest('.govuk-checkboxes__item')
      .find('input[type="checkbox"]')
      .check();
    return this;
  }

  public checkResultsContainDateSchoolAddedToMsi(dateSchoolAddedToMsi: string): this {
    const [year, month] = dateSchoolAddedToMsi.split(' ');

    cy.get('.govuk-table__cell').first().each(($row) => {
      const rowText = $row.text();
      expect(rowText).to.include(year);
      if (month) {
        expect(rowText).to.include(month);
      } else {
        cy.log('Month not provided, only checking year');
      }
    });

    return this;
  }

  public verifyFilterApplied(filterType: string, filterValue?: string): this {
    switch (filterType.toLowerCase()) {
      case 'assigned to':
        if (filterValue) {
          this.hasAssignedToFilter(filterValue);
        }
        break;
      case 'advised by':
        if (filterValue) {
          this.hasAdvisedByFilter(filterValue);
        }
        break;
      case 'region':
        if (filterValue) {
          this.hasSelectedRegionFilter(filterValue);
        }
        break;
      case 'local authority':
        if (filterValue) {
          this.hasSelectedLocalAuthorityFilter(filterValue);
        }
        break;
      case 'trust':
        if (filterValue) {
          this.hasSelectedTrustFilter(filterValue);
        }
        break;
      default:
        throw new Error(`Filter type '${filterType}' is not supported for verification.`);
    }
    return this;
  }
}
const homePage = new HomePage();

export default homePage;
