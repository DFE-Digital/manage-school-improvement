using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public class LocalAuthorityServiceTests
    {
        private readonly Mock<IDfeHttpClientFactory> _mockDfeHttpClientFactory;
        private readonly Mock<ILogger<LocalAuthorityService>> _mockLogger;
        private readonly Mock<IHttpClientService> _mockHttpClientService;

        public LocalAuthorityServiceTests()
        {
            _mockDfeHttpClientFactory = new Mock<IDfeHttpClientFactory>();
            _mockLogger = new Mock<ILogger<LocalAuthorityService>>();
            _mockHttpClientService = new Mock<IHttpClientService>();
        }

        private LocalAuthorityService CreateService()
        {
            return new LocalAuthorityService(
                _mockDfeHttpClientFactory.Object,
                _mockLogger.Object,
                _mockHttpClientService.Object);
        }

        #region GetLocalAuthorityByCode Tests

        [Fact]
        public async Task GetLocalAuthorityByCode_ReturnsLocalAuthority_WhenApiCallIsSuccessful()
        {
            // Arrange
            var code = "123";
            var expectedLocalAuthority = new NameAndCodeDto { Name = "Test Local Authority", Code = "123" };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().EndsWith($"/v4/local-authorities/123")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedLocalAuthority)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var service = CreateService();

            // Act
            var result = await service.GetLocalAuthorityByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLocalAuthority.Name, result.Name);
            Assert.Equal(expectedLocalAuthority.Code, result.Code);
        }

        [Fact]
        public async Task GetLocalAuthorityByCode_ReturnsEmptyDto_WhenApiCallFails()
        {
            // Arrange
            var code = "invalid";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var service = CreateService();

            // Act
            var result = await service.GetLocalAuthorityByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NameAndCodeDto>(result);

            // Verify warning was logged
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unable to get local auto data for local authority with code")),
                    null,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task GetLocalAuthorityByCode_EncodesCodeParameter_WhenContainsSpecialCharacters()
        {
            // Arrange
            var code = "test code&special";
            var expectedLocalAuthority = new NameAndCodeDto { Name = "Test Local Authority", Code = code };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.AbsolutePath.ToString().Contains("test%20code%26special")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedLocalAuthority)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var service = CreateService();

            // Act
            var result = await service.GetLocalAuthorityByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLocalAuthority.Name, result.Name);
        }

        [Fact]
        public async Task GetLocalAuthorityByCode_ThrowsInvalidOperationException_WhenResponseContentIsNull()
        {
            // Arrange
            var code = "123";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var service = CreateService();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetLocalAuthorityByCode(code));
        }

        #endregion

        #region SearchLocalAuthorities Tests

        [Fact]
        public async Task SearchLocalAuthorities_UsesNameQuery_WhenSearchIsNotNumeric()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            string? capturedPath = null;
            var localAuthorities = new List<NameAndCodeDto>
            {
                new() { Name = "Birmingham Council", Code = "330" },
                new() { Name = "Birmingham Education", Code = "331" }
            };

            _mockHttpClientService
                .Setup(s => s.Get<IEnumerable<NameAndCodeDto>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Callback<HttpClient, string>((_, path) => capturedPath = path)
                .ReturnsAsync(new ApiResponse<IEnumerable<NameAndCodeDto>>(HttpStatusCode.OK, localAuthorities));

            var service = CreateService();

            // Act
            var result = await service.SearchLocalAuthorities("  Birmingham  ");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("/v4/local-authorities?name=Birmingham", capturedPath);
        }

        [Fact]
        public async Task SearchLocalAuthorities_UsesCodeQuery_WhenSearchIsNumeric()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            string? capturedPath = null;
            var localAuthorities = new List<NameAndCodeDto>
            {
                new() { Name = "Test Council", Code = "123" }
            };

            _mockHttpClientService
                .Setup(s => s.Get<IEnumerable<NameAndCodeDto>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Callback<HttpClient, string>((_, path) => capturedPath = path)
                .ReturnsAsync(new ApiResponse<IEnumerable<NameAndCodeDto>>(HttpStatusCode.OK, localAuthorities));

            var service = CreateService();

            // Act
            var result = await service.SearchLocalAuthorities("123");

            // Assert
            Assert.Single(result);
            Assert.Equal("/v4/local-authorities?code=123", capturedPath);
        }

        [Fact]
        public async Task SearchLocalAuthorities_HandlesEmptySearchQuery_ReturnsEmptyResult()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            string? capturedPath = null;
            var localAuthorities = new List<NameAndCodeDto>();

            _mockHttpClientService
                .Setup(s => s.Get<IEnumerable<NameAndCodeDto>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Callback<HttpClient, string>((_, path) => capturedPath = path)
                .ReturnsAsync(new ApiResponse<IEnumerable<NameAndCodeDto>>(HttpStatusCode.OK, localAuthorities));

            var service = CreateService();

            // Act
            var result = await service.SearchLocalAuthorities("   ");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal("/v4/local-authorities?name=", capturedPath);
        }

        [Fact]
        public async Task SearchLocalAuthorities_HandlesNullSearchQuery_ReturnsEmptyResult()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            string? capturedPath = null;
            var localAuthorities = new List<NameAndCodeDto>();

            _mockHttpClientService
                .Setup(s => s.Get<IEnumerable<NameAndCodeDto>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Callback<HttpClient, string>((_, path) => capturedPath = path)
                .ReturnsAsync(new ApiResponse<IEnumerable<NameAndCodeDto>>(HttpStatusCode.OK, localAuthorities));

            var service = CreateService();

            // Act
            var result = await service.SearchLocalAuthorities(string.Empty);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal("/v4/local-authorities?name=", capturedPath);
        }

        [Fact]
        public async Task SearchLocalAuthorities_ReturnsEmpty_WhenBodyIsNull()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            _mockHttpClientService
                .Setup(s => s.Get<IEnumerable<NameAndCodeDto>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<IEnumerable<NameAndCodeDto>>(HttpStatusCode.OK, null!));

            var service = CreateService();

            // Act
            var result = await service.SearchLocalAuthorities("test");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchLocalAuthorities_ThrowsApiResponseException_WhenApiCallFails()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            _mockHttpClientService
                .Setup(s => s.Get<IEnumerable<NameAndCodeDto>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<IEnumerable<NameAndCodeDto>>(HttpStatusCode.InternalServerError, null!));

            var service = CreateService();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiResponseException>(() => service.SearchLocalAuthorities("test"));
            Assert.Contains("Request to Api failed | StatusCode - InternalServerError", exception.Message);
        }

        [Theory]
        [InlineData("Birmingham Council", "/v4/local-authorities?name=Birmingham%20Council")]
        [InlineData("Test & Special", "/v4/local-authorities?name=Test%20%26%20Special")]
        [InlineData("Council+Plus", "/v4/local-authorities?name=Council%2BPlus")]
        public async Task SearchLocalAuthorities_EscapesSpecialCharacters_InNameQuery(string searchTerm, string expectedPath)
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            string? capturedPath = null;
            var localAuthorities = new List<NameAndCodeDto>();

            _mockHttpClientService
                .Setup(s => s.Get<IEnumerable<NameAndCodeDto>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Callback<HttpClient, string>((_, path) => capturedPath = path)
                .ReturnsAsync(new ApiResponse<IEnumerable<NameAndCodeDto>>(HttpStatusCode.OK, localAuthorities));

            var service = CreateService();

            // Act
            await service.SearchLocalAuthorities(searchTerm);

            // Assert
            Assert.Equal(expectedPath, capturedPath);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("456789")]
        [InlineData("0")]
        public async Task SearchLocalAuthorities_TreatsNumericStringsAsCode(string numericSearch)
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            _mockDfeHttpClientFactory.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            string? capturedPath = null;
            var localAuthorities = new List<NameAndCodeDto>();

            _mockHttpClientService
                .Setup(s => s.Get<IEnumerable<NameAndCodeDto>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Callback<HttpClient, string>((_, path) => capturedPath = path)
                .ReturnsAsync(new ApiResponse<IEnumerable<NameAndCodeDto>>(HttpStatusCode.OK, localAuthorities));

            var service = CreateService();

            // Act
            await service.SearchLocalAuthorities(numericSearch);

            // Assert
            Assert.Equal($"/v4/local-authorities?code={numericSearch}", capturedPath);
        }

        #endregion
    }
}
