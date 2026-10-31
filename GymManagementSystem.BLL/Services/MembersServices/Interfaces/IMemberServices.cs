using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagementSystem.BLL.Services.MembersServices.Interfaces;

public interface IMemberServices
{
    public Task<Result<IEnumerable<MemberViewModel>>> GetMemoriesAsync();
    public Task<Result> AddMemberAsync(CreateMemberViewModel model);
    public Task<Result<MemberDetailsViewModel>> GetMemberDetailsAsync(int? id);
    public Task<Result<HealthRecordViewModel?>> GetHealthRecordDetailsAsync(int? id);
    public Task<Result<MemberToUpdateViewModel?>> GetMemberToUpdateAsync(int? id);
    public Task<Result> UpdateMemberAsync(MemberToUpdateViewModel member);
    public Task<Result> DeleteMemberAsync(int? id);
    public Task<IEnumerable<MemberForDropDownListViewModel>> GetMembersForDropDownListAsync();
}
