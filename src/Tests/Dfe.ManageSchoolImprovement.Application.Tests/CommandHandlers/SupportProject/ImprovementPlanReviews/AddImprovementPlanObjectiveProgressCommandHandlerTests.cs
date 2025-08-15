using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlans;

public class AddImprovementPlanObjectiveProgressCommandHandlerTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly ImprovementPlanId _improvementPlanId;
    private readonly ImprovementPlanReviewId _improvementPlanReviewId;
    private readonly ImprovementPlanObjectiveId _improvementPlanObjectiveId;
    private readonly CancellationToken _cancellationToken;

    public AddImprovementPlanObjectiveProgressCommandHandlerTests()
    {
        _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
        var fixture = new Fixture();
        _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
        _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
        _improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
        _improvementPlanObjectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
        _cancellationToken = CancellationToken.None;

        // Set up the support project with the required improvement plan structure
        SetupMockSupportProject();
    }

    private void SetupMockSupportProject()
    {
        // Add improvement plan to the support project
        _mockSupportProject.AddImprovementPlan(_improvementPlanId, _mockSupportProject.Id);

        // Add an objective to the improvement plan
        _mockSupportProject.AddImprovementPlanObjective(
            _improvementPlanObjectiveId,
            _improvementPlanId,
            "Quality of education",
            "Test objective");

        // Add a review to the improvement plan
        _mockSupportProject.AddImprovementPlanReview(
            _improvementPlanReviewId,
            _improvementPlanId,
            "Test Reviewer",
            DateTime.UtcNow);
    }

    #region Handle Tests

    [Fact]
    public async Task Handle_ValidCommand_CreatesObjectiveProgress()
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress has been made"
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ImprovementPlanObjectiveProgressId>(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullSupportProject_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress has been made"
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
            .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(command, _cancellationToken));

        Assert.Equal($"Support project with id {_mockSupportProject.Id} not found", exception.Message);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("On track", "Making excellent progress")]
    [InlineData("Behind", "Some challenges encountered")]
    [InlineData("At risk", "Significant issues need addressing")]
    [InlineData("Complete", "All objectives have been met")]
    public async Task Handle_ValidCommandWithDifferentProgressStatuses_CreatesObjectiveProgress(string progressStatus, string progressDetails)
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            progressStatus,
            progressDetails
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ImprovementPlanObjectiveProgressId>(result);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "Some details")]
    [InlineData("Status", "")]
    public async Task Handle_WithEmptyStrings_CreatesObjectiveProgress(string progressStatus, string progressDetails)
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            progressStatus,
            progressDetails
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ImprovementPlanObjectiveProgressId>(result);
    }


    [Fact]
    public async Task Handle_ValidCommand_CallsAddImprovementPlanObjectiveProgressOnSupportProject()
    {
        // Arrange
        var progressStatus = "On track";
        var progressDetails = "Making good progress";
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            progressStatus,
            progressDetails
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Get the initial count of objective progresses in the review
        var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
        var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
        var initialProgressCount = review.ImprovementPlanObjectiveProgresses.Count();

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ImprovementPlanObjectiveProgressId>(result);

        // Verify that a new objective progress was added
        var finalProgressCount = review.ImprovementPlanObjectiveProgresses.Count();
        Assert.Equal(initialProgressCount + 1, finalProgressCount);

        // Verify the added progress has the correct properties
        var addedProgress = review.ImprovementPlanObjectiveProgresses.Last();
        Assert.Equal(progressStatus, addedProgress.HowIsSchoolProgressing);
        Assert.Equal(progressDetails, addedProgress.ProgressDetails);
        Assert.Equal(_improvementPlanObjectiveId, addedProgress.ImprovementPlanObjectiveId);
        Assert.Equal(_improvementPlanReviewId, addedProgress.ImprovementPlanReviewId);
    }

    [Fact]
    public async Task Handle_ValidCommand_GeneratesUniqueProgressId()
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress"
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result1 = await handler.Handle(command, _cancellationToken);
        var result2 = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.NotEqual(result1.Value, result2.Value);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsCorrectType()
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress"
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.IsType<ImprovementPlanObjectiveProgressId>(result);
        Assert.NotEqual(Guid.Empty, result.Value);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress"
        );

        var expectedException = new InvalidOperationException("Database error");
        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
            .ThrowsAsync(expectedException);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, _cancellationToken));

        Assert.Equal(expectedException.Message, actualException.Message);
    }

    [Fact]
    public async Task Handle_UpdateAsyncFails_PropagatesException()
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress"
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var expectedException = new InvalidOperationException("Update failed");
        _mockSupportProjectRepository
            .Setup(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken))
            .ThrowsAsync(expectedException);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, _cancellationToken));

        Assert.Equal(expectedException.Message, actualException.Message);
    }

    [Fact]
    public async Task Handle_WithLongProgressDetails_CreatesObjectiveProgress()
    {
        // Arrange
        var longProgressDetails = new string('A', 2000); // Very long string
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            longProgressDetails
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ImprovementPlanObjectiveProgressId>(result);
    }

    #endregion

    #region Command Tests

    [Fact]
    public void AddImprovementPlanObjectiveProgressCommand_WithValidParameters_CreatesCommand()
    {
        // Arrange & Act
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress"
        );

        // Assert
        Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
        Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
        Assert.Equal(_improvementPlanReviewId, command.ImprovementPlanReviewId);
        Assert.Equal(_improvementPlanObjectiveId, command.ImprovementPlanObjectiveId);
        Assert.Equal("On track", command.progressStatus);
        Assert.Equal("Good progress", command.progressDetails);
    }

    [Fact]
    public void AddImprovementPlanObjectiveProgressCommand_ImplementsIRequest()
    {
        // Assert
        Assert.True(typeof(AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand)
            .IsAssignableTo(typeof(MediatR.IRequest<ImprovementPlanObjectiveProgressId>)));
    }

    #endregion

    #region Repository Interaction Tests

    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectSupportProjectId()
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress"
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        await handler.Handle(command, _cancellationToken);

        // Assert
        _mockSupportProjectRepository.Verify(repo =>
            repo.GetSupportProjectById(_mockSupportProject.Id, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_CallsUpdateAsyncOnRepository()
    {
        // Arrange
        var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
            _mockSupportProject.Id,
            _improvementPlanId,
            _improvementPlanReviewId,
            _improvementPlanObjectiveId,
            "On track",
            "Good progress"
        );

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
            .ReturnsAsync(_mockSupportProject);

        var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        await handler.Handle(command, _cancellationToken);

        // Assert
        _mockSupportProjectRepository.Verify(repo =>
            repo.UpdateAsync(_mockSupportProject, _cancellationToken), Times.Once);
    }

    #endregion
}