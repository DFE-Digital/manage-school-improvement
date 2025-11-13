using Xunit.Abstractions;
using static Dfe.ManageSchoolImprovement.Frontend.Services.DateRangeValidationService;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public class DateRangeValidationServiceTests
    {
        public static TheoryData<DateRangeTestCase> DateRangeTestCases
        {
            get
            {
                var data = new TheoryData<DateRangeTestCase>();

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    DateRange.Past,
                    true,
                    ""));

                data.Add(new DateRangeTestCase(
                    DateTime.Now.AddYears(1),
                    DateRange.Past,
                    false,
                    "You must enter a date in the past"));

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    DateRange.PastOrToday,
                    true,
                    ""));

                data.Add(new DateRangeTestCase(
                    DateTime.Now.AddYears(1),
                    DateRange.PastOrToday,
                    false,
                    "Enter today's date or a date in the past"));

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    DateRange.Future,
                    false,
                    "Enter a date in the future"));

                data.Add(new DateRangeTestCase(
                    DateTime.Now.AddYears(1),
                    DateRange.Future,
                    true,
                    ""));

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    DateRange.FutureOrToday,
                    false,
                    "Enter today's date or a date in the future"));

                data.Add(new DateRangeTestCase(
                    DateTime.Now.AddYears(1),
                    DateRange.FutureOrToday,
                    true,
                    ""));

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    DateRange.PastOrFuture,
                    true,
                    ""));

                return data;
            }
        }

        // Use MemberData to inject the data into the test
        [Theory]
        [MemberData(nameof(DateRangeTestCases))]
        public void Validate_ShouldReturnCorrectResult(DateRangeTestCase testCase)
        {
            // Act
            var (isValid, message) = Validate(testCase.Date, testCase.DateRange, "Test");

            // Assert
            Assert.Equal(testCase.ExpectedIsValid, isValid);
            Assert.Equal(testCase.ExpectedMessage, message);
        }

        [Fact]
        public void Validate_PastRange_ShouldReturnErrorForFutureDate()
        {
            // Arrange
            var futureDate = DateTime.Today.AddDays(1);

            // Act
            var (isValid, message) = Validate(futureDate, DateRange.Past, "Test");

            // Assert
            Assert.False(isValid);
            Assert.Equal("Enter a date in the past", message);
        }

        [Fact]
        public void Validate_PastOrTodayRange_ShouldReturnErrorForFutureDate()
        {
            // Arrange
            var futureDate = DateTime.Today.AddDays(1);

            // Act
            var (isValid, message) = Validate(futureDate, DateRange.PastOrToday, "Test");

            // Assert
            Assert.False(isValid);
            Assert.Equal("Enter today's date or a date in the past", message);
        }

        [Fact]
        public void Validate_FutureRange_ShouldReturnErrorForPastDate()
        {
            // Arrange
            var pastDate = DateTime.Today.AddDays(-1);

            // Act
            var (isValid, message) = Validate(pastDate, DateRange.Future, "Test");

            // Assert
            Assert.False(isValid);
            Assert.Equal("Enter a date in the future", message);
        }

        [Fact]
        public void Validate_FutureOrTodayRange_ShouldReturnErrorForPastDate()
        {
            // Arrange
            var pastDate = DateTime.Today.AddDays(-1);

            // Act
            var (isValid, message) = Validate(pastDate, DateRange.FutureOrToday, "Test");

            // Assert
            Assert.False(isValid);
            Assert.Equal("Enter today's date or a date in the future", message);
        }

        [Fact]
        public void Validate_PastOrFutureRange_ShouldAlwaysReturnValid()
        {
            // Arrange
            var anyDate = DateTime.Today;

            // Act
            var (isValid, message) = Validate(anyDate, DateRange.PastOrFuture, "Test");

            // Assert
            Assert.True(isValid);
            Assert.Equal("", message);
        }
    }

    public class DateRangeTestCase : IXunitSerializable
    {
        public DateTime Date { get; private set; }
        public DateRange DateRange { get; private set; }
        public bool ExpectedIsValid { get; private set; }
        public string? ExpectedMessage { get; private set; }

        // Required parameterless constructor for serialization
        public DateRangeTestCase()
        {
        }

        public DateRangeTestCase(DateTime date, DateRange dateRange, bool expectedIsValid, string expectedMessage)
        {
            Date = date;
            DateRange = dateRange;
            ExpectedIsValid = expectedIsValid;
            ExpectedMessage = expectedMessage;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Date = info.GetValue<DateTime>(nameof(Date));
            DateRange = info.GetValue<DateRange>(nameof(DateRange));
            ExpectedIsValid = info.GetValue<bool>(nameof(ExpectedIsValid));
            ExpectedMessage = info.GetValue<string>(nameof(ExpectedMessage));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Date), Date);
            info.AddValue(nameof(DateRange), DateRange);
            info.AddValue(nameof(ExpectedIsValid), ExpectedIsValid);
            info.AddValue(nameof(ExpectedMessage), ExpectedMessage);
        }
    }
}
