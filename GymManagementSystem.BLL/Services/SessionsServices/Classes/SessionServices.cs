using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.SessionsServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.SessionsServices.Classes;

public class SessionServices : ISessionsServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SessionServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SessionViewModel>?>> GetSessionsAsync()
    {
        var sessions = await _unitOfWork.SessionRepository.GetAllSessiosWithTrainerAndCategoryAsync();

        var MapedSession = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
        foreach (var session in MapedSession)
            session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id);

        return Result<IEnumerable<SessionViewModel>?>.Ok(MapedSession);
    }

    public async Task<Result> CreateSeassionAsync(CreateSessionViewModel model)
    {
        try
        {
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After Start");
            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In The Future");
            if (model.Capacity < 1 || model.Capacity > 25)
                return Result.Validation("Capacity Must Be Between 1 , 25");
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null)
                return Result.NotFound($"The Trainer With ID {model.TrainerId} Is Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (trainer == null)
                return Result.NotFound($"The Category With ID {model.CategoryId} Is Not Found");

            var IsValid = Enum.TryParse<Specialty>(category!.CategoryName, ignoreCase: true, out var categorySpecialty);

            if (!IsValid || trainer.Specialty != categorySpecialty)
                return Result.Validation($"Can Not Create This Session Becous Trainer : {trainer.Name} Not Match Category {categorySpecialty.ToString()}");

            var Session = _mapper.Map<Session>(model);
            await _unitOfWork.SessionRepository.AddAsync(Session);
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result ? Result.Ok() : Result.Fail("Failed To Create Session");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Internal Server Error : {ex.Message}");
        }
    }

    public async Task<Result<SessionViewModel>> GetSessionDetailsAsync(int? sessionId)
    {
        if (sessionId == null)
            return Result<SessionViewModel>.Fail("The Session Id Is Null", ResultKind.ValidationFailed);
        var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategoryAsync(sessionId.Value);
        if (session == null)
            return Result<SessionViewModel>.NotFound($"Can Not Found Session With Id {sessionId}");
        return Result<SessionViewModel>.Ok(_mapper.Map<SessionViewModel>(session));
    }

    public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int? sessionId)
    {
        if (sessionId == null)
            return Result<UpdateSessionViewModel>.Fail("The Session Id Is Null", ResultKind.ValidationFailed);
        var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId.Value);
        if (session == null)
            return Result<UpdateSessionViewModel>.NotFound($"Can Not Found Session With Id {sessionId}");
        if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now)
            return Result<UpdateSessionViewModel>.Fail($"Can Not Edit Ongoing Session", ResultKind.Conflict);
        if (session.EndDate <= DateTime.Now)
            return Result<UpdateSessionViewModel>.Fail($"Can Not Edit Completed Session", ResultKind.Conflict);
        return Result<UpdateSessionViewModel>.Ok(_mapper.Map<UpdateSessionViewModel>(session));
    }

    public async Task<Result> UpdateSessionAsync(UpdateSessionViewModel model)
    {
        try
        {
            if (model == null)
                return Result.Validation("The Model Is Null");
            var session = await _unitOfWork.SessionRepository.FristOrDefaultAsync(s => s.Id == model.Id);
            if (session == null)
                return Result.NotFound($"Can Not Found Session With Id {model.Id}");
            if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now)
                return Result.Fail($"Can Not Edit Ongoing Session", ResultKind.Conflict);
            if (session.EndDate <= DateTime.Now)
                return Result.Fail($"Can Not Edit Completed Session", ResultKind.Conflict);
            await _unitOfWork.SessionRepository.UpdateAsync(_mapper.Map(model, session));
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                return Result.Ok();
            else
                return Result.Fail("Can Not Update Session");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Internal Server Error Message : {ex.Message}");
        }
    }

    public async Task<Result> DeleteSessionAsync(int? sessionId)
    {
        try
        {
            if (sessionId == null)
                return Result.Validation("Session Id Is Null");
            if (sessionId == 0)
                return Result.Validation("Session Id Can Not Be 0");
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId.Value);
            if (session == null)
                return Result.NotFound($"Can Not Found Session With Id {sessionId}");
            if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now)
                return Result.Fail($"Can Not Delete Ongoing", ResultKind.Conflict);
            await _unitOfWork.SessionRepository.DeleteAsync(session);
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                return Result.Ok();
            return Result.Fail("Session Can Not Be Deleted");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Internal Server Error Message : {ex.Message}");
        }
    }
}
