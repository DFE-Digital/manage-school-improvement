using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class GetLocalAuthoritiesCacheDecorator(IGetLocalAuthority getLocalAuthority, IHttpContextAccessor httpContextAccessor) : IGetLocalAuthority
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

    public Task<IEnumerable<NameAndCodeDto>> SearchLocalAuthorities(string searchQuery)
    {
        string key = $"local-authorities-{searchQuery}";
        if (_httpContext.Items.ContainsKey(key) && _httpContext.Items[key] is IEnumerable<NameAndCodeDto> cached)
        {
            return Task.FromResult(cached);
        }
        Task<IEnumerable<NameAndCodeDto>> localAuthorities = getLocalAuthority.SearchLocalAuthorities(searchQuery);

        _httpContext.Items[key] = localAuthorities;

        return localAuthorities;
    }
}
