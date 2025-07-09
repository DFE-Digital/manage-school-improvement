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
            var name = "John";
            var author = "Author";
            var organisation = "Organisation";
            var email = "john@school.gov.uk";
            var phone = "0123456789";
            var roleId = RolesIds.Other;
            var otherRoleName = "Other Role"; 
            var createdOn = DateTime.UtcNow;
            var supportProjectContact = new SupportProjectContact(supportProjectContactId, name, roleId, otherRoleName, organisation, email, phone, author, createdOn, supportProjectId);
            var lastModifiedOn = DateTime.UtcNow;
            name = "Jane";

            // Act
            supportProjectContact.SetContact(name, roleId, otherRoleName, organisation, email, phone, author, lastModifiedOn);

            // Assert
            Assert.Equal(name, supportProjectContact.Name);
            Assert.Equal(roleId, supportProjectContact.RoleId);
            Assert.Equal(otherRoleName, supportProjectContact.OtherRoleName);
            Assert.Equal(organisation, supportProjectContact.Organisation); 
            Assert.Equal(phone, supportProjectContact.Phone);
            Assert.Equal(author, supportProjectContact.LastModifiedBy);
            Assert.Equal(organisation, supportProjectContact.Organisation);
            Assert.Equal(email, supportProjectContact.Email);
            Assert.Equal(createdOn, supportProjectContact.CreatedOn); 
            Assert.Equal(lastModifiedOn, supportProjectContact.LastModifiedOn);
            Assert.Equal(author, supportProjectContact.LastModifiedBy); 
        }
    }
}
