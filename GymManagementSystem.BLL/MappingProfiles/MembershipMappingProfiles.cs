using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.MappingProfiles;

public class MembershipMappingProfiles : Profile
{
    public MembershipMappingProfiles()
    {
        CreateMap<CreateMemberShipViewModel, MemberShip>()
            .ForMember(dest => dest.EndDate, opt => opt.Ignore());

        CreateMap<MemberShip, MembershipForMemberViewModel>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
            .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<MemberShip, MembershipViewModel>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
            .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt));

    }
}
