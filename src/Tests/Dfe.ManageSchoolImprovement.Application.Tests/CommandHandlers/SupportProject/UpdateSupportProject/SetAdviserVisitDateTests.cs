using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;


public class SetAdviserVisitDateTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;

    public SetAdviserVisitDateTests()
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
        var command = new SetAdviserVisitDateCommand(
            _mockSupportProject.Id,
            DateTime.UtcNow,
            true
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setAdviserVisitDateCommandHandler = new SetAdviserVisitDate.SetAdviserVisitDateCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setAdviserVisitDateCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommandWithNoDate_UpdatesSupportProject()
    {
        //Arrange
        var command = new SetAdviserVisitDateCommand(
            _mockSupportProject.Id,
            null,
            null
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setAdviserVisitDateCommandHandler = new SetAdviserVisitDate.SetAdviserVisitDateCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setAdviserVisitDateCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        var command = new SetAdviserVisitDateCommand(
            _mockSupportProject.Id,
            DateTime.UtcNow,
            true
        );

        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(
            It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), 
            It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);
        
        var setAdviserVisitDateCommandHandler = new SetAdviserVisitDate.SetAdviserVisitDateCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setAdviserVisitDateCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.False(result);
    }
}
