using Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails; 

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Validation
{
    public class EmailValidationAttributeTests
    {
        private readonly EmailValidationAttribute _emailValidationAttribute;

        public EmailValidationAttributeTests()
        {
            _emailValidationAttribute = new EmailValidationAttribute();
        }

        // Test 1: Valid email formats (using InlineData)
        [Theory]
        [InlineData("valid.email@example.com")]
        [InlineData("user123@domain.com")]
        [InlineData("user@sub.domain.com")]
        [InlineData("user@domain-name.com")]
        [InlineData("user_name@domain.com")]
        [InlineData("user@longdomainname.com")]
        [InlineData("user@domain.co.uk")]
        [InlineData("user.name@domain.com")]
        [InlineData("jane-smith.doe@domain.com")]
        [InlineData("john.doe-smith@domain.com")]
        public void ShouldReturnSuccessForValidEmails(string email)
        {
            var result = _emailValidationAttribute.IsValid(email);

            Assert.True(result);
        }

        // Test 2: Invalid email formats (using InlineData)
        [Theory]
        [InlineData("invalid-email")]
        [InlineData("invalid@domain")]
        [InlineData("invalid@")]
        [InlineData("@domain.com")]
        [InlineData("user@domain@domain.com")]
        [InlineData("user@domain..com")]
        [InlineData("")]
        public void ShouldReturnErrorForInvalidEmails(string? email)
        {
            var result = _emailValidationAttribute.IsValid(email);

            Assert.False(result); 
        }
    }
}
