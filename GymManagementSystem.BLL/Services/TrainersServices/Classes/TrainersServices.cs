using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.TrainersServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.TrainersServices.Classes;

public class TrainersServices : ITrainersServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public TrainersServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result> AddTrainerAsync(CreateTrainerViewModel model)
    {
        try
        {
            //await _unitOfWork.GetRepository<Trainer>().AddAsync(new()
            //{
            //    Name = model.Name,
            //    Email = model.Email,
            //    Gender = model.Gender,
            //    DateOfBirth = model.DateOfBirth,
            //    Phone = model.Phone,
            //    Address = new() { BuildingNumber = model.BuildingNumber, City = model.City, Street = model.Street },
            //});
            await _unitOfWork.GetRepository<Trainer>().AddAsync(_mapper.Map<Trainer>(model));
            return await _unitOfWork.SaveChangesAsync() > 0 ? Result.Ok() : Result.Fail("Trainer Is Not Added");
        }
        catch
        {
            return Result.Fail("Internal Server Error");
        }
    }
    public async Task<Result> DeleteTrainerAsync(int? id)
    {
        try
        {
            if (id == null)
                return Result.Validation("Id Is Null");
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id.Value);
            if (trainer == null)
                return Result.NotFound($"Can Not Found Trainer With Id : {id}");
            var Sessions = await _unitOfWork.GetRepository<Session>().GetAllAsync();
            if (Sessions.Any(s => s.TrainerId == id && s.EndDate > DateTime.Now))
                return Result.Validation($"The {trainer.Name} Trainer Can Not Deleted Becouse It Has Got Active Session");
            await _unitOfWork.GetRepository<Trainer>().DeleteAsync(trainer);
            return await _unitOfWork.SaveChangesAsync() > 0 ? Result.Ok() : Result.Fail($"The {trainer.Name} Trainer Is Not Updated");
        }
        catch
        {
            return Result.Fail("Internal Server Error");
        }
    }
    public async Task<Result<TrainerDetailsViewModel?>> GetTrainerDetailsAsync(int? id)
    {
        if (id == null)
            return Result<TrainerDetailsViewModel?>.Fail("Id Is Null", ResultKind.ValidationFailed);
        var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id.Value);
        if (trainer == null)
            return Result<TrainerDetailsViewModel?>.NotFound($"Can Not Found Trainer With Id : {id}");
        return Result<TrainerDetailsViewModel>.Ok(_mapper.Map<TrainerDetailsViewModel>(trainer))!;
    }
    public async Task<Result<IEnumerable<TrainerViewModel>>> GetTrainersAsync()
    {
        var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync();
        if (trainers.Any())
            return Result<IEnumerable<TrainerViewModel>>.Ok(_mapper.Map<IEnumerable<TrainerViewModel>>(trainers));
        return Result<IEnumerable<TrainerViewModel>>.NotFound("Not Found Any Trainers");
    }
    public async Task<Result<IEnumerable<TrainerSelectedViewModel>>> GetTrainersForDropDownListAsync()
    {
        var result = _unitOfWork.GetRepository<Trainer>().GetAll();
        return Result<IEnumerable<TrainerSelectedViewModel>>.Ok(_mapper.Map<IEnumerable<TrainerSelectedViewModel>>(result));
    }
    public async Task<Result<TrainerToUpdateViewModel?>> GetTrainerToUpdateAsync(int? id)
    {
        if (id == null)
            return Result<TrainerToUpdateViewModel?>.Fail("Id Is Null", ResultKind.ValidationFailed);
        var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id.Value);
        if (trainer == null)
            return Result<TrainerToUpdateViewModel?>.NotFound($"Can Not Founf Trainer With Id {id}");

        return Result<TrainerToUpdateViewModel>.Ok(_mapper.Map<TrainerToUpdateViewModel>(trainer))!;
    }
    public async Task<Result> UpdateTrainerAsync(TrainerToUpdateViewModel model)
    {
        try
        {
            if (model == null)
                return Result.Validation("The Model Is Null");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.Id);
            if (trainer == null)
                return Result.NotFound("We Can Not Found Any Trainer Like This Trainer");

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;
            trainer.Specialty = model.Specialties;
            await _unitOfWork.GetRepository<Trainer>().UpdateAsync(trainer);
            return await _unitOfWork.SaveChangesAsync() > 0 ? Result.Ok() : Result.Fail("Trainer Is Not Updated");
        }
        catch
        {
            return Result.Fail("Internal Server Error");
        }
    }
}
