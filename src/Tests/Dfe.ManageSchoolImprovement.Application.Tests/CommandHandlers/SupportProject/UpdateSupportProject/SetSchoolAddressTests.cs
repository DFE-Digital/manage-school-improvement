using System.Linq.Expressions;
using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetSchoolAddress;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;

public class SetSchoolAddressTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;

    public SetSchoolAddressTests()
    {
        _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
        var fixture = new Fixture();
        _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesSupportProject()
    {
        // Arrange
        var address = "1 Test Street, Test Town, TT1 1TT";
        var command = new SetSchoolAddressCommand(_mockSupportProject.Id, address);

        _mockSupportProjectRepository
            .Setup(repo =>
                repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockSupportProject);

        var handler = new SetSchoolAddressHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.True(result);
        Assert.Equal(address, _mockSupportProject.Address);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
    {
        // Arrange
        var command = new SetSchoolAddressCommand(_mockSupportProject.Id, null);

        _mockSupportProjectRepository
            .Setup(repo =>
                repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockSupportProject);

        var handler = new SetSchoolAddressHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.True(result);
        Assert.Null(_mockSupportProject.Address);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        var address = "1 Test Street, Test Town, TT1 1TT";
        var command = new SetSchoolAddressCommand(_mockSupportProject.Id, address);

        _mockSupportProjectRepository
            .Setup(repo =>
                repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))!
            .ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);

        var handler = new SetSchoolAddressHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Assert
        Assert.False(result);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}

