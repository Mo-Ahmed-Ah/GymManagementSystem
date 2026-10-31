using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.PlansServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAl.Models;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.PlansServices.Classes;

public class PlanServices : IPlanServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlanServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> CreatePlanAsync(PlanToCreateViewModel model)
    {
        try
        {
            if (model == null)
                return Result.Validation("The Model Is Null");
            await _unitOfWork.GetRepository<Plan>().AddAsync(_mapper.Map<Plan>(model));
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                return Result.Ok();
            return Result.Fail("Plan Can Not Be Created");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Internal Server Error Message : {ex.Message}");
        }
    }

    public async Task<Result<PLanDetailsViewModel>> GetPlanDetailsAsync(int? id)
    {
        if (id == null || id == 0)
            return Result<PLanDetailsViewModel>.Fail("Id Is Null");
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id.Value);
        if (plan == null)
            return Result<PLanDetailsViewModel>.NotFound("Plan Not Found");

        return Result<PLanDetailsViewModel>.Ok(_mapper.Map<PLanDetailsViewModel>(plan));
    }
    public async Task<Result<IEnumerable<PlanViewModel>>> GetPLansAsync()
    {
        var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync();
        return Result<IEnumerable<PlanViewModel>>.Ok(_mapper.Map<IEnumerable<PlanViewModel>>(plans));
    }

    public async Task<IEnumerable<PlanForDropDownListViewModel>> GetPlansForDropDownListAsync()
        => _mapper.Map<IEnumerable<PlanForDropDownListViewModel>>(await _unitOfWork.GetRepository<Plan>().GetAllAsync(p => p.IsActive));


    public async Task<Result<PlanToUpdateViewModel>> GetPlanToUpdateAsync(int? id)
    {
        if (id == null || id == 0)
            return Result<PlanToUpdateViewModel>.Fail("Id Is Null");
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id!.Value);
        if (plan == null)
            return Result<PlanToUpdateViewModel>.NotFound("Plan Not Found");

        return Result<PlanToUpdateViewModel>.Ok(_mapper.Map<PlanToUpdateViewModel>(plan));
    }
    public async Task<Result> UpdatePlanAsync(PlanToUpdateViewModel model)
    {
        try
        {
            if (model == null)
                return Result.Fail("Model Is Null");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(model.Id);
            if (plan == null)
                return Result.Fail("Plan Not Found");

            var memberShips = await _unitOfWork.GetRepository<MemberShip>().GetAllAsync();
            if (memberShips.Any(m => m.PlanId == model.Id && m.CreatedAt < DateTime.Now && m.EndDate >= DateTime.Now))
                return Result.Fail("This plan Has Got Active Sessions Now");

            await _unitOfWork.GetRepository<Plan>().UpdateAsync(_mapper.Map(model, plan));
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                return Result.Ok();
            return Result.Fail("Plan can not be updated");
        }
        catch
        {
            return Result.Fail("Internal Server Error");
        }
    }
    public async Task<Result> UpdatePlanSatus(int? id)
    {
        try
        {
            if (id == null)
                return Result.Fail("Id Is Null");
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id.Value);
            if (plan == null)
                return Result.NotFound("Plan Not Found");
            plan.IsActive = !plan.IsActive;
            await _unitOfWork.GetRepository<Plan>().UpdateAsync(plan);
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                return Result.Ok();
            return Result.Fail("Plan Status Can Not Be updated");
        }
        catch
        {
            return Result.Fail("Internal Server Error");
        }
    }
}
