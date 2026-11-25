using Dfe.ManageSchoolImprovement.Frontend.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using User = Microsoft.Graph.Models.User;

namespace Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;

public class GraphUserService(IGraphClientFactory graphClientFactory, IOptions<AzureAdOptions> azureAdOptions) : IGraphUserService
{
    private readonly AzureAdOptions _azureAdOptions = azureAdOptions.Value;
    private readonly GraphServiceClient _client = graphClientFactory.Create();

    public Task<IEnumerable<User>> GetAllUsers() => GetGroupMembers(_azureAdOptions.GroupId);

    public Task<IEnumerable<User>> GetAllRiseAdvisers() => GetGroupMembers(_azureAdOptions.RiseAdviserGroupId);

    private async Task<IEnumerable<User>> GetGroupMembers(Guid groupId)
    {
        var members = await _client.Groups[groupId.ToString()].Members.GetAsync(request =>
        {
            request.QueryParameters.Count = true;
            request.QueryParameters.Top = 999;
            request.QueryParameters.Select = ["givenName", "surname", "id", "mail"];
            request.Headers.Add("ConsistencyLevel", "eventual");
        });

        return members?.Value?.Cast<User>().ToList() ?? [];
    }
}
