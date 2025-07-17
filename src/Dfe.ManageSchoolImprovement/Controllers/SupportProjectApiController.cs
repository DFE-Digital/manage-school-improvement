using Dfe.ManageSchoolImprovement.Application.Common.Exceptions;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.DeleteSupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dfe.ManageSchoolImprovement.Frontend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SupportProjectApiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public SupportProjectApiController(
        IMediator mediator,
        IHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _mediator = mediator;
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    /// <summary>
    /// Delete a support project (hard delete - completely removes from database). 
    /// This endpoint is only available for Cypress test users in Test and Development environments.
    /// </summary>
    /// <param name="id">The ID of the support project to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>NoContent if successful, NotFound if project doesn't exist, Unauthorized if not authorized, Forbidden if wrong environment</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupportProject(int id, CancellationToken cancellationToken)
    {
        // Restrict this endpoint to only Test and Development environments
        if (!_environment.IsDevelopment() && !_environment.IsEnvironment("Test"))
        {
            return new ObjectResult(new CustomProblemDetails(HttpStatusCode.Forbidden, "This endpoint is only available in Test and Development environments"))
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
        }

        // Check if the request is authorized through the Cypress test secret
        if (!HeaderRequirementHandler.ClientSecretHeaderValid(_environment, _httpContextAccessor, _configuration))
        {
            return new ObjectResult(new CustomProblemDetails(HttpStatusCode.Unauthorized, "Not authorized to access this endpoint"))
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
        }

        try
        {
            var command = new DeleteSupportProjectCommand(
                new SupportProjectId(id)
            );

            var result = await _mediator.Send(command, cancellationToken);

            if (!result)
            {
                return new ObjectResult(new CustomProblemDetails(HttpStatusCode.NotFound, "Support project not found"))
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return new ObjectResult(new CustomProblemDetails(HttpStatusCode.InternalServerError, ex.Message))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
} 
