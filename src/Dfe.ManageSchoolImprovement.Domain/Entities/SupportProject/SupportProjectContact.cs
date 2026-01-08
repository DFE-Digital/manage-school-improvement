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
            OrganisationTypeSubCategory = details.OrganisationTypeSubCategory;
            OrganisationTypeSubCategoryOther = details.OrganisationTypeSubCategoryOther ?? string.Empty;
            OrganisationType = details.OrganisationType;
            Email = details.Email;
            Phone = details.Phone ?? string.Empty;
            CreatedBy = author;
            CreatedOn = createOn;
            SupportProjectId = supportProjectId;
            JobTitle = details.JobTitle;
        }

        public SupportProjectContactId? Id { get; private set; }
        public SupportProjectId? SupportProjectId { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public RolesIds RoleId { get; private set; }
        public string OtherRoleName { get; private set; } = string.Empty;
        public string Organisation { get; private set; } = string.Empty;
        public string OrganisationTypeSubCategory { get; private set; } = string.Empty;
        public string? OrganisationTypeSubCategoryOther { get; private set; } = string.Empty;
        public string OrganisationType { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string? JobTitle { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? LastModifiedOn { get; set; }

        public string? LastModifiedBy { get; set; }

        public void SetContact(SupportProjectContactDetails details, string author, DateTime lastModifiedOn)
        {
            Name = details.Name;
            OrganisationTypeSubCategory = details.OrganisationTypeSubCategory;
            OrganisationTypeSubCategoryOther = details.OrganisationTypeSubCategoryOther ?? string.Empty;
            OrganisationType = details.OrganisationType;
            Email = details.Email;
            Phone = details.Phone ?? string.Empty;
            LastModifiedBy = author;
            LastModifiedOn = lastModifiedOn;
        }

    }
}
