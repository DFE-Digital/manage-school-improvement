using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Authentication;

/// <summary>
/// Happy-path tests for the Entra ID (Microsoft.Identity.Web) sign-in setup, run against the real
/// application pipeline. Entra ID itself is not called: the OpenID Connect metadata is supplied
/// statically, and signed-in users are created with the app's real authentication cookie.
/// </summary>
public class AuthenticationFlowTests(AuthenticationFlowTests.AuthWebApplicationFactory factory)
    : IClassFixture<AuthenticationFlowTests.AuthWebApplicationFactory>
{
    private const string ProtectedPage = "/Errors";

    [Fact]
    public async Task Anonymous_user_is_redirected_to_entra_id_to_sign_in()
    {
        var client = factory.CreateClient(NoRedirects);

        var response = await client.GetAsync(ProtectedPage);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var location = response.Headers.Location!;
        Assert.Equal(AuthWebApplicationFactory.AuthorizationEndpoint, location.GetLeftPart(UriPartial.Path));

        var query = System.Web.HttpUtility.ParseQueryString(location.Query);
        Assert.Equal(AuthWebApplicationFactory.ClientId, query["client_id"]);
        Assert.Equal("https://localhost/signin-oidc", query["redirect_uri"]);
        Assert.Contains("openid", query["scope"]!.Split(' '));
    }

    [Fact]
    public async Task Signed_in_user_with_allowed_role_can_access_a_protected_page()
    {
        var client = await SignedInClient(role: "msi.edit");

        var response = await client.GetAsync(ProtectedPage);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Signed_in_user_without_allowed_role_is_sent_to_access_denied()
    {
        var client = await SignedInClient(role: "some.other.role");

        var response = await client.GetAsync(ProtectedPage);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/access-denied", response.Headers.Location!.AbsolutePath);
    }

    private static readonly WebApplicationFactoryClientOptions NoRedirects = new()
    {
        AllowAutoRedirect = false,
        BaseAddress = new Uri("https://localhost")
    };

    /// <summary>Returns a client holding the app's real auth cookie for a user with the given role.</summary>
    private async Task<HttpClient> SignedInClient(string role)
    {
        var client = factory.CreateClient(NoRedirects);
        var signIn = await client.GetAsync($"{AuthWebApplicationFactory.TestSignInPath}?role={role}");
        Assert.Equal(HttpStatusCode.OK, signIn.StatusCode);
        return client;
    }

    public class AuthWebApplicationFactory : WebApplicationFactory<Program>
    {
        public const string ClientId = "00000000-0000-0000-0000-00000000c11e";
        public const string TenantId = "00000000-0000-0000-0000-000000007e7";
        public const string TestSignInPath = "/__test/sign-in";
        public const string AuthorizationEndpoint = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/authorize";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Production: no developer/Cypress header bypass, so only real authentication applies.
            builder.UseEnvironment("Production");
            builder.UseSetting("AzureAd:Instance", "https://login.microsoftonline.com/");
            builder.UseSetting("AzureAd:TenantId", TenantId);
            builder.UseSetting("AzureAd:ClientId", ClientId);
            builder.UseSetting("AzureAd:ClientSecret", "test-secret");
            builder.UseSetting("AzureAd:AllowedRoles", "msi.edit");
            // Never opened by these tests; start-up just needs a value.
            builder.UseSetting("ConnectionStrings:DefaultConnection", "Server=unused;Database=unused;");

            builder.ConfigureTestServices(services =>
            {
                // Supply Entra ID metadata so no call is made to login.microsoftonline.com.
                services.PostConfigure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    var configuration = new OpenIdConnectConfiguration
                    {
                        Issuer = $"https://login.microsoftonline.com/{TenantId}/v2.0",
                        AuthorizationEndpoint = AuthorizationEndpoint,
                        TokenEndpoint = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token",
                        EndSessionEndpoint = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/logout"
                    };
                    options.Configuration = configuration;
                    options.ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(configuration);
                });

                // Session state lives in SQL Server; keep it in memory so these tests need no database.
                services.RemoveAll<IDistributedCache>();
                services.AddDistributedMemoryCache();

                // Stands in for the /signin-oidc callback: issues the app's real auth cookie.
                services.AddTransient<IStartupFilter, TestSignInStartupFilter>();
            });
        }
    }

    private sealed class TestSignInStartupFilter : IStartupFilter
    {
        // A plain path check rather than app.Map(), which would set PathBase and scope the cookie to this path.
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
        {
            app.Use(async (context, nextMiddleware) =>
            {
                if (context.Request.Path != AuthWebApplicationFactory.TestSignInPath)
                {
                    await nextMiddleware();
                    return;
                }

                var identity = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, "test.user@education.gov.uk"),
                    new Claim(ClaimTypes.Role, context.Request.Query["role"].ToString())
                ], CookieAuthenticationDefaults.AuthenticationScheme);

                // Identity.Web makes OpenIdConnect the default scheme, and it only accepts a cookie that it
                // issued itself. The real /signin-oidc callback marks the ticket like this, so do the same.
                var properties = new AuthenticationProperties();
                properties.Items[".AuthScheme"] = OpenIdConnectDefaults.AuthenticationScheme;

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
            });
            next(app);
        };
    }
}
