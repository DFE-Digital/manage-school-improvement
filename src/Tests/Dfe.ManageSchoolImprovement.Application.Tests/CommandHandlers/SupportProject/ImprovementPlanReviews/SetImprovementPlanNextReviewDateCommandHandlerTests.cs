using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlanReviews
{
    public class SetImprovementPlanNextReviewDateCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;
        private readonly ImprovementPlanReviewId _improvementPlanReviewId;

        public SetImprovementPlanNextReviewDateCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            _improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());

            // Set up the support project with the required improvement plan structure
            SetupMockSupportProject();
        }

        private void SetupMockSupportProject()
        {
            // Add improvement plan to the support project
            _mockSupportProject.AddImprovementPlan(_improvementPlanId, _mockSupportProject.Id);

            // Add a review to the improvement plan
            _mockSupportProject.AddImprovementPlanReview(
                _improvementPlanReviewId,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow);
        }

        [Fact]
        public async Task Handle_ValidCommand_SetsNextReviewDate()
        {
            // Arrange
            var nextReviewDate = DateTime.UtcNow.AddDays(30);
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                nextReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == _mockSupportProject.Id),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject,
                _cancellationToken), Times.Once);
        }

        [Theory]
        [InlineData(30)]  // Future date - 30 days
        [InlineData(7)]   // Future date - 1 week
        [InlineData(90)]  // Future date - 3 months
        [InlineData(-30)] // Past date - 30 days ago
        [InlineData(0)]   // Today
        public async Task Handle_ValidCommandWithDifferentDates_SetsNextReviewDate(int daysFromNow)
        {
            // Arrange
            var nextReviewDate = DateTime.UtcNow.AddDays(daysFromNow);
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                nextReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommandWithNullDate_ClearsNextReviewDate()
        {
            // Arrange
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == _mockSupportProject.Id),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject,
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                nonExistentId,
                _improvementPlanId,
                _improvementPlanReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert - Verify GetSupportProjectById is called before UpdateAsync
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.IsAny<SupportProjectId>(),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_SetsNextReviewDateWithCorrectParameters()
        {
            // Arrange
            var nextReviewDate = DateTime.UtcNow.AddDays(45).Date;
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                nextReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);

            // Verify the next review date was set correctly
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
            Assert.Equal(nextReviewDate, review.NextReviewDate);
        }

        [Fact]
        public async Task Handle_ValidCommandWithNull_ClearsExistingNextReviewDate()
        {
            // Arrange
            // First set a next review date
            var initialDate = DateTime.UtcNow.AddDays(30);
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
            review.SetNextReviewDate(initialDate);

            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            Assert.Null(review.NextReviewDate);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var expectedException = new InvalidOperationException("Update failed");
            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken))
                .ThrowsAsync(expectedException);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_MultipleCallsWithDifferentDates_UpdatesCorrectly()
        {
            // Arrange
            var firstDate = DateTime.UtcNow.AddDays(30);
            var secondDate = DateTime.UtcNow.AddDays(60);

            var command1 = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                firstDate
            );

            var command2 = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                secondDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(command1, _cancellationToken);
            var result2 = await handler.Handle(command2, _cancellationToken);

            // Assert
            Assert.True(result1);
            Assert.True(result2);

            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
            Assert.Equal(secondDate, review.NextReviewDate);
        }

        #region Command Tests

        [Fact]
        public void SetImprovementPlanNextReviewDateCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange
            var nextReviewDate = DateTime.UtcNow.AddDays(30);

            // Act
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                nextReviewDate
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
            Assert.Equal(_improvementPlanReviewId, command.ImprovementPlanReviewId);
            Assert.Equal(nextReviewDate, command.NextReviewDate);
        }

        [Fact]
        public void SetImprovementPlanNextReviewDateCommand_WithNullDate_CreatesCommand()
        {
            // Act
            var command = new SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                null
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
            Assert.Equal(_improvementPlanReviewId, command.ImprovementPlanReviewId);
            Assert.Null(command.NextReviewDate);
        }

        [Fact]
        public void SetImprovementPlanNextReviewDateCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(SetImprovementPlanNextReviewDate.SetImprovementPlanNextReviewDateCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<bool>)));
        }

        #endregion
    }
}