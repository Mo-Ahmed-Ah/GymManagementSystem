using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MemberSessionViewModel;
using GymManagementSystem.BLL.ViewModels.SessionBookingViewModel;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.MappingProfiles;

public class MembersSessionMappingProfiles : Profile
{
    public MembersSessionMappingProfiles()
    {
        CreateMap<MemberSession, MemberSessionViewModel>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
            .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-mm-dd HH:mm")));
        CreateMap<CreateSessionBookingViewModel, MemberSession>();
    }
}
