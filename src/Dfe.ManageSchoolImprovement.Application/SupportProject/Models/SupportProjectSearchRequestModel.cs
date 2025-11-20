namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public class SupportProjectSearchRequest
    {
        public string? Title { get; set; }
        public IEnumerable<string>? States { get; set; }
        public IEnumerable<string>? AssignedUsers { get; set; }
        public IEnumerable<string>? AssignedAdvisers { get; set; }
        public IEnumerable<string>? Regions { get; set; }
        public IEnumerable<string>? LocalAuthorities { get; set; }
        public IEnumerable<string>? Trusts { get; set; }
        public IEnumerable<string>? Months { get; set; }
        public IEnumerable<string>? Years { get; set; }
        public string PagePath { get; set; } = string.Empty;
        public int Page { get; set; }
        public int Count { get; set; }
    }
}
