using System.Linq.Expressions;
using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetInitialContactTheResponsibleBodyDetails;


namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;

public class SetInitialContactTheResponsibleBodyDetailsTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;
    
    public SetInitialContactTheResponsibleBodyDetailsTests()
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
        bool? initialContactResponsibleBody = true; 
        DateTime? responsibleBodyInitialContactDate = DateTime.UtcNow;

        var command = new SetInitialContactTheResponsibleBodyDetailsCommand(
            _mockSupportProject.Id,
            initialContactResponsibleBody,
            responsibleBodyInitialContactDate
           
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setContactTheResponsibleBodyDetailsCommandHandler = new SetInitialContactTheResponsibleBodyDetailsCommandHandler(_mockSupportProjectRepository.Object);
        
        // Act
        var result = await setContactTheResponsibleBodyDetailsCommandHandler.Handle(command, _cancellationToken);
        
        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
    {
        // Arrange
        var command = new SetInitialContactTheResponsibleBodyDetailsCommand(
            _mockSupportProject.Id,
            null,
            null
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setContactTheResponsibleBodyDetailsCommandHandler = new SetInitialContactTheResponsibleBodyDetailsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setContactTheResponsibleBodyDetailsCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.True(result);
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFalse()
    {
        // Arrange
        bool? initialContactResponsibleBody = true;
        DateTime? responsibleBodyInitialContactDate = DateTime.UtcNow;

        var command = new SetInitialContactTheResponsibleBodyDetailsCommand(
            _mockSupportProject.Id,
            initialContactResponsibleBody,
            responsibleBodyInitialContactDate
        );

        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(
            It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), 
            It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);
        var setAdviserConflictOfInterestDetailsCommandHandler = new SetInitialContactTheResponsibleBodyDetailsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setAdviserConflictOfInterestDetailsCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.False(result);
    }


  
}
