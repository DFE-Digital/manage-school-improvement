using Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Validation
{
    public class PostcodeValidationAttributeTests
    {
        private readonly PostcodeValidationAttribute _attribute;

        public PostcodeValidationAttributeTests()
        {
            _attribute = new PostcodeValidationAttribute();
        }

        #region Helper Methods

        private static ValidationContext CreateValidationContext(object instance, string memberName = "Postcode", string displayName = "Postcode")
        {
            return new ValidationContext(instance)
            {
                MemberName = memberName,
                DisplayName = displayName
            };
        }

        private static ValidationResult? CallProtectedIsValid(PostcodeValidationAttribute attribute, object? value, ValidationContext context)
        {
            var method = typeof(PostcodeValidationAttribute).GetMethod(
                "IsValid",
                BindingFlags.NonPublic | BindingFlags.Instance,
                new[] { typeof(object), typeof(ValidationContext) });

            return (ValidationResult?)method?.Invoke(attribute, new object?[] { value, context });
        }

        #endregion

        #region Valid Postcode Tests (using public IsValid method)

        [Theory]
        [InlineData("SW1A 1AA")]
        [InlineData("SW1A1AA")]
        [InlineData("EC1A 1BB")]
        [InlineData("W1A 0AX")]
        [InlineData("M1 1AE")]
        [InlineData("B33 8TH")]
        [InlineData("CR2 6XH")]
        [InlineData("DN55 1PT")]
        [InlineData("sw1a 1aa")]
        public void IsValid_WithValidPostcodes_ReturnsTrue(string postcode)
        {
            // Act
            var result = _attribute.IsValid(postcode);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("  SW1A 1AA  ")]
        [InlineData("\tEC1A 1BB\t")]
        [InlineData("\nW1A 0AX\n")]
        public void IsValid_WithValidPostcodesAndWhitespace_ReturnsTrue(string postcodeWithWhitespace)
        {
            // Act
            var result = _attribute.IsValid(postcodeWithWhitespace);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Invalid Postcode Tests (using public IsValid method)

        [Theory]
        [InlineData("12345")]
        [InlineData("SW1A 1A")]
        [InlineData("SW1A!1AA")]
        [InlineData("SW1A-1AA")]
        [InlineData("SW1A  1AA")]
        [InlineData("user@example.com")]
        [InlineData("")]
        [InlineData("   ")]
        public void IsValid_WithInvalidPostcodes_ReturnsFalse(string invalidPostcode)
        {
            // Act
            var result = _attribute.IsValid(invalidPostcode);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Protected IsValid Method Tests (using reflection)

        [Fact]
        public void ProtectedIsValid_WithValidPostcode_ReturnsSuccess()
        {
            // Arrange
            var context = CreateValidationContext(new { Postcode = "SW1A 1AA" });

            // Act
            var result = CallProtectedIsValid(_attribute, "SW1A 1AA", context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void ProtectedIsValid_WithInvalidPostcode_ReturnsValidationError()
        {
            // Arrange
            var context = CreateValidationContext(new { Postcode = "INVALID" });

            // Act
            var result = CallProtectedIsValid(_attribute, "INVALID", context);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Postcode must be in the correct format", result.ErrorMessage);
        }

        [Fact]
        public void ProtectedIsValid_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var customAttribute = new PostcodeValidationAttribute
            {
                ErrorMessage = "Enter a valid postcode for {0}"
            };
            var context = CreateValidationContext(new { Postcode = "INVALID" });

            // Act
            var result = CallProtectedIsValid(customAttribute, "INVALID", context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Enter a valid postcode for Postcode", result.ErrorMessage);
        }

        [Fact]
        public void ProtectedIsValid_WithInvalidPostcode_IncludesMemberNameInResult()
        {
            // Arrange
            var context = CreateValidationContext(
                new { PostalCode = "INVALID" },
                memberName: "PostalCode",
                displayName: "Postal Code");

            // Act
            var result = CallProtectedIsValid(_attribute, "INVALID", context);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("PostalCode", result.MemberNames);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void IsValid_WithNullValue_ReturnsTrue()
        {
            // Act
            var result = _attribute.IsValid(null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WithNonStringValue_ReturnsTrue()
        {
            // Act
            var result = _attribute.IsValid(123);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WithBooleanValue_ReturnsTrue()
        {
            // Act
            var result = _attribute.IsValid(true);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region FormatErrorMessage Tests

        [Fact]
        public void FormatErrorMessage_WithDefaultMessage_ReturnsDefaultMessage()
        {
            // Act
            var message = _attribute.FormatErrorMessage("Postcode");

            // Assert
            Assert.Equal("Postcode must be in the correct format", message);
        }

        [Fact]
        public void FormatErrorMessage_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var customAttribute = new PostcodeValidationAttribute
            {
                ErrorMessage = "Enter a valid postcode for {0}"
            };

            // Act
            var message = customAttribute.FormatErrorMessage("School Postcode");

            // Assert
            Assert.Equal("Enter a valid postcode for School Postcode", message);
        }

        #endregion
    }
}

