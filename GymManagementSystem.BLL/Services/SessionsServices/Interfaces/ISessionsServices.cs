using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;

namespace GymManagementSystem.BLL.Services.SessionsServices.Interfaces;

public interface ISessionsServices
{
    public Task<Result<IEnumerable<SessionViewModel>?>> GetSessionsAsync();
    public Task<Result> CreateSeassionAsync(CreateSessionViewModel model);
    public Task<Result<SessionViewModel>> GetSessionDetailsAsync(int? sessionId);
    public Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int? sessionId);
    public Task<Result> UpdateSessionAsync(UpdateSessionViewModel? model);
    public Task<Result> DeleteSessionAsync(int? sessionId);
}
