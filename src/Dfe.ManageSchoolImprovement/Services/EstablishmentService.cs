using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;
using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;
using DfE.CoreLibs.Contracts.Academies.V4.Trusts;


namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class EstablishmentService(IDfeHttpClientFactory httpClientFactory,
                            ILogger<EstablishmentService> logger,
                            IHttpClientService httpClientService) : IGetEstablishment
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateAcademiesClient();

    public async Task<EstablishmentDto> GetEstablishmentByUrn(string urn)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/v4/establishment/urn/{urn}");
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Unable to get establishment data for establishment with URN: {Urn}", urn);
            return new EstablishmentDto();
        }

        return await response.Content.ReadFromJsonAsync<EstablishmentDto>();
    }
    public async Task<MisEstablishmentResponse> GetEstablishmentOfstedDataByUrn(string urn)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/establishment/urn/{urn}");

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Unable to get establishment data for establishment with URN: {Urn}", urn);
            return new MisEstablishmentResponse();
        }

        var establishment = await response.Content.ReadFromJsonAsync<EstablishmentResponse>();

        return establishment?.MisEstablishment ?? new MisEstablishmentResponse();
    }

    public async Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery)
    {
        string path = int.TryParse(searchQuery, out int urn)
           ? $"/v4/establishments?urn={urn}&excludeClosed=true"
           : $"/v4/establishments?name={searchQuery}&excludeClosed=true";

        ApiResponse<IEnumerable<EstablishmentSearchResponse>> result = await httpClientService.Get<IEnumerable<EstablishmentSearchResponse>>(_httpClient, path);

        if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

        return result.Body;
    }
    public async Task<TrustDto> GetEstablishmentTrust(string urn)
    {
        string path = "/v4/trusts/establishments/urns";
        var payload = new { urns = new int[] { int.Parse(urn) } };

        ApiResponse<Dictionary<int, TrustDto>> result = await httpClientService.Post<object, Dictionary<int, TrustDto>>(_httpClient, path, payload);

        if (!result.Success) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

        return result.Body.FirstOrDefault()!.Value;
    }
}
