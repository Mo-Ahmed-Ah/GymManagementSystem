using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.CategoryViewModels;

namespace GymManagementSystem.BLL.Services.CategoriesServices.Interfaces;

public interface ICategoryServices
{
    public Task<Result<IEnumerable<CategorySelectedViewModel>>> GetCategoriesForDropDownListAsync();
}
