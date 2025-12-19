using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class ContactViewModelTests
    {
        [Fact]
        public void ContactViewModel_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var model = new ContactViewModel();

            // Assert
            Assert.Equal(string.Empty, model.Name);
            Assert.Equal(string.Empty, model.RoleName);
            Assert.Equal(string.Empty, model.Email);
            Assert.Equal(string.Empty, model.Phone);
            Assert.Equal(string.Empty, model.Address);
            Assert.False(model.ManuallyAdded);
            Assert.Null(model.SupportProjectId);
            Assert.Null(model.ContactId);
            Assert.Equal(default(DateTime), model.CreatedOn);
            Assert.Equal(string.Empty, model.CreatedBy);
            Assert.Null(model.LastModifiedOn);
            Assert.Null(model.LastModifiedBy);
        }

        [Fact]
        public void ContactViewModel_ShouldSetAndGetValues()
        {
            // Arrange
            var expectedName = "John Smith";
            var expectedRoleName = "Chief Financial Officer";
            var expectedEmail = "john.smith@example.com";
            var expectedPhone = "+44 20 7946 0958";
            var expectedAddress = "1 Test Street, Test Town, TT1 1TT";
            var expectedManuallyAdded = true;
            int? expectedSupportProjectId = 123;
            var expectedContactId = new SupportProjectContactId(Guid.NewGuid());
            var expectedCreatedOn = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
            var expectedCreatedBy = "test.user@education.gov.uk";
            var expectedLastModifiedOn = new DateTime(2024, 1, 20, 14, 45, 0, DateTimeKind.Utc);
            var expectedLastModifiedBy = "admin.user@education.gov.uk";

            // Act
            var model = new ContactViewModel
            {
                Name = expectedName,
                RoleName = expectedRoleName,
                Email = expectedEmail,
                Phone = expectedPhone,
                Address = expectedAddress,
                ManuallyAdded = expectedManuallyAdded,
                SupportProjectId = expectedSupportProjectId,
                ContactId = expectedContactId,
                CreatedOn = expectedCreatedOn,
                CreatedBy = expectedCreatedBy,
                LastModifiedOn = expectedLastModifiedOn,
                LastModifiedBy = expectedLastModifiedBy
            };

            // Assert
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedRoleName, model.RoleName);
            Assert.Equal(expectedEmail, model.Email);
            Assert.Equal(expectedPhone, model.Phone);
            Assert.Equal(expectedAddress, model.Address);
            Assert.Equal(expectedManuallyAdded, model.ManuallyAdded);
            Assert.Equal(expectedSupportProjectId, model.SupportProjectId);
            Assert.Equal(expectedContactId, model.ContactId);
            Assert.Equal(expectedCreatedOn, model.CreatedOn);
            Assert.Equal(expectedCreatedBy, model.CreatedBy);
            Assert.Equal(expectedLastModifiedOn, model.LastModifiedOn);
            Assert.Equal(expectedLastModifiedBy, model.LastModifiedBy);
        }

        [Fact]
        public void ContactViewModel_ShouldHandleNullableProperties()
        {
            // Arrange
            var model = new ContactViewModel
            {
                Name = "Jane Doe",
                RoleName = "Accounting Officer",
                Email = "jane.doe@example.com",
                Phone = "01234 567890",
                Address = "2 Example Road, Example City, EC2 2EC",
                ManuallyAdded = false,
                SupportProjectId = null,
                ContactId = null,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "system.admin",
                LastModifiedOn = null,
                LastModifiedBy = null
            };

            // Act & Assert
            Assert.Equal("Jane Doe", model.Name);
            Assert.Equal("Accounting Officer", model.RoleName);
            Assert.Equal("jane.doe@example.com", model.Email);
            Assert.Equal("01234 567890", model.Phone);
            Assert.Equal("2 Example Road, Example City, EC2 2EC", model.Address);
            Assert.False(model.ManuallyAdded);
            Assert.Null(model.SupportProjectId);
            Assert.Null(model.ContactId);
            Assert.NotEqual(default(DateTime), model.CreatedOn);
            Assert.Equal("system.admin", model.CreatedBy);
            Assert.Null(model.LastModifiedOn);
            Assert.Null(model.LastModifiedBy);
        }

        [Fact]
        public void ContactViewModel_ShouldAllowEmptyStrings()
        {
            // Arrange & Act
            var model = new ContactViewModel
            {
                Name = "",
                RoleName = "",
                Email = "",
                Phone = "",
                Address = "",
                CreatedBy = ""
            };

            // Assert
            Assert.Equal(string.Empty, model.Name);
            Assert.Equal(string.Empty, model.RoleName);
            Assert.Equal(string.Empty, model.Email);
            Assert.Equal(string.Empty, model.Phone);
            Assert.Equal(string.Empty, model.Address);
            Assert.Equal(string.Empty, model.CreatedBy);
        }

        [Theory]
        [InlineData("Chair of Local Governing Body")]
        [InlineData("Chief Financial Officer")]
        [InlineData("Accounting Officer")]
        [InlineData("Headteacher")]
        [InlineData("Chief Executive Officer")]
        public void ContactViewModel_ShouldAcceptValidRoleNames(string roleName)
        {
            // Arrange
            var model = new ContactViewModel
            {
                RoleName = roleName
            };

            // Act & Assert
            Assert.Equal(roleName, model.RoleName);
        }

        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@education.gov.uk")]
        [InlineData("admin@school.co.uk")]
        [InlineData("contact@trust.org.uk")]
        public void ContactViewModel_ShouldAcceptValidEmailFormats(string email)
        {
            // Arrange
            var model = new ContactViewModel
            {
                Email = email
            };

            // Act & Assert
            Assert.Equal(email, model.Email);
        }

        [Theory]
        [InlineData("+44 20 7946 0958")]
        [InlineData("020 7946 0958")]
        [InlineData("01234 567890")]
        [InlineData("07123 456789")]
        [InlineData("")]
        public void ContactViewModel_ShouldAcceptValidPhoneNumbers(string phoneNumber)
        {
            // Arrange
            var model = new ContactViewModel
            {
                Phone = phoneNumber
            };

            // Act & Assert
            Assert.Equal(phoneNumber, model.Phone);
        }

        [Fact]
        public void ContactViewModel_ShouldHandleDateTimeKinds()
        {
            // Arrange
            var utcDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
            var localDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Local);
            var unspecifiedDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Unspecified);

            // Act & Assert for CreatedOn
            var model1 = new ContactViewModel { CreatedOn = utcDate };
            Assert.Equal(utcDate, model1.CreatedOn);
            Assert.Equal(DateTimeKind.Utc, model1.CreatedOn.Kind);

            var model2 = new ContactViewModel { CreatedOn = localDate };
            Assert.Equal(localDate, model2.CreatedOn);
            Assert.Equal(DateTimeKind.Local, model2.CreatedOn.Kind);

            var model3 = new ContactViewModel { CreatedOn = unspecifiedDate };
            Assert.Equal(unspecifiedDate, model3.CreatedOn);
            Assert.Equal(DateTimeKind.Unspecified, model3.CreatedOn.Kind);
        }

        [Fact]
        public void ContactViewModel_ShouldHandleNullableLastModifiedOnDateTime()
        {
            // Arrange
            var model = new ContactViewModel();
            var testDate = new DateTime(2024, 1, 20, 15, 45, 0, DateTimeKind.Utc);

            // Act & Assert - Initially null
            Assert.Null(model.LastModifiedOn);

            // Act & Assert - Set to a value
            model.LastModifiedOn = testDate;
            Assert.NotNull(model.LastModifiedOn);
            Assert.Equal(testDate, model.LastModifiedOn.Value);

            // Act & Assert - Set back to null
            model.LastModifiedOn = null;
            Assert.Null(model.LastModifiedOn);
        }

        [Fact]
        public void ContactViewModel_ShouldSupportObjectInitializerSyntax()
        {
            // Arrange & Act
            var model = new ContactViewModel
            {
                Name = "Test Contact",
                RoleName = "Test Role",
                Email = "test@example.com",
                Phone = "01234 567890",
                Address = "99 Demo Ave, Demo City, DC9 9DC",
                ManuallyAdded = true,
                SupportProjectId = 999,
                ContactId = new SupportProjectContactId(Guid.NewGuid()),
                CreatedOn = new DateTime(2024, 1, 1),
                CreatedBy = "test.user",
                LastModifiedOn = new DateTime(2024, 1, 2),
                LastModifiedBy = "modifier.user"
            };

            // Assert
            Assert.Equal("Test Contact", model.Name);
            Assert.Equal("Test Role", model.RoleName);
            Assert.Equal("test@example.com", model.Email);
            Assert.Equal("01234 567890", model.Phone);
            Assert.Equal("99 Demo Ave, Demo City, DC9 9DC", model.Address);
            Assert.True(model.ManuallyAdded);
            Assert.Equal(999, model.SupportProjectId);
            Assert.IsType<SupportProjectContactId>(model.ContactId);
            Assert.Equal(new DateTime(2024, 1, 1), model.CreatedOn);
            Assert.Equal("test.user", model.CreatedBy);
            Assert.Equal(new DateTime(2024, 1, 2), model.LastModifiedOn);
            Assert.Equal("modifier.user", model.LastModifiedBy);
        }

        [Fact]
        public void ContactViewModel_ShouldAllowIndependentPropertyModification()
        {
            // Arrange
            var model = new ContactViewModel
            {
                Name = "Original Name",
                Email = "original@example.com"
            };

            // Act
            model.Name = "Updated Name";
            // Leave Email unchanged

            // Assert
            Assert.Equal("Updated Name", model.Name);
            Assert.Equal("original@example.com", model.Email); // Should remain unchanged
        }
    }
}
