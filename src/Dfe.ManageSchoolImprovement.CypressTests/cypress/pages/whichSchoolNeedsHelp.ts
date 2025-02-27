class WhichSchoolNeedsHelp {

public hasHeader(header : string) : this {
    
    header = 'Which school needs help?'

    return this;
}

public withSchoolName(school: string): this {
    cy.getById("SearchQuery").typeFast(school);

    // reassign school from 'coo' to 'Commbe Dean School'
    school = 'Coombe Dean School'

    cy.contains(school).click()

    return this;

    }

public clickContinue(): this
{
    cy.contains('Continue').click()

    return this
}

}

const whichSchoolNeedsHelp = new WhichSchoolNeedsHelp();

export default whichSchoolNeedsHelp;