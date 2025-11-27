using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class TrustService(IDfeHttpClientFactory httpClientFactory,
                            ILogger<TrustService> logger,
                            IHttpClientService httpClientService) : IGetTrust
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateAcademiesClient();

    public async Task<IEnumerable<TrustSearchResponse>> SearchTrusts(string searchQuery)
    {
        string term = (searchQuery ?? string.Empty).Trim();

        string path = int.TryParse(term, out int ukprn)
           ? $"/v4/trusts?ukPrn={ukprn}"
           : $"/v4/trusts?groupName={Uri.EscapeDataString(term)}";

        ApiResponse<TrustListResponse<TrustSearchResponse>> result = await httpClientService.Get<TrustListResponse<TrustSearchResponse>>(_httpClient, path);

        if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

        return result.Body?.Data ?? Enumerable.Empty<TrustSearchResponse>();
    }
}
