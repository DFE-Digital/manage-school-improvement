using System.Linq.Expressions;
using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetContactTheResponsibleBodyDetails;


namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject;

public class SetContactTheResponsibleBodyDetailsTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;
    
    public SetContactTheResponsibleBodyDetailsTests()
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
        bool? discussTheBestApproach =  true;
        bool? emailTheResponsibleBody = true; 
        DateTime? responsibleBodyContactedDate = DateTime.UtcNow;

        var command = new SetContactTheResponsibleBodyDetailsCommand(
            _mockSupportProject.Id,
            discussTheBestApproach,
            emailTheResponsibleBody,
            responsibleBodyContactedDate
           
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setContactTheResponsibleBodyDetailsCommandHandler = new SetContactTheResponsibleBodyDetailsCommandHandler(_mockSupportProjectRepository.Object);
        
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
        var command = new SetContactTheResponsibleBodyDetailsCommand(
            _mockSupportProject.Id,
            null,
            null,
            null
        );
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var setContactTheResponsibleBodyDetailsCommandHandler = new SetContactTheResponsibleBodyDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
        
        
        bool? discussTheBestApproach =  true;
        bool? emailTheResponsibleBody = true;
        DateTime? responsibleBodyContactedDate = DateTime.UtcNow;

        var command = new SetContactTheResponsibleBodyDetailsCommand(
            _mockSupportProject.Id,
            discussTheBestApproach,
            emailTheResponsibleBody,
            responsibleBodyContactedDate
        );

        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null);
        var setAdviserConflictOfInterestDetailsCommandHandler = new SetContactTheResponsibleBodyDetailsCommandHandler(_mockSupportProjectRepository.Object);

        // Act
        var result = await setAdviserConflictOfInterestDetailsCommandHandler.Handle(command, _cancellationToken);

        // Verify
        Assert.False(result);
    }


  
}
