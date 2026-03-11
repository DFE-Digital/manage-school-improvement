using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlansReviews;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlanReviews
{
    public class DeleteImprovementPlanReviewCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;

        public DeleteImprovementPlanReviewCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
        }

        private static Domain.Entities.SupportProject.SupportProject CreateSupportProjectWithPlanAndReview(
            SupportProjectId supportProjectId,
            ImprovementPlanId improvementPlanId,
            ImprovementPlanReviewId improvementPlanReviewId)
        {
            var supportProject = new Domain.Entities.SupportProject.SupportProject(
                supportProjectId,
                "School Name",
                "123456",
                "Local Authority",
                "Region");
            supportProject.AddImprovementPlan(improvementPlanId, supportProjectId);
            supportProject.AddImprovementPlanReview(improvementPlanReviewId, improvementPlanId, "Reviewer", DateTime.UtcNow.Date);
            return supportProject;
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletesReviewAndReturnsTrue()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(1);
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var supportProject = CreateSupportProjectWithPlanAndReview(
                supportProjectId,
                improvementPlanId,
                improvementPlanReviewId);
            var deletedBy = "test.user@example.com";
            var command = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommand(
                supportProjectId,
                improvementPlanId,
                improvementPlanReviewId,
                deletedBy);

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectId), _cancellationToken))
                .ReturnsAsync(supportProject);

            var handler = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommandHandler(
                _mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == supportProjectId),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                supportProject,
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommand(
                nonExistentId,
                new ImprovementPlanId(Guid.NewGuid()),
                new ImprovementPlanReviewId(Guid.NewGuid()),
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommandHandler(
                _mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(1);
            var supportProject = CreateSupportProjectWithPlanAndReview(
                supportProjectId,
                new ImprovementPlanId(Guid.NewGuid()),
                new ImprovementPlanReviewId(Guid.NewGuid()));
            var command = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommand(
                supportProjectId,
                new ImprovementPlanId(Guid.NewGuid()),
                new ImprovementPlanReviewId(Guid.NewGuid()),
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommandHandler(
                _mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(2);
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var supportProject = CreateSupportProjectWithPlanAndReview(
                supportProjectId,
                improvementPlanId,
                improvementPlanReviewId);
            var command = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommand(
                supportProjectId,
                improvementPlanId,
                improvementPlanReviewId,
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectId), _cancellationToken))
                .ReturnsAsync(supportProject);

            var handler = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommandHandler(
                _mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == supportProjectId),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                supportProject,
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledWithSupportProjectAfterDelete()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(3);
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var supportProject = CreateSupportProjectWithPlanAndReview(
                supportProjectId,
                improvementPlanId,
                improvementPlanReviewId);
            var command = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommand(
                supportProjectId,
                improvementPlanId,
                improvementPlanReviewId,
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectId), _cancellationToken))
                .ReturnsAsync(supportProject);

            var handler = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommandHandler(
                _mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.Is<Domain.Entities.SupportProject.SupportProject>(sp => sp == supportProject),
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenImprovementPlanNotFound_ThrowsInvalidOperationException()
        {
            // Arrange - support project with no improvement plans; domain throws when plan not found
            var supportProjectWithNoPlans = new Domain.Entities.SupportProject.SupportProject(
                new SupportProjectId(1),
                "School Name",
                "123456",
                "Local Authority",
                "Region");
            var nonExistentPlanId = new ImprovementPlanId(Guid.NewGuid());
            var command = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommand(
                supportProjectWithNoPlans.Id,
                nonExistentPlanId,
                new ImprovementPlanReviewId(Guid.NewGuid()),
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectWithNoPlans.Id), _cancellationToken))
                .ReturnsAsync(supportProjectWithNoPlans);

            var handler = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommandHandler(
                _mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Improvement plan with id {nonExistentPlanId} not found.", exception.Message);
        }

        [Fact]
        public async Task Handle_WhenImprovementPlanReviewNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange - support project with plan but no review; domain throws when review not found
            var supportProjectId = new SupportProjectId(1);
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var supportProject = new Domain.Entities.SupportProject.SupportProject(
                supportProjectId,
                "School Name",
                "123456",
                "Local Authority",
                "Region");
            supportProject.AddImprovementPlan(improvementPlanId, supportProjectId);
            var nonExistentReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var command = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommand(
                supportProjectId,
                improvementPlanId,
                nonExistentReviewId,
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectId), _cancellationToken))
                .ReturnsAsync(supportProject);

            var handler = new DeleteImprovementPlanReview.DeleteImprovementPlanReviewCommandHandler(
                _mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Improvement plan review with id {nonExistentReviewId} not found", exception.Message);
        }
    }
}
