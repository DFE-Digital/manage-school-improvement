using System.Collections.ObjectModel;
using GovUK.Dfe.PersonsApi.Client.Contracts;

namespace Dfe.ManageSchoolImprovement.Frontend;

public class MockEstablishmentsClient : IEstablishmentsClient
{
    private readonly HttpClient _httpClient;
    
    public MockEstablishmentsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public virtual async Task<MemberOfParliament> GetMemberOfParliamentBySchoolUrnAsync(int urn)
    {
        return new MemberOfParliament
        {
            Id = 123,
            FirstName = "Alice",
            LastName = "Johnson",
            ConstituencyName = "North Testshire",
            Email = "alice.johnson.mp@parliament.uk"
        };
    }

    public virtual async Task<ObservableCollection<AcademyGovernance>> GetAllPersonsAssociatedWithAcademyByUrnAsync(
        int urn,
        CancellationToken cancellationToken)
    {
        return new ObservableCollection<AcademyGovernance>
        {
            new AcademyGovernance
            {
                Id = 13213123,
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.org",
                Ukprn = "100001",
                Urn = urn,
                DateOfAppointment = "2021/09/01",
                DateTermOfOfficeEndsEnded = "2025/08/31",
                Roles = new List<string> { "Chimney Sweep" }
            },
            new AcademyGovernance
            {
                Id = 3452525,
                FirstName = "Michael",
                LastName = "Brown",
                Email = "michael.brown@example.org",
                Ukprn = "100002",
                Urn = urn,
                DisplayName = "Micky Brown",
                DateOfAppointment = "2022/03/15",
                DateTermOfOfficeEndsEnded = "2026/03/14",
                Roles = new List<string> { "Chair of Governors", "Chimney Sweep" }
            },
            new AcademyGovernance
            {
                Id = 4542535,
                FirstName = "Sarah",
                LastName = "Patel",
                Email = "sarah.patel@example.org",
                Ukprn = "100003",
                Urn = urn,
                DateOfAppointment = "2020/11/10",
                DateTermOfOfficeEndsEnded = "2024/11/09",
                Roles = new List<string> { "Accounting Officer", "Chimney Sweep" }
            }
        };
    }
    
    public virtual async Task<ObservableCollection<AcademyGovernance>> GetAllPersonsAssociatedWithAcademyByUrnAsync(int urn)
    {
        return new ObservableCollection<AcademyGovernance>
        {
            new AcademyGovernance
            {
                Id = 13213123,
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.org",
                Ukprn = "100001",
                Urn = urn,
                DateOfAppointment = "2021/09/01",
                DateTermOfOfficeEndsEnded = "2025/08/31",
                Roles = new List<string> { "Chimney Sweep" }
            },
            new AcademyGovernance
            {
                Id = 3452525,
                FirstName = "Michael",
                LastName = "Brown",
                Email = "michael.brown@example.org",
                Ukprn = "100002",
                Urn = urn,
                DateOfAppointment = "2022/03/15",
                DateTermOfOfficeEndsEnded = "2026/03/14",
                Roles = new List<string> { "Chair Of Governors", "Chimney Sweep" }
            },
            new AcademyGovernance
            {
                Id = 4542535,
                FirstName = "Sarah",
                LastName = "Patel",
                Email = "sarah.patel@example.org",
                Ukprn = "100003",
                Urn = urn,
                DateOfAppointment = "2020/11/10",
                DateTermOfOfficeEndsEnded = "2024/11/09",
                Roles = new List<string> { "Accounting Officer", "Chimney Sweep" }
            }
        };
    }
    
    
    public virtual async Task<MemberOfParliament> GetMemberOfParliamentBySchoolUrnAsync(
        int urn,
        CancellationToken cancellationToken)
    {
        return new MemberOfParliament
        {
            Id = 123,
            FirstName = "Alice",
            LastName = "Johnson",
            ConstituencyName = "North Testshire",
            Email = "alice.johnson.mp@parliament.uk"
        };
    }
}