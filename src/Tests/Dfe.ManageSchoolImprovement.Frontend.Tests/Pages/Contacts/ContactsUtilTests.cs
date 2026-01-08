using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts;
using Dfe.ManageSchoolImprovement.Utils;
using FluentAssertions;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages.Contacts;

public class ContactsUtilTests
{
    #region GetSchoolRadioButtons Tests

    [Fact]
    public void GetSchoolRadioButtons_WithNullOtherRole_ShouldReturnAllSchoolRoles()
    {
        // Arrange & Act
        var result = ContactsUtil.GetSchoolRadioButtons(null);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(4); // All SchoolOrginisationTypes enum values

        // Verify all school roles are present
        result.Should().Contain(r => r.Name == "Headteacher (permanent)");
        result.Should().Contain(r => r.Name == "Headteacher (interim)");
        result.Should().Contain(r => r.Name == "Chair of governors");
        result.Should().Contain(r => r.Name == "Other job title");
    }

    [Fact]
    public void GetSchoolRadioButtons_WithOtherRoleValue_ShouldSetOtherInput()
    {
        // Arrange
        var otherRoleValue = "Custom School Role";

        // Act
        var result = ContactsUtil.GetSchoolRadioButtons(otherRoleValue);

        // Assert
        var otherRoleOption = result.FirstOrDefault(r => r.Name == "Other job title");
        otherRoleOption.Should().NotBeNull();
        otherRoleOption!.Input.Should().NotBeNull();
        otherRoleOption.Input!.Value.Should().Be(otherRoleValue);
        otherRoleOption.Input.Id.Should().Be("organisationTypeSubCategoryOther");
        otherRoleOption.Input.ValidationMessage.Should().Be("Enter name of job title");
        otherRoleOption.Input.Paragraph.Should().Be("Name of job title");
        otherRoleOption.Input.IsValid.Should().BeTrue();
        otherRoleOption.Input.IsTextArea.Should().BeFalse();
        otherRoleOption.DisplayAsOr.Should().BeTrue();
    }

    [Fact]
    public void GetSchoolRadioButtons_WithValidationError_ShouldSetInputAsInvalid()
    {
        // Arrange & Act
        var result = ContactsUtil.GetSchoolRadioButtons("School Role", isValid: false);

        // Assert
        var otherRoleOption = result.FirstOrDefault(r => r.Name == "Other job title");
        otherRoleOption!.Input!.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(SchoolOrginisationTypes.PermanentHeadteacher, "headteacher-(permanent)")]
    [InlineData(SchoolOrginisationTypes.InterimHeadteacher, "headteacher-(interim)")]
    [InlineData(SchoolOrginisationTypes.ChairOfGovernors, "chair-of-governors")]
    [InlineData(SchoolOrginisationTypes.Other, "other-job-title")]
    public void GetSchoolRadioButtons_ShouldGenerateCorrectIdsFromDisplayNames(SchoolOrginisationTypes role, string expectedId)
    {
        // Act
        var result = ContactsUtil.GetSchoolRadioButtons(null);

        // Assert
        var roleOption = result.FirstOrDefault(r => r.Value == role.GetDisplayName());
        roleOption.Should().NotBeNull();
        roleOption!.Id.Should().Be(expectedId);
    }

    [Fact]
    public void GetSchoolRadioButtons_ShouldUseDisplayNameAsValue()
    {
        // Act
        var result = ContactsUtil.GetSchoolRadioButtons(null);

        // Assert
        foreach (SchoolOrginisationTypes role in Enum.GetValues<SchoolOrginisationTypes>())
        {
            var expectedDisplayName = role.GetDisplayName();
            var roleOption = result.FirstOrDefault(r => r.Value == expectedDisplayName);
            roleOption.Should().NotBeNull($"Role {role} should be present with display name as value");
            roleOption!.Value.Should().Be(expectedDisplayName);
            roleOption.Name.Should().Be(expectedDisplayName);
        }
    }

    [Fact]
    public void GetSchoolRadioButtons_NonOtherRoles_ShouldNotHaveInput()
    {
        // Act
        var result = ContactsUtil.GetSchoolRadioButtons(null);

        // Assert
        var nonOtherRoles = result.Where(r => r.Name != "Other job title");
        foreach (var role in nonOtherRoles)
        {
            role.Input.Should().BeNull($"Non-Other role '{role.Name}' should not have Input");
            role.DisplayAsOr.Should().BeFalse($"Non-Other role '{role.Name}' should not have DisplayAsOr set");
        }
    }

    #endregion

    #region GetSupportingOrganisationRadioButtons Tests

