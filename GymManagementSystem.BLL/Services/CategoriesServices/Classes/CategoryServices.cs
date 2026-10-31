using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.CategoriesServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.CategoryViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.CategoriesServices.Classes;

public class CategoryServices : ICategoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<CategorySelectedViewModel>>> GetCategoriesForDropDownListAsync()
    {
        var result = _unitOfWork.GetRepository<Category>().GetAll();
        return Result<IEnumerable<CategorySelectedViewModel>>.Ok(_mapper.Map<IEnumerable<CategorySelectedViewModel>>(result));
    }
}
