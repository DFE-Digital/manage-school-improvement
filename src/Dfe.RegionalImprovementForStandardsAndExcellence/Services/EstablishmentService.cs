﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Dfe.RegionalImprovementForStandardsAndExcellence.Application.Common.Exceptions;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Models;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Services.Http;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.Extensions.Logging;


namespace Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Services;

public class EstablishmentService : IGetEstablishment
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger<EstablishmentService> _logger;

    public EstablishmentService(IDfeHttpClientFactory httpClientFactory,
                                ILogger<EstablishmentService> logger,
                                IHttpClientService httpClientService)
    {
        _httpClient = httpClientFactory.CreateAcademiesClient();
        _logger = logger;
        _httpClientService = httpClientService;
    }

    public async Task<EstablishmentDto> GetEstablishmentByUrn(string urn)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/v4/establishment/urn/{urn}");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Unable to get establishment data for establishment with URN: {urn}", urn);
            return new EstablishmentDto();
        }

        return await response.Content.ReadFromJsonAsync<EstablishmentDto>();
    }

    public async Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery)
    {
        string path = int.TryParse(searchQuery, out int urn)
           ? $"/v4/establishments?urn={urn}"
           : $"/v4/establishments?name={searchQuery}";

        ApiResponse<IEnumerable<EstablishmentSearchResponse>> result = await _httpClientService.Get<IEnumerable<EstablishmentSearchResponse>>(_httpClient, path);

        if (result.Success is false) throw new ApiResponseException($"Request to Api failed | StatusCode - {result.StatusCode}");

        return result.Body;
    }
}
