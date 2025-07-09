using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages.Contacts
{
    public class PhoneValidationAttributeTests
    {
        [AttributeUsage(AttributeTargets.Property)]
        private class TestablePhoneValidationAttribute : PhoneValidationAttribute
        {
            public ValidationResult TestIsValid(object? value, ValidationContext validationContext)
            {
                return base.IsValid(value, validationContext);
            }
        }

        private readonly TestablePhoneValidationAttribute _validator;

        public PhoneValidationAttributeTests()
        {
            _validator = new TestablePhoneValidationAttribute();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValid_WhenValueIsNullOrWhitespace_ReturnsSuccess(string? value)
        {
            // Arrange
            var context = new ValidationContext(new object());

            // Act
            var result = _validator.TestIsValid(value, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("07123456789")]     // UK mobile
        [InlineData("+447123456789")]   // UK mobile with country code
        [InlineData("+44 7123456789")]  // UK mobile with space after country code
        [InlineData("01234567890")]     // UK landline
        [InlineData("+441234567890")]   // UK landline with country code
        [InlineData("+44 1234567890")]  // UK landline with space after country code
        public void IsValid_WhenValidPhoneNumber_ReturnsSuccess(string phoneNumber)
        {
            // Arrange
            var context = new ValidationContext(new object());

            // Act
            var result = _validator.TestIsValid(phoneNumber, context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("123")]             // Too short
        [InlineData("12345678901234")] // Too long
        [InlineData("abcdefghijk")]    // Contains letters
        [InlineData("04123456789")]    // Invalid prefix
        public void IsValid_WhenInvalidPhoneNumber_ReturnsValidationError(string phoneNumber)
        {
            // Arrange
            var context = new ValidationContext(new object());

            // Act
            var result = _validator.TestIsValid(phoneNumber, context);

            // Assert
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Enter a valid phone number", result.ErrorMessage);
        }
    }
}