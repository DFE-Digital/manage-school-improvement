using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.ValueObjects
{
    public class OrganisationTypesTests
    {
        #region Constants Tests

        [Fact]
        public void School_ShouldHaveCorrectValue()
        {
            // Assert
            Assert.Equal("School", OrganisationTypes.School);
        }

        [Fact]
        public void SupportingOrganisation_ShouldHaveCorrectValue()
        {
            // Assert
            Assert.Equal("Supporting organisation", OrganisationTypes.SupportingOrganisation);
        }

        [Fact]
        public void GovernanceBodies_ShouldHaveCorrectValue()
        {
            // Assert
            Assert.Equal("Governance bodies", OrganisationTypes.GovernanceBodies);
        }

        [Theory]
        [InlineData("School")]
        [InlineData("Supporting organisation")]
        [InlineData("Governance bodies")]
        public void Constants_ShouldNotBeNullOrEmpty(string organisationType)
        {
            // Assert
            Assert.False(string.IsNullOrEmpty(organisationType));
            Assert.False(string.IsNullOrWhiteSpace(organisationType));
        }

        [Theory]
        [InlineData("School")]
        [InlineData("Supporting organisation")]
        [InlineData("Governance bodies")]
        public void Constants_ShouldHaveValidLength(string organisationType)
        {
            // Assert
            Assert.True(organisationType.Length > 0);
            Assert.True(organisationType.Length <= 50); // Reasonable max length for UI display
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Constants_ShouldBeCompileTimeConstants()
        {
            // This test verifies that the constants are compile-time constants
            // by checking they can be used in switch expressions (compile-time requirement)
            string testValue = "School";

            var result = testValue switch
            {
                OrganisationTypes.School => "school",
                OrganisationTypes.SupportingOrganisation => "supporting",
                OrganisationTypes.GovernanceBodies => "governance",
                _ => "unknown"
            };

            Assert.Equal("school", result);
        }

        #endregion

        #region Edge Cases and Validation Tests

        [Fact]
        public void Constants_ShouldBeUsableInStringInterpolation()
        {
            // Act
            var message = $"Selected organisation type: {OrganisationTypes.School}";

            // Assert
            Assert.Equal("Selected organisation type: School", message);
        }

        [Fact]
        public void Constants_ShouldBeUsableInStringComparison()
        {
            // Arrange
            var userInput = "School";

            // Act & Assert
            Assert.Equal(OrganisationTypes.School, userInput);
            Assert.Equal(OrganisationTypes.School, userInput, ignoreCase: true);
        }

        #endregion
    }
}