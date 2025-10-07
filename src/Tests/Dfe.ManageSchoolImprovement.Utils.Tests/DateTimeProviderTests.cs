using Moq;

namespace Dfe.ManageSchoolImprovement.Utils.Tests
{
    public class DateTimeProviderTests
    {
        private readonly DateTimeProvider _dateTimeProvider;

        public DateTimeProviderTests()
        {
            _dateTimeProvider = new DateTimeProvider();
        }

        [Fact]
        public void Now_ShouldReturnValidUtcDateTime()
        {
            // Act
            DateTime result = _dateTimeProvider.Now;

            // Assert
            Assert.Equal(DateTimeKind.Utc, result.Kind);
            Assert.True(result <= DateTime.UtcNow);
            Assert.True(result > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void MockDateTimeProvider_ShouldReturnMockedDateTime()
        {
            // Arrange: Mock or create a custom IDateTimeProvider implementation
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(provider => provider.Now).Returns(new DateTime(2025, 01, 01, 12, 0, 0, DateTimeKind.Utc));

            // Act
            DateTime result = mockDateTimeProvider.Object.Now;

            // Assert
            Assert.Equal(new DateTime(2025, 01, 01, 12, 0, 0, DateTimeKind.Utc), result);
        }
    }
}
