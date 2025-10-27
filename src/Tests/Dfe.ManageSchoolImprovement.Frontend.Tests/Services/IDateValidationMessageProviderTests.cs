using Dfe.ManageSchoolImprovement.Frontend.Services;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public sealed class IDateValidationMessageProviderTests
    {
        private sealed class DefaultImpl : IDateValidationMessageProvider
        {
            // Intentionally empty to use interface default implementations
        }

        private static DefaultImpl CreateProvider()
        {
            return new DefaultImpl();
        }

        [Fact]
        public void Defaults_ShouldReturnExpectedStaticMessages()
        {
            IDateValidationMessageProvider provider = CreateProvider();

            Assert.Equal("Enter a date in the correct format", provider.DefaultMessage);
            Assert.Equal("Month must be between 1 and 12", provider.MonthOutOfRange);
            Assert.Equal("Year must be between 2000 and 2050", provider.YearOutOfRange);
            Assert.Equal("Enter a date", provider.AllMissing);
        }

        [Theory]
        [InlineData(new[] { "day" }, "Date must include a day")]
        [InlineData(new[] { "day", "month" }, "Date must include a day and month")]
        [InlineData(new[] { "day", "month", "year" }, "Date must include a day and month and year")]
        public void SomeMissing_ShouldJoinMissingPartsWithAnd(string[] missingParts, string expected)
        {
            IDateValidationMessageProvider provider = CreateProvider();

            var message = provider.SomeMissing("ignored-display-name", missingParts);

            Assert.Equal(expected, message);
        }

        [Theory]
        [InlineData(28)]
        [InlineData(29)]
        [InlineData(30)]
        [InlineData(31)]
        public void DayOutOfRange_ShouldReturnBoundedMessage(int daysInMonth)
        {
            IDateValidationMessageProvider provider = CreateProvider();

            var message = provider.DayOutOfRange(daysInMonth);

            Assert.Equal($"Day must be between 1 and {daysInMonth}", message);
        }

        [Fact]
        public void ContextSpecificValidation_Default_ShouldReturnTrueAndEmptyMessage()
        {
            IDateValidationMessageProvider provider = CreateProvider();

            var (valid, message) = provider.ContextSpecificValidation(1, 1, 2000);

            Assert.True(valid);
            Assert.Equal(string.Empty, message);
        }
    }
}
