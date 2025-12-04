using System.Web;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;

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
    
    public async Task<TrustDto> GetTrustByUkprn(string ukprn)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/v4/trust/{HttpUtility.UrlEncode(ukprn)}");
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Unable to get trust data for trust with UKPRN: {Ukprn}", ukprn);
            return new TrustDto();
        }

        return await response.Content.ReadFromJsonAsync<TrustDto>() ?? throw new InvalidOperationException();
    }
}
