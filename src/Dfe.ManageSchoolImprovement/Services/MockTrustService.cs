using Dfe.ManageSchoolImprovement.Frontend.Models;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class MockTrustService : IGetTrust
{
    public Task<IEnumerable<TrustSearchResponse>> SearchTrusts(string searchQuery)
    {
        var results = new List<TrustSearchResponse>
        {
            new TrustSearchResponse
            {
                Ukprn = "10012345",
                Name = "Danfield Academy Trust"
            },
            new TrustSearchResponse
            {
                Ukprn = "10054321",
                Name = "Springfield Learning Trust"
            },
            new TrustSearchResponse
            {
                Ukprn = "10098765",
                Name = "North London Education Trust"
            }
        };

        return Task.FromResult<IEnumerable<TrustSearchResponse>>(results);
    }

    public Task<TrustDto> GetTrustByUkprn(string ukprn)
    {
        var trust = new TrustDto
        {
            Ukprn = ukprn,
            Name = "Danfield Academy Trust",
            CompaniesHouseNumber = "09876543",
            ReferenceNumber = "TRUST-001",
            Type = new NameAndCodeDto
            {
                Name = "Multi-academy trust",
                Code = "MAT"
            },
            Address = new AddressDto
            {
                Street = "123 Education Road",
                Locality = "Westminster",
                Town = "London",
                County = "Greater London",
                Postcode = "SW1A 1AA",
                Additional = "Floor 2, Education House"
            }
        };

        return Task.FromResult(trust);
    }
}