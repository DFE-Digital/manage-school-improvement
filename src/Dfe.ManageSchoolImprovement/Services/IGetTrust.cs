using Dfe.ManageSchoolImprovement.Frontend.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public interface IGetTrust
{
    Task<IEnumerable<TrustSearchResponse>> SearchTrusts(string searchQuery);
}