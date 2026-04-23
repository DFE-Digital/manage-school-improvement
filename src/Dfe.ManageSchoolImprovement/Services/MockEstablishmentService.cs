using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Services.Dtos;
using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class MockEstablishmentService : IGetEstablishment
{
    public Task<EstablishmentDto> GetEstablishmentByUrn(string urn)
    {
        var establishment = new EstablishmentDto
{
    Urn = "123456",
    Ukprn = "98765432",
    EstablishmentNumber = "12346",
    Name = "Danland Primary School",
    LocalAuthorityName = "Springfield Council",
    LocalAuthorityCode = "SPR01",

    OfstedLastInspection = "15/01/2024",
    OfstedRating = "Good",
    GiasLastChangedDate = "10/02/2025",

    HeadteacherTitle = "Mr",
    HeadteacherFirstName = "John",
    HeadteacherLastName = "Smith",
    HeadteacherPreferredJobTitle = "Headteacher",

    MainPhone = "02071234567",

    NoOfGirls = "150",
    NoOfBoys = "140",

    Pan = "60",
    SchoolCapacity = "420",

    SenUnitCapacity = "20",
    SenUnitOnRoll = "15",

    StatutoryLowAge = "4",
    StatutoryHighAge = "11",

    Deficit = "No",
    Pfi = "No",
    ViabilityIssue = "None",

    ReligousEthos = "Christian",

    Address = new AddressDto
    {
        Street = "1 School Lane",
        Locality = "Springfield",
        Town = "London",
        Postcode = "SW1A 1AA"
    },

    EstablishmentType = new NameAndCodeDto
    {
        Name = "Community School",
        Code = "CS"
    },

    PhaseOfEducation = new NameAndCodeDto
    {
        Name = "Primary",
        Code = "PRI"
    },

    ReligiousCharacter = new NameAndCodeDto
    {
        Name = "Church of England",
        Code = "CE"
    },

    Gor = new NameAndCodeDto
    {
        Name = "London",
        Code = "LON"
    },

    Diocese = new NameAndCodeDto
    {
        Name = "London Diocese",
        Code = "LDN"
    },

    ParliamentaryConstituency = new NameAndCodeDto
    {
        Name = "Westminster",
        Code = "WST"
    },

    Census = new CensusDto
    {
        NumberOfPupils = "290",
        PercentageFsm = "25",
        PercentageFsmLastSixYears = "30",
        PercentageEnglishAsSecondLanguage = "18",
        PercentageSen = "12"
    },

    MISEstablishment = new MisEstablishmentDto
    {
        DateOfLatestSection8Inspection = "12/03/2024",
        InspectionEndDate = "13/03/2024",
        OverallEffectiveness = "Good",
        QualityOfEducation = "Good",
        BehaviourAndAttitudes = "Outstanding",
        PersonalDevelopment = "Good",
        EffectivenessOfLeadershipAndManagement = "Good",
        EarlyYearsProvision = "Outstanding",
        SixthFormProvision = "Not applicable",
        Weblink = "https://reports.ofsted.gov.uk/provider/21/123456"
    },

    PreviousEstablishment = new PreviousEstablishmentDto
    {
        Urn = "111111",
    }
};
        return Task.FromResult(establishment);
    }

    public Task<MisEstablishmentResponse> GetEstablishmentOfstedDataByUrn(string urn)
    {
        var ofsted = new MisEstablishmentResponse
        {
            
            SiteName = "urn",
           
        };

        return Task.FromResult(ofsted);
    }

    public Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery)
    {
        var results = new List<EstablishmentSearchResponse>
        {
            new EstablishmentSearchResponse
            {
                Urn = "123456",
                Name = "Danland Primary School",
             
            },
            new EstablishmentSearchResponse
            {
                Urn = "654321",
                Name = "Danvillville High School",
            
            }
        };

        return Task.FromResult<IEnumerable<EstablishmentSearchResponse>>(results);
    }

    public Task<TrustDto?> GetEstablishmentTrust(string urn)
    {
        var trust = new TrustDto
        {
            Ukprn = "10012345",
            Name = "Danfield Academy Trust",
            CompaniesHouseNumber = "09876543",
            ReferenceNumber = "234124"
        };

        return Task.FromResult<TrustDto?>(trust);
    }
}