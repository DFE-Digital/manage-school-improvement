using Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AllocateAdviser;
using System.ComponentModel.DataAnnotations; 

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Validation
{
    public class RiseAdviserEmailAttributeTests
    {
        private readonly RiseAdviserEmailAttribute _attribute = new RiseAdviserEmailAttribute();

        [Theory]
        [InlineData("john.doe-rise@education.gov.uk")]
        [InlineData("jane-lee.smith-rise@education.gov.uk")]
        [InlineData("john.doe-smith-rise@education.gov.uk")]
        public void IsValid_ValidEmail_ReturnsSuccess(string email)
        {
            var result = _attribute.GetValidationResult(email, new ValidationContext(new { }));
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("john.doe@education.gov.uk")]
        [InlineData("johndoe-rise@education.gov.uk")]
        [InlineData("john.doe-rise@gmail.com")]
        [InlineData("john.doe-rise@education.com")]
        [InlineData("john.doe-rise@education.gov.uk.co")]
        [InlineData("john.doe--rise@education.gov.uk")]
        [InlineData("john.doe.rise@education.gov.uk")]
        [InlineData("")]
        [InlineData(null)]
        public void IsValid_InvalidEmail_ReturnsValidationError(string? email)
        {
            var result = _attribute.GetValidationResult(email, new ValidationContext(new { }));
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Contains("Email must be in the format", result?.ErrorMessage);
        } 
    }

}
