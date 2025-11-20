using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts;
using FluentAssertions;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages.Contacts;

public class ContactsUtilTests
{
    #region GetRadioButtons Tests

    [Fact]
    public void GetRadioButtons_WithNullOtherRole_ShouldReturnAllRoleOptions()
    {
        // Arrange & Act
        var result = ContactsUtil.GetRadioButtons(null);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(7); // All enum values

        // Verify all roles are present
        result.Should().Contain(r => r.Name == "Director of Education");
        result.Should().Contain(r => r.Name == "Headteacher");
        result.Should().Contain(r => r.Name == "Chair of governors");
        result.Should().Contain(r => r.Name == "Trust relationship manager");
        result.Should().Contain(r => r.Name == "Trust CEO");
        result.Should().Contain(r => r.Name == "Trust accounting officer");
        result.Should().Contain(r => r.Name == "Other role");
    }

    [Fact]
    public void GetRadioButtons_WithEmptyOtherRole_ShouldReturnAllRoleOptions()
    {
        // Arrange & Act
        var result = ContactsUtil.GetRadioButtons("");

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(7);
    }

    [Fact]
    public void GetRadioButtons_WithOtherRoleValue_ShouldSetOtherRoleInput()
    {
        // Arrange
        var otherRoleValue = "Custom Role Title";

        // Act
        var result = ContactsUtil.GetRadioButtons(otherRoleValue);

        // Assert
        var otherRoleOption = result.FirstOrDefault(r => r.Name == "Other role");
        otherRoleOption.Should().NotBeNull();
        otherRoleOption!.Input.Should().NotBeNull();
        otherRoleOption.Input!.Value.Should().Be(otherRoleValue);
        otherRoleOption.Input.Id.Should().Be("OtherRole");
        otherRoleOption.Input.ValidationMessage.Should().Be("Enter a role");
        otherRoleOption.Input.Paragraph.Should().Be("Enter a role");
        otherRoleOption.Input.IsValid.Should().BeTrue();
        otherRoleOption.Input.IsTextArea.Should().BeFalse();
    }

    [Fact]
    public void GetRadioButtons_WithValidationError_ShouldSetInputAsInvalid()
    {
        // Arrange & Act
        var result = ContactsUtil.GetRadioButtons("Custom Role", isValid: false);

        // Assert
        var otherRoleOption = result.FirstOrDefault(r => r.Name == "Other role");
        otherRoleOption.Should().NotBeNull();
        otherRoleOption!.Input.Should().NotBeNull();
        otherRoleOption.Input!.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(RolesIds.DirectorOfEducation, "director-of-education")]
    [InlineData(RolesIds.Headteacher, "headteacher")]
    [InlineData(RolesIds.ChairOfGovernors, "chair-of-governors")]
    [InlineData(RolesIds.TrustRelationshipManager, "trust-relationship-manager")]
    [InlineData(RolesIds.TrustCEO, "trust-ceo")]
    [InlineData(RolesIds.TrustAccountingOfficer, "trust-accounting-officer")]
    [InlineData(RolesIds.Other, "other-role")]
    public void GetRadioButtons_ShouldGenerateCorrectIdsFromDisplayNames(RolesIds role, string expectedId)
    {
        // Act
        var result = ContactsUtil.GetRadioButtons(null);

        // Assert
        var roleOption = result.FirstOrDefault(r => r.Value == role.GetHashCode().ToString());
        roleOption.Should().NotBeNull();
        roleOption!.Id.Should().Be(expectedId);
    }

