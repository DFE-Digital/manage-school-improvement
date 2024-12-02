//using Asp.Versioning;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Application.Common.Models;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Application.Schools.Commands.CreateReport;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Application.Schools.Commands.CreateSchool;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Application.Schools.Queries.GetPrincipalBySchool;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Application.Schools.Queries.GetPrincipalsBySchools;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.ValueObjects;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;
//using System.Net;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Application.Common.Exceptions;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Infrastructure.Security.Configurations;

//namespace Dfe.RegionalImprovementForStandardsAndExcellence.Api.Controllers
//{
//    [ApiController]
//    [ApiVersion("1.0")]
//    [Route("v{version:apiVersion}/[controller]")]
//    public class SchoolsController(ISender sender) : ControllerBase
//    {
//        /// <summary>
//        /// Retrieve Principal by school name
//        /// </summary>
//        /// <param name="schoolName">The school name.</param>
//        /// <param name="cancellationToken">The cancellation token.</param>
//        [Authorize(Policy = PolicyNames.CanRead)]
//        [HttpGet("{schoolName}/principal")]
//        [SwaggerResponse(200, "A Person object representing the Principal.", typeof(Principal))]
//        [SwaggerResponse(404, "School not found.")]
//        [SwaggerResponse(400, "School cannot be null or empty.")]
//        public async Task<IActionResult> GetPrincipalBySchoolAsync([FromRoute] string schoolName, CancellationToken cancellationToken)
//        {
//            var result = await sender.Send(new GetPrincipalBySchoolQuery(schoolName), cancellationToken);

//            return !result.IsSuccess ? NotFound(new CustomProblemDetails(HttpStatusCode.NotFound, result.Error)) : Ok(result.Value);
//        }

//        /// <summary>
//        /// Retrieve a collection of principals by a collection of school names
//        /// </summary>
//        /// <param name="request">The request.</param>
//        /// <param name="cancellationToken">The cancellation token.</param>
//        [Authorize(Policy = PolicyNames.CanRead)]
//        [HttpPost("principals")]
//        [SwaggerResponse(200, "A collection of Principal objects.", typeof(IEnumerable<Principal>))]
//        [SwaggerResponse(400, "School names cannot be null or empty.")]
//        public async Task<IActionResult> GetPrincipalsBySchoolsAsync([FromBody] GetPrincipalsBySchoolsQuery request, CancellationToken cancellationToken)
//        {
//            var result = await sender.Send(request, cancellationToken);

//            return !result.IsSuccess ? NotFound(new CustomProblemDetails(HttpStatusCode.NotFound, result.Error)) : Ok(result.Value);
//        }

//        /// <summary>
//        /// Creates a new School along with the Principal Details
//        /// </summary>
//        /// <param name="request">The request.</param>
//        /// <param name="cancellationToken">The cancellation token.</param>
//        [Authorize(Policy = PolicyNames.CanReadWrite)]
//        [HttpPost]
//        [SwaggerResponse(201, "School created successfully.", typeof(SchoolId))]
//        [SwaggerResponse(400, "Invalid request data.")]
//        public async Task<IActionResult> CreateSchoolAsync([FromBody] CreateSchoolCommand request, CancellationToken cancellationToken)
//        {
//            var schoolId = await sender.Send(request, cancellationToken);
//            return Created("", schoolId);
//        }

//        /// <summary>
//        /// An example endpoint to trigger a background task
//        /// </summary>
//        /// <param name="request">The request.</param>
//        /// <param name="cancellationToken">The cancellation token.</param>
//        [AllowAnonymous]
//        [HttpPost("createReport")]
//        [SwaggerResponse(200, "Task queued successfully.", typeof(bool))]
//        [SwaggerResponse(400, "Invalid request data.")]
//        public async Task<IActionResult> CreateReportAsync([FromBody] CreateReportCommand request, CancellationToken cancellationToken)
//        {
//            var result = await sender.Send(request, cancellationToken);
//            return Ok(result);
//        }
//    }
//}