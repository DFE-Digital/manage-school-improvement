using Dfe.ManageSchoolImprovement.Application.Common.Exceptions;
using System.Net;

namespace Dfe.ManageSchoolImprovement.Application.Tests.Common.Exceptions
{
    public class CustomProblemDetailsTests
    {
        [Theory]
        [InlineData(HttpStatusCode.NotFound, "Not Found", "https://tools.ietf.org/html/rfc9110#section-15.5.5")]
        [InlineData(HttpStatusCode.Unauthorized, "Unauthorized", "https://tools.ietf.org/html/rfc7235#section-3.1")]
        [InlineData(HttpStatusCode.Forbidden, "Forbidden", "https://tools.ietf.org/html/rfc7231#section-6.5.3")]
        [InlineData(HttpStatusCode.BadRequest, "Bad Request", "https://tools.ietf.org/html/rfc7231#section-6.5.1")]
        [InlineData(HttpStatusCode.InternalServerError, "Internal Server Error", "https://tools.ietf.org/html/rfc7231#section-6.6.1")]
        [InlineData(HttpStatusCode.OK, "An error occurred", "https://tools.ietf.org/html/rfc7231#section-6.6.1")]
        [InlineData(HttpStatusCode.Created, "An error occurred", "https://tools.ietf.org/html/rfc7231#section-6.6.1")]
        [InlineData(HttpStatusCode.Accepted, "An error occurred", "https://tools.ietf.org/html/rfc7231#section-6.6.1")]
        public void Constructor_SetsPropertiesCorrectly_ForSupportedAndUnsupportedStatusCodes(
            HttpStatusCode statusCode, string expectedTitle, string expectedType)
        {
            // Arrange & Act
            var problemDetails = new CustomProblemDetails(statusCode);

            // Assert
            Assert.Equal((int)statusCode, problemDetails.Status);
            Assert.Equal(expectedTitle, problemDetails.Title);
            Assert.Equal(expectedType, problemDetails.Type);
            Assert.Null(problemDetails.Detail); // Detail should be null by default
        }

        [Fact]
        public void Constructor_SetsDetail_WhenProvided()
        {
            // Arrange
            var expectedDetail = "Some additional information";
            var statusCode = HttpStatusCode.BadRequest;

            // Act
            var problemDetails = new CustomProblemDetails(statusCode, expectedDetail);

            // Assert
            Assert.Equal(expectedDetail, problemDetails.Detail); // Detail should be set as provided
        }
    }
}
