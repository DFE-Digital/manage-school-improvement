using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Pages;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages
{
    public class BaseContactsPageModelTests
    {
        private readonly Mock<ISupportProjectQueryService> _mockQueryService;
        private readonly ErrorService _errorService;
        private readonly BaseContactsPageModel _pageModel;
        private readonly CancellationToken _cancellationToken;

        public BaseContactsPageModelTests()
        {
            _mockQueryService = new Mock<ISupportProjectQueryService>();
            _errorService = new ErrorService();
            _pageModel = CreatePageModel();
            _cancellationToken = CancellationToken.None;
        }

        private BaseContactsPageModel CreatePageModel()
        {
            var pageModel = new BaseContactsPageModel(_mockQueryService.Object, _errorService);

            var serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), new ModelStateDictionary());
            pageModel.PageContext = new PageContext(actionContext);

            return pageModel;
        }

        [Fact]
        public async Task GetSupportProjectWithContacts_ReturnsPageResult_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;
            var mockProject = new SupportProjectDto(projectId, DateTime.Now, DateTime.Now, "schoolName", "URN234",
                "local Authority", "Region", ProjectStatus: ProjectStatusValue.InProgress);
            var result = Result<SupportProjectDto?>.Success(mockProject);

            _mockQueryService.Setup(s => s.GetSupportProjectWithContacts(projectId, _cancellationToken)).ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectWithContacts(projectId, CancellationToken.None);

            // Assert
            Assert.IsType<PageResult>(response);
            Assert.NotNull(_pageModel.SupportProject);
        }

        [Fact]
        public async Task GetSupportProjectWithContacts_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;
            var result = Result<SupportProjectDto?>.Failure("");
            _mockQueryService.Setup(s => s.GetSupportProjectWithContacts(projectId, _cancellationToken)).ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectWithContacts(projectId, _cancellationToken);

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

