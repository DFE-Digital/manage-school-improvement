using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using DfE.CoreLibs.Contracts.Academies.V4;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries
{
    public interface ISupportProjectQueryService
    {
        Task<Result<IEnumerable<SupportProjectDto>>> GetAllSupportProjects(CancellationToken cancellationToken);
        Task<Result<SupportProjectDto?>> GetSupportProject(int id, CancellationToken cancellationToken);
        Task<Result<PagedDataResponse<SupportProjectDto>?>> SearchForSupportProjects(
            string? title,
            IEnumerable<string>? states,
            IEnumerable<string>? assignedUsers,
            IEnumerable<string>? regions,
            IEnumerable<string>? localAuthorities,
            IEnumerable<string>? trusts,
            string pagePath,
            int page,
            int count,
            CancellationToken cancellationToken);

        Task<Result<IEnumerable<string>>> GetAllProjectLocalAuthorities(CancellationToken cancellationToken);
        Task<Result<IEnumerable<string>>> GetAllProjectRegions(CancellationToken cancellationToken);
        Task<Result<IEnumerable<string>>> GetAllProjectAssignedUsers(CancellationToken cancellationToken);
        Task<Result<IEnumerable<string>>> GetAllProjectTrusts(CancellationToken cancellationToken);
    }
}
