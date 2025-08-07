using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject
{
    public class SupportProjectContactTests
    {
        [Fact]
        public void SetContact_ShouldUpdateContactAndModificationProperties()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(1);
            var supportProjectContactId = new SupportProjectContactId(Guid.NewGuid());
            var author = "Author";
            var createdOn = DateTime.UtcNow;

            var details = new SupportProjectContactDetails
            {
                Name = "John",
                RoleId = RolesIds.Other,
                OtherRoleName = "Other Role",
                Organisation = "Organisation",
                Email = "john@school.gov.uk",
                Phone = "0123456789"
            };

            var supportProjectContact = new SupportProjectContact(supportProjectContactId, details, author, createdOn, supportProjectId);
            var lastModifiedOn = DateTime.UtcNow;
            details.Name = "Jane";

            // Act
            supportProjectContact.SetContact(details, author, lastModifiedOn);

            // Assert
            Assert.Equal(details.Name, supportProjectContact.Name);
            Assert.Equal(details.RoleId, supportProjectContact.RoleId);
            Assert.Equal(details.OtherRoleName, supportProjectContact.OtherRoleName);
            Assert.Equal(details.Organisation, supportProjectContact.Organisation);
            Assert.Equal(details.Phone, supportProjectContact.Phone);
            Assert.Equal(details.Email, supportProjectContact.Email);
            Assert.Equal(createdOn, supportProjectContact.CreatedOn);
            Assert.Equal(lastModifiedOn, supportProjectContact.LastModifiedOn);
            Assert.Equal(author, supportProjectContact.LastModifiedBy); 
        }
    }
}
