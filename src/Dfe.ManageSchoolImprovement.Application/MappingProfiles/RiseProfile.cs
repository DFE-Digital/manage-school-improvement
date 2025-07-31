using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.MappingProfiles
{

    public class RiseProfile : Profile
    {
        public RiseProfile()
        {
            // Explicit mapping from SupportProjectId to int and vice versa
            CreateMap<SupportProjectId, int>()
                .ConvertUsing(src => src.Value);

            CreateMap<int, SupportProjectId>()
                .ConvertUsing(value => new SupportProjectId(value));

            CreateMap<Domain.Entities.SupportProject.SupportProject, SupportProjectDto>()
               .ForCtorParam("id", opt =>
           opt.MapFrom(src => src.Id != null ? src.Id.Value : (int?)null)) // Map Id only if not null
                .ReverseMap();

            CreateMap<FundingHistoryId, Guid>()
                .ConvertUsing(src => src.Value);

            CreateMap<Guid, FundingHistoryId>()
                .ConvertUsing(value => new FundingHistoryId(value));

            CreateMap<Domain.Entities.SupportProject.FundingHistory, FundingHistoryDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => src.Id != null ? src.Id.Value : (Guid?)null))
               .ReverseMap();

            CreateMap<Domain.Entities.SupportProject.ImprovementPlan, ImprovementPlanDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => src.Id != null ? src.Id.Value : (Guid?)null))
               .ReverseMap();

            CreateMap<Domain.Entities.SupportProject.ImprovementPlanObjective, ImprovementPlanObjectiveDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => src.Id != null ? src.Id.Value : (Guid?)null))
               .ForCtorParam("improvementPlanId", opt => opt.MapFrom(src => src.ImprovementPlanId != null ? src.ImprovementPlanId.Value : (Guid?)null))
               .ReverseMap();
        }
    }
}
