using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories
{
    public interface ISupportProjectRepository : IRepository<SupportProject>
    {
        Task<(IEnumerable<SupportProject> projects, int totalCount)> SearchForSupportProjects(
            SupportProjectSearchCriteria searchCriteria,
            int page,
            int count,
            CancellationToken cancellationToken);

        Task<IEnumerable<string>> GetAllProjectRegions(CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllProjectLocalAuthorities(CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllProjectAssignedUsers(CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllProjectAssignedAdvisers(CancellationToken cancellationToken);

        Task<SupportProject?> GetSupportProjectById(SupportProjectId id, CancellationToken cancellationToken);
        Task<SupportProject?> GetSupportProjectByUrn(string urn, CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllProjectTrusts(CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllProjectYears(CancellationToken cancellationToken);
        Task<IEnumerable<KeyValuePair<string, string>>> GetAllProjectStatuses(CancellationToken cancellationToken);
    }
}
