using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;

public class SetChoosePreferredSupportingOrganisationTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;

    public SetChoosePreferredSupportingOrganisationTests()
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
        var command = new SetChoosePreferredSupportingOrganisationCommand(
            _mockSupportProject.Id,
            "Org",
            "1223a",
            "Trust",
            DateTime.Now,
            true,
            "1 High Street, TownVille, TW1 1AA",
            "Joe Bloggs",
            "joebloggs@example.com",
            "01234567890",
            DateTime.Now
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setChosePreferredSupportingOrganisationHandler = new SetChoosePreferredSupportingOrganisation.SetChoosePreferredSupportingOrganisationHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setChosePreferredSupportingOrganisationHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
    {
        // Arrange
        var command = new SetChoosePreferredSupportingOrganisationCommand(
            _mockSupportProject.Id,
            null,
           null,
           null,
           null,
           null,
            null,
            null,
            null,
            null,
            null
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setChosePreferredSupportingOrganisationHandler = new SetChoosePreferredSupportingOrganisation.SetChoosePreferredSupportingOrganisationHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setChosePreferredSupportingOrganisationHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        var command = new SetChoosePreferredSupportingOrganisationCommand(
            _mockSupportProject.Id,
            "Org",
            "1223a",
            "Trust",
            DateTime.Now,
            true,
            "1 High Street, TownVille, TW1 1AA",
            "Joe Bloggs",
            "joebloggs@example.com",
            "01234567890",
            DateTime.Now
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(
            It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
            It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);
        var setChosePreferredSupportingOrganisationHandler = new SetChoosePreferredSupportingOrganisation.SetChoosePreferredSupportingOrganisationHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setChosePreferredSupportingOrganisationHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.False(result);
    }
}
