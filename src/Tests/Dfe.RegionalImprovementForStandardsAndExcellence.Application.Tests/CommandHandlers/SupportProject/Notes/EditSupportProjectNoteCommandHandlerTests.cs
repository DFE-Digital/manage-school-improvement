using AutoFixture;
using Dfe.RegionalImprovementForStandardsAndExcellence.Application.SupportProject.Commands.CreateSupportProjectNote;
using Dfe.RegionalImprovementForStandardsAndExcellence.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.Interfaces.Repositories;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.ValueObjects;
using Moq;
using System.Linq.Expressions;
using Dfe.RegionalImprovementForStandardsAndExcellence.Application.SupportProject.Commands.EditSupportProjectNote;
using Dfe.RegionalImprovementForStandardsAndExcellence.Utils;

namespace Dfe.RegionalImprovementForStandardsAndExcellence.Application.Tests.CommandHandlers.SupportProject.Notes;

public class EditSupportProjectNoteCommandHandlerTests
{
    private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
    private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
    private readonly Domain.Entities.SupportProject.SupportProjectNote _mockSupportProjectNote;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IDateTimeProvider> _iDateTimeProvider;

    public EditSupportProjectNoteCommandHandlerTests()
    {
        _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
        var fixture = new Fixture();
        _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
        _mockSupportProjectNote = fixture.Create<Domain.Entities.SupportProject.SupportProjectNote>();
        _cancellationToken = new CancellationToken();
        _cancellationToken = CancellationToken.None;
        _iDateTimeProvider = new Mock<IDateTimeProvider>();
    }

    [Fact]
    public async Task Handle_ValidCommand_EditNote()
    {

        var command = new EditSupportProjectNote.EditSupportProjectNoteCommand(
            _mockSupportProjectNote.SupportProjectId,
            "Note",_mockSupportProjectNote.Id,_mockSupportProjectNote.CreatedBy);
        
        _mockSupportProject.AddNote(_mockSupportProjectNote.Id,
            _mockSupportProjectNote.Note, 
            _mockSupportProjectNote.CreatedBy,
            _mockSupportProjectNote.CreatedOn,
            _mockSupportProjectNote.SupportProjectId);

        _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockSupportProject);
        
        var editSupportProjectNoteCommandHandler =
            new EditSupportProjectNote.EditSupportProjectNoteCommandHandler(_mockSupportProjectRepository.Object,_iDateTimeProvider.Object);
        
        var result = await editSupportProjectNoteCommandHandler.Handle(command, _cancellationToken);
        Assert.IsType<SupportProjectNoteId>(result);
        
       Assert.Equal(result,_mockSupportProjectNote.Id);
        
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_InvalidValidCommand_EditNote()
    {

        var command = new EditSupportProjectNote.EditSupportProjectNoteCommand(
           null,
            "Note",null,_mockSupportProjectNote.CreatedBy);
        
        _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockSupportProject);
        
        var editSupportProjectNoteCommandHandler =
            new EditSupportProjectNote.EditSupportProjectNoteCommandHandler(_mockSupportProjectRepository.Object,_iDateTimeProvider.Object);
        
        var result = await editSupportProjectNoteCommandHandler.Handle(command, _cancellationToken);
        Assert.IsNotType<SupportProjectNoteId>(result);
        
        _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
  
}
