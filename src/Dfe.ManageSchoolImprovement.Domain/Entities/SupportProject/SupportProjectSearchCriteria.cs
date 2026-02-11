namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public record SupportProjectSearchCriteria
    {
        public string? Title { get; init; }
        public IEnumerable<string>? AssignedUsers { get; init; }
        public IEnumerable<string>? AssignedAdvisers { get; init; }
        public IEnumerable<string>? Regions { get; init; }
        public IEnumerable<string>? LocalAuthorities { get; init; }
        public IEnumerable<string>? Trusts { get; init; }
        public IEnumerable<string>? Dates { get; init; }
        public IEnumerable<string>? States { get; init; }
    }
}
