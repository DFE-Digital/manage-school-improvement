class CheckSchoolDetails {
    
    public hasHeader(header : string): this
    {
        header = 'Check school details'

        return  this;
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

    public hasSchoolType(schoolType : string) : this
    {
        schoolType = 'Community school'

        return this;
    }

    public hasFaithSchool(faithSchool : string) : this
    {
        faithSchool = 'Does not apply'

        return this;
    }

    public hasOfstedRating(ofstedRating : string) : this
    {
        ofstedRating = 'Good'

        return this;
    }

    public hasLastInspection(lastInspection : string) : this
    {
        lastInspection = '16 January 2011'

        return this;
    }

    public hasPFI(pfi : string) : this
    {
        pfi = 'Does not apply'

        return this;
    }

    public clickContinue(): this
    {
        cy.contains('Continue').click()
    
        return this;
    }
    
    }
    
    const checkSchoolDetails = new CheckSchoolDetails();
    
    export default checkSchoolDetails;