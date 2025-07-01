public record EngagementConcernEscalationDetails
{
    public bool? ConfirmStepsTaken { get; init; }
    public string? PrimaryReason { get; init; }
    public string? Details { get; init; }
    public DateTime? DateOfDecision { get; init; }
}
