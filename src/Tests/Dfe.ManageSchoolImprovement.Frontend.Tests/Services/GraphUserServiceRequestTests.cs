using System.Net;
using System.Text;
using System.Web;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services;

/// <summary>
/// Pins the Microsoft Graph request GraphUserService sends, and how it reads the response,
/// using a fake HTTP handler (no call to Graph).
/// </summary>
public class GraphUserServiceRequestTests
{
    private static readonly Guid GroupId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid RiseAdviserGroupId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private const string GroupMembersResponse = """
        {"value":[
          {"@odata.type":"#microsoft.graph.user","id":"u1","givenName":"Ann","surname":"Smith","mail":"ann.smith@education.gov.uk"},
          {"@odata.type":"#microsoft.graph.user","id":"u2","givenName":"Bob","surname":"Jones","mail":"bob.jones@education.gov.uk"}
        ]}
        """;

    [Theory]
    [InlineData(nameof(GraphUserService.GetAllUsers), "11111111-1111-1111-1111-111111111111")]
    [InlineData(nameof(GraphUserService.GetAllRiseAdvisers), "22222222-2222-2222-2222-222222222222")]
    public async Task Requests_group_members_and_returns_users(string method, string expectedGroupId)
    {
        var handler = new RecordingHandler(GroupMembersResponse);
        var service = CreateService(handler);

        var users = method == nameof(GraphUserService.GetAllUsers)
            ? await service.GetAllUsers()
            : await service.GetAllRiseAdvisers();

        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal($"/V1.0/groups/{expectedGroupId}/members", request.RequestUri!.AbsolutePath.Replace("//", "/"));

        var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
        Assert.Equal("true", query["$count"]);
        Assert.Equal("999", query["$top"]);
        Assert.Equal("givenName,surname,id,mail", query["$select"]);
        Assert.Equal("eventual", Assert.Single(request.Headers.GetValues("ConsistencyLevel")));
        Assert.Equal("Bearer test-token", request.Headers.Authorization!.ToString());

        Assert.Collection(users,
            u => Assert.Equal(("u1", "Ann", "Smith", "ann.smith@education.gov.uk"), (u.Id, u.GivenName, u.Surname, u.Mail)),
            u => Assert.Equal(("u2", "Bob", "Jones", "bob.jones@education.gov.uk"), (u.Id, u.GivenName, u.Surname, u.Mail)));
    }

    [Fact]
    public async Task Returns_no_users_when_group_is_empty()
    {
        var service = CreateService(new RecordingHandler("""{"value":[]}"""));

        Assert.Empty(await service.GetAllUsers());
    }

    private static GraphUserService CreateService(HttpMessageHandler handler)
    {
        var options = new AzureAdOptions { GroupId = GroupId, RiseAdviserGroupId = RiseAdviserGroupId };
        var client = new GraphServiceClient(
            new HttpClient(handler),
            new BaseBearerTokenAuthenticationProvider(new FakeTokenProvider()),
            $"{options.ApiUrl}/V1.0"); // same base URL as GraphClientFactory

        var factory = new Mock<IGraphClientFactory>();
        factory.Setup(f => f.Create()).Returns(client);
        return new GraphUserService(factory.Object, Options.Create(options));
    }

    private sealed class RecordingHandler(string responseJson) : HttpMessageHandler
    {
        public List<HttpRequestMessage> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });
        }
    }

    private sealed class FakeTokenProvider : IAccessTokenProvider
    {
        public AllowedHostsValidator AllowedHostsValidator { get; } = new();

        public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default) => Task.FromResult("test-token");
    }
}
