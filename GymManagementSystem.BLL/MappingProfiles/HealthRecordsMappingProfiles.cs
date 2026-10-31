using AutoMapper;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.MappingProfiles;

public class HealthRecordsMappingProfiles : Profile
{
    public HealthRecordsMappingProfiles()
    {
        CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
    }
}
