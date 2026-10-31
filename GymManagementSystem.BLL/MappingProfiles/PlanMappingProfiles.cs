using AutoMapper;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAl.Models;

namespace GymManagementSystem.BLL.MappingProfiles;

public class PlanMappingProfiles : Profile
{
    public PlanMappingProfiles()
    {
        CreateMap<Plan, PlanViewModel>();
        CreateMap<Plan, PLanDetailsViewModel>();
        CreateMap<Plan, PlanToUpdateViewModel>();
        CreateMap<PlanToUpdateViewModel, Plan>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Plan, PlanForDropDownListViewModel>();
        CreateMap<PlanToCreateViewModel, Plan>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
            .ForMember(dest => dest.MemberShips, opt => opt.Ignore());
    }
}
