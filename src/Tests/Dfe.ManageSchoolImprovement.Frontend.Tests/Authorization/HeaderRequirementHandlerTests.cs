using Dfe.ManageSchoolImprovement.Frontend.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Authorization
{
    public class HeaderRequirementHandlerTests
    {
        private readonly Mock<IHostEnvironment> _mockEnvironment;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly TestableHeaderRequirementHandler _handler;
        private readonly AuthorizationHandlerContext _authorizationHandlerContext;
        private readonly DenyAnonymousAuthorizationRequirement _requirement;

        public HeaderRequirementHandlerTests()
        {
            _mockEnvironment = new Mock<IHostEnvironment>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"CypressTestSecret", "valid-secret"}
                }!)
                .Build();

            _handler = new TestableHeaderRequirementHandler(_mockEnvironment.Object, _mockHttpContextAccessor.Object, _configuration);
            _requirement = new DenyAnonymousAuthorizationRequirement();
            _authorizationHandlerContext = new AuthorizationHandlerContext(new[] { _requirement }, new ClaimsPrincipal(), null);
        }

        [Fact]
        public async Task HandleRequirementAsync_ShouldSucceed_WhenClientSecretHeaderValid()
        {
            // Arrange
            _mockEnvironment.Setup(env => env.EnvironmentName).Returns("Development");
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = "Bearer valid-secret";
            _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(context);
 
            // Act
            await _handler.TestHandleRequirementAsync(_authorizationHandlerContext, _requirement);

            // Assert
            Assert.True(_authorizationHandlerContext.HasSucceeded);
        }

        [Fact]
        public async Task HandleRequirementAsync_ShouldNotSucceed_WhenClientSecretHeaderInvalid()
        {
            // Arrange
            _mockEnvironment.Setup(env => env.EnvironmentName).Returns("Development");
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = "Bearer invalid-secret";
            _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(context); 

            // Act
            await _handler.TestHandleRequirementAsync(_authorizationHandlerContext, _requirement);

            // Assert
            Assert.False(_authorizationHandlerContext.HasSucceeded);
        }

        [Fact]
        public void ClientSecretHeaderValid_ShouldReturnFalse_WhenNotInDevOrStaging()
        {
            // Arrange
            _mockEnvironment.Setup(env => env.EnvironmentName).Returns("Production");

            // Act
            var result = HeaderRequirementHandler.ClientSecretHeaderValid(_mockEnvironment.Object, _mockHttpContextAccessor.Object, _configuration);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ClientSecretHeaderValid_ShouldReturnFalse_WhenHeaderOrSecretIsEmpty()
        {
            // Arrange
            _mockEnvironment.Setup(env => env.EnvironmentName).Returns("Development");
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = string.Empty;
            _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(context);
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"CypressTestSecret", ""}
                }!)
                .Build(); 

            // Act
            var result = HeaderRequirementHandler.ClientSecretHeaderValid(_mockEnvironment.Object, _mockHttpContextAccessor.Object, configuration);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ClientSecretHeaderValid_ShouldReturnTrue_WhenHeaderAndSecretMatch()
        {
            // Arrange
            _mockEnvironment.Setup(env => env.EnvironmentName).Returns("Development");
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = "Bearer valid-secret";
            _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(context); 

            // Act
            var result = HeaderRequirementHandler.ClientSecretHeaderValid(_mockEnvironment.Object, _mockHttpContextAccessor.Object, _configuration);

            // Assert
            Assert.True(result);
        }

        private class TestableHeaderRequirementHandler(IHostEnvironment environment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : HeaderRequirementHandler(environment, httpContextAccessor, configuration)
        {
            public Task TestHandleRequirementAsync(AuthorizationHandlerContext context, DenyAnonymousAuthorizationRequirement requirement)
            {
                return HandleRequirementAsync(context, requirement);
            }
        }
    }
}
