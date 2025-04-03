using Dfe.ManageSchoolImprovement.Application.Common.Behaviours;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.Common.Behaviours
{
    public class PerformanceBehaviourTests
    {
        private readonly Mock<ILogger<TestRequest>> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly PerformanceBehaviour<TestRequest, TestResponse> _performanceBehaviour;
        private readonly Mock<RequestHandlerDelegate<TestResponse>> _mockNext;

        public PerformanceBehaviourTests()
        {
            _mockLogger = new Mock<ILogger<TestRequest>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockNext = new Mock<RequestHandlerDelegate<TestResponse>>();
            _performanceBehaviour = new PerformanceBehaviour<TestRequest, TestResponse>(_mockLogger.Object, _mockHttpContextAccessor.Object);
        }

        //[Fact]
        //public async Task Handle_ReturnsResponse_WhenElapsedTimeIsUnderThreshold()
        //{
        //    // Arrange
        //    var response = new TestResponse();
        //    _mockNext.Setup(n => n()).ReturnsAsync(response);

        //    // Act
        //    var result = await _performanceBehaviour.Handle(new TestRequest(), _mockNext.Object, CancellationToken.None);

        //    // Assert
        //    Assert.Equal(response, result);
        //    _mockLogger.Verify(
        //        x => x.Log(
        //            It.IsAny<LogLevel>(),
        //            It.IsAny<EventId>(),
        //            It.IsAny<It.IsAnyType>(),
        //            It.IsAny<Exception>(),
        //            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
        //        Times.Never);
        //}

        [Fact]
        public async Task Handle_LogsWarning_WhenElapsedTimeExceedsThreshold()
        {
            // Arrange
            var response = new TestResponse();
            _mockNext.Setup(n => n()).Returns(async () =>
            {
                await Task.Delay(600); // Simulate long-running task
                return response;
            });

            // Act
            var result = await _performanceBehaviour.Handle(new TestRequest(), _mockNext.Object, CancellationToken.None);

            // Assert
            Assert.Equal(response, result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Long Running Request")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        //[Fact]
        //public async Task Handle_LogsWarning_WithIdentityName()
        //{
        //    // Arrange
        //    var response = new TestResponse();
        //    _mockNext.Setup(n => n()).Returns(async () =>
        //    {
        //        await Task.Delay(600); // Simulate long-running task
        //        return response;
        //    });

        //    var user = new ClaimsPrincipal(new ClaimsIdentity(
        //    [
        //        new Claim(ClaimTypes.Name, "test.user")
        //    ], "mock"));
        //    var context = new DefaultHttpContext { User = user };

        //    _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(context);

        //    // Act
        //    var result = await _performanceBehaviour.Handle(new TestRequest(), _mockNext.Object, CancellationToken.None);

        //    // Assert
        //    Assert.Equal(response, result);
        //    _mockLogger.Verify(
        //        x => x.Log(
        //            LogLevel.Warning,
        //            It.IsAny<EventId>(),
        //            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("test.user")),
        //            It.IsAny<Exception>(),
        //            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
        //        Times.Once);
        //}

        [Fact]
        public async Task Handle_LogsWarning_WithoutIdentityName()
        {
            // Arrange
            var response = new TestResponse();
            _mockNext.Setup(n => n()).Returns(async () =>
            {
                await Task.Delay(600); // Simulate long-running task
                return response;
            });

            var context = new DefaultHttpContext(); // No user set
            _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(context);

            // Act
            var result = await _performanceBehaviour.Handle(new TestRequest(), _mockNext.Object, CancellationToken.None);

            // Assert
            Assert.Equal(response, result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Long Running Request")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
