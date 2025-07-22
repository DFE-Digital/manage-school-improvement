using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.Factories;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using DfE.CoreLibs.Contracts.Academies.V4;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Queries
{
    public class SupportProjectQueryService(ISupportProjectRepository supportProjectRepository, IMapper mapper) : ISupportProjectQueryService
    {
        public async Task<Result<IEnumerable<SupportProjectDto>>> GetAllSupportProjects(CancellationToken cancellationToken)
        {
            var supportProjects = await supportProjectRepository.FetchAsync(sp => true, cancellationToken);

            var result = supportProjects.Select(x => mapper.Map<SupportProjectDto>(x)).ToList();

            return Result<IEnumerable<SupportProjectDto>>.Success(result);
        }

        public async Task<Result<PagedDataResponse<SupportProjectDto>?>> SearchForSupportProjects(
            string? title,
            IEnumerable<string>? states,
            IEnumerable<string>? assignedUsers,
            IEnumerable<string>? assignedAdvisers,
            IEnumerable<string>? regions,
            IEnumerable<string>? localAuthorities,
            IEnumerable<string>? trusts,
            string pagePath,
            int page,
            int count,
            CancellationToken cancellationToken)
        {
            var (projects, totalCount) = await supportProjectRepository.SearchForSupportProjects(
                title, 
                states, 
                assignedUsers, 
                assignedAdvisers, 
                regions, 
                localAuthorities,
                trusts,
                page, 
                count, 
                cancellationToken);

            var pageResponse = PagingResponseFactory.Create(pagePath, page, count, totalCount, []);

            var result = projects.Select(x => mapper.Map<SupportProjectDto>(x)).ToList();

            return Result<PagedDataResponse<SupportProjectDto>?>.Success(new PagedDataResponse<SupportProjectDto>(result,
                pageResponse));
        }

        public async Task<Result<SupportProjectDto?>> GetSupportProject(int id, CancellationToken cancellationToken)
        {
            var supportProjectId = new SupportProjectId(id);
            var supportProject = await supportProjectRepository.GetSupportProjectById(supportProjectId, cancellationToken);

            var result = mapper.Map<SupportProjectDto?>(supportProject);

            return result == null ? Result<SupportProjectDto?>.Failure("") : Result<SupportProjectDto?>.Success(result);
        }

        public async Task<Result<IEnumerable<string>>> GetAllProjectLocalAuthorities(CancellationToken cancellationToken)
        {
            var result = await supportProjectRepository.GetAllProjectLocalAuthorities(cancellationToken);
            return result == null ? Result<IEnumerable<string>>.Failure("") : Result<IEnumerable<string>>.Success(result);
        }

        public async Task<Result<IEnumerable<string>>> GetAllProjectRegions(CancellationToken cancellationToken)
        {
            var result = await supportProjectRepository.GetAllProjectRegions(cancellationToken);
            return result == null ? Result<IEnumerable<string>>.Failure("") : Result<IEnumerable<string>>.Success(result);
        }

        public async Task<Result<IEnumerable<string>>> GetAllProjectAssignedUsers(CancellationToken cancellationToken)
        {
            var result = await supportProjectRepository.GetAllProjectAssignedUsers(cancellationToken);
            return result == null ? Result<IEnumerable<string>>.Failure("") : Result<IEnumerable<string>>.Success(result);
        }
        
        public async Task<Result<IEnumerable<string>>> GetAllProjectAssignedAdvisers(CancellationToken cancellationToken)
        {
            var result = await supportProjectRepository.GetAllProjectAssignedAdvisers(cancellationToken);
            return result == null ? Result<IEnumerable<string>>.Failure("") : Result<IEnumerable<string>>.Success(result);
        }

        public async Task<Result<IEnumerable<string>>> GetAllProjectTrusts(CancellationToken cancellationToken)
        {
            var result = await supportProjectRepository.GetAllProjectTrusts(cancellationToken);
            return result == null ? Result<IEnumerable<string>>.Failure("") : Result<IEnumerable<string>>.Success(result);
        }
    }
}
