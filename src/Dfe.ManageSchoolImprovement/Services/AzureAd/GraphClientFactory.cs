using System.Net.Http.Headers;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

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

        DelegateAuthenticationProvider provider = new(async requestMessage =>
        {
            AuthenticationResult result = await app.AcquireTokenForClient(_azureAdOptions.Scopes).ExecuteAsync();
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        });

        return new GraphServiceClient($"{_azureAdOptions.ApiUrl}/V1.0/", provider);
    }
}
