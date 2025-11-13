using Microsoft.AspNetCore.Routing;
using GovUK.Dfe.CoreLibs.Testing.Authorization;
using GovUK.Dfe.CoreLibs.Testing.Mocks.WebApplicationFactory;
using Microsoft.Extensions.DependencyInjection;
using GovUK.Dfe.CoreLibs.Testing.Authorization.Helpers;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages
{
    public class PageSecurityTests
    {
        private readonly AuthorizationTester _validator;
        private static readonly Lazy<IEnumerable<RouteEndpoint>> _endpoints = new(InitializeEndpoints);
        private const bool _globalAuthorizationEnabled = true;

        public PageSecurityTests()
        {
            _validator = new AuthorizationTester(_globalAuthorizationEnabled);
        }

        public static TheoryData<string, string> PageSecurityTestData
        {
            get
            {
                var theoryData = new TheoryData<string, string>();
                var configFilePath = "ExpectedSecurityConfig.json";
                var pages = EndpointTestDataProvider.GetPageSecurityTestDataFromFile(configFilePath, _endpoints.Value, _globalAuthorizationEnabled);
            
                foreach (var page in pages)
                {
                    theoryData.Add((string)page[0], (string)page[1]);
                }

                return theoryData;
            }
        }

        [Theory]
        [MemberData(nameof(PageSecurityTestData))]
        public void ValidatePageSecurity(string route, string expectedSecurity)
        {
            var result = _validator.ValidatePageSecurity(route, expectedSecurity, _endpoints.Value);
            Assert.Null(result.Message);
        }

        private static IEnumerable<RouteEndpoint> InitializeEndpoints()
        {
            // Using a temporary factory to access the EndpointDataSource for lazy initialization
            var factory = new CustomWebApplicationFactory<Program>();
            var endpointDataSource = factory.Services.GetRequiredService<EndpointDataSource>();

            var endpoints = endpointDataSource.Endpoints
               .OfType<RouteEndpoint>()
               .Where(x => !x.RoutePattern.RawText!.Contains("MicrosoftIdentity") &&
                           !x.RoutePattern.RawText.Equals("/") &&
                           !x.Metadata.Any(m => m is RouteNameMetadata && ((RouteNameMetadata)m).RouteName == "default"));

            return endpoints;
        }
    }
}
