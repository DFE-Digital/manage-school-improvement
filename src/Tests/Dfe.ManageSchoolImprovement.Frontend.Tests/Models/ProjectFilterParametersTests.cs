using Dfe.ManageSchoolImprovement.Frontend.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class ProjectFilterParametersTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly_WhenValidListsProvided()
        {
            // Arrange
            var regions = new List<string> { "North", "South" };
            var localAuthorities = new List<string> { "Authority1", "Authority2" };

            // Act
            var projectFilterParameters = new ProjectFilterParameters
            {
                Regions = regions,
                LocalAuthorities = localAuthorities
            };

            // Assert
            Assert.Equal(regions, projectFilterParameters.Regions);
            Assert.Equal(localAuthorities, projectFilterParameters.LocalAuthorities);
        }

        [Fact]
        public void Properties_IsInitializedAsEmptyList_ByDefault()
        {
            // Arrange & Act
            var projectFilterParameters = new ProjectFilterParameters();

            // Assert
            Assert.Empty(projectFilterParameters.Regions);
            Assert.Empty(projectFilterParameters.LocalAuthorities);
        }
    }
}
