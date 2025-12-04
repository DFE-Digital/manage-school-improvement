using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;
using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public class TrustServiceTests
    {
        [Fact]
        public async Task SearchTrusts_UsesGroupNameQuery_WhenSearchIsNotNumeric()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<TrustService>>();

            string? capturedPath = null;
            var httpClientServiceMock = new Mock<IHttpClientService>();
            var items = new List<TrustSearchResponse>
            {
                new() { Name = "Acme Trust", Ukprn = "10000001" },
                new() { Name = "Acme Education", Ukprn = "10000002" }
            };
            var body = new TrustListResponse<TrustSearchResponse> { Data = items };
            httpClientServiceMock
                .Setup(s => s.Get<TrustListResponse<TrustSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Callback<HttpClient, string>((_, path) => capturedPath = path)
                .ReturnsAsync(new ApiResponse<TrustListResponse<TrustSearchResponse>>(HttpStatusCode.OK, body));

            var service = new TrustService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.SearchTrusts("  Acme Trust  ");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("/v4/trusts?groupName=Acme%20Trust", capturedPath);
        }

        [Fact]
        public async Task SearchTrusts_UsesUkprnQuery_WhenSearchIsNumeric()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<TrustService>>();

            string? capturedPath = null;
            var httpClientServiceMock = new Mock<IHttpClientService>();
            var items = new List<TrustSearchResponse> { new() { Name = "Numeric Trust", Ukprn = "12345678" } };
            var body = new TrustListResponse<TrustSearchResponse> { Data = items };

            httpClientServiceMock
                .Setup(s => s.Get<TrustListResponse<TrustSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .Callback<HttpClient, string>((_, path) => capturedPath = path)
                .ReturnsAsync(new ApiResponse<TrustListResponse<TrustSearchResponse>>(HttpStatusCode.OK, body));

            var service = new TrustService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.SearchTrusts("12345678");

            // Assert
            Assert.Single(result);
            Assert.Equal("/v4/trusts?ukPrn=12345678", capturedPath);
        }

        [Fact]
        public async Task SearchTrusts_ReturnsEmpty_WhenBodyIsNull()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<TrustService>>();

            var httpClientServiceMock = new Mock<IHttpClientService>();
            httpClientServiceMock
                .Setup(s => s.Get<TrustListResponse<TrustSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<TrustListResponse<TrustSearchResponse>>(HttpStatusCode.OK, null!));

            var service = new TrustService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.SearchTrusts("anything");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchTrusts_ReturnsEmpty_WhenDataIsNull()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<TrustService>>();

            var httpClientServiceMock = new Mock<IHttpClientService>();
            var body = new TrustListResponse<TrustSearchResponse> { Data = null };
            httpClientServiceMock
                .Setup(s => s.Get<TrustListResponse<TrustSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<TrustListResponse<TrustSearchResponse>>(HttpStatusCode.OK, body));

            var service = new TrustService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.SearchTrusts("anything");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchTrusts_Throws_WhenApiCallFails()
        {
            // Arrange
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://fakeapi.com") };
            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<TrustService>>();

            var httpClientServiceMock = new Mock<IHttpClientService>();
            httpClientServiceMock
                .Setup(s => s.Get<TrustListResponse<TrustSearchResponse>>(It.IsAny<HttpClient>(), It.IsAny<string>()))
                .ReturnsAsync(new ApiResponse<TrustListResponse<TrustSearchResponse>>(HttpStatusCode.InternalServerError, null!));

            var service = new TrustService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ApiResponseException>(() => service.SearchTrusts("fail"));
        }

        [Fact]
        public async Task GetTrustByUkprn_ReturnsTrust_WhenApiCallIsSuccessful()
        {
            // Arrange
            var ukprn = "12345678";
            var expected = new TrustDto { Name = "Test Trust", ReferenceNumber = "TRUST123" };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().EndsWith($"/v4/trust/{ukprn}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expected)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://fakeapi.com") };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<TrustService>>();
            var httpClientServiceMock = new Mock<IHttpClientService>(); // not used by this method

            var service = new TrustService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.GetTrustByUkprn(ukprn);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Name, result.Name);
            Assert.Equal(expected.ReferenceNumber, result.ReferenceNumber);
        }

        [Fact]
        public async Task GetTrustByUkprn_ReturnsEmptyAndLogsWarning_WhenApiCallFails()
        {
            // Arrange
            var ukprn = "12345678";

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

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<TrustService>>();
            var httpClientServiceMock = new Mock<IHttpClientService>(); // not used by this method

            var service = new TrustService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.GetTrustByUkprn(ukprn);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TrustDto>(result);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unable to get trust data for trust with UKPRN")),
                    null,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}

