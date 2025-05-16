using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class GetEstablishmentItemCacheDecorator(IGetEstablishment getEstablishment, IHttpContextAccessor httpContextAccessor) : IGetEstablishment
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

    public async Task<EstablishmentDto> GetEstablishmentByUrn(string urn)
    {
        string key = $"establishment-{urn}";
        if (_httpContext.Items.ContainsKey(key) && _httpContext.Items[key] is EstablishmentDto cached)
        {
            return cached;
        }

        EstablishmentDto establishment = await getEstablishment.GetEstablishmentByUrn(urn);

        _httpContext.Items[key] = establishment;

        return establishment;
    }

    public async Task<MISEstablishmentResponse> GetEstablishmentOfstedDataByUrn(string urn)
    {
        string key = $"establishment-ofsted-{urn}";

        if (_httpContext.Items.ContainsKey(key) && _httpContext.Items[key] is MISEstablishmentResponse cached)
        {
            return cached;
        }

        MISEstablishmentResponse establishment = await getEstablishment.GetEstablishmentOfstedDataByUrn(urn);

        _httpContext.Items[key] = establishment;

        return establishment;
    }

    public Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery)
    {
        string key = $"establishments-{searchQuery}";
        if (_httpContext.Items.ContainsKey(key) && _httpContext.Items[key] is IEnumerable<EstablishmentSearchResponse> cached)
        {
            return Task.FromResult(cached);
        }
        Task<IEnumerable<EstablishmentSearchResponse>> establishments = getEstablishment.SearchEstablishments(searchQuery);

        _httpContext.Items[key] = establishments;

        return establishments;
    }
}
