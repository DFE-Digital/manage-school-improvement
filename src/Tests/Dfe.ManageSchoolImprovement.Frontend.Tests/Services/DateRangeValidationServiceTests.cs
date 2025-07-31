
using Dfe.ManageSchoolImprovement.Frontend.Services;
using static Dfe.ManageSchoolImprovement.Frontend.Services.DateRangeValidationService;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public class DateRangeValidationServiceTests
    {
        [Serializable]
        public record DateRangeTestCase(DateTime Date, DateRange DateRange, bool ExpectedIsValid, string ExpectedMessage);
        public static TheoryData<DateRangeTestCase> DateRangeTestCases
        {
            get
            {
                var data = new TheoryData<DateRangeTestCase>();
            
                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1), 
                    DateRange.Past, 
                    true, 
                    ""));

                data.Add(new DateRangeTestCase(
                    DateTime.Now.AddYears(1), 
                    DateRange.Past, 
                    false, 
                    "You must enter a date in the past"));

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1), 
                    DateRange.PastOrToday, 
                    true, 
                    ""));

                data.Add(new DateRangeTestCase(
                    DateTime.Now.AddYears(1), 
                    DateRange.PastOrToday, 
                    false, 
                    "You must enter today's date or a date in the past"));

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1), 
                    DateRange.Future, 
                    false, 
                    "You must enter a date in the future"));

                data.Add(new DateRangeTestCase(
                    DateTime.Now.AddYears(1), 
                    DateRange.Future, 
                    true, 
                    ""));

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1), 
                    DateRange.FutureOrToday, 
                    false, 
                    "You must enter today's date or a date in the future"));

                data.Add(new DateRangeTestCase(
                    DateTime.Now.AddYears(1), 
                    DateRange.FutureOrToday, 
                    true, 
                    ""));

                data.Add(new DateRangeTestCase(
                    new DateTime(2022, 1, 1), 
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
            Assert.Equal("You must enter a date in the past", message);
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
            Assert.Equal("You must enter today's date or a date in the past", message);
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
            Assert.Equal("You must enter a date in the future", message);
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
            Assert.Equal("You must enter today's date or a date in the future", message);
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
}
