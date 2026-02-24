using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Pages;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages
{
    public class BaseSupportProjectPageModelTests
    {
        private readonly Mock<ISupportProjectQueryService> _mockQueryService;
        private readonly Mock<ErrorService> _mockErrorService;
        private readonly BaseSupportProjectPageModel _pageModel;
        private CancellationToken _cancellationToken;

        public BaseSupportProjectPageModelTests()
        {
            _mockQueryService = new Mock<ISupportProjectQueryService>();
            _mockErrorService = new Mock<ErrorService>();
            _pageModel = new BaseSupportProjectPageModel(_mockQueryService.Object, _mockErrorService.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetSupportProject_ReturnsPageResult_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;
            var mockProject = new SupportProjectDto(projectId, DateTime.Now, DateTime.Now, "schoolName", "URN234",
                "local Authority", "Region", ProjectStatus: ProjectStatusValue.InProgress);
            var result = Result<SupportProjectDto?>.Success(mockProject);

            _mockQueryService.Setup(s => s.GetSupportProject(projectId, _cancellationToken)).ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProject(projectId, CancellationToken.None);

            // Assert
            Assert.IsType<PageResult>(response);
            Assert.NotNull(_pageModel.SupportProject);
        }

        [Fact]
        public async Task GetSupportProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;
            var result = Result<SupportProjectDto?>.Failure("");
            _mockQueryService.Setup(s => s.GetSupportProject(projectId, _cancellationToken)).ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProject(projectId, _cancellationToken);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void IsReadOnly_ReturnsTrue_WhenSupportProjectIsNull()
        {
            // Arrange - SupportProject is null by default
            // Act & Assert
            Assert.True(_pageModel.IsReadOnly);
        }

        [Fact]
        public void IsReadOnly_ReturnsFalse_WhenProjectStatusIsInProgress()
        {
            // Arrange
            var dto = new SupportProjectDto(1, DateTime.Now, DateTime.Now, "schoolName", "URN234",
                "local Authority", "Region", ProjectStatus: ProjectStatusValue.InProgress);
            _pageModel.SupportProject = SupportProjectViewModel.Create(dto);

            // Act & Assert
            Assert.False(_pageModel.IsReadOnly);
        }

        [Fact]
        public void IsReadOnly_ReturnsTrue_WhenProjectStatusIsPaused()
        {
            // Arrange
            var dto = new SupportProjectDto(1, DateTime.Now, DateTime.Now, "schoolName", "URN234",
                "local Authority", "Region", ProjectStatus: ProjectStatusValue.Paused);
            _pageModel.SupportProject = SupportProjectViewModel.Create(dto);

            // Act & Assert
            Assert.True(_pageModel.IsReadOnly);
        }

        [Fact]
        public void IsReadOnly_ReturnsTrue_WhenProjectStatusIsStopped()
        {
            // Arrange
            var dto = new SupportProjectDto(1, DateTime.Now, DateTime.Now, "schoolName", "URN234",
                "local Authority", "Region", ProjectStatus: ProjectStatusValue.Stopped);
            _pageModel.SupportProject = SupportProjectViewModel.Create(dto);

            // Act & Assert
            Assert.True(_pageModel.IsReadOnly);
        }
    }
}
