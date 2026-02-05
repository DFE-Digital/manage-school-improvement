using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.Audit;

/// <summary>
/// Represents an audit record for a specific field of a SupportProject
/// </summary>
/// <typeparam name="T">The type of the field being audited</typeparam>
public class SupportProjectFieldAudit<T>
{
    public SupportProjectId SupportProjectId { get; set; } = null!;
    public string FieldName { get; set; } = string.Empty;
    public T? FieldValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}