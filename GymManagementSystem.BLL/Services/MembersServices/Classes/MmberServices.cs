using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.AttachmentServices.Interfaces;
using GymManagementSystem.BLL.Services.MembersServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAl.Models;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.MembersServices.Classes;

public class MemberServices : IMemberServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAttachmentServices _attachmentServices;

    public MemberServices(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentServices attachmentServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _attachmentServices = attachmentServices;
    }
    public async Task<Result<IEnumerable<MemberViewModel>>> GetMemoriesAsync()
    {
        var members = await _unitOfWork.MemberRepository.GetAllWithRelatedDateAsync();

        return Result<IEnumerable<MemberViewModel>>.Ok(_mapper.Map<IEnumerable<MemberViewModel>>(members));

    }

    public async Task<Result> AddMemberAsync(CreateMemberViewModel viewModel)
    {
        try
        {
            var PhotoName = await _attachmentServices.UploadFile(
                viewModel.PhotoFile.OpenReadStream(),
                viewModel.PhotoFile.FileName,
                "MemberPhotos"
                );

            viewModel.Photo = PhotoName;
            var model = _mapper.Map<Member>(viewModel);
            await _unitOfWork.GetRepository<Member>().AddAsync(model);
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                return Result.Ok();
            await _attachmentServices.DeleteAsync(PhotoName, "MemberPhotos");
            return Result.Fail("Member Is Not Added");
        }
        catch
        {
            await _attachmentServices.DeleteAsync(viewModel.PhotoFile.FileName, "MemberPhotos");
            return Result.Fail("Internal Server Error");
        }
    }

    public async Task<Result<MemberDetailsViewModel?>> GetMemberDetailsAsync(int? id)
    {
        if (id == null || id == 0)
            return Result<MemberDetailsViewModel>.Fail("Id Not Found")!;

        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id.Value);

        if (member == null)
            return Result<MemberDetailsViewModel>.Fail("Member Not Found")!;
        //var model = new MemberDetailsViewModel()
        //{
        //    Id = member.Id,
        //    Name = member.Name,
        //    Email = member.Email,
        //    Gender = member.Gender.ToString(),
        //    Phone = member.Phone,
        //    Photo = member.Photo,
        //    Address = member.Address == null ? "No Address Attached" : $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
        //    DateOfBirth = member.DateOfBirth.ToString("yyyy-MM-dd"),
        //    //MembershipStartDate = member.MemberShips.Where(ms => ms.IsActive).Select(ms => ms.CreatedAt).FirstOrDefault().ToString("yyyy-MM-dd"),
        //    //MembershipEndDate = member.MemberShips.Where(ms => ms.IsActive).Select(ms => ms.EndDate).FirstOrDefault().ToString("yyyy-MM-dd"),
        //    //PlanName = member.MemberShips.Where(ms => ms.IsActive).Select(ms => ms.Plan.Name).FirstOrDefault()

        //};
        var model = _mapper.Map<MemberDetailsViewModel>(member);
        //var model = _mapper.Map<Member , MemberDetailsViewModel>(member);

        var memberShips = await _unitOfWork.GetRepository<MemberShip>().GetAllAsync();
        if (memberShips.Any())
        {
            var membershipsActive = memberShips.Where(ms => ms.MemberId == id && ms.IsActive).FirstOrDefault();
            if (membershipsActive != null)
            {
                model.MembershipStartDate = membershipsActive.CreatedAt.ToString("yyyy-MM-dd");
                model.MembershipEndDate = membershipsActive.EndDate.ToString("yyyy-MM-dd");
                var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(membershipsActive.PlanId);
                if (plan != null)
                    model.PlanName = plan?.Name;
            }
        }


        return Result<MemberDetailsViewModel>.Ok(model)!;
    }

    public async Task<Result<HealthRecordViewModel?>> GetHealthRecordDetailsAsync(int? id)
    {
        if (id == null || id == 0)
            return Result<HealthRecordViewModel>.Fail("Id Not Found")!;
        var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().GetByIdAsync(id.Value);
        if (healthRecord == null)
            return Result<HealthRecordViewModel>.Fail("Health Record Not Found")!;
        //return new HealthRecordViewModel()
        //{
        //    Height = healthRecord.Height,
        //    Weight = healthRecord.Weight,
        //    BloodType = healthRecord.BloodType,
        //    Note = healthRecord.Note
        //};
        return Result<HealthRecordViewModel>.Ok(_mapper.Map<HealthRecordViewModel>(healthRecord))!;
    }

    public async Task<Result<MemberToUpdateViewModel?>> GetMemberToUpdateAsync(int? id)
    {
        if (id == null || id == 0)
            return Result<MemberToUpdateViewModel>.Fail("Id Not Found")!;

        var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id.Value);

        if (member == null)
            return Result<MemberToUpdateViewModel>.Fail("Member Not Found")!;

        return Result<MemberToUpdateViewModel>.Ok(_mapper.Map<MemberToUpdateViewModel>(member))!;
    }

    public async Task<Result> UpdateMemberAsync(MemberToUpdateViewModel model)
    {
        try
        {
            if (model == null)
                return Result.Fail("Model is null")!;

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(model.Id);

            if (member == null)
                return Result.Fail("Member Not Found")!;

            var mappedmember = _mapper.Map(model, member);
            await _unitOfWork.GetRepository<Member>().UpdateAsync(mappedmember);
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                return Result.Ok();
            else
                return Result.Fail("Member can not be updated");
        }
        catch
        {
            return Result.Fail("Internal Server Error");
        }
    }

    public async Task<Result> DeleteMemberAsync(int? id)
    {
        try
        {
            if (id == null || id == 0)
                return Result.Fail("Id Not Found")!;

            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id.Value);

            if (member == null)
                return Result.NotFound("Member Not Found")!;
            if (!string.IsNullOrWhiteSpace(member.Photo))
                await _attachmentServices.DeleteAsync(member.Photo, "MemberPhotos");
            await _unitOfWork.GetRepository<Member>().DeleteAsync(member);
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (result)
                return Result.Ok();
            else
                return Result.Fail("Member Can Not Be Deleted");
        }
        catch
        {
            return Result.Fail("Internal Server Error");
        }
    }

    public async Task<IEnumerable<MemberForDropDownListViewModel>> GetMembersForDropDownListAsync()
           => _mapper.Map<IEnumerable<MemberForDropDownListViewModel>>(await _unitOfWork.GetRepository<Member>().GetAllAsync());
}