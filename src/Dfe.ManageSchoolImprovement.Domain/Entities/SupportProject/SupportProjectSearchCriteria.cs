namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class SupportProjectSearchCriteria(string? title,
        IEnumerable<string>? assignedUsers,
        IEnumerable<string>? assignedAdvisers,
        IEnumerable<string>? regions,
        IEnumerable<string>? localAuthorities,
        IEnumerable<string>? trusts,
        IEnumerable<string>? dates,
        IEnumerable<string>? states)
    {
        public string? Title { get; set; } = title;
        public IEnumerable<string>? AssignedUsers { get; set; } = assignedUsers;
        public IEnumerable<string>? AssignedAdvisers { get; set; } = assignedAdvisers;
        public IEnumerable<string>? Regions { get; set; } = regions;
        public IEnumerable<string>? LocalAuthorities { get; set; } = localAuthorities;
        public IEnumerable<string>? Trusts { get; set; } = trusts;
        public IEnumerable<string>? Dates { get; set; } = dates;
        public IEnumerable<string>? States { get; set; } = states;
    }
}
