using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MemberSessionViewModel;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BLL.ViewModels.SessionBookingViewModel;

namespace GymManagementSystem.BLL.Services.MembersSessionBookingServices.Interfaces;

public interface IMembersSessionBookingServices
{
    public Task<Result<IEnumerable<MemberSessionViewModel>>> GetAllSessionBookingAsync(int? sessionId);
    public Task<Result> CreateNewSessionBookingAsync(CreateSessionBookingViewModel model);
    public Task<Result> CancelSessionBooking(int? memberId, int? sessionId);
    public Task<Result> MarkAttend(int? memberId, int? sessionId);
    public Task<Result<IEnumerable<MemberForDropDownListViewModel>>> GetMembersForBookingSession(int? sessionId);
}
