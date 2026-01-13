using Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Validation
{
    public class EmailValidationAttributeTests
    {
        private readonly EmailValidationAttribute _attribute;

        public EmailValidationAttributeTests()
        {
            _attribute = new EmailValidationAttribute();
        }

        #region Helper Methods

        private static ValidationContext CreateValidationContext(object instance, string memberName = "Email", string displayName = "Email")
        {
            return new ValidationContext(instance)
            {
                MemberName = memberName,
                DisplayName = displayName
            };
        }

        // Helper method to call the protected IsValid method using reflection
        private ValidationResult? CallProtectedIsValid(EmailValidationAttribute attribute, object? value, ValidationContext context)
        {
            var method = typeof(EmailValidationAttribute).GetMethod("IsValid",
                BindingFlags.NonPublic | BindingFlags.Instance,
                new[] { typeof(object), typeof(ValidationContext) });

            return (ValidationResult?)method?.Invoke(attribute, new object?[] { value, context });
        }

        #endregion

        #region Valid Email Tests (using public IsValid method)

        [Theory]
        [InlineData("user@example.com")]
        [InlineData("test.email@domain.co.uk")]
        [InlineData("user123@test-domain.com")]
        [InlineData("firstname.lastname@company.org")]
        [InlineData("user_name@domain.info")]
        [InlineData("test@subdomain.domain.com")]
        [InlineData("a@b.co")]
        [InlineData("user+tag@example.com")]
        [InlineData("user.name+tag@example.com")]
        public void IsValid_WithValidEmails_ReturnsTrue(string email)
        {
            // Act
            var result = _attribute.IsValid(email);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("  user@example.com  ")]
        [InlineData("\ttest@domain.com\t")]
        [InlineData("\nuser@example.org\n")]
        public void IsValid_WithValidEmailsAndWhitespace_ReturnsTrue(string emailWithWhitespace)
        {
            // Act
            var result = _attribute.IsValid(emailWithWhitespace);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Invalid Email Tests (using public IsValid method)

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("@domain.com")]
        [InlineData("user@")]
        [InlineData("user@domain")]
        [InlineData("user@domain.")]
        [InlineData("user@@domain.com")]
        [InlineData("user@domain@com")]
        [InlineData("user@domain..com")]
        [InlineData("user@.domain.com")]
        [InlineData("user@domain.c")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("user name@domain.com")]
        [InlineData("user@domain .com")]
        [InlineData("user@domain. com")]
        public void IsValid_WithInvalidEmails_ReturnsFalse(string invalidEmail)
        {
            // Act
            var result = _attribute.IsValid(invalidEmail);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Protected IsValid Method Tests (using reflection)

        [Fact]
        public void ProtectedIsValid_WithValidEmail_ReturnsSuccess()
        {
            // Arrange
            var context = CreateValidationContext(new { Email = "test@example.com" });

            // Act
            var result = CallProtectedIsValid(_attribute, "test@example.com", context);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void ProtectedIsValid_WithInvalidEmail_ReturnsValidationError()
        {
            // Arrange
            var context = CreateValidationContext(new { Email = "invalid" });

            // Act
            var result = CallProtectedIsValid(_attribute, "invalid", context);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Email address must be in the correct format", result.ErrorMessage);
        }

        [Fact]
        public void ProtectedIsValid_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var customAttribute = new EmailValidationAttribute
            {
                ErrorMessage = "Please enter a valid email address like name@example.com"
            };
            var context = CreateValidationContext(new { Email = "invalid" });

            // Act
            var result = CallProtectedIsValid(customAttribute, "invalid", context);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Please enter a valid email address like name@example.com", result.ErrorMessage);
        }

        [Fact]
        public void ProtectedIsValid_WithInvalidEmail_IncludesMemberNameInResult()
        {
            // Arrange
            var context = CreateValidationContext(
                new { EmailAddress = "invalid" },
                memberName: "EmailAddress",
                displayName: "Email Address");

            // Act
            var result = CallProtectedIsValid(_attribute, "invalid", context);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("EmailAddress", result.MemberNames);
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
            var message = _attribute.FormatErrorMessage("Email");

            // Assert
            Assert.Equal("Email address must be in the correct format", message);
        }

        [Fact]
        public void FormatErrorMessage_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var customAttribute = new EmailValidationAttribute
            {
                ErrorMessage = "Enter a valid email address for {0}"
            };

            // Act
            var message = customAttribute.FormatErrorMessage("Email Address");

            // Assert
            Assert.Equal("Enter a valid email address for Email Address", message);
        }

        #endregion

        #region Constructor Tests

        [Fact]
        public void Constructor_SetsDefaultErrorMessage()
        {
            // Arrange & Act
            var attribute = new EmailValidationAttribute();

            // Assert
            Assert.Equal("Email address must be in the correct format", attribute.FormatErrorMessage("Test"));
        }

        [Fact]
        public void Constructor_AllowsErrorMessageOverride()
        {
            // Arrange & Act
            var attribute = new EmailValidationAttribute
            {
                ErrorMessage = "Custom error message"
            };

            // Assert
            Assert.Equal("Custom error message", attribute.FormatErrorMessage("Test"));
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void IsValid_WithRealWorldEmailAddresses_WorksCorrectly()
        {
            // Arrange
            var validEmails = new[]
            {
                "john.doe@company.co.uk",
                "support@github.com",
                "user+label@example.org",
                "firstname-lastname@domain-name.info"
            };

            var invalidEmails = new[]
            {
                "plainaddress",
                "user@",
                "@missingdomain.com",
                "user.domain.com",
                "user@domain@domain.com"
            };

            // Act & Assert
            foreach (var email in validEmails)
            {
                var result = _attribute.IsValid(email);
                Assert.True(result, $"Expected {email} to be valid");
            }

            foreach (var email in invalidEmails)
            {
                var result = _attribute.IsValid(email);
                Assert.False(result, $"Expected {email} to be invalid");
            }
        }

        [Fact]
        public void IsValid_WithEmailsContainingSpaces_ReturnsFalse()
        {
            // Arrange
            var emailsWithInternalSpaces = new[]
            {
                "user name@domain.com",
                "user@domain name.com",
                "user @ domain.com"
            };

            // Act & Assert
            foreach (var email in emailsWithInternalSpaces)
            {
                var result = _attribute.IsValid(email);
                Assert.False(result, $"Expected {email} to be invalid due to spaces");
            }
        }

        #endregion
    }
}