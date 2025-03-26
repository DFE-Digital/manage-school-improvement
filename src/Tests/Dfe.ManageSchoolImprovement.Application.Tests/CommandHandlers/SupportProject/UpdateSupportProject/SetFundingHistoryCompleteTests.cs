using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetFundingHistoryComplete;


namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;

public class SetFundingHistoryCompleteTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;

    public SetFundingHistoryCompleteTests()
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
        bool? setFundingHistoryComplete = true;

        var command = new SetFundingHistoryCompleteCommand(
            _mockSupportProject.Id,
            setFundingHistoryComplete
        );
        _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setFundingHistoryCompleteCommandHandler = new SetFundingHistoryCompleteCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setFundingHistoryCompleteCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
    {
        // Arrange
        var command = new SetFundingHistoryCompleteCommand(
            _mockSupportProject.Id,
            null
        );

        _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setFundingHistoryCompleteCommandHandler = new SetFundingHistoryCompleteCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setFundingHistoryCompleteCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        bool? setFundingHistoryComplete = true;

        var command = new SetFundingHistoryCompleteCommand(
            _mockSupportProject.Id,
            setFundingHistoryComplete
        );

        _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null);
        var setFundingHistoryCompleteCommandHandler = new SetFundingHistoryCompleteCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setFundingHistoryCompleteCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.False(result);
    }
}