    [Fact]
    public void GetSupportingOrganisationRadioButtons_WithNullOtherRole_ShouldReturnAllSupportingOrgRoles()
    {
        // Arrange & Act
        var result = ContactsUtil.GetSupportingOrganisationRadioButtons(null);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3); // All SupportOrganisationTypes enum values

        // Verify all supporting organisation roles are present
        result.Should().Contain(r => r.Name == "Accounting officer");
        result.Should().Contain(r => r.Name == "Headteacher");
        result.Should().Contain(r => r.Name == "Other job title");
    }

    [Fact]
    public void GetSupportingOrganisationRadioButtons_WithOtherRoleValue_ShouldSetOtherInput()
    {
        // Arrange
        var otherRoleValue = "Custom Support Role";

        // Act
        var result = ContactsUtil.GetSupportingOrganisationRadioButtons(otherRoleValue);

        // Assert
        var otherRoleOption = result.FirstOrDefault(r => r.Name == "Other job title");
        otherRoleOption.Should().NotBeNull();
        otherRoleOption!.Input.Should().NotBeNull();
        otherRoleOption.Input!.Value.Should().Be(otherRoleValue);
        otherRoleOption.Input.Id.Should().Be("organisationTypeSubCategoryOther");
        otherRoleOption.Input.ValidationMessage.Should().Be("Enter name of job title");
        otherRoleOption.Input.Paragraph.Should().Be("Name of job title");
        otherRoleOption.DisplayAsOr.Should().BeTrue();
    }

    [Theory]
    [InlineData(SupportOrganisationTypes.ProjectLead, "accounting-officer")]
    [InlineData(SupportOrganisationTypes.Headteacher, "headteacher")]
    [InlineData(SupportOrganisationTypes.Other, "other-job-title")]
    public void GetSupportingOrganisationRadioButtons_ShouldGenerateCorrectIdsFromDisplayNames(SupportOrganisationTypes role, string expectedId)
    {
        // Act
        var result = ContactsUtil.GetSupportingOrganisationRadioButtons(null);

        // Assert
        var roleOption = result.FirstOrDefault(r => r.Value == role.GetDisplayName());
        roleOption.Should().NotBeNull();
        roleOption!.Id.Should().Be(expectedId);
    }

    [Fact]
    public void GetSupportingOrganisationRadioButtons_ShouldUseDisplayNameAsValue()
    {
        // Act
        var result = ContactsUtil.GetSupportingOrganisationRadioButtons(null);

        // Assert
        foreach (SupportOrganisationTypes role in Enum.GetValues<SupportOrganisationTypes>())
        {
            var expectedDisplayName = role.GetDisplayName();
            var roleOption = result.FirstOrDefault(r => r.Value == expectedDisplayName);
            roleOption.Should().NotBeNull($"Role {role} should be present with display name as value");
            roleOption!.Value.Should().Be(expectedDisplayName);
            roleOption.Name.Should().Be(expectedDisplayName);
        }
    }

    #endregion

    #region GetGoverningBodyRadioButtons Tests

    [Fact]
    public void GetGoverningBodyRadioButtons_WithNullOtherRole_ShouldReturnAllGovernanceBodyTypes()
    {
        // Arrange & Act
        var result = ContactsUtil.GetGoverningBodyRadioButtons(null);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(6); // All GovernanceBodyTypes enum values

        // Verify all governance body types are present
        result.Should().Contain(r => r.Name == "Trust");
        result.Should().Contain(r => r.Name == "Local authority");
        result.Should().Contain(r => r.Name == "Diocese");
        result.Should().Contain(r => r.Name == "Foundation");
        result.Should().Contain(r => r.Name == "Federation");
        result.Should().Contain(r => r.Name == "Other body");
    }

    [Fact]
    public void GetGoverningBodyRadioButtons_WithOtherRoleValue_ShouldSetOtherInput()
    {
        // Arrange
        var otherRoleValue = "Custom Governance Body";

        // Act
        var result = ContactsUtil.GetGoverningBodyRadioButtons(otherRoleValue);

        // Assert
        var otherRoleOption = result.FirstOrDefault(r => r.Name == "Other body");
        otherRoleOption.Should().NotBeNull();
        otherRoleOption!.Input.Should().NotBeNull();
        otherRoleOption.Input!.Value.Should().Be(otherRoleValue);
        otherRoleOption.Input.Id.Should().Be("organisationTypeSubCategoryOther");
        otherRoleOption.Input.ValidationMessage.Should().Be("Enter name of governance body");
        otherRoleOption.Input.Paragraph.Should().Be("Name of governance body");
        otherRoleOption.DisplayAsOr.Should().BeTrue();
    }

    [Theory]
    [InlineData(GovernanceBodyTypes.Trust, "trust")]
    [InlineData(GovernanceBodyTypes.LocalAuthority, "local-authority")]
    [InlineData(GovernanceBodyTypes.Diocese, "diocese")]
    [InlineData(GovernanceBodyTypes.Foundation, "foundation")]
    [InlineData(GovernanceBodyTypes.Federation, "federation")]
    [InlineData(GovernanceBodyTypes.Other, "other-body")]
    public void GetGoverningBodyRadioButtons_ShouldGenerateCorrectIdsFromDisplayNames(GovernanceBodyTypes role, string expectedId)
    {
        // Act
        var result = ContactsUtil.GetGoverningBodyRadioButtons(null);

        // Assert
        var roleOption = result.FirstOrDefault(r => r.Value == role.GetDisplayName());
        roleOption.Should().NotBeNull();
        roleOption!.Id.Should().Be(expectedId);
    }

    [Fact]
    public void GetGoverningBodyRadioButtons_ShouldUseDisplayNameAsValue()
    {
        // Act
        var result = ContactsUtil.GetGoverningBodyRadioButtons(null);

        // Assert
        foreach (GovernanceBodyTypes role in Enum.GetValues<GovernanceBodyTypes>())
        {
            var expectedDisplayName = role.GetDisplayName();
            var roleOption = result.FirstOrDefault(r => r.Value == expectedDisplayName);
            roleOption.Should().NotBeNull($"Role {role} should be present with display name as value");
            roleOption!.Value.Should().Be(expectedDisplayName);
            roleOption.Name.Should().Be(expectedDisplayName);
        }
    }

    #endregion

    #region Cross-Method Validation Tests

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("Test Role")]
    [InlineData("Manager Position")]
    public void AllMethods_WithSameOtherRoleValue_ShouldPreserveValueInOtherOptions(string otherRoleValue)
    {
        // Act
        var schoolButtons = ContactsUtil.GetSchoolRadioButtons(otherRoleValue);
        var supportingButtons = ContactsUtil.GetSupportingOrganisationRadioButtons(otherRoleValue);
        var governanceButtons = ContactsUtil.GetGoverningBodyRadioButtons(otherRoleValue);

        // Assert
        var schoolOther = schoolButtons.FirstOrDefault(r => r.Name == "Other job title");
        var supportingOther = supportingButtons.FirstOrDefault(r => r.Name == "Other job title");
        var governanceOther = governanceButtons.FirstOrDefault(r => r.Name == "Other body");

        schoolOther!.Input!.Value.Should().Be(otherRoleValue);
        supportingOther!.Input!.Value.Should().Be(otherRoleValue);
        governanceOther!.Input!.Value.Should().Be(otherRoleValue);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AllMethods_WithValidationState_ShouldApplyToOtherOptions(bool isValid)
    {
        // Act
        var schoolButtons = ContactsUtil.GetSchoolRadioButtons("Test", isValid);
        var supportingButtons = ContactsUtil.GetSupportingOrganisationRadioButtons("Test", isValid);
        var governanceButtons = ContactsUtil.GetGoverningBodyRadioButtons("Test", isValid);

        // Assert
        var schoolOther = schoolButtons.FirstOrDefault(r => r.Name == "Other job title");
        var supportingOther = supportingButtons.FirstOrDefault(r => r.Name == "Other job title");
        var governanceOther = governanceButtons.FirstOrDefault(r => r.Name == "Other body");

        schoolOther!.Input!.IsValid.Should().Be(isValid);
        supportingOther!.Input!.IsValid.Should().Be(isValid);
        governanceOther!.Input!.IsValid.Should().Be(isValid);
    }

    [Fact]
    public void AllMethods_ShouldHandleNullOtherRoleConsistently()
    {
        // Act
        var schoolButtons = ContactsUtil.GetSchoolRadioButtons(null);
        var supportingButtons = ContactsUtil.GetSupportingOrganisationRadioButtons(null);
        var governanceButtons = ContactsUtil.GetGoverningBodyRadioButtons(null);

        // Assert - All should return non-null collections
        schoolButtons.Should().NotBeNull().And.NotBeEmpty();
        supportingButtons.Should().NotBeNull().And.NotBeEmpty();
        governanceButtons.Should().NotBeNull().And.NotBeEmpty();

        // Other options should have null values
        var schoolOther = schoolButtons.FirstOrDefault(r => r.Name == "Other job title");
        var supportingOther = supportingButtons.FirstOrDefault(r => r.Name == "Other job title");
        var governanceOther = governanceButtons.FirstOrDefault(r => r.Name == "Other body");

        schoolOther!.Input!.Value.Should().BeNull();
        supportingOther!.Input!.Value.Should().BeNull();
        governanceOther!.Input!.Value.Should().BeNull();
    }

    #endregion

    #region Input Validation Message Tests

    [Fact]
    public void GetSchoolRadioButtons_OtherOption_ShouldHaveSchoolSpecificValidationMessage()
    {
        // Act
        var result = ContactsUtil.GetSchoolRadioButtons("Test");

        // Assert
        var otherOption = result.FirstOrDefault(r => r.Name == "Other job title");
        otherOption!.Input!.ValidationMessage.Should().Be("Enter name of job title");
        otherOption.Input.Paragraph.Should().Be("Name of job title");
    }

    [Fact]
    public void GetSupportingOrganisationRadioButtons_OtherOption_ShouldHaveSupportingOrgSpecificValidationMessage()
    {
        // Act
        var result = ContactsUtil.GetSupportingOrganisationRadioButtons("Test");

        // Assert
        var otherOption = result.FirstOrDefault(r => r.Name == "Other job title");
        otherOption!.Input!.ValidationMessage.Should().Be("Enter name of job title");
        otherOption.Input.Paragraph.Should().Be("Name of job title");
    }

    [Fact]
    public void GetGoverningBodyRadioButtons_OtherOption_ShouldHaveGovernanceBodySpecificValidationMessage()
    {
        // Act
        var result = ContactsUtil.GetGoverningBodyRadioButtons("Test");

        // Assert
        var otherOption = result.FirstOrDefault(r => r.Name == "Other body");
        otherOption!.Input!.ValidationMessage.Should().Be("Enter name of governance body");
        otherOption.Input.Paragraph.Should().Be("Name of governance body");
    }

    #endregion

    #region Input ID Tests

    [Theory]
    [InlineData("School")]
    [InlineData("Supporting")]
    [InlineData("Governance")]
    public void OrganisationSpecificMethods_OtherOptions_ShouldHaveConsistentInputId(string methodType)
    {
        // Act
        var result = methodType switch
        {
            "School" => ContactsUtil.GetSchoolRadioButtons("Test"),
            "Supporting" => ContactsUtil.GetSupportingOrganisationRadioButtons("Test"),
            "Governance" => ContactsUtil.GetGoverningBodyRadioButtons("Test"),
            _ => throw new ArgumentException($"Unknown method type: {methodType}")
        };

        // Assert
        var otherOption = result.FirstOrDefault(r => r.Name.Contains("Other"));
        otherOption!.Input!.Id.Should().Be("organisationTypeSubCategoryOther");
    }

    #endregion

    #region Performance and Edge Case Tests

    [Fact]
    public void AllMethods_ShouldHandleEmptyStringOtherRoleConsistently()
    {
        // Act
        var schoolButtons = ContactsUtil.GetSchoolRadioButtons("");
        var supportingButtons = ContactsUtil.GetSupportingOrganisationRadioButtons("");
        var governanceButtons = ContactsUtil.GetGoverningBodyRadioButtons("");

        // Assert - All should preserve empty string
        var schoolOther = schoolButtons.FirstOrDefault(r => r.Name == "Other job title");
        var supportingOther = supportingButtons.FirstOrDefault(r => r.Name == "Other job title");
        var governanceOther = governanceButtons.FirstOrDefault(r => r.Name == "Other body");

        schoolOther!.Input!.Value.Should().Be("");
        supportingOther!.Input!.Value.Should().Be("");
        governanceOther!.Input!.Value.Should().Be("");
    }

    [Fact]
    public void AllMethods_ShouldHandleWhitespaceOtherRole()
    {
        // Arrange
        var whitespaceValue = "   \t\n   ";

        // Act & Assert - Should preserve whitespace as-is
        var schoolOther = ContactsUtil.GetSchoolRadioButtons(whitespaceValue)
            .FirstOrDefault(r => r.Name == "Other job title");
        schoolOther!.Input!.Value.Should().Be(whitespaceValue);
    }

    [Fact]
    public void AllMethods_ShouldReturnNewInstancesEachTime()
    {
        // Act
        var firstCall = ContactsUtil.GetSchoolRadioButtons("Test");
        var secondCall = ContactsUtil.GetSchoolRadioButtons("Test");

        // Assert - Should be different instances but equivalent content
        firstCall.Should().NotBeSameAs(secondCall);
        firstCall.Should().BeEquivalentTo(secondCall);
    }

    #endregion
}