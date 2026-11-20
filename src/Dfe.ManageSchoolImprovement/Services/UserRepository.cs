using GovUK.Dfe.CoreLibs.Utilities.Extensions;
using Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;
using User = Dfe.ManageSchoolImprovement.Frontend.Models.User;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class UserRepository(IGraphUserService graphUserService, IHostEnvironment environment) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        IEnumerable<Microsoft.Graph.Models.User> users = await graphUserService.GetAllUsers();

        var result = users
           .Select(u => new User(u.Id ?? string.Empty, u.Mail ?? string.Empty, $"{u.GivenName} {FormatSurname(u.Surname)}"));

        // add a test user in if in dev and test for cypress testing
        if (environment.IsStaging() || environment.IsEnvironment("Test") || environment.IsDevelopment())
        {
            result = result.Append(new User(Guid.NewGuid().ToString(), "TestFirstName.TestSurname@education.gov.uk", $"TestFirstName TestSurname"));
        }
        
        return result;
    }

    public async Task<IEnumerable<User>> GetAllRiseAdvisers()
    {
        IEnumerable<Microsoft.Graph.Models.User> users = await graphUserService.GetAllRiseAdvisers();

        var result = users.Select(u => new User(u.Id ?? string.Empty, u.Mail ?? string.Empty, $"{u.GivenName} {FormatSurname(u.Surname).Replace("-rise", string.Empty)}"));

        // add a test user in if in dev and test for cypress testing
        if (environment.IsStaging() || environment.IsEnvironment("Test") || environment.IsDevelopment())
        {
            result = result.Append(new User(Guid.NewGuid().ToString(), "TestFirstName.TestSurname-rise@education.gov.uk", $"TestFirstName TestSurname"));
        }

        return result;
    }

    // Graph can return users without a surname; ToFirstUpper throws on null or empty input.
    private static string FormatSurname(string? surname) =>
        string.IsNullOrEmpty(surname) ? string.Empty : surname.ToFirstUpper();
}
