using GovUK.Dfe.CoreLibs.Contracts.Academies.V4;
using Dfe.ManageSchoolImprovement.Frontend.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class PaginationViewModelTests
    {
        [Fact]
        public void PaginationViewModel_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var pagination = new PaginationViewModel();

            // Assert
            Assert.Equal(10, pagination.PageSize);
            Assert.Equal(1, pagination.CurrentPage);
            Assert.Equal("/", pagination.PagePath);
            Assert.NotNull(pagination.Paging);
            Assert.False(pagination.HasPreviousPage);
            Assert.False(pagination.HasNextPage);
            Assert.Equal(1, pagination.StartingPage);
        }

        [Fact]
        public void HasPreviousPage_ShouldBeTrue_WhenCurrentPageGreaterThanOne()
        {
            // Arrange
            var pagination = new PaginationViewModel { CurrentPage = 2 };

            // Act & Assert
            Assert.True(pagination.HasPreviousPage);
        }

        [Fact]
        public void HasNextPage_ShouldBeTrue_WhenNextPageUrlExists()
        {
            // Arrange
            var pagination = new PaginationViewModel
            {
                Paging = new PagingResponse { NextPageUrl = "https://example.com/next" }
            };

            // Act & Assert
            Assert.True(pagination.HasNextPage);
        }

        [Fact]
        public void StartingPage_ShouldBeOne_WhenCurrentPageLessThanOrEqualToFive()
        {
            // Arrange
            var pagination = new PaginationViewModel { CurrentPage = 4 };

            // Act & Assert
            Assert.Equal(1, pagination.StartingPage);
        }

        [Fact]
        public void StartingPage_ShouldBeOffset_WhenCurrentPageGreaterThanFive()
        {
            // Arrange
            var pagination = new PaginationViewModel { CurrentPage = 10 };

            // Act & Assert
            Assert.Equal(5, pagination.StartingPage);
        }

        [Fact]
        public void PreviousPage_ShouldBeOneLessThanCurrentPage()
        {
            // Arrange
            var pagination = new PaginationViewModel { CurrentPage = 5 };

            // Act & Assert
            Assert.Equal(4, pagination.PreviousPage);
        }

        [Fact]
        public void NextPage_ShouldBeOneMoreThanCurrentPage()
        {
            // Arrange
            var pagination = new PaginationViewModel { CurrentPage = 3 };

            // Act & Assert
            Assert.Equal(4, pagination.NextPage);
        }

        [Theory]
        [InlineData(50, 10, 5)]
        [InlineData(51, 10, 6)]
        [InlineData(100, 20, 5)]
        [InlineData(99, 20, 5)]
        public void TotalPages_ShouldCalculateCorrectly(int recordCount, int pageSize, int expectedTotalPages)
        {
            // Arrange
            var pagination = new PaginationViewModel
            {
                PageSize = pageSize,
                Paging = new PagingResponse { RecordCount = recordCount }
            };

            // Act & Assert
            Assert.Equal(expectedTotalPages, pagination.TotalPages);
        }
    }
}
