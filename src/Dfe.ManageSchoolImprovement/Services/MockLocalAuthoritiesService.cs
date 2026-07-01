using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class MockLocalAuthorityService : IGetLocalAuthority
{
    private readonly List<NameAndCodeDto> _localAuthorities =
    [
        new NameAndCodeDto
        {
            Code = "202",
            Name = "Camden"
        },
        new NameAndCodeDto
        {
            Code = "201",
            Name = "City of London"
        },
        new NameAndCodeDto
        {
            Code = "203",
            Name = "Westminster"
        },
        new NameAndCodeDto
        {
            Code = "204",
            Name = "Islington"
        }
    ];

    public Task<NameAndCodeDto> GetLocalAuthorityByCode(string code)
    {
        if (string.IsNullOrEmpty(code))
            return Task.FromResult(new NameAndCodeDto());

        var result = _localAuthorities
            .FirstOrDefault(x => x.Code == code);

        return Task.FromResult(result ?? new NameAndCodeDto());
    }

    public Task<IEnumerable<NameAndCodeDto>> SearchLocalAuthorities(string searchQuery)
    {
        var term = (searchQuery ?? string.Empty).Trim();

        IEnumerable<NameAndCodeDto> results;

        if (int.TryParse(term, out _))
        {
            results = _localAuthorities
                .Where(x => x.Code.Contains(term));
        }
        else
        {
            results = _localAuthorities
                .Where(x => x.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(results);
    }
}