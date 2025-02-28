using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Dfe.ManageSchoolImprovement.Api.ResponseModels
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
