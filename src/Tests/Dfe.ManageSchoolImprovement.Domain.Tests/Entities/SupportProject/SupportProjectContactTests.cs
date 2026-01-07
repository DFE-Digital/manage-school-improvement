using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;

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
                OrganisationTypeSubCategory = SchoolOrginisationTypes.PermanentHeadteacher.GetDisplayName(),
                OrganisationTypeSubCategoryOther = "Other Role",
                OrganisationType = "Organisation",
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
            Assert.Equal(details.OrganisationTypeSubCategory, supportProjectContact.OrganisationTypeSubCategory);
            Assert.Equal(details.OrganisationTypeSubCategoryOther, supportProjectContact.OrganisationTypeSubCategoryOther);
            Assert.Equal(details.OrganisationType, supportProjectContact.OrganisationType);
            Assert.Equal(details.Phone, supportProjectContact.Phone);
            Assert.Equal(details.Email, supportProjectContact.Email);
            Assert.Equal(createdOn, supportProjectContact.CreatedOn);
            Assert.Equal(lastModifiedOn, supportProjectContact.LastModifiedOn);
            Assert.Equal(author, supportProjectContact.LastModifiedBy);
        }
    }
}
