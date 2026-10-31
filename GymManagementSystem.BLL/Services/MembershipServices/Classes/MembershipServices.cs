using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.MembershipServices.Interfaces;
using GymManagementSystem.BLL.Services.MembersServices.Interfaces;
using GymManagementSystem.BLL.Services.PlansServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using GymManagementSystem.DAl.Models;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.MembershipServices.Classes;

public class MembershipServices : IMembershipServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemberServices _memberServices;
    private readonly IPlanServices _planServices;
    private readonly IMapper _mapper;

    public MembershipServices(IUnitOfWork unitOfWork, IPlanServices planServices, IMemberServices memberServices, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _planServices = planServices;
        _memberServices = memberServices;
        _mapper = mapper;
    }

    public async Task<Result> CreateAsync(CreateMemberShipViewModel model, CancellationToken cancellationToken = default)
    {
        if (model is null)
            return Result.Validation("Model Is Null");

        var isMemberFound = _unitOfWork.MemberRepository.GetAll().Any(m => m.Id == model.MemberId);

        if (!isMemberFound)
            return Result.NotFound($"Can Not Found Any Member With Id {model.MemberId}");

        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId);

        if (plan is null)
            return Result.NotFound($"Can Not Found Any Plan With Id {model.PlanId}");

        if (!plan.IsActive)
            return Result.Fail($"The Plan {plan.Name} Is Not Active");

        var isMemberHasActivePlan = await _unitOfWork._membershipRepository.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate < DateTime.Now);

        if (isMemberHasActivePlan)
            return Result.Fail($"The Member Already has Active Plan");

        var memberShipVM = _mapper.Map<MemberShip>(model);

        memberShipVM.EndDate = DateTime.Now.AddDays(plan.DurationDays);

        await _unitOfWork._membershipRepository.AddAsync(memberShipVM);

        if (await _unitOfWork.SaveChangesAsync() > 0)
            return Result.Ok();

        return Result.Fail("Internal Server Error");
    }

    public async Task<Result> DeleteActiveMemberShipAsync(int? memberShipId, CancellationToken cancellationToken = default)
    {
        if (memberShipId == null)
            return Result.Validation("Membership Id Is Null");
        var membership = await _unitOfWork._membershipRepository.GetByIdAsync(memberShipId.Value);
        if (membership is null)
            return Result.NotFound($"Can Not Found Membership Whit This Id {memberShipId}");
        if (membership.EndDate < DateTime.Now)
            return Result.Fail("The Membership Is Expired", ResultKind.Conflict);
        await _unitOfWork._membershipRepository.DeleteAsync(membership);
        if (await _unitOfWork.SaveChangesAsync() > 0)
            return Result.Ok();
        return Result.Fail("Internal Server Error");
    }

    public async Task<IEnumerable<MembershipViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var memberships = await _unitOfWork._membershipRepository
                                           .GetAllMembershipsWithRelatedDataAsync(m => m.EndDate > DateTime.Now, cancellationToken);

        return _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);
    }
}
