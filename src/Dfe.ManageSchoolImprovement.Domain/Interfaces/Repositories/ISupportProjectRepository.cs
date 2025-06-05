using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories
{
    public interface ISupportProjectRepository : IRepository<SupportProject>
    {
        Task<(IEnumerable<SupportProject> projects, int totalCount)> SearchForSupportProjects(
            string? title,
            IEnumerable<string>? states,
            IEnumerable<string>? assignedUsers,
            IEnumerable<string>? regions,
            IEnumerable<string>? localAuthorities,
            int page,
            int count,
            CancellationToken cancellationToken);

        Task<IEnumerable<string>> GetAllProjectRegions(CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllProjectLocalAuthorities(CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetAllProjectAssignedUsers(CancellationToken cancellationToken);

        Task<SupportProject?> GetSupportProjectById(SupportProjectId id, CancellationToken cancellationToken);
    }
}
