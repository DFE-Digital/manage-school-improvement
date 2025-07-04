
using Dfe.ManageSchoolImprovement.Frontend.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Authorization
{
    public class ClaimsRequirementHandlerTests
    {
        private readonly Mock<IHostEnvironment> _mockEnvironment;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly TestableClaimsRequirementHandler _handler;
        private readonly AuthorizationHandlerContext _authorizationHandlerContext;
        private readonly ClaimsAuthorizationRequirement _requirement;

        public ClaimsRequirementHandlerTests()
        {
            _mockEnvironment = new Mock<IHostEnvironment>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"CypressTestSecret", "valid-secret"}
                }!)
                .Build();

            _handler = new TestableClaimsRequirementHandler(_mockEnvironment.Object, _mockHttpContextAccessor.Object, _configuration);
            _requirement = new ClaimsAuthorizationRequirement("TestClaim", ["TestValue"]);
            _authorizationHandlerContext = new AuthorizationHandlerContext([_requirement], new ClaimsPrincipal(), null);
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

        private class TestableClaimsRequirementHandler(IHostEnvironment environment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : ClaimsRequirementHandler(environment, httpContextAccessor, configuration)
        {
            public Task TestHandleRequirementAsync(AuthorizationHandlerContext context, ClaimsAuthorizationRequirement requirement)
            {
                return HandleRequirementAsync(context, requirement);
            }
        }
    }
}