    [Theory]
    [InlineData(RolesIds.DirectorOfEducation, "Director of Education")]
    [InlineData(RolesIds.Headteacher, "Headteacher")]
    [InlineData(RolesIds.ChairOfGovernors, "Chair of governors")]
    [InlineData(RolesIds.TrustRelationshipManager, "Trust relationship manager")]
    [InlineData(RolesIds.TrustCEO, "Trust CEO")]
    [InlineData(RolesIds.TrustAccountingOfficer, "Trust accounting officer")]
    [InlineData(RolesIds.Other, "Other role")]
    public void GetRadioButtons_ShouldSetCorrectDisplayNames(RolesIds role, string expectedName)
    {
        // Act
        var result = ContactsUtil.GetRadioButtons(null);

        // Assert
        var roleOption = result.FirstOrDefault(r => r.Value == role.GetHashCode().ToString());
        roleOption.Should().NotBeNull();
        roleOption!.Name.Should().Be(expectedName);
    }

    [Fact]
    public void GetRadioButtons_ShouldSetCorrectHashCodeValues()
    {
        // Act
        var result = ContactsUtil.GetRadioButtons(null);

        // Assert
        foreach (RolesIds role in Enum.GetValues<RolesIds>())
        {
            var roleOption = result.FirstOrDefault(r => r.Value == role.GetHashCode().ToString());
            roleOption.Should().NotBeNull($"Role {role} should be present in results");
            roleOption!.Value.Should().Be(role.GetHashCode().ToString());
        }
    }

    [Fact]
    public void GetRadioButtons_NonOtherRoles_ShouldNotHaveInput()
    {
        // Act
        var result = ContactsUtil.GetRadioButtons(null);

        // Assert
        var nonOtherRoles = result.Where(r => r.Name != "Other role");
        foreach (var role in nonOtherRoles)
        {
            role.Input.Should().BeNull($"Non-Other role '{role.Name}' should not have Input");
        }
    }

    [Fact]
    public void GetRadioButtons_OtherRole_ShouldAlwaysHaveInput()
    {
        // Act
        var result = ContactsUtil.GetRadioButtons(null);

        // Assert
        var otherRole = result.FirstOrDefault(r => r.Name == "Other role");
        otherRole.Should().NotBeNull();
        otherRole!.Input.Should().NotBeNull();
    }

    [Theory]
    [InlineData("Custom Role")]
    [InlineData("Special Position")]
    [InlineData("Senior Manager")]
    [InlineData("")]
    [InlineData("   ")]
    public void GetRadioButtons_WithDifferentOtherRoleValues_ShouldPreserveValue(string otherRoleValue)
    {
        // Act
        var result = ContactsUtil.GetRadioButtons(otherRoleValue);

        // Assert
        var otherRole = result.FirstOrDefault(r => r.Name == "Other role");
        otherRole!.Input!.Value.Should().Be(otherRoleValue);
    }

    #endregion

    #region IsOtherRoleFieldValidation Tests

