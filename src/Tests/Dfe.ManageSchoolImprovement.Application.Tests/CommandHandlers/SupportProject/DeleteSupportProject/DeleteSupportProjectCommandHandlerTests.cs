using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.DeleteSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.DeleteSupportProject;

public class DeleteSupportProjectCommandHandlerTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly DeleteSupportProjectCommandHandler _handler;
    private readonly string _schoolUrn;
    private readonly CancellationToken _cancellationToken;
    private readonly Fixture _fixture;

    public DeleteSupportProjectCommandHandlerTests()
    {
        _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
        _handler = new DeleteSupportProjectCommandHandler(_mockSupportProjectRepository.Object);
        _schoolUrn = "123456";
        _cancellationToken = CancellationToken.None;
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_ValidSchoolUrn_DeletesProjectAndReturnsTrue()
    {
        // Arrange
        var supportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
        var command = new DeleteSupportProjectCommand(_schoolUrn);

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectByUrnIgnoringFilters(_schoolUrn, _cancellationToken))
            .ReturnsAsync(supportProject);

        _mockSupportProjectRepository
            .Setup(repo => repo.RemoveAsync(supportProject, _cancellationToken))
            .ReturnsAsync(supportProject);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectByUrnIgnoringFilters(_schoolUrn, _cancellationToken), Times.Once);
        _mockSupportProjectRepository.Verify(repo => repo.RemoveAsync(supportProject, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentSchoolUrn_ReturnsFalse()
    {
        // Arrange
        var command = new DeleteSupportProjectCommand(_schoolUrn);

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectByUrnIgnoringFilters(_schoolUrn, _cancellationToken))
            .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.False(result);
        _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectByUrnIgnoringFilters(_schoolUrn, _cancellationToken), Times.Once);
        _mockSupportProjectRepository.Verify(repo => repo.RemoveAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), _cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var supportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
        var command = new DeleteSupportProjectCommand(_schoolUrn);

        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectByUrnIgnoringFilters(_schoolUrn, _cancellationToken))
            .ReturnsAsync(supportProject);

        _mockSupportProjectRepository
            .Setup(repo => repo.RemoveAsync(supportProject, _cancellationToken))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, _cancellationToken));
    }
} 
