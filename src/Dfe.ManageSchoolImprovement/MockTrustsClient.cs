using System.Collections.ObjectModel;
using GovUK.Dfe.PersonsApi.Client.Contracts;

namespace Dfe.ManageSchoolImprovement.Frontend
{
    public class MockTrustsClient : ITrustsClient
    {
        private readonly HttpClient _httpClient;
        
        public MockTrustsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public virtual async Task<ObservableCollection<TrustGovernance>> GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(string id)
        {
            return new ObservableCollection<TrustGovernance>
            {
                new TrustGovernance
                {
                    Id = 1001,
                    FirstName = "David",
                    LastName = "Miller",
                    Email = "david.miller@example.org",
                    Ukprn = "200001",
                    Trn = id,
                    DateOfAppointment = "2021/05/01",
                    DateTermOfOfficeEndsEnded = "2025/04/30",
                    Roles = new List<string> { "Chair of Governors", "Finance Committee" }
                },
                new TrustGovernance
                {
                    Id = 1002,
                    FirstName = "Emily",
                    LastName = "Taylor",
                    Email = "emily.taylor@example.org",
                    Ukprn = "200002",
                    Trn = id,
                    DateOfAppointment = "2022/01/15",
                    DateTermOfOfficeEndsEnded = "2026/01/14",
                    Roles = new List<string> { "Trustee" }
                },
                new TrustGovernance
                {
                    Id = 1003,
                    FirstName = "James",
                    LastName = "Wilson",
                    Email = "james.wilson@example.org",
                    Ukprn = "200003",
                    Trn = id,
                    DisplayName = "James Wilson",
                    DateOfAppointment = "2020/09/10",
                    DateTermOfOfficeEndsEnded = "2024/09/50",
                    Roles = new List<string> { "Accounting Officer", "Safeguarding Lead" }
                }
            };
        }

        public virtual async Task<ObservableCollection<TrustGovernance>> GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(
            string id,
            CancellationToken cancellationToken)
        {
            return new ObservableCollection<TrustGovernance>
            {
                new TrustGovernance
                {
                    Id = 1001,
                    FirstName = "David",
                    LastName = "Miller",
                    Email = "david.miller@example.org",
                    Ukprn = "200001",
                    Trn = id,
                    DateOfAppointment = "2021/05/01",
                    DateTermOfOfficeEndsEnded = "2025/04/30",
                    Roles = new List<string> { "Chair of Trustees", "Finance Committee" }
                },
                new TrustGovernance
                {
                    Id = 1002,
                    FirstName = "Emily",
                    LastName = "Taylor",
                    Email = "emily.taylor@example.org",
                    Ukprn = "200002",
                    Trn = id,
                    DateOfAppointment = "2022/01/15",
                    DateTermOfOfficeEndsEnded = "2026/01/14",
                    Roles = new List<string> { "Trustee" }
                },
                new TrustGovernance
                {
                    Id = 1003,
                    FirstName = "James",
                    LastName = "Wilson",
                    Email = "james.wilson@example.org",
                    Ukprn = "200003",
                    Trn = id,
                    DisplayName = "Jimmy  Wilson",
                    DateOfAppointment = "2020/09/10",
                    DateTermOfOfficeEndsEnded = "2024/09/50",
                    Roles = new List<string> { "Accounting Officer", "Safeguarding Lead" }
                }
            };
        }
    }
}