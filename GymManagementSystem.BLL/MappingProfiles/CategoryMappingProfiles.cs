using AutoMapper;
using GymManagementSystem.BLL.ViewModels.CategoryViewModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.MappingProfiles;

public class CategoryMappingProfiles : Profile
{
    public CategoryMappingProfiles()
    {
        CreateMap<Category, CategorySelectedViewModel>();
    }
}
