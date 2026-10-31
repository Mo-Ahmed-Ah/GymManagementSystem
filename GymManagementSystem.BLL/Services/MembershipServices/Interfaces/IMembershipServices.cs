using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;

namespace GymManagementSystem.BLL.Services.MembershipServices.Interfaces;

public interface IMembershipServices
{
    public Task<IEnumerable<MembershipViewModel>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<Result> CreateAsync(CreateMemberShipViewModel model, CancellationToken cancellationToken = default);
    public Task<Result> DeleteActiveMemberShipAsync(int? memberShipId, CancellationToken cancellationToken = default);
}
