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
                                                                           //     .ForMember(dest => dest.id, opt => {
                                                                           //         opt.Condition(src => src.Id != null); // Map only if Id is not null
                                                                           //         opt.MapFrom(src => src.Id.Value);    // Map the actual value from the ValueObject
                                                                           //     })
                .ReverseMap();
            //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PrincipalId.Value))
            //.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.NameDetails.NameListAs!.Split(",", StringSplitOptions.None)[1].Trim()))
            //.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.NameDetails.NameListAs!.Split(",", StringSplitOptions.None)[0].Trim()))
            //.ForMember(dest => dest.Email, opt => opt.MapFrom(src => 6src.PrincipalDetails.Email))
            //.ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.NameDetails.NameDisplayAs))
            //.ForMember(dest => dest.DisplayNameWithTitle, opt => opt.MapFrom(src => src.NameDetails.NameFullTitle))
            //.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => new List<string> { "Student" }))
            //.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.LastRefresh))
            //.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PrincipalDetails.Phone))
            //.ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.SchoolName));

            CreateMap<FundingHistoryId, Guid>()
                .ConvertUsing(src => src.Value);

            CreateMap<Guid, FundingHistoryId>()
                .ConvertUsing(value => new FundingHistoryId(value));

            CreateMap<Domain.Entities.SupportProject.FundingHistory, FundingHistoryDto>()
               .ForCtorParam("id", opt => opt.MapFrom(src => src.Id != null ? src.Id.Value : (Guid?)null))
               .ReverseMap();
        }
    }
}
