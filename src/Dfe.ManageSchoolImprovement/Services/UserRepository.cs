using Dfe.Academisation.ExtensionMethods;
using Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;
using User = Dfe.ManageSchoolImprovement.Frontend.Models.User;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class UserRepository(IGraphUserService graphUserService, IHostEnvironment environment) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        IEnumerable<Microsoft.Graph.User> users = await graphUserService.GetAllUsers();

        return users
           .Select(u => new User(u.Id, u.Mail, $"{u.GivenName} {u.Surname.ToFirstUpper()}"));
    }

    public async Task<IEnumerable<User>> GetAllRiseAdvisers()
    {
        IEnumerable<Microsoft.Graph.User> users = await graphUserService.GetAllRiseAdvisers();

        var result = users.Select(u => new User(u.Id, u.Mail, $"{u.GivenName} {u.Surname.ToFirstUpper().Replace("-rise", string.Empty)}"));

        // add a test user in if in dev and test for cypress testing
        if (environment.IsStaging() || environment.IsEnvironment("Test") || environment.IsDevelopment())
        {
            result = result.Append(new User(Guid.NewGuid().ToString(), "TestFirstName.TestSurname-rise@education.gov.uk", $"TestFirstName TestSurname"));
        }

        return result;
    }
}
