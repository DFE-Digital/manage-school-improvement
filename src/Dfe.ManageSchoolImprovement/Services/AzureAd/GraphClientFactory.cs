using Dfe.ManageSchoolImprovement.Frontend.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;

public class GraphClientFactory(IOptions<AzureAdOptions> azureAdOptions) : IGraphClientFactory
{
    private readonly AzureAdOptions _azureAdOptions = azureAdOptions.Value;

    public GraphServiceClient Create()
    {
        IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_azureAdOptions.ClientId.ToString())
            .WithClientSecret(_azureAdOptions.ClientSecret)
            .WithAuthority(new Uri(_azureAdOptions.Authority))
            .Build();

        var authenticationProvider = new BaseBearerTokenAuthenticationProvider(
            new ClientCredentialsTokenProvider(app, _azureAdOptions.Scopes));

        return new GraphServiceClient(authenticationProvider, $"{_azureAdOptions.ApiUrl}/V1.0");
    }

    /// <summary>Acquires an app-only token with MSAL, exactly as the Graph v4 DelegateAuthenticationProvider did.</summary>
    private sealed class ClientCredentialsTokenProvider(IConfidentialClientApplication app, IEnumerable<string> scopes)
        : IAccessTokenProvider
    {
        public AllowedHostsValidator AllowedHostsValidator { get; } = new();

        public async Task<string> GetAuthorizationTokenAsync(Uri uri,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default)
        {
            AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync(cancellationToken);
            return result.AccessToken;
        }
    }
}
