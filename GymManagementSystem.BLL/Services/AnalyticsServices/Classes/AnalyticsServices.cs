using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.AnalyticsServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.AnalyticsServices.Classes;

public class AnalyticsServices : IAnalyticsServices
{
    private readonly IUnitOfWork _unitOfWork;

    public AnalyticsServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AnalyticsViewModel>> GetAnalyticsAsync()
    {
        AnalyticsViewModel model = new();
        try
        {
            model.TotalMembers = await _unitOfWork.GetRepository<Member>().CountAsync();
            model.ActiveMembers = await _unitOfWork.GetRepository<MemberShip>().CountAsync(x => x.EndDate > DateTime.Now);
            model.TotalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync();
            model.UpcomingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(x => x.StartDate > DateTime.Now);
            model.OngoingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(x => x.StartDate <= DateTime.Now && x.EndDate > DateTime.Now);
            model.CompletedSessions = await _unitOfWork.GetRepository<Session>().CountAsync(x => x.EndDate <= DateTime.Now);
            return Result<AnalyticsViewModel>.Ok(model);
        }
        catch (Exception ex)
        {
            return Result<AnalyticsViewModel>.Fail($"Internal Server Error : {ex.Message}");
        }
    }

}
