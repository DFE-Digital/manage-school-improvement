using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class SupportProjectContact : BaseAggregateRoot, IEntity<SupportProjectContactId>
    {
        private SupportProjectContact() { }

        public SupportProjectContact(SupportProjectContactId id,
        string name, RolesIds roleId, string? otherRoleName, string organisation, string email, string phone, string author, DateTime createOn, SupportProjectId supportProjectId)
        {
            Id = id;
            Name = name;
            RoleId = roleId;
            OtherRoleName = otherRoleName ?? string.Empty;
            Organisation = organisation;
            Email = email;
            Phone = phone ?? string.Empty;
            CreatedBy = author;
            CreatedOn = createOn;
            SupportProjectId = supportProjectId;
        } 
        public SupportProjectContactId Id { get; private set; }
        public SupportProjectId SupportProjectId { get; private set; }

        public string Name { get; private set; }
        public RolesIds RoleId { get; private set; }
        public string OtherRoleName { get; private set; }
        public string Organisation { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; } 
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public string? LastModifiedBy { get; set; }

        public void SetContact(string name, RolesIds roleId, string? otherRoleName, string organisation, string email, string phone, string author, DateTime lastModifiedOn)
        {
            Name = name;
            RoleId = roleId;
            OtherRoleName = otherRoleName ?? string.Empty;
            Organisation = organisation;
            Email = email;
            Phone = phone ?? string.Empty;
            LastModifiedBy = author;
            LastModifiedOn = lastModifiedOn;
        }
    }
}
