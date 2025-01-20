﻿namespace Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Services.Http;

public interface IDfeHttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates an http client pointing to the trams/academies api, with correlation context headers configured
    /// </summary>
    /// <returns></returns>
    HttpClient CreateAcademiesClient();
}
