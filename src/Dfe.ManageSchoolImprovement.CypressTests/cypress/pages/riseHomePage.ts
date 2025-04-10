class RiseHomePage {
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

  public hasProjectCount(): this {
    cy.get('[data-cy="select-projectlist-filter-count"]').should(
      "contain.text",
      "schools found"
    );
    return this;
  }

  public withProjectFilter(project: string): this {
    cy.getByTestId("search-by-project").typeFast(project);

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

  public applyFilters(): this {
    cy.get('[data-cy="select-projectlist-filter-apply"]').click();

    return this;
  }

  public selectSchool(school: string): this {
    cy.contains(school).click();

    return this;
  }

  public hasSchoolName(school: string = "Plymouth Grove Primary School"): this {
    cy.get('[data-cy="trust-name-0"]').contains(school);

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

  public withFilterRegions(): this {
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

    // Apply the filters
    cy.get('[data-cy="select-projectlist-filter-apply"]').click();

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
    cy.get('[data-cy="clear-filter"]').click();

    return this;
  }
}
const riseHomePage = new RiseHomePage();

export default riseHomePage;
