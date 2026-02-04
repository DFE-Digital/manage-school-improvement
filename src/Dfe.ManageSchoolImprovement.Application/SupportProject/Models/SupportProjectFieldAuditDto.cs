namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

/// <summary>
/// Data Transfer Object representing an audit record for a specific field of a SupportProject
/// </summary>
/// <typeparam name="T">The type of the field being audited</typeparam>
public record SupportProjectFieldAuditDto<T>(
    int SupportProjectId,
    string FieldName,
    T? FieldValue,
    DateTime ValidFrom,
    DateTime ValidTo,
    string? LastModifiedBy,
    DateTime? LastModifiedOn
);