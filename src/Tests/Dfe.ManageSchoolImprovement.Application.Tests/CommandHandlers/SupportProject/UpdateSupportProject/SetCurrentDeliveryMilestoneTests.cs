using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using System.Linq.Expressions;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetCurrentDeliveryMilestone;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;

public class SetCurrentDeliveryMilestoneTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;

    public SetCurrentDeliveryMilestoneTests()
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
        var milestone = Milestone.InitialDiagnosis;
        var command = new SetCurrentDeliveryMilestoneCommand(
            _mockSupportProject.Id,
            milestone
        );
        _mockSupportProjectRepository
            .Setup(repo => repo.FindAsync(
                It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockSupportProject);
        var handler = new SetCurrentDeliveryMilestoneCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        Assert.Equal(milestone, _mockSupportProject.CurrentDeliveryMilestone);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        var command = new SetCurrentDeliveryMilestoneCommand(
            _mockSupportProject.Id,
            Milestone.MatchingComplete
        );
        _mockSupportProjectRepository
            .Setup(repo => repo.FindAsync(
                It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                It.IsAny<CancellationToken>()))!
            .ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);
        var handler = new SetCurrentDeliveryMilestoneCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await handler.Handle(command, _cancellationToken);

        // Verify
        Assert.False(result);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
