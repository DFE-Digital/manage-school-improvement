using Dfe.ManageSchoolImprovement.Application.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.Common.Behaviours
{
    public class UnhandledExceptionBehaviourTests
    {
        private readonly Mock<ILogger<TestRequest>> _mockLogger;
        private readonly UnhandledExceptionBehaviour<TestRequest, TestResponse> _unhandledExceptionBehaviour;
        private readonly Mock<RequestHandlerDelegate<TestResponse>> _mockNext;

        public UnhandledExceptionBehaviourTests()
        {
            _mockLogger = new Mock<ILogger<TestRequest>>();
            _mockNext = new Mock<RequestHandlerDelegate<TestResponse>>();
            _unhandledExceptionBehaviour = new UnhandledExceptionBehaviour<TestRequest, TestResponse>(_mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ReturnsResponse_WhenNoException()
        {
            // Arrange
            var response = new TestResponse();
            _mockNext.Setup(n => n()).ReturnsAsync(response);

            // Act
            var result = await _unhandledExceptionBehaviour.Handle(new TestRequest(), _mockNext.Object, CancellationToken.None);

            // Assert
            Assert.Equal(response, result);
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!),
                Times.Never);
        }

        [Fact]
        public async Task Handle_LogsError_WhenExceptionIsThrown()
        {
            // Arrange
            var exception = new InvalidOperationException("Test Exception");
            _mockNext.Setup(n => n()).ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _unhandledExceptionBehaviour.Handle(new TestRequest(), _mockNext.Object, CancellationToken.None)
            );

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unhandled Exception")),
                    exception,
                    ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!),
                Times.Once);
        }

        [Fact]
        public async Task Handle_RethrowsException_WhenExceptionIsThrown()
        {
            // Arrange
            var exception = new InvalidOperationException("Test Exception");
            _mockNext.Setup(n => n()).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _unhandledExceptionBehaviour.Handle(new TestRequest(), _mockNext.Object, CancellationToken.None)
            );

            Assert.Equal("Test Exception", ex.Message);
        }
    }
}
