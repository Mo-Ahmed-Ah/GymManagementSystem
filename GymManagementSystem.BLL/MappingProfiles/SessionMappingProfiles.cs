using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.MappingProfiles;

public class SessionMappingProfiles : Profile
{
    public SessionMappingProfiles()
    {
        CreateMap<Session, SessionViewModel>()
            .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

        CreateMap<CreateSessionViewModel, Session>();
        CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

    }
}
