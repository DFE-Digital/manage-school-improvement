using Dfe.ManageSchoolImprovement.Application.Common.EventHandlers;
using Dfe.ManageSchoolImprovement.Domain.Common;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.Common.EventHandlers
{
    public class TestEvent : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }

    public class TestEventHandler(ILogger<BaseEventHandler<TestEvent>> logger) : BaseEventHandler<TestEvent>(logger)
    {
        public bool HandleEventCalled { get; private set; }
        public bool ShouldThrowException { get; set; }

        protected override Task HandleEvent(TestEvent notification, CancellationToken cancellationToken)
        {
            HandleEventCalled = true;

            if (ShouldThrowException)
            {
                throw new InvalidOperationException("Test exception");
            }

            return Task.CompletedTask;
        }
    }
    public class BaseEventHandlerTests
    {
        private readonly Mock<ILogger<BaseEventHandler<TestEvent>>> _mockLogger;
        private readonly TestEventHandler _handler;

        public BaseEventHandlerTests()
        {
            _mockLogger = new Mock<ILogger<BaseEventHandler<TestEvent>>>();
            _handler = new TestEventHandler(_mockLogger.Object);
        }

        [Fact]
        public async Task Handle_LogsInformation_WhenEventIsHandledSuccessfully()
        {
            // Arrange
            var testEvent = new TestEvent();

            // Act
            await _handler.Handle(testEvent, CancellationToken.None);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handling event: TestEvent")),
                    It.IsAny<Exception>(),
                    ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!),
                Times.Once);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Event handled successfully: TestEvent")),
                    It.IsAny<Exception>(),
                    ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!),
                Times.Once);
        }

        [Fact]
        public async Task Handle_LogsErrorAndRethrows_WhenExceptionOccurs()
        {
            // Arrange
            var testEvent = new TestEvent();
            _handler.ShouldThrowException = true;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(testEvent, CancellationToken.None)
            );

            Assert.Equal("Test exception", exception.Message);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error handling event: TestEvent")),
                    exception,
                    ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!),
                Times.Once);
        }

        [Fact]
        public async Task Handle_CallsHandleEvent()
        {
            // Arrange
            var testEvent = new TestEvent();

            // Act
            await _handler.Handle(testEvent, CancellationToken.None);

            // Assert
            Assert.True(_handler.HandleEventCalled);
        }
    }
}
