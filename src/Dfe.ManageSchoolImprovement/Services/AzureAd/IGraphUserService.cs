using User = Microsoft.Graph.User;

namespace Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;

public interface IGraphUserService
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<IEnumerable<User>> GetAllRiseAdvisers();
}
