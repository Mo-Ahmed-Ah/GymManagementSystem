using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.MappingProfiles;

public class MemberMappingProfiles : Profile
{
    public MemberMappingProfiles()
    {
        CreateMap<Member, MemberDetailsViewModel>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address == null ? "No Address Attached" : $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("d/M/yyyy")));

        CreateMap<Member, MemberViewModel>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));


        CreateMap<Member, MemberToUpdateViewModel>()
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

        CreateMap<MemberToUpdateViewModel, Member>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
            });

        CreateMap<CreateMemberViewModel, Member>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address()
            {
                BuildingNumber = src.BuildingNumber,
                Street = src.Street,
                City = src.City
            }))
            .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));

        CreateMap<Member, MemberForDropDownListViewModel>();

    }

}
