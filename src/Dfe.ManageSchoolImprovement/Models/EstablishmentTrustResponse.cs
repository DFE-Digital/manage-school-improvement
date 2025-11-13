using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;

namespace Dfe.ManageSchoolImprovement.Frontend.Models;

public class EstablishmentTrustResponse
{
    public string Urn { get; set; } = string.Empty;

    public TrustDto? Trust { get; set; }
}
