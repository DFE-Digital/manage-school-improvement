using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public interface IGetEstablishment
{
    Task<GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto> GetEstablishmentByUrn(string urn);
    Task<MisEstablishmentResponse> GetEstablishmentOfstedDataByUrn(string urn);
    Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery);
    Task<TrustDto?> GetEstablishmentTrust(string urn);
}
