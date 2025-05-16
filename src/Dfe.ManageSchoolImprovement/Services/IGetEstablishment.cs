using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public interface IGetEstablishment
{
    Task<DfE.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto> GetEstablishmentByUrn(string urn);
    Task<MISEstablishmentResponse> GetEstablishmentOfstedDataByUrn(string urn);
    Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery);
}
