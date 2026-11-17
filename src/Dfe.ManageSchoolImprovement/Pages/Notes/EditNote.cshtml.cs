using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EditSupportProjectNote;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Application.Common.Mediation;
using Microsoft.AspNetCore.Mvc;


namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Notes;


public class EditNoteModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IAppMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }
    
    [BindProperty(Name = "project-note-body")]
    public string ProjectNoteBody { get; set; }
    
    public Guid ProjectNoteId { get; set; }
    public bool ShowError => _errorService.HasErrors();
    
    public async Task<IActionResult> OnGetAsync(int id,Guid noteid, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.Notes.Index.Page;
        
        await base.GetSupportProject(id, cancellationToken);

        var note = SupportProject.Notes.FirstOrDefault(a => a.Id.Value == noteid);

        if (note != null)
        {
            ProjectNoteBody = note.Note;
            ProjectNoteId = note.Id.Value;
            
        }
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id,DateTime projectNoteDate, Guid projectNoteId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(ProjectNoteBody))
        {
            _errorService.AddError("project-note-body", "Enter a note");

            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }
        
        var supportProjectId = new SupportProjectId(id);
        
        var supportProjectNoteId = new SupportProjectNoteId(projectNoteId);

        var request = new EditSupportProjectNote.EditSupportProjectNoteCommand(supportProjectId,ProjectNoteBody,supportProjectNoteId,User.Identity.Name);
        
        var result = await mediator.Send(request, cancellationToken);

        if (result != supportProjectNoteId)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }
        
        TempData["editNote"] = true;
        return RedirectToPage(@Links.Notes.Index.Page, new { id });
    }
}
