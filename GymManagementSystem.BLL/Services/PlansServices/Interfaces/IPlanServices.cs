using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;

namespace GymManagementSystem.BLL.Services.PlansServices.Interfaces;

public interface IPlanServices
{
    public Task<Result<IEnumerable<PlanViewModel>>> GetPLansAsync();
    public Task<Result<PLanDetailsViewModel>> GetPlanDetailsAsync(int? id);
    public Task<Result<PlanToUpdateViewModel>> GetPlanToUpdateAsync(int? id);
    public Task<Result> UpdatePlanAsync(PlanToUpdateViewModel model);
    public Task<Result> UpdatePlanSatus(int? id);
    public Task<IEnumerable<PlanForDropDownListViewModel>> GetPlansForDropDownListAsync();
    public Task<Result> CreatePlanAsync(PlanToCreateViewModel model);
}
