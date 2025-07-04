using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using System.Linq.Expressions;
using Dfe.ManageSchoolImprovement.Utils;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.Notes;

public class CreateSupportProjectNoteCommandHandlerTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IDateTimeProvider> _iDateTimeProvider;

    public CreateSupportProjectNoteCommandHandlerTests()
    {
        _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
        var fixture = new Fixture();
        _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
        _cancellationToken = new CancellationToken();
        _cancellationToken = CancellationToken.None;
        _iDateTimeProvider = new Mock<IDateTimeProvider>();
    }

    [Fact]
    public async Task Handle_ValidCommand_CreateNote()
    {
        
        var command = new CreateSupportProjectNote.CreateSupportProjectNoteCommand(
            _mockSupportProject.Id,
            "Note",
            "dave.dave@example.com"
            
        );
        
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var CreateNoteCommandHandler =
            new CreateSupportProjectNote.CreateSupportProjectNoteCommandHandler(_mockSupportProjectRepository.Object,_iDateTimeProvider.Object);
        
        var result = await CreateNoteCommandHandler.Handle(command, _cancellationToken);
        Assert.IsType<SupportProjectNoteId>(result);
        
        Assert.IsType<Guid>(result.Value);
        
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_EmptyCommand_CreateNote()
    {
        var command = new CreateSupportProjectNote.CreateSupportProjectNoteCommand(
            null!,
            null!,
            null!
        );
        
        _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
        var createNoteCommandHandler =
            new CreateSupportProjectNote.CreateSupportProjectNoteCommandHandler(_mockSupportProjectRepository.Object,_iDateTimeProvider.Object);
        
        var result = await createNoteCommandHandler.Handle(command, _cancellationToken);
        Assert.IsType<SupportProjectNoteId>(result);
        
        Assert.IsType<Guid>(result.Value);
        
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