    [Fact]
    public void IsOtherRoleFieldValidation_WhenRoleIsOtherAndOtherRoleHasValue_ShouldReturnTrue()
    {
        // Arrange
        var otherRoleHashCode = RolesIds.Other.GetHashCode();
        var otherRole = "Custom Role";

        // Act
        var result = ContactsUtil.IsOtherRoleFieldValidation(otherRoleHashCode, otherRole);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsOtherRoleFieldValidation_WhenRoleIsOtherAndOtherRoleIsNull_ShouldReturnFalse()
    {
        // Arrange
        var otherRoleHashCode = RolesIds.Other.GetHashCode();
        string? otherRole = null;

        // Act
        var result = ContactsUtil.IsOtherRoleFieldValidation(otherRoleHashCode, otherRole);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsOtherRoleFieldValidation_WhenRoleIsOtherAndOtherRoleIsEmpty_ShouldReturnFalse()
    {
        // Arrange
        var otherRoleHashCode = RolesIds.Other.GetHashCode();
        var otherRole = "";

        // Act
        var result = ContactsUtil.IsOtherRoleFieldValidation(otherRoleHashCode, otherRole);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsOtherRoleFieldValidation_WhenRoleIsOtherAndOtherRoleIsWhitespace_ShouldReturnFalse()
    {
        // Arrange
        var otherRoleHashCode = RolesIds.Other.GetHashCode();
        var otherRole = "   ";

        // Act
        var result = ContactsUtil.IsOtherRoleFieldValidation(otherRoleHashCode, otherRole);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(RolesIds.DirectorOfEducation)]
    [InlineData(RolesIds.Headteacher)]
    [InlineData(RolesIds.ChairOfGovernors)]
    [InlineData(RolesIds.TrustRelationshipManager)]
    [InlineData(RolesIds.TrustCEO)]
    [InlineData(RolesIds.TrustAccountingOfficer)]
    public void IsOtherRoleFieldValidation_WhenRoleIsNotOther_ShouldReturnFalse(RolesIds role)
    {
        // Arrange
        var roleHashCode = role.GetHashCode();
        var otherRole = "Some Role";

        // Act
        var result = ContactsUtil.IsOtherRoleFieldValidation(roleHashCode, otherRole);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsOtherRoleFieldValidation_WhenRoleIdIsNull_ShouldReturnFalse()
    {
        // Arrange
        int? roleId = null;
        var otherRole = "Custom Role";

        // Act
        var result = ContactsUtil.IsOtherRoleFieldValidation(roleId, otherRole);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("Valid Role Name")]
    [InlineData("A")]
    [InlineData("Very Long Role Name That Should Still Work")]
    [InlineData("Role-With-Hyphens")]
    [InlineData("Role With Spaces")]
    [InlineData("123 Numeric Role")]
    public void IsOtherRoleFieldValidation_WhenRoleIsOtherAndOtherRoleIsValid_ShouldReturnTrue(string otherRole)
    {
        // Arrange
        var otherRoleHashCode = RolesIds.Other.GetHashCode();

        // Act
        var result = ContactsUtil.IsOtherRoleFieldValidation(otherRoleHashCode, otherRole);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(999999)]  // Invalid role ID
    [InlineData(-1)]      // Negative role ID
    [InlineData(0)]       // Zero (assuming this doesn't match any enum value)
    public void IsOtherRoleFieldValidation_WhenRoleIdIsInvalid_ShouldReturnFalse(int invalidRoleId)
    {
        // Arrange
        var otherRole = "Custom Role";

        // Act
        var result = ContactsUtil.IsOtherRoleFieldValidation(invalidRoleId, otherRole);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void GetRadioButtons_ResultShouldBeCompatibleWithIsOtherRoleFieldValidation()
    {
        // Arrange
        var otherRoleText = "Integration Test Role";

        // Act
        var radioButtons = ContactsUtil.GetRadioButtons(otherRoleText);
        var otherRoleButton = radioButtons.FirstOrDefault(r => r.Name == "Other role");
        var isValid = ContactsUtil.IsOtherRoleFieldValidation(
            int.Parse(otherRoleButton!.Value!),
            otherRoleText);

        // Assert
        isValid.Should().BeTrue("GetRadioButtons and IsOtherRoleFieldValidation should be compatible");
    }

    [Fact]
    public void ContactsUtil_ShouldHandleAllEnumValues()
    {
        // Arrange
        var allEnumValues = Enum.GetValues<RolesIds>();

        // Act
        var radioButtons = ContactsUtil.GetRadioButtons("Test Role");

        // Assert
        radioButtons.Should().HaveCount(allEnumValues.Length, "Should include all enum values");

        foreach (var enumValue in allEnumValues)
        {
            var button = radioButtons.FirstOrDefault(rb => rb.Value == enumValue.GetHashCode().ToString());
            button.Should().NotBeNull($"Enum value {enumValue} should have corresponding radio button");
        }
    }

    [Fact]
    public void ContactsUtil_ShouldMaintainConsistentHashCodes()
    {
        // Arrange
        var firstCall = ContactsUtil.GetRadioButtons(null);
        var secondCall = ContactsUtil.GetRadioButtons(null);

        // Act & Assert
        for (int i = 0; i < firstCall.Count; i++)
        {
            firstCall[i].Value.Should().Be(secondCall[i].Value,
                "Hash codes should be consistent across multiple calls");
        }
    }

    #endregion
}