using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;

namespace GymManagementSystem.BLL.Services.TrainersServices.Interfaces;

public interface ITrainersServices
{
    public Task<Result<IEnumerable<TrainerViewModel>>> GetTrainersAsync();
    public Task<Result<TrainerDetailsViewModel?>> GetTrainerDetailsAsync(int? id);
    public Task<Result> AddTrainerAsync(CreateTrainerViewModel model);
    public Task<Result<TrainerToUpdateViewModel?>> GetTrainerToUpdateAsync(int? id);
    public Task<Result> UpdateTrainerAsync(TrainerToUpdateViewModel model);
    public Task<Result> DeleteTrainerAsync(int? id);
    public Task<Result<IEnumerable<TrainerSelectedViewModel>>> GetTrainersForDropDownListAsync();
}
