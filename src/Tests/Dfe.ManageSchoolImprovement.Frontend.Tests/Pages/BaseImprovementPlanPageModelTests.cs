using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Pages;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages
{
    public class BaseImprovementPlanPageModelTests
    {
        private readonly Mock<ISupportProjectQueryService> _mockQueryService;
        private readonly Mock<ErrorService> _mockErrorService;
        private readonly BaseImprovementPlanPageModel _pageModel;
        private readonly CancellationToken _cancellationToken;

        public BaseImprovementPlanPageModelTests()
        {
            _mockQueryService = new Mock<ISupportProjectQueryService>();
            _mockErrorService = new Mock<ErrorService>();
            _pageModel = new BaseImprovementPlanPageModel(_mockQueryService.Object, _mockErrorService.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetSupportProject_ReturnsPageResult_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;
            var mockProject = new SupportProjectDto(
                projectId,
                DateTime.Now,
                DateTime.Now,
                "Test School",
                "URN123456",
                "Test Local Authority",
                "South West",
                ProjectStatus: ProjectStatusValue.InProgress);

            var result = Result<SupportProjectDto?>.Success(mockProject);
            _mockQueryService
                .Setup(s => s.GetSupportProjectImprovementPlanAllData(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProject(projectId, _cancellationToken);

            // Assert
            Assert.IsType<PageResult>(response);
            Assert.NotNull(_pageModel.SupportProject);
            Assert.Equal("Test School", _pageModel.SupportProject.SchoolName);
        }

        [Fact]
        public async Task GetSupportProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;
            var result = Result<SupportProjectDto?>.Failure("Project not found");
            _mockQueryService
                .Setup(s => s.GetSupportProjectImprovementPlanAllData(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProject(projectId, _cancellationToken);

            // Assert
            Assert.IsType<NotFoundResult>(response);
            Assert.Null(_pageModel.SupportProject);
        }
        
        [Fact]
        public async Task GetSupportProjectImprovementPlanAndObjectives_ReturnsPageResult_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;
            var improvementPlanId = Guid.NewGuid();
            var objectiveId = Guid.NewGuid();

            var objectives = new List<ImprovementPlanObjectiveDto>
            {
                new ImprovementPlanObjectiveDto(
                    objectiveId,
                    1,
                    improvementPlanId,
                    1,
                    "Improve Teaching Quality",
                    "Focus on evidence-based teaching strategies")
            };

            var improvementPlans = new List<ImprovementPlanDto>
            {
                new ImprovementPlanDto(
                    improvementPlanId,
                    1,
                    projectId,
                    true,
                    objectives,
                    Enumerable.Empty<ImprovementPlanReviewDto>())
            };

            var mockProject = new SupportProjectDto(
                projectId,
                DateTime.Now,
                DateTime.Now,
                "Test School",
                "URN123456",
                "Test Local Authority",
                "South West",
                ImprovementPlans: improvementPlans,
                ProjectStatus: ProjectStatusValue.InProgress);

            var result = Result<SupportProjectDto?>.Success(mockProject);
            _mockQueryService
                .Setup(s => s.GetSupportProjectImprovementPlanAndObjectives(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectImprovementPlanAndObjectives(projectId, _cancellationToken);

            // Assert
            Assert.IsType<PageResult>(response);
            Assert.NotNull(_pageModel.SupportProject);
            Assert.NotNull(_pageModel.SupportProject.ImprovementPlans);
            Assert.Single(_pageModel.SupportProject.ImprovementPlans);
            Assert.NotNull(_pageModel.SupportProject.ImprovementPlans.First().ImprovementPlanObjectives);
            Assert.Single(_pageModel.SupportProject.ImprovementPlans.First().ImprovementPlanObjectives);
        }

        [Fact]
        public async Task GetSupportProjectImprovementPlanAndObjectives_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;
            var result = Result<SupportProjectDto?>.Failure("Project not found");
            _mockQueryService
                .Setup(s => s.GetSupportProjectImprovementPlanAndObjectives(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectImprovementPlanAndObjectives(projectId, _cancellationToken);

            // Assert
            Assert.IsType<NotFoundResult>(response);
            Assert.Null(_pageModel.SupportProject);
        }



        [Fact]
        public async Task GetSupportProjectProgressReviews_ReturnsPageResult_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;
            var improvementPlanId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();

            var reviews = new List<ImprovementPlanReviewDto>
            {
                new ImprovementPlanReviewDto(
                    reviewId,
                    1,
                    improvementPlanId,
                    DateTime.Now.AddDays(-30),
                    DateTime.Now.AddDays(30),
                    "Test Reviewer",
                    "First Progress Review",
                    1,
                    "School is making good progress",
                    "Significant improvements in teaching quality",
                    Enumerable.Empty<ImprovementPlanObjectiveProgressDto>())
            };

            var improvementPlans = new List<ImprovementPlanDto>
            {
                new ImprovementPlanDto(
                    improvementPlanId,
                    1,
                    projectId,
                    true,
                    Enumerable.Empty<ImprovementPlanObjectiveDto>(),
                    reviews)
            };

            var mockProject = new SupportProjectDto(
                projectId,
                DateTime.Now,
                DateTime.Now,
                "Test School",
                "URN123456",
                "Test Local Authority",
                "South West",
                ImprovementPlans: improvementPlans,
                ProjectStatus: ProjectStatusValue.InProgress);

            var result = Result<SupportProjectDto?>.Success(mockProject);
            _mockQueryService
                .Setup(s => s.GetImprovementPlanProgressReviews(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectProgressReviews(projectId, _cancellationToken);

            // Assert
            Assert.IsType<PageResult>(response);
            Assert.NotNull(_pageModel.SupportProject);
            Assert.NotNull(_pageModel.SupportProject.ImprovementPlans);
            Assert.Single(_pageModel.SupportProject.ImprovementPlans);
            Assert.NotNull(_pageModel.SupportProject.ImprovementPlans.First().ImprovementPlanReviews);
            Assert.Single(_pageModel.SupportProject.ImprovementPlans.First().ImprovementPlanReviews);
        }

        [Fact]
        public async Task GetSupportProjectProgressReviews_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;
            var result = Result<SupportProjectDto?>.Failure("Project not found");
            _mockQueryService
                .Setup(s => s.GetImprovementPlanProgressReviews(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectProgressReviews(projectId, _cancellationToken);

            // Assert
            Assert.IsType<NotFoundResult>(response);
            Assert.Null(_pageModel.SupportProject);
        }


        

        [Fact]
        public async Task GetSupportProjectSummary_ReturnsPageResult_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;
            var mockSummary = new SupportProjectSummaryDto(projectId, "Test School");
            var result = Result<SupportProjectSummaryDto?>.Success(mockSummary);

            _mockQueryService
                .Setup(s => s.GetSupportProjectSummary(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectSummary(projectId, _cancellationToken);

            // Assert
            Assert.IsType<PageResult>(response);
            Assert.NotNull(_pageModel.SupportProjectSummary);
            Assert.Equal(projectId, _pageModel.SupportProjectSummary.Id);
            Assert.Equal("Test School", _pageModel.SupportProjectSummary.SchoolName);
        }

        [Fact]
        public async Task GetSupportProjectSummary_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;
            var result = Result<SupportProjectSummaryDto?>.Failure("Project not found");

            _mockQueryService
                .Setup(s => s.GetSupportProjectSummary(projectId, _cancellationToken))
                .ReturnsAsync(result);

            // Act
            var response = await _pageModel.GetSupportProjectSummary(projectId, _cancellationToken);

            // Assert
            Assert.IsType<NotFoundResult>(response);
            Assert.Null(_pageModel.SupportProjectSummary);
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
            var dto = new SupportProjectDto(
                1,
                DateTime.Now,
                DateTime.Now,
                "Test School",
                "URN123456",
                "Test Local Authority",
                "South West",
                ProjectStatus: ProjectStatusValue.InProgress);

            _pageModel.SupportProject = SupportProjectViewModel.Create(dto);

            // Act & Assert
            Assert.False(_pageModel.IsReadOnly);
        }

        [Fact]
        public void IsReadOnly_ReturnsTrue_WhenProjectStatusIsPaused()
        {
            // Arrange
            var dto = new SupportProjectDto(
                1,
                DateTime.Now,
                DateTime.Now,
                "Test School",
                "URN123456",
                "Test Local Authority",
                "South West",
                ProjectStatus: ProjectStatusValue.Paused);

            _pageModel.SupportProject = SupportProjectViewModel.Create(dto);

            // Act & Assert
            Assert.True(_pageModel.IsReadOnly);
        }

        [Fact]
        public void IsReadOnly_ReturnsTrue_WhenProjectStatusIsStopped()
        {
            // Arrange
            var dto = new SupportProjectDto(
                1,
                DateTime.Now,
                DateTime.Now,
                "Test School",
                "URN123456",
                "Test Local Authority",
                "South West",
                ProjectStatus: ProjectStatusValue.Stopped);

            _pageModel.SupportProject = SupportProjectViewModel.Create(dto);

            // Act & Assert
            Assert.True(_pageModel.IsReadOnly);
        }
        

        [Fact]
        public void TaskUpdated_CanBeSetAndRetrieved()
        {
            // Arrange
            var initialValue = _pageModel.TaskUpdated;

            // Act
            _pageModel.TaskUpdated = true;

            // Assert
            Assert.True(_pageModel.TaskUpdated);
            Assert.False(initialValue);
        }

    }
}

