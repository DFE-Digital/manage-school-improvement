using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public interface IGetLocalAuthority
{
    Task<IEnumerable<NameAndCodeDto>> SearchLocalAuthorities(string searchQuery);
}