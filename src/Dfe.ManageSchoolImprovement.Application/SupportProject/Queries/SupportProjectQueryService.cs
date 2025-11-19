using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.Factories;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using DfE.CoreLibs.Contracts.Academies.V4;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;

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
        
        public string[] AddAllSelectedMonths(IEnumerable<string>? Years, IEnumerable<string>? Months)
        {
            var monthsList = Months.ToList();
        
            foreach (var year in Years)
            {
                var hasMonthsForYear = Months.Any(month => month.StartsWith($"{year} "));
        
                if (!hasMonthsForYear)
                {
                    for (var month = 1; month <= 12; month++)
                    {
                        monthsList.Add($"{year} {month}");
                    }
                }
            }
        
            return monthsList.ToArray();
        }

        public async Task<Result<PagedDataResponse<SupportProjectDto>?>> SearchForSupportProjects(
            SupportProjectSearchRequest request,
            CancellationToken cancellationToken)
        {

            var dates = AddAllSelectedMonths(request.Years, request.Months);
            
            var (projects, totalCount) = await supportProjectRepository.SearchForSupportProjects(
                new SupportProjectSearchCriteria(
                    request.Title,
                    request.AssignedUsers, 
                    request.AssignedAdvisers, 
                    request.Regions, 
                    request.LocalAuthorities,
                    request.Trusts,
                    dates
                    ),
                request.Page, 
                request.Count,
                cancellationToken);

            var pageResponse = PagingResponseFactory.Create(request.PagePath, request.Page, request.Count, totalCount, []);

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

        public async Task<Result<IEnumerable<string>>> GetAllProjectYears(CancellationToken cancellationToken)
        {
            var result = await supportProjectRepository.GetAllProjectYears(cancellationToken);
            return result == null ? Result<IEnumerable<string>>.Failure("") : Result<IEnumerable<string>>.Success(result);
        }
    }
}
