using System.Net;
using System.Text;
using System.Web;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services;

/// <summary>
/// Covers GraphClientFactory without calling Entra ID: MSAL's HTTP traffic goes to a fake token endpoint.
/// </summary>
public class GraphClientFactoryTests
{
    private static readonly AzureAdOptions Options = new()
    {
        ClientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        ClientSecret = "test-secret",
        TenantId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
    };

    [Fact]
    public void Create_returns_a_client_for_the_configured_graph_endpoint()
    {
        var client = new GraphClientFactory(Microsoft.Extensions.Options.Options.Create(Options)).Create();

        Assert.Equal("https://graph.microsoft.com//V1.0", client.RequestAdapter.BaseUrl);
    }

    [Fact]
    public async Task Token_provider_acquires_an_app_only_token_using_client_credentials()
    {
        var entraId = new FakeEntraId();
        var factory = new GraphClientFactory(Microsoft.Extensions.Options.Options.Create(Options))
        {
            MsalHttpClientFactory = entraId
        };

        var token = await factory.CreateTokenProvider()
            .GetAuthorizationTokenAsync(new Uri("https://graph.microsoft.com/v1.0/groups"));

        Assert.Equal("fake-access-token", token);

        var tokenRequest = Assert.Single(entraId.TokenRequests);
        Assert.Equal($"https://login.microsoftonline.com/{Options.TenantId}/oauth2/v2.0/token", tokenRequest.Url);
        Assert.Equal("client_credentials", tokenRequest.Form["grant_type"]);
        Assert.Equal(Options.ClientId.ToString(), tokenRequest.Form["client_id"]);
        Assert.Equal("test-secret", tokenRequest.Form["client_secret"]);
        Assert.Equal("https://graph.microsoft.com/.default", tokenRequest.Form["scope"]);
    }

    /// <summary>Answers MSAL's instance-discovery and token requests like Entra ID would.</summary>
    private sealed class FakeEntraId : HttpMessageHandler, IMsalHttpClientFactory
    {
        public List<(string Url, System.Collections.Specialized.NameValueCollection Form)> TokenRequests { get; } = [];

        public HttpClient GetHttpClient() => new(this, disposeHandler: false);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var url = request.RequestUri!.GetLeftPart(UriPartial.Path);

            if (url.EndsWith("/oauth2/v2.0/token", StringComparison.OrdinalIgnoreCase))
            {
                var body = await request.Content!.ReadAsStringAsync(cancellationToken);
                TokenRequests.Add((url, HttpUtility.ParseQueryString(body)));
                return Json("""{"token_type":"Bearer","expires_in":3600,"access_token":"fake-access-token"}""");
            }

            // Instance discovery: confirm login.microsoftonline.com is a known Entra ID host.
            return Json("""
                {"tenant_discovery_endpoint":"https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration",
                 "metadata":[{"preferred_network":"login.microsoftonline.com","preferred_cache":"login.windows.net",
                              "aliases":["login.microsoftonline.com","login.windows.net"]}]}
                """);
        }

        private static HttpResponseMessage Json(string json) =>
            new(HttpStatusCode.OK) { Content = new StringContent(json, Encoding.UTF8, "application/json") };
    }
}
