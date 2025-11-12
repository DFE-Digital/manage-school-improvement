using Dfe.ManageSchoolImprovement.Extensions;
using Dfe.ManageSchoolImprovement.Utils;
using FluentAssertions;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Extensions;

public class PersonsApiExtensionsTests
{
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
    private readonly DateTime _testCurrentDate;

    public PersonsApiExtensionsTests()
    {
        _mockDateTimeProvider = new Mock<IDateTimeProvider>();
        _testCurrentDate = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc);
        _mockDateTimeProvider.Setup(x => x.Now).Returns(_testCurrentDate);
    }

    #region AcademyGovernance IsHistorical Tests

    [Fact]
    public void IsHistorical_AcademyGovernance_WithNullObject_ShouldReturnFalse()
    {
        // Arrange
        AcademyGovernance? academyGovernance = null;

        // Act
        var result = academyGovernance?.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsHistorical_AcademyGovernance_WithNullEndDate_ShouldReturnFalse()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = null
        };

        // Act
        var result = academyGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsHistorical_AcademyGovernance_WithEmptyEndDate_ShouldReturnFalse()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = ""
        };

        // Act
        var result = academyGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsHistorical_AcademyGovernance_WithWhitespaceEndDate_ShouldReturnFalse()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "   "
        };

        // Act
        var result = academyGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsHistorical_AcademyGovernance_WithInvalidDateFormat_ShouldReturnFalse()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "invalid-date"
        };

        // Act
        var result = academyGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("2024-01-14")] // One day before current date
    [InlineData("2023-12-31")] // Previous year
    [InlineData("2024-01-01")] // Earlier in current month
    public void IsHistorical_AcademyGovernance_WithPastDate_ShouldReturnTrue(string endDate)
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = endDate
        };

        // Act
        var result = academyGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("2024-01-15")] // Same as current date
    [InlineData("2024-01-16")] // One day after current date
    [InlineData("2024-02-01")] // Future month
    [InlineData("2025-01-15")] // Future year
    public void IsHistorical_AcademyGovernance_WithCurrentOrFutureDate_ShouldReturnFalse(string endDate)
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = endDate
        };

        // Act
        var result = academyGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("14/01/2024")] // UK format - past
    [InlineData("01/14/2024")] // US format - past
    [InlineData("14-01-2024")] // UK with dashes - past
    [InlineData("2024/01/14")] // ISO with slashes - past
    [InlineData("14 Jan 2024")] // Readable format - past
    [InlineData("Jan 14, 2024")] // US readable format - past
    public void IsHistorical_AcademyGovernance_WithVariousDateFormats_ShouldParseCorrectly(string endDate)
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = endDate
        };

        // Act
        var result = academyGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region TrustGovernance IsHistorical Tests

    [Fact]
    public void IsHistorical_TrustGovernance_WithNullObject_ShouldReturnFalse()
    {
        // Arrange
        TrustGovernance? trustGovernance = null;

        // Act
        var result = trustGovernance?.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsHistorical_TrustGovernance_WithNullEndDate_ShouldReturnFalse()
    {
        // Arrange
        var trustGovernance = new TrustGovernance
        {
            DateTermOfOfficeEndsEnded = null
        };

        // Act
        var result = trustGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsHistorical_TrustGovernance_WithPastDate_ShouldReturnTrue()
    {
        // Arrange
        var trustGovernance = new TrustGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-14"
        };

        // Act
        var result = trustGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsHistorical_TrustGovernance_WithFutureDate_ShouldReturnFalse()
    {
        // Arrange
        var trustGovernance = new TrustGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-16"
        };

        // Act
        var result = trustGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region AcademyGovernance IsCurrent Tests

    [Fact]
    public void IsCurrent_AcademyGovernance_WithNullEndDate_ShouldReturnTrue()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = null
        };

        // Act
        var result = academyGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCurrent_AcademyGovernance_WithPastDate_ShouldReturnFalse()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-14"
        };

        // Act
        var result = academyGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsCurrent_AcademyGovernance_WithFutureDate_ShouldReturnTrue()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-16"
        };

        // Act
        var result = academyGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCurrent_AcademyGovernance_WithCurrentDate_ShouldReturnTrue()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-15"
        };

        // Act
        var result = academyGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCurrent_AcademyGovernance_WithInvalidDate_ShouldReturnTrue()
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "invalid-date"
        };

        // Act
        var result = academyGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region TrustGovernance IsCurrent Tests

    [Fact]
    public void IsCurrent_TrustGovernance_WithNullEndDate_ShouldReturnTrue()
    {
        // Arrange
        var trustGovernance = new TrustGovernance
        {
            DateTermOfOfficeEndsEnded = null
        };

        // Act
        var result = trustGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCurrent_TrustGovernance_WithPastDate_ShouldReturnFalse()
    {
        // Arrange
        var trustGovernance = new TrustGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-14"
        };

        // Act
        var result = trustGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsCurrent_TrustGovernance_WithFutureDate_ShouldReturnTrue()
    {
        // Arrange
        var trustGovernance = new TrustGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-16"
        };

        // Act
        var result = trustGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Edge Cases and Special Scenarios

    [Fact]
    public void IsHistorical_WithTimeInDateString_ShouldIgnoreTimeComponent()
    {
        // Arrange
        var academyGovernanceWithTime = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-14T23:59:59" // Past date with time
        };

        var academyGovernanceWithTimeZ = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-14T23:59:59Z" // Past date with UTC time
        };

        // Act
        var resultWithTime = academyGovernanceWithTime.IsHistorical(_mockDateTimeProvider.Object);
        var resultWithTimeZ = academyGovernanceWithTimeZ.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        resultWithTime.Should().BeTrue();
        resultWithTimeZ.Should().BeTrue();
    }

    [Fact]
    public void IsHistorical_WithDateTimeProviderReturningDifferentTime_ShouldUseProviderTime()
    {
        // Arrange
        var customDate = new DateTime(2024, 6, 15, 14, 30, 0, DateTimeKind.Utc);
        var customMockProvider = new Mock<IDateTimeProvider>();
        customMockProvider.Setup(x => x.Now).Returns(customDate);

        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-06-14" // One day before custom date
        };

        // Act
        var result = academyGovernance.IsHistorical(customMockProvider.Object);

        // Assert
        result.Should().BeTrue();
        customMockProvider.Verify(x => x.Now, Times.Once);
    }

    [Theory]
    [InlineData("2024-13-01")] // Invalid month
    [InlineData("2024-02-30")] // Invalid day for February
    [InlineData("not-a-date")] // Completely invalid
    [InlineData("32/01/2024")] // Invalid day
    [InlineData("2024/13/01")] // Invalid month in different format
    public void IsHistorical_WithInvalidDates_ShouldReturnFalse(string invalidDate)
    {
        // Arrange
        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = invalidDate
        };

        // Act
        var result = academyGovernance.IsHistorical(_mockDateTimeProvider.Object);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsCurrent_ShouldBeOppositeOfIsHistorical()
    {
        // Arrange
        var historicalGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-14" // Past date
        };

        var currentGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-01-16" // Future date
        };

        // Act
        var historicalResult = historicalGovernance.IsHistorical(_mockDateTimeProvider.Object);
        var currentResult = historicalGovernance.IsCurrent(_mockDateTimeProvider.Object);

        var currentHistoricalResult = currentGovernance.IsHistorical(_mockDateTimeProvider.Object);
        var currentCurrentResult = currentGovernance.IsCurrent(_mockDateTimeProvider.Object);

        // Assert
        historicalResult.Should().BeTrue();
        currentResult.Should().BeFalse();

        currentHistoricalResult.Should().BeFalse();
        currentCurrentResult.Should().BeTrue();
    }

    [Fact]
    public void Extensions_ShouldHandleLeapYear()
    {
        // Arrange
        var leapYearDate = new DateTime(2024, 2, 29, 0, 0, 0, DateTimeKind.Utc); // Leap year
        var mockProvider = new Mock<IDateTimeProvider>();
        mockProvider.Setup(x => x.Now).Returns(leapYearDate);

        var academyGovernance = new AcademyGovernance
        {
            DateTermOfOfficeEndsEnded = "2024-02-28" // Day before leap day
        };

        // Act
        var result = academyGovernance.IsHistorical(mockProvider.Object);

        // Assert
        result.Should().BeTrue();
    }

    #endregion
}