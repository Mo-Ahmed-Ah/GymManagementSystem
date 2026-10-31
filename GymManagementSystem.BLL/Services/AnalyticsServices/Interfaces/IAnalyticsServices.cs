using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;

namespace GymManagementSystem.BLL.Services.AnalyticsServices.Interfaces;

public interface IAnalyticsServices
{
    public Task<Result<AnalyticsViewModel>> GetAnalyticsAsync();
}
