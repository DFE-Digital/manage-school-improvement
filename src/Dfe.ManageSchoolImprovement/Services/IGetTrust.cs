using Dfe.ManageSchoolImprovement.Frontend.Models;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public interface IGetTrust
{
    Task<IEnumerable<TrustSearchResponse>> SearchTrusts(string searchQuery);
    
    Task<TrustDto> GetTrustByUkprn(string ukprn);
}