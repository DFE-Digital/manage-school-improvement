using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class SupportProjectContact : BaseAggregateRoot, IEntity<SupportProjectContactId>
    {
        private SupportProjectContact() { }

        public SupportProjectContact(SupportProjectContactId id, SupportProjectContactDetails details, string author, DateTime createOn, SupportProjectId supportProjectId)
        {
            Id = id;
            Name = details.Name;
            RoleId = details.RoleId;
            OtherRoleName = details.OtherRoleName ?? string.Empty;
            Organisation = details.Organisation;
            Email = details.Email;
            Phone = details.Phone ?? string.Empty;
            CreatedBy = author;
            CreatedOn = createOn;
            SupportProjectId = supportProjectId;
        }

        public SupportProjectContactId? Id { get; private set; }
        public SupportProjectId? SupportProjectId { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public RolesIds RoleId { get; private set; }
        public string OtherRoleName { get; private set; } = string.Empty;
        public string Organisation { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; }  = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? LastModifiedOn { get; set; }

        public string? LastModifiedBy { get; set; }

        public void SetContact(SupportProjectContactDetails details, string author, DateTime lastModifiedOn)
        {
            Name = details.Name;
            RoleId = details.RoleId;
            OtherRoleName = details.OtherRoleName ?? string.Empty;
            Organisation = details.Organisation;
            Email = details.Email;
            Phone = details.Phone ?? string.Empty;
            LastModifiedBy = author;
            LastModifiedOn = lastModifiedOn;
        }

    }
}
