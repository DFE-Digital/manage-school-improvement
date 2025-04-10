using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace Dfe.ManageSchoolImprovement.Frontend.Authorization;

[ExcludeFromCodeCoverage]
public class ClaimsRequirementHandler(IHostEnvironment environment,
                                IHttpContextAccessor httpContextAccessor,
                                IConfiguration configuration) : AuthorizationHandler<ClaimsAuthorizationRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimsAuthorizationRequirement requirement)
    {
        if (HeaderRequirementHandler.ClientSecretHeaderValid(environment, httpContextAccessor, configuration))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
