using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using User = Microsoft.Graph.User;

namespace Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;

public interface IGraphUserService 
{
   Task<IEnumerable<User>> GetAllUsers();
}
