using AutoFixture;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;
using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
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
    }
}