using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;

public class SupportProjectNote : BaseAggregateRoot, IEntity<SupportProjectNoteId>
{
    private SupportProjectNote() { }
    
    public SupportProjectNote(SupportProjectNoteId id,
        string note,
        string author,
        DateTime date, SupportProjectId supportProjectId)
    {
        Id = id;
        Note = note;
        CreatedBy = author;
        CreatedOn = date;
        SupportProjectId = supportProjectId;
    }
    
    public SupportProjectId? SupportProjectId { get; private set; }
    public SupportProjectNoteId Id { get; private set; }
    public string Note { get; private set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    
    public DateTime? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public void SetNote(string note, string author, DateTime dateUpdated)
    {
        Note = note;
        LastModifiedBy = author;
        LastModifiedOn = dateUpdated;
    }

}
