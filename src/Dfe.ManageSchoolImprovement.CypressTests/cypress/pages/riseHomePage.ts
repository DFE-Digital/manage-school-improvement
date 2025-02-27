class RiseHomePage {

    public AddSchool(): this { 
        cy.contains("Add a school").click();
        //   cy.login({role: ProjectRecordCreator})
        return this;
    }
   
    public hasAddSchool(): this { 
        cy.get('[data-cy="select-heading"]').should('contain.text', 'Schools identified for targeted intervention')
        cy.contains("Add a school").should('be.visible')
        return this;
    }

    public hasProjectCount(): this { 
        cy.get('[data-cy="select-projectlist-filter-count"]')
            .should("contain.text", 'schools found');
        return this;
    }

    public withProjectFilter(project: string): this {
        cy.getByTestId("search-by-project").typeFast(project);

        return this;
    }

    public hasProjectFilter(): this {
        cy.get('.moj-filter__options')
            .should('be.visible');

        return this;
    }

    public hasRegionFilter(region: string): this {
        cy.getByTestId(`${region}-option`).should("be.checked");

        return this;
    }


    public hasLocalAuthorityFilter(localAuthority: string): this {
        cy.getByTestId(`${localAuthority}-option`).click()

        return this;
    }


    public applyFilters(): this {
        cy.get('[data-cy="select-projectlist-filter-apply"]').click();

        return this;
    }

    public clearFilters(): this {
        cy.getByTestId("clear-filters").click();

        return this;
    }
    
    public selectSchool(school : string): this {
        cy.contains(school).click()

        return this;
    }

    public hasSchoolName(school : string) : this
    {
        school = "Plymouth Grove Primary School"

        return this;
    }

    public hasURN(URN : string) : this
    {
        URN = '105443'

        return this;
    }

    public hasLocalAuthority(localAuthority : string) : this
    {
        localAuthority = 'Manchester'

        return this;
    }

    public hasRegion(region : string) : this
    {
        region = 'North West'

        return this;
    }
}

const riseHomePage = new RiseHomePage();

export default riseHomePage;
