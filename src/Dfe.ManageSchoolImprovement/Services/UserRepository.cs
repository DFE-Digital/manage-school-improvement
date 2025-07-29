using Dfe.Academisation.ExtensionMethods;
using Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;
using User = Dfe.ManageSchoolImprovement.Frontend.Models.User;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class UserRepository(IGraphUserService graphUserService) : IUserRepository
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

        return users
           .Select(u => new User(u.Id, u.Mail, $"{u.GivenName} {u.Surname.ToFirstUpper().Replace("-rise", string.Empty)}"));
    }
}
