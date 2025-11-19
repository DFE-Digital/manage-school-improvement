
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Microsoft.Extensions.Primitives;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class ProjectListFiltersTests
    {
        private readonly IDictionary<string, object?> _store;
        private readonly string _title;
        private readonly string[]? _statuses;

        public ProjectListFiltersTests()
        {
            _store = new Dictionary<string, object?>();
            _title = "Test Title";
            _statuses = ["Status1", "Status2"];
        }

        [Fact]
        public void Constructor_SetsEmptyValues_WhenInitialized()
        {
            // Arrange & Act
            var projectListFilters = new ProjectListFilters();

            // Assert
            Assert.Empty(projectListFilters.AvailableDeliveryOfficers);
            Assert.Empty(projectListFilters.AvailableAdvisers);
            Assert.Empty(projectListFilters.AvailableRegions);
            Assert.Empty(projectListFilters.AvailableLocalAuthorities);
            Assert.Empty(projectListFilters.AvailableYears);
            Assert.Null(projectListFilters.Title);
            Assert.Empty(projectListFilters.SelectedStatuses);
            Assert.Empty(projectListFilters.SelectedOfficers);
            Assert.Empty(projectListFilters.SelectedRegions);
            Assert.Empty(projectListFilters.SelectedLocalAuthorities);
            Assert.Empty(projectListFilters.SelectedMonths);
            Assert.Empty(projectListFilters.SelectedYears);
        }

        [Fact]
        public void PersistUsing_CachesFilterValues()
        {
            // Arrange
            var projectListFilters = new ProjectListFilters();
            string[] titles = [_title];
            var store = new Dictionary<string, object?>
            {
                { ProjectListFilters.FilterTitle, titles },
                { ProjectListFilters.FilterStatuses, _statuses }
            };

            // Act
            projectListFilters.PersistUsing(store);

            // Assert
            Assert.Equal(_title, projectListFilters.Title);
            Assert.Equal(_statuses, projectListFilters.SelectedStatuses);
        }

        [Fact]
        public void IsVisible_ReturnsTrue_WhenAnySelectedFiltersExist()
        {
            // Arrange
            var projectListFilters = new ProjectListFilters
            {
                SelectedStatuses = ["Status1"]
            };

            // Act
            var isVisible = projectListFilters.IsVisible;

            // Assert
            Assert.True(isVisible);
        }

        [Fact]
        public void IsVisible_ReturnsFalse_WhenNoSelectedFiltersExist()
        {
            // Arrange
            var projectListFilters = new ProjectListFilters();

            // Act
            var isVisible = projectListFilters.IsVisible;

            // Assert
            Assert.False(isVisible);
        }

        [Fact]
        public void PopulateFrom_ClearsFilters_WhenQueryStringContainsClearKey()
        {
            // Arrange
            var query = new Dictionary<string, StringValues>
            {
                { "clear", "true" }
            };

            var projectListFilters = new ProjectListFilters();
            projectListFilters.PersistUsing(new Dictionary<string, object?>
            {
                { ProjectListFilters.FilterStatuses, _statuses }
            });

            // Act
            projectListFilters.PopulateFrom(query);

            // Assert
            Assert.Null(projectListFilters.Title);
            Assert.Empty(projectListFilters.SelectedStatuses);
            Assert.Empty(projectListFilters.SelectedOfficers);
            Assert.Empty(projectListFilters.SelectedAdvisers);
            Assert.Empty(projectListFilters.SelectedRegions);
            Assert.Empty(projectListFilters.SelectedLocalAuthorities);
            Assert.Empty(projectListFilters.SelectedMonths);
            Assert.Empty(projectListFilters.SelectedYears);

        }

        [Fact]
        public void PopulateFrom_RemovesFilters_WhenQueryStringContainsRemoveKey()
        {
            // Arrange
            var query = new Dictionary<string, StringValues>
            {
                { "remove", "true" },
                { "SelectedStatuses", new StringValues(["Status1"]) }
            };
            var expectedStatus = new string[] { "Status2" };

            var store = new Dictionary<string, object?>
            {
                { ProjectListFilters.FilterStatuses, _statuses }
            };
            var projectListFilters = new ProjectListFilters();
            projectListFilters.PersistUsing(store);

            // Act
            projectListFilters.PopulateFrom(query);

            // Assert
            Assert.Equal(expectedStatus, projectListFilters.SelectedStatuses);
        }
    }
}
