using Dfe.ManageSchoolImprovement.Frontend.Services;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public sealed class DateValidationServiceTests
    {
        private sealed class TestMessages : IDateValidationMessageProvider
        {
            public string DefaultMessage => "Enter a date in the correct format";
            public string MonthOutOfRange => "Month must be between 1 and 12";
            public string YearOutOfRange => "Year must be between 2000 and 2050";
            public string AllMissing => "Enter date";
            public string SomeMissing(string displayName, IEnumerable<string> missingParts) => $"{displayName} must include a {string.Join(" and ", missingParts)}";
            public string DayOutOfRange(int daysInMonth) => $"Day must be between 1 and {daysInMonth}";
            public (bool, string) ContextSpecificValidation(int day, int month, int year) => (true, string.Empty);
        }

        private static DateValidationService CreateService(IDateValidationMessageProvider? provider = null)
        {
            return new DateValidationService(provider ?? new TestMessages());
        }

        [Fact]
        public void Validate_AllPartsMissing_ReturnsAllMissingMessage()
        {
            var service = CreateService();

            var (isValid, message) = service.Validate(" ", " ", " ", "Date");

            Assert.False(isValid);
            Assert.Equal("Enter date", message);
        }

        [Theory]
        [InlineData("", "2", "2024", "Start date", "Start date must include a day")]
        [InlineData("1", "", "2024", "Start date", "Start date must include a month")]
        [InlineData("1", "2", "", "Start date", "Start date must include a year")]
        [InlineData("", "", "2024", "Start date", "Start date must include a day and month")]
        [InlineData("1", "", "", "Start date", "Start date must include a month and year")]
        [InlineData("", "2", "", "Start date", "Start date must include a day and year")]
        public void Validate_SomePartsMissing_ReturnsSomeMissingMessage(string day, string month, string year, string display, string expected)
        {
            var service = CreateService();

            var (isValid, message) = service.Validate(day, month, year, display);

            Assert.False(isValid);
            Assert.Equal(expected, message);
        }

        [Theory]
        [InlineData("a", "1", "2024")]
        [InlineData("1", "b", "2024")]
        [InlineData("1", "1", "c")]
        public void Validate_NonNumericInputs_ReturnsDefaultMessage(string day, string month, string year)
        {
            var service = CreateService();

            var (isValid, message) = service.Validate(day, month, year, "Date");

            Assert.False(isValid);
            Assert.Equal("Enter a date in the correct format", message);
        }

        [Theory]
        [InlineData("1", "0", "2024", "Month must be between 1 and 12")]
        [InlineData("1", "13", "2024", "Month must be between 1 and 12")]
        public void Validate_MonthOutOfRange_ReturnsMonthOutOfRange(string day, string month, string year, string expected)
        {
            var service = CreateService();

            var (isValid, message) = service.Validate(day, month, year, "Date");

            Assert.False(isValid);
            Assert.Equal(expected, message);
        }

        [Theory]
        [InlineData("1", "1", "1999")]
        [InlineData("1", "1", "2051")]
        public void Validate_YearOutOfRange_ReturnsYearOutOfRange(string day, string month, string year)
        {
            var service = CreateService();

            var (isValid, message) = service.Validate(day, month, year, "Date");

            Assert.False(isValid);
            Assert.Equal("Year must be between 2000 and 2050", message);
        }

        [Theory]
        [InlineData("31", "4", "2024", 30)] // April has 30 days
        [InlineData("31", "6", "2024", 30)] // June has 30 days
        [InlineData("31", "9", "2024", 30)] // September has 30 days
        [InlineData("31", "11", "2024", 30)] // November has 30 days
        [InlineData("30", "2", "2021", 28)] // Feb 2021 has 28 days
        [InlineData("29", "2", "2021", 28)] // Feb 2021 not leap year
        [InlineData("31", "2", "2024", 29)] // Feb 2024 leap year has 29 days
        [InlineData("0", "1", "2024", 31)] // Day less than 1
        public void Validate_DayOutOfRange_ReturnsDayOutOfRange(string day, string month, string year, int maxDays)
        {
            var service = CreateService();

            var (isValid, message) = service.Validate(day, month, year, "Date");

            Assert.False(isValid);
            Assert.Equal($"Day must be between 1 and {maxDays}", message);
        }

        [Fact]
        public void Validate_InvalidDateCombination_ReturnsDefaultMessage()
        {
            var service = CreateService();

            var day = "2";
            var month = "2";
            var year = "This is not valid";

            var (isValid, message) = service.Validate(day, month, year, "Date");

            Assert.False(isValid);
            Assert.Equal("Enter a date in the correct format", message);
        }

        [Theory]
        [InlineData("1", "1", "2000")]
        [InlineData("29", "2", "2024")] // leap year valid
        [InlineData("31", "1", "2050")]
        public void Validate_ValidDates_ReturnsValid(string day, string month, string year)
        {
            var service = CreateService();

            var (isValid, message) = service.Validate(day, month, year, "Date");

            Assert.True(isValid);
            Assert.Equal(string.Empty, message);
        }
    }
}
