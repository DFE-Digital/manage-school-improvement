namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record EngagementConcernDto(
        Guid Id,
        int ReadableId,
        int SupportProjectId,
        string? EngagementConcernDetails,
        DateTime? EngagementConcernRaisedDate,
        bool? EngagementConcernResolved,
        string? EngagementConcernResolvedDetails,
        DateTime? EngagementConcernResolvedDate,
        bool? EngagementConcernEscalationConfirmStepsTaken,
        string? EngagementConcernEscalationPrimaryReason,
        string? EngagementConcernEscalationDetails,
        DateTime? EngagementConcernEscalationDateOfDecision,
        string? EngagementConcernEscalationWarningNotice,
        string? EngagementConcernSummary
    )
    {
    }
}