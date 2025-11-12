namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ContactViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? LastModifiedOn { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
