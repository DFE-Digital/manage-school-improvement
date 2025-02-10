using Microsoft.AspNetCore.Http;

namespace DfE.ManageSchoolImprovement.Infrastructure.Security
{
    public interface IUserContextService
    {
        string GetCurrentUsername();
    }

    public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
    {
        public string GetCurrentUsername()
        {
            return httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System"; // Default to "System" if null
        }
    }

}
