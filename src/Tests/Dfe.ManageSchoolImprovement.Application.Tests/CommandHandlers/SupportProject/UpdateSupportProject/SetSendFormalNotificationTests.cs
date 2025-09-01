using System.Linq.Expressions;
using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetSendFormalNotification;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;

public class SetSendFormalNotificationTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;

    public SetSendFormalNotificationTests()
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
        bool? useEnrolmentLetterTemplateToDraftEmail = true;
        bool? attachTargetedInterventionInformationSheet = true;
        bool? addRecipientsForFormalNotification = true;
        bool? formalNotificationSent = true;
        DateTime? dateFormalNotificationSent = DateTime.UtcNow;

        var command = new SetSendFormalNotificationCommand(
            _mockSupportProject.Id,
            useEnrolmentLetterTemplateToDraftEmail,
            attachTargetedInterventionInformationSheet,
            addRecipientsForFormalNotification,
            formalNotificationSent,
            dateFormalNotificationSent
        );
        _mockSupportProjectRepository
            .Setup(repo =>
                repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setSendFormalNotificationCommandHandler =
            new SetSendFormalNotification.SetSendFormalNotificationCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setSendFormalNotificationCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
    {
        // Arrange
        var command = new SetSendFormalNotificationCommand(
            _mockSupportProject.Id,
            null,
            null,
            null,
            null,
            null
        );
        _mockSupportProjectRepository
            .Setup(repo =>
                repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setSendFormalNotificationCommandHandler =
            new SetSendFormalNotificationCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setSendFormalNotificationCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        bool? useEnrolmentLetterTemplateToDraftEmail = true;
        bool? attachTargetedInterventionInformationSheet = true;
        bool? addRecipientsForFormalNotification = true;
        bool? formalNotificationSent = true;
        DateTime? dateFormalNotificationSent = DateTime.UtcNow;

        var command = new SetSendFormalNotificationCommand(
            _mockSupportProject.Id,
            useEnrolmentLetterTemplateToDraftEmail,
            attachTargetedInterventionInformationSheet,
            addRecipientsForFormalNotification,
            formalNotificationSent,
            dateFormalNotificationSent
        );

        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(
            It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
            It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);
        var setSendFormalNotificationCommandHandler =
            new SetSendFormalNotificationCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setSendFormalNotificationCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.False(result);
    }
}