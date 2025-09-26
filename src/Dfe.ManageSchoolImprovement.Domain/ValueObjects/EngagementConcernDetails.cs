namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public class EngagementConcernDetails
{
    public string? Details { get; set; }
    public string? Summary { get; set; }
    public DateTime? RaisedDate { get; set; }
    public bool? Resolved { get; set; }
    public string? ResolvedDetails { get; set; }
    public DateTime? ResolvedDate { get; set; }
}