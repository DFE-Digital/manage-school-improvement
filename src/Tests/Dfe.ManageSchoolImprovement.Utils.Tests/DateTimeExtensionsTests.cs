using System.Text.RegularExpressions;

namespace Dfe.ManageSchoolImprovement.Utils.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void ConvertsUtcInWinter_GMT()
        {
            var utcDateTime = new DateTime(2025, 1, 15, 7, 30, 0, DateTimeKind.Utc); // GMT
            var result = utcDateTime.UtcDateTimeToGdsDateTimeString();

            Assert.Equal("15 January 2025 at 7:30am", result);
        }

        [Fact]
        public void ConvertsUtcInSummer_BST()
        {
            var utcDateTime = new DateTime(2025, 6, 15, 6, 15, 0, DateTimeKind.Utc); // BST (UTC+1)
            var result = utcDateTime.UtcDateTimeToGdsDateTimeString();

            Assert.Equal("15 June 2025 at 7:15am", result); // BST = UTC + 1hr
        }

        [Fact]
        public void HandlesUnspecifiedKind_BySpecifyingUtc()
        {
            var utcDateTime = new DateTime(2025, 3, 14, 6, 26, 0, DateTimeKind.Unspecified);
            var result = utcDateTime.UtcDateTimeToGdsDateTimeString();

            // Expected local time should be 6:26 (or 7:26) depending on DST
            Assert.Contains("14 March 2025 at", result);
        }

        [Theory]
        [InlineData(8, "am")]
        [InlineData(18, "pm")]
        public void ConvertsCorrectlyAndFormatsAmPmProperly(int hour, string expectedAmPm)
        {
            var utcDateTime = new DateTime(2025, 5, 10, hour - 1, 0, 0, DateTimeKind.Utc); // BST adds +1 hour
            var result = utcDateTime.UtcDateTimeToGdsDateTimeString();

            Assert.EndsWith(expectedAmPm, result);
        }

        [Fact]
        public void OutputsCorrectFormat()
        {
            var date = new DateTime(2025, 3, 14, 6, 26, 0, DateTimeKind.Utc);
            var result = date.UtcDateTimeToGdsDateTimeString();

            var match = GdsDateTimeFormatRegex.Match(result);
            Assert.True(match.Success);
        }

        private static readonly Regex GdsDateTimeFormatRegex = new(
            @"^\d{2} [A-Z][a-z]+ \d{4} at \d{1,2}:\d{2}(am|pm)$",
            RegexOptions.None,
            TimeSpan.FromMilliseconds(200)
        );
    }
}
