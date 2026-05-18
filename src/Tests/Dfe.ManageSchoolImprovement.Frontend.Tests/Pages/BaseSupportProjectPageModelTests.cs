using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Pages;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
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
    public class BaseSupportProjectPageModelTests
    {
        private readonly Mock<ISupportProjectQueryService> _mockQueryService;
        private readonly ErrorService _errorService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly BaseSupportProjectPageModel _pageModel;
        private readonly CancellationToken _cancellationToken;

        public BaseSupportProjectPageModelTests()
        {
            _mockQueryService = new Mock<ISupportProjectQueryService>();
            _errorService = new ErrorService();
            _mockMediator = new Mock<IMediator>();
            _pageModel = CreatePageModelWithMediator();
            _cancellationToken = CancellationToken.None;
        }

        private BaseSupportProjectPageModel CreatePageModelWithMediator()
        {
            var pageModel = new BaseSupportProjectPageModel(_mockQueryService.Object, _errorService);

            var serviceProvider = new ServiceCollection()
                .AddSingleton(_mockMediator.Object)
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
        
        [Fact]
        public async Task GetSupportProjectSummary_ReturnsPageResult_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;

            var mockProject = new SupportProjectSummaryDto(
                projectId,
                "School A");

            var result = Result<SupportProjectSummaryDto?>.Success(mockProject);

            _mockQueryService
                .Setup(s => s.GetSupportProjectSummary(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectSummary(
                projectId,
                _cancellationToken);

            // Assert
            Assert.IsType<PageResult>(response);

            Assert.NotNull(_pageModel.SupportProjectSummary);

            Assert.Equal(projectId, _pageModel.SupportProjectSummary!.Id);
            Assert.Equal("School A", _pageModel.SupportProjectSummary.SchoolName);
        }
        
        [Fact]
        public async Task GetSupportProjectSummary_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;

            var result = Result<SupportProjectSummaryDto?>.Failure("");

            _mockQueryService
                .Setup(s => s.GetSupportProjectSummary(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectSummary(
                projectId,
                _cancellationToken);

            // Assert
            Assert.IsType<NotFoundResult>(response);

            Assert.Null(_pageModel.SupportProjectSummary);
        }

        [Fact]
        public async Task UpdateCurrentDeliveryMilestone_SendsCommand_WhenCurrentMilestoneIsNull()
        {
            // Arrange
            const int projectId = 1;
            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetCurrentDeliveryMilestoneCommand>(), _cancellationToken))
                .ReturnsAsync(true);

            // Act
            await _pageModel.UpdateCurrentDeliveryMilestone(
                projectId,
                null,
                Milestone.InitialDiagnosis,
                _cancellationToken);

            // Assert
            _mockMediator.Verify(m => m.Send(
                It.Is<SetCurrentDeliveryMilestoneCommand>(cmd =>
                    cmd.SupportProjectId == new SupportProjectId(projectId) &&
                    cmd.CurrentDeliveryMilestone == Milestone.InitialDiagnosis),
                _cancellationToken), Times.Once);
            Assert.False(_errorService.HasErrors());
        }

        [Fact]
        public async Task UpdateCurrentDeliveryMilestone_SendsCommand_WhenNewMilestoneIsGreaterThanCurrent()
        {
            // Arrange
            const int projectId = 1;
            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetCurrentDeliveryMilestoneCommand>(), _cancellationToken))
                .ReturnsAsync(true);

            // Act
            await _pageModel.UpdateCurrentDeliveryMilestone(
                projectId,
                Milestone.FormallyInformResponsibleBody,
                Milestone.InitialDiagnosis,
                _cancellationToken);

            // Assert
            _mockMediator.Verify(m => m.Send(
                It.Is<SetCurrentDeliveryMilestoneCommand>(cmd =>
                    cmd.SupportProjectId == new SupportProjectId(projectId) &&
                    cmd.CurrentDeliveryMilestone == Milestone.InitialDiagnosis),
                _cancellationToken), Times.Once);
            Assert.False(_errorService.HasErrors());
        }

        [Fact]
        public async Task UpdateCurrentDeliveryMilestone_DoesNotSendCommand_WhenNewMilestoneEqualsCurrent()
        {
            // Arrange
            const int projectId = 1;

            // Act
            await _pageModel.UpdateCurrentDeliveryMilestone(
                projectId,
                Milestone.InitialDiagnosis,
                Milestone.InitialDiagnosis,
                _cancellationToken);

            // Assert
            _mockMediator.Verify(
                m => m.Send(It.IsAny<SetCurrentDeliveryMilestoneCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Assert.False(_errorService.HasErrors());
        }

        [Fact]
        public async Task UpdateCurrentDeliveryMilestone_DoesNotSendCommand_WhenNewMilestoneIsLessThanCurrent()
        {
            // Arrange
            const int projectId = 1;

            // Act
            await _pageModel.UpdateCurrentDeliveryMilestone(
                projectId,
                Milestone.InitialDiagnosis,
                Milestone.FormallyInformResponsibleBody,
                _cancellationToken);

            // Assert
            _mockMediator.Verify(
                m => m.Send(It.IsAny<SetCurrentDeliveryMilestoneCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);
            Assert.False(_errorService.HasErrors());
        }

        [Fact]
        public async Task UpdateCurrentDeliveryMilestone_AddsApiError_WhenMediatorReturnsFalse()
        {
            // Arrange
            const int projectId = 1;
            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetCurrentDeliveryMilestoneCommand>(), _cancellationToken))
                .ReturnsAsync(false);

            // Act
            await _pageModel.UpdateCurrentDeliveryMilestone(
                projectId,
                null,
                Milestone.MatchingComplete,
                _cancellationToken);

            // Assert
            _mockMediator.Verify(m => m.Send(It.IsAny<SetCurrentDeliveryMilestoneCommand>(), _cancellationToken), Times.Once);
            Assert.True(_errorService.HasErrors());
            Assert.Contains("There is a system problem", _errorService.GetErrors().Single().Message);
        }

        [Fact]
        public async Task UpdateCurrentDeliveryMilestone_DoesNotAddApiError_WhenMediatorReturnsTrue()
        {
            // Arrange
            const int projectId = 1;
            _mockMediator
                .Setup(m => m.Send(It.IsAny<SetCurrentDeliveryMilestoneCommand>(), _cancellationToken))
                .ReturnsAsync(true);

            // Act
            await _pageModel.UpdateCurrentDeliveryMilestone(
                projectId,
                Milestone.FirstRiseMeeting,
                Milestone.MatchingComplete,
                _cancellationToken);

            // Assert
            _mockMediator.Verify(m => m.Send(It.IsAny<SetCurrentDeliveryMilestoneCommand>(), _cancellationToken), Times.Once);
            Assert.False(_errorService.HasErrors());
        }
    }
}
