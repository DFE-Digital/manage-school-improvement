using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;


namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;

public class SetHasReceivedFundingInThelastTwoYearsTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;

    public SetHasReceivedFundingInThelastTwoYearsTests()
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
        bool? hasReceivedFundingInThelastTwoYears = true;

        var command = new SetHasReceivedFundingInThelastTwoYearsCommand(
            _mockSupportProject.Id,
            hasReceivedFundingInThelastTwoYears
        );
        _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setHasReceivedFundingInThelastTwoYearsCommandHandler = new SetHasReceivedFundingInThelastTwoYearsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setHasReceivedFundingInThelastTwoYearsCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
    {
        // Arrange
        var command = new SetHasReceivedFundingInThelastTwoYearsCommand(
            _mockSupportProject.Id,
            null
        );
        _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setHasReceivedFundingInThelastTwoYearsCommandHandler = new SetHasReceivedFundingInThelastTwoYearsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setHasReceivedFundingInThelastTwoYearsCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        bool? hasReceivedFundingInThelastTwoYears = true;

        var command = new SetHasReceivedFundingInThelastTwoYearsCommand(
            _mockSupportProject.Id,
            hasReceivedFundingInThelastTwoYears
        );
        _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null);
        var setHasReceivedFundingInThelastTwoYearsCommandHandler = new SetHasReceivedFundingInThelastTwoYearsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setHasReceivedFundingInThelastTwoYearsCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.False(result);
    }
}
