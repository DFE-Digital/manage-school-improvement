class CheckSchoolDetails {
    
    public clickContinue(): this
    {
        cy.contains('Continue').click()
    
        return this
    }
    
    }
    
    const checkSchoolDetails = new CheckSchoolDetails();
    
    export default checkSchoolDetails;