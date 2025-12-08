using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public interface IGetLocalAuthority
{
    Task<NameAndCodeDto> GetLocalAuthorityByCode(string code);
    Task<IEnumerable<NameAndCodeDto>> SearchLocalAuthorities(string searchQuery);
}