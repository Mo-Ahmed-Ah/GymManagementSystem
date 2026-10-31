using AutoMapper;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.MappingProfiles;

public class TrainerMappingProfiles : Profile
{
    public TrainerMappingProfiles()
    {
        CreateMap<Trainer, TrainerSelectedViewModel>();

        CreateMap<CreateTrainerViewModel, Trainer>()
            .ForMember(dest => dest.Specialty, opt => opt.MapFrom(ctvm => ctvm.Specialties))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.MapFrom(ctvm => new Address() { BuildingNumber = ctvm.BuildingNumber, City = ctvm.City, Street = ctvm.Street }));

        CreateMap<Trainer, TrainerDetailsViewModel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(t => t.Address != null ? $"{t.Address.BuildingNumber} - {t.Address.Street} - {t.Address.City}" : "No Address"))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(t => t.DateOfBirth.ToString("MM/dd/yyyy")))
            .ForMember(dest => dest.Specialties, opt => opt.MapFrom(t => t.Specialty.ToString()));

        CreateMap<Trainer, TrainerViewModel>()
            .ForMember(dest => dest.Specialties, opt => opt.MapFrom(t => t.Specialty.ToString()));

        CreateMap<Trainer, TrainerToUpdateViewModel>()
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(t => t.Address.BuildingNumber))
            .ForMember(dest => dest.City, opt => opt.MapFrom(t => t.Address == null ? "" : t.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(t => t.Address == null ? "" : t.Address.Street))
            ;
    }
}
