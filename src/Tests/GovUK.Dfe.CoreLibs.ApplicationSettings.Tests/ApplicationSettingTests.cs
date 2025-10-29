using FluentAssertions;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Entities;

namespace GovUK.Dfe.CoreLibs.ApplicationSettings.Tests.Entities;

public class ApplicationSettingTests
{
    [Fact]
    public void ApplicationSetting_ShouldInitializeWithDefaultValues()
    {
        // Act
        var setting = new ApplicationSetting();

        // Assert
        setting.Key.Should().Be(string.Empty);
        setting.Value.Should().Be(string.Empty);
        setting.Category.Should().Be("General");
        setting.IsEncrypted.Should().BeFalse();
        setting.IsActive.Should().BeTrue();
        setting.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        setting.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ApplicationSetting_ShouldAllowSettingProperties()
    {
        // Arrange
        var key = "TestKey";
        var value = "TestValue";
        var description = "Test Description";
        var category = "TestCategory";
        var createdBy = "TestUser";

        // Act
        var setting = new ApplicationSetting
        {
            Key = key,
            Value = value,
            Description = description,
            Category = category,
            CreatedBy = createdBy,
            IsEncrypted = true,
            IsActive = false
        };

        // Assert
        setting.Key.Should().Be(key);
        setting.Value.Should().Be(value);
        setting.Description.Should().Be(description);
        setting.Category.Should().Be(category);
        setting.CreatedBy.Should().Be(createdBy);
        setting.IsEncrypted.Should().BeTrue();
        setting.IsActive.Should().BeFalse();
    }
}