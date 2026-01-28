using Dfe.ManageSchoolImprovement.Frontend.Validation;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Validation
{
    public class NameValidationAttributeTests
    {
        private readonly NameValidationAttribute _attribute;

        public NameValidationAttributeTests()
        {
            _attribute = new NameValidationAttribute();
        }

        [Theory]
        [InlineData("John Smith", true)]
        [InlineData("John Doe-Smith", true)]
        [InlineData("John-Smith Doe", true)]
        [InlineData("John Doe-Smith Jr.", false)]
        [InlineData("john smith", false)]
        [InlineData("John smith", false)]
        [InlineData("john Smith", false)]
        [InlineData("John", false)]
        [InlineData("Smith", false)]
        [InlineData("JohnSmith", false)]
        [InlineData("", true)]
        [InlineData(null, true)]
        public void IsValid_ValidatesNameCorrectly(string? value, bool expected)
        {
            // Act
            var result = _attribute.IsValid(value!);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FormatErrorMessage_ReturnsCorrectMessage()
        {
            // Arrange
            var expectedMessage = "First name must start with a capital letter. Last name must start with a capital letter followed by one or more letters (e.g., Jane Smith)";

            // Act
            var errorMessage = _attribute.FormatErrorMessage("Name");

            // Assert
            Assert.Equal(expectedMessage, errorMessage);
        }
    }
}
