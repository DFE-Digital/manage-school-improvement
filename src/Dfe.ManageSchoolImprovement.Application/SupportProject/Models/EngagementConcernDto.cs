using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record EngagementConcernDto(
        Guid Id,
        int supportProjectId,
        bool? EngagementConcernRecorded,
        string? EngagementConcernDetails,
        DateTime? EngagementConcernRaisedDate,
        bool? EngagementConcernResolved,
        string? EngagementConcernResolvedDetails,
        DateTime? EngagementConcernResolvedDate,
        bool? EngagementConcernEscalationConfirmStepsTaken,
        string? EngagementConcernEscalationPrimaryReason,
        string? EngagementConcernEscalationDetails,
        DateTime? EngagementConcernEscalationDateOfDecision,
        string? EngagementConcernEscalationWarningNotice
    )
    {
    }
}