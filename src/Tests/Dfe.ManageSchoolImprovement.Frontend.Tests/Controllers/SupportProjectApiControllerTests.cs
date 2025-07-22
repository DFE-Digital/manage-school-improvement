using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.DeleteSupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Controllers;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Net;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Controllers;

public class SupportProjectApiControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IHostEnvironment> _mockEnvironment;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly SupportProjectApiController _controller;
    private readonly Fixture _fixture;

    public SupportProjectApiControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockEnvironment = new Mock<IHostEnvironment>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockConfiguration = new Mock<IConfiguration>();
        _fixture = new Fixture();

        _controller = new SupportProjectApiController(
            _mockMediator.Object,
            _mockEnvironment.Object,
            _mockHttpContextAccessor.Object,
            _mockConfiguration.Object
        );
    }

    [Fact]
    public async Task DeleteSupportProject_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var schoolUrn = "123456";
        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteSupportProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        SetupValidCypressAuth();

        // Act
        var result = await _controller.DeleteSupportProject(schoolUrn, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockMediator.Verify(m => m.Send(It.Is<DeleteSupportProjectCommand>(cmd => 
            cmd.SchoolUrn == schoolUrn), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSupportProject_ProjectNotFound_ReturnsNotFound()
    {
        // Arrange
        var schoolUrn = "999999";
        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteSupportProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        SetupValidCypressAuth();

        // Act
        var result = await _controller.DeleteSupportProject(schoolUrn, CancellationToken.None);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);
    }

    [Fact]
    public async Task DeleteSupportProject_UnauthorizedAccess_ReturnsUnauthorized()
    {
        // Arrange
        var schoolUrn = "123456";
        SetupInvalidCypressAuth();

        // Act
        var result = await _controller.DeleteSupportProject(schoolUrn, CancellationToken.None);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal((int)HttpStatusCode.Unauthorized, objectResult.StatusCode);
        _mockMediator.Verify(m => m.Send(It.IsAny<DeleteSupportProjectCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteSupportProject_ProductionEnvironment_ReturnsForbidden()
    {
        // Arrange
        var schoolUrn = "123456";
        _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Production");

        // Act
        var result = await _controller.DeleteSupportProject(schoolUrn, CancellationToken.None);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal((int)HttpStatusCode.Forbidden, objectResult.StatusCode);
        _mockMediator.Verify(m => m.Send(It.IsAny<DeleteSupportProjectCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteSupportProject_StagingEnvironment_ReturnsForbidden()
    {
        // Arrange
        var schoolUrn = "123456";
        _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Staging");

        // Act
        var result = await _controller.DeleteSupportProject(schoolUrn, CancellationToken.None);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal((int)HttpStatusCode.Forbidden, objectResult.StatusCode);
        _mockMediator.Verify(m => m.Send(It.IsAny<DeleteSupportProjectCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteSupportProject_TestEnvironment_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var schoolUrn = "123456";
        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteSupportProjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        SetupValidCypressAuthForTestEnvironment();

        // Act
        var result = await _controller.DeleteSupportProject(schoolUrn, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockMediator.Verify(m => m.Send(It.Is<DeleteSupportProjectCommand>(cmd => 
            cmd.SchoolUrn == schoolUrn), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSupportProject_MediatorThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var schoolUrn = "123456";
        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteSupportProjectCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Test exception"));

        SetupValidCypressAuth();

        // Act
        var result = await _controller.DeleteSupportProject(schoolUrn, CancellationToken.None);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
    }

    private void SetupValidCypressAuth()
    {
        _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
    
        var mockHttpContext = new Mock<HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        var mockHeaders = new HeaderDictionary();  // Use actual HeaderDictionary instead of mock
    
        mockHeaders.Add("Authorization", "Bearer test-secret");
        mockRequest.Setup(r => r.Headers).Returns(mockHeaders);
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
    
        // Ensure HttpContext is never null
        _mockHttpContextAccessor.Setup(a => a.HttpContext)
            .Returns(mockHttpContext.Object);
    
        _mockConfiguration.Setup(x => x["CypressTestSecret"])
            .Returns("test-secret");
    }

    private void SetupInvalidCypressAuth()
    {
        _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
        
        var mockHttpContext = new Mock<HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        var mockHeaders = new Mock<IHeaderDictionary>();
        
        mockHeaders.Setup(h => h.Authorization).Returns("Bearer invalid-secret");
        mockRequest.Setup(r => r.Headers).Returns(mockHeaders.Object);
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);
        
        _mockConfiguration.Setup(x => x["CypressTestSecret"]).Returns("test-secret");
    }

    private void SetupValidCypressAuthForTestEnvironment()
    {
        _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Test");
        
        var mockHttpContext = new Mock<HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        var mockHeaders = new Mock<IHeaderDictionary>();
        
        mockHeaders.Setup(h => h.Authorization).Returns("Bearer test-secret");
        mockRequest.Setup(r => r.Headers).Returns(mockHeaders.Object);
        mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);
        
        _mockConfiguration.Setup(x => x["CypressTestSecret"]).Returns("test-secret");
    }
} 
