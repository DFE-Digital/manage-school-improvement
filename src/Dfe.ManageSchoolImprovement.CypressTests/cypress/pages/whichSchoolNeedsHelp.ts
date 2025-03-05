class WhichSchoolNeedsHelp {

public hasHeader(header : string) : this {
    
    header = 'Which school needs help?'

    cy.get('h1').contains(header)

    return this;
}

public withSchoolName(school: string): this {
    cy.getById("SearchQuery").typeFast(school);

    // reassign school from 'Plym' to 'Plymouth Grove Primary School'
    school = 'Plymouth Grove Primary School'

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