using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectInformationPowersDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.EngagementConcern;

public class SetSupportProjectInformationPowersDetailsTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;

    public SetSupportProjectInformationPowersDetailsTests()
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
        var informationPowersInUse = true;
        var informationPowersDetails = "test details";
        var powersUsedDate = DateTime.UtcNow;

        var command = new SetSupportProjectInformationPowersDetailsCommand(
            _mockSupportProject.Id, 
            informationPowersInUse, 
            informationPowersDetails,
            powersUsedDate
        );
        
        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        
        var handler = new SetSupportProjectInformationPowersDetailsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Verify
        Assert.IsType<bool>(result);
        Assert.True(result);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(
                It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                    x.InformationPowersInUse == informationPowersInUse &&
                    x.InformationPowersDetails == informationPowersDetails &&
                    x.PowersUsedDate == powersUsedDate), 
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
    {
        // Arrange
        var command = new SetSupportProjectInformationPowersDetailsCommand(
            _mockSupportProject.Id, 
            null, 
            null,
            null
        );
        
        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        
        var handler = new SetSupportProjectInformationPowersDetailsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Verify
        Assert.IsType<bool>(result);
        Assert.True(result);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(
                It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                    x.InformationPowersInUse == null && 
                    x.InformationPowersDetails == null &&
                    x.PowersUsedDate == null), 
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        var informationPowersInUse = true;
        var informationPowersDetails = "test details";
        var powersUsedDate = DateTime.UtcNow;

        var command = new SetSupportProjectInformationPowersDetailsCommand(
            _mockSupportProject.Id, 
            informationPowersInUse, 
            informationPowersDetails,
            powersUsedDate
        );
        
        _mockSupportProjectRepository
            .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                It.IsAny<CancellationToken>())).ReturnsAsync(null as Domain.Entities.SupportProject.SupportProject);
        
        var handler = new SetSupportProjectInformationPowersDetailsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Verify
        Assert.IsType<bool>(result);
        Assert.False(result);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                It.IsAny<CancellationToken>()), 
            Times.Never);
    }
}
