using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.AccountViwModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.Services.AccountServices.Interfaces;

public interface IAccountServices
{
    public Task<Result<ApplicationUser>> Login(LoginViewModel model);
    public Task LogoutAsync();
}
