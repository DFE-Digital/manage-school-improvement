using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Dfe.ManageSchoolImprovement.Frontend.Authorization;

[ExcludeFromCodeCoverage]
//Handler is registered from the method RequireAuthenticatedUser()
public class HeaderRequirementHandler(IHostEnvironment environment,
                                IHttpContextAccessor httpContextAccessor,
                                IConfiguration configuration) : AuthorizationHandler<DenyAnonymousAuthorizationRequirement>,
   IAuthorizationRequirement
{

    /// <summary>
    ///    Checks for a value in Authorization header of the request
    ///    If this matches the Secret, Authorization is granted on Dev/Staging
    /// </summary>
    /// <param name="hostEnvironment">Environment</param>
    /// <param name="httpContextAccessor">Used to check header bearer token value </param>
    /// <param name="configuration">Used to access secret value</param>
    /// <returns>True if secret and header value match</returns>
    public static bool ClientSecretHeaderValid(IHostEnvironment hostEnvironment,
                                               IHttpContextAccessor httpContextAccessor,
                                               IConfiguration configuration)
    {
        //Header authorisation not applicable for production - and include Test - (Thanks Paddy Clark!)
        if (!hostEnvironment.IsStaging() && !hostEnvironment.IsEnvironment("Test") && !hostEnvironment.IsDevelopment())
        {
            return false;
        }

        //Allow client secret in header
        string authHeader = httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString()
           .Replace("Bearer ", string.Empty);

        string secret = configuration.GetValue<string>("CypressTestSecret");

        if (string.IsNullOrWhiteSpace(authHeader) || string.IsNullOrWhiteSpace(secret))
        {
            return false;
        }

        return authHeader == secret;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   DenyAnonymousAuthorizationRequirement requirement)
    {
        if (ClientSecretHeaderValid(environment, httpContextAccessor, configuration))
        {
            context.Succeed(requirement);
            string headerRole = httpContextAccessor.HttpContext?.Request.Headers["AuthorizationRole"].ToString();
            if (!string.IsNullOrWhiteSpace(headerRole))
            {
                string[] claims = headerRole.Split(',');
                foreach (string claim in claims)
                {
                    context.User.Identities.FirstOrDefault()?.AddClaim(new Claim(ClaimTypes.Role, claim));
                }
            }
        }

        return Task.CompletedTask;
    }
}
