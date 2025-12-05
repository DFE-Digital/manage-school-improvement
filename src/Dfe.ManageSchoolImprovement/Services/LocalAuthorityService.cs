using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class LocalAuthorityService(IDfeHttpClientFactory httpClientFactory,
                            ILogger<LocalAuthorityService> logger,
                            IHttpClientService httpClientService) : IGetLocalAuthority
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateAcademiesClient();

    public async Task<IEnumerable<NameAndCodeDto>> SearchLocalAuthorities(string searchQuery)
    {
        string term = (searchQuery ?? string.Empty).Trim();

        string path = int.TryParse(term, out int code)
           ? $"/v4/local-authorities?code={code}"
           : $"/v4/local-authorities?name={Uri.EscapeDataString(term)}";

        ApiResponse<IEnumerable<NameAndCodeDto>> result = await httpClientService.Get<IEnumerable<NameAndCodeDto>>(_httpClient, path);

        if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

        return result.Body ?? [];
    }
}
