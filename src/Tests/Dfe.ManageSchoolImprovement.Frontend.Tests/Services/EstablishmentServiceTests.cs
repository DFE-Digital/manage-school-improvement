using AutoFixture;
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
    public class EstablishmentServiceTests
    {
        [Fact]
        public async Task GetEstablishmentOfstedDataByUrn_ReturnsData_WhenApiCallIsSuccessful()
        {
            var fixture = new Fixture();

            // Arrange
            var urn = "123456";
            var expectedResponse = fixture.Create<MisEstablishmentResponse>();
            var establishmentResponse = new EstablishmentResponse { MisEstablishment = expectedResponse };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString().Contains($"/establishment/urn/{urn}")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(establishmentResponse)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<EstablishmentService>>();
            var httpClientServiceMock = new Mock<IHttpClientService>(); // not used in this method

            var service = new EstablishmentService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.GetEstablishmentOfstedDataByUrn(urn);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expectedResponse, result);
        }

        [Fact]
        public async Task GetEstablishmentOfstedDataByUrn_ReturnsEmptyResponse_WhenApiCallFails()
        {
            // Arrange
            var urn = "123456";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<EstablishmentService>>();
            var httpClientServiceMock = new Mock<IHttpClientService>();

            var service = new EstablishmentService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.GetEstablishmentOfstedDataByUrn(urn);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MisEstablishmentResponse>(result);
        }

        [Fact]
        public async Task GetEstablishmentTrust_ReturnsExpectedTrust_WhenApiCallIsSuccessful()
        {
            var fixture = new Fixture();

            // Arrange
            var urn = "123456";
            var expectedTrust = new TrustDto 
            { 
                Name = "Test Trust", 
                ReferenceNumber = "TR12345"
            };
            var apiResponse = new Dictionary<int, TrustDto> { { 123456, expectedTrust } };
            var mockApiResponse = new ApiResponse<Dictionary<int, TrustDto>>(HttpStatusCode.OK, apiResponse);

            var httpClientServiceMock = new Mock<IHttpClientService>();
            httpClientServiceMock
                .Setup(x => x.Post<object, Dictionary<int, TrustDto>>(
                    It.IsAny<HttpClient>(),
                    "/v4/trusts/establishments/urns",
                    It.IsAny<object>()))
                .ReturnsAsync(mockApiResponse);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<EstablishmentService>>();

            var service = new EstablishmentService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.GetEstablishmentTrust(urn);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTrust.Name, result.Name);
            Assert.Equal(expectedTrust.ReferenceNumber, result.ReferenceNumber);
        }

        [Fact]
        public async Task GetEstablishmentTrust_ReturnsNull_WhenTrustNotFound()
        {
            // Arrange
            var urn = "123456";
            var emptyResponse = new Dictionary<int, TrustDto>();
            var mockApiResponse = new ApiResponse<Dictionary<int, TrustDto>>(HttpStatusCode.OK, emptyResponse);

            var httpClientServiceMock = new Mock<IHttpClientService>();
            httpClientServiceMock
                .Setup(x => x.Post<object, Dictionary<int, TrustDto>>(
                    It.IsAny<HttpClient>(),
                    "/v4/trusts/establishments/urns",
                    It.IsAny<object>()))
                .ReturnsAsync(mockApiResponse);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<EstablishmentService>>();

            var service = new EstablishmentService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.GetEstablishmentTrust(urn);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEstablishmentTrust_ReturnsNull_WhenApiReturns404()
        {
            // Arrange
            var urn = "123456";
            var mockApiResponse = new ApiResponse<Dictionary<int, TrustDto>>(HttpStatusCode.NotFound, null!);

            var httpClientServiceMock = new Mock<IHttpClientService>();
            httpClientServiceMock
                .Setup(x => x.Post<object, Dictionary<int, TrustDto>>(
                    It.IsAny<HttpClient>(),
                    "/v4/trusts/establishments/urns",
                    It.IsAny<object>()))
                .ReturnsAsync(mockApiResponse);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<EstablishmentService>>();

            var service = new EstablishmentService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            var result = await service.GetEstablishmentTrust(urn);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEstablishmentTrust_ThrowsApiResponseException_WhenApiReturnsError()
        {
            // Arrange
            var urn = "123456";
            var mockApiResponse = new ApiResponse<Dictionary<int, TrustDto>>(HttpStatusCode.InternalServerError, null!);

            var httpClientServiceMock = new Mock<IHttpClientService>();
            httpClientServiceMock
                .Setup(x => x.Post<object, Dictionary<int, TrustDto>>(
                    It.IsAny<HttpClient>(),
                    "/v4/trusts/establishments/urns",
                    It.IsAny<object>()))
                .ReturnsAsync(mockApiResponse);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<EstablishmentService>>();

            var service = new EstablishmentService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiResponseException>(() => service.GetEstablishmentTrust(urn));
            Assert.Contains("InternalServerError", exception.Message);
        }

        [Fact]
        public async Task GetEstablishmentTrust_ThrowsFormatException_WhenUrnIsNotNumeric()
        {
            // Arrange
            var invalidUrn = "invalid-urn";

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<EstablishmentService>>();
            var httpClientServiceMock = new Mock<IHttpClientService>();

            var service = new EstablishmentService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => service.GetEstablishmentTrust(invalidUrn));
        }

        [Fact]
        public async Task GetEstablishmentTrust_SendsCorrectPayload_WhenCalled()
        {
            // Arrange
            var urn = "123456";
            var expectedTrust = new TrustDto { Name = "Test Trust", ReferenceNumber = "TR12345" };
            var apiResponse = new Dictionary<int, TrustDto> { { 123456, expectedTrust } };
            var mockApiResponse = new ApiResponse<Dictionary<int, TrustDto>>(HttpStatusCode.OK, apiResponse);

            object? capturedPayload = null;
            var httpClientServiceMock = new Mock<IHttpClientService>();
            httpClientServiceMock
                .Setup(x => x.Post<object, Dictionary<int, TrustDto>>(
                    It.IsAny<HttpClient>(),
                    "/v4/trusts/establishments/urns",
                    It.IsAny<object>()))
                .Callback<HttpClient, string, object>((client, path, payload) => capturedPayload = payload)
                .ReturnsAsync(mockApiResponse);

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var httpClientFactoryMock = new Mock<IDfeHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateAcademiesClient()).Returns(httpClient);

            var loggerMock = new Mock<ILogger<EstablishmentService>>();

            var service = new EstablishmentService(httpClientFactoryMock.Object, loggerMock.Object, httpClientServiceMock.Object);

            // Act
            await service.GetEstablishmentTrust(urn);

            // Assert
            Assert.NotNull(capturedPayload);
            var payloadJson = System.Text.Json.JsonSerializer.Serialize(capturedPayload);
            Assert.Contains("123456", payloadJson);
        }
    }
}