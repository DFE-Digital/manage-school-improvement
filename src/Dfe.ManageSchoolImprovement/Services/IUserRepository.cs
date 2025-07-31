using Dfe.ManageSchoolImprovement.Frontend.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<IEnumerable<User>> GetAllRiseAdvisers();
}
