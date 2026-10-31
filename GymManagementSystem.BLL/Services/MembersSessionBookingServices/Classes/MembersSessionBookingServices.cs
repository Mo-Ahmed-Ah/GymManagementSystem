using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.MembersSessionBookingServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberSessionViewModel;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BLL.ViewModels.SessionBookingViewModel;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;


namespace GymManagementSystem.BLL.Services.MembersSessionBookingServices.Classes;

public class MembersSessionBookingServices : IMembersSessionBookingServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MembersSessionBookingServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<MemberSessionViewModel>>> GetAllSessionBookingAsync(int? sessionId)
    {
        if (sessionId == null)
            return Result<IEnumerable<MemberSessionViewModel>>.Fail("The Session Id Is Null", ResultKind.ValidationFailed);
        var sessionBokkings = await _unitOfWork.SessionMembersRepository.GetSessionMemberBySessionId(sessionId.Value);
        return Result<IEnumerable<MemberSessionViewModel>>.Ok(_mapper.Map<IEnumerable<MemberSessionViewModel>>(sessionBokkings));
    }
    public async Task<Result> CreateNewSessionBookingAsync(CreateSessionBookingViewModel model)
    {
        if (model == null)
            return Result.Validation("The Model Is Null");
        var session = await _unitOfWork.SessionRepository.GetByIdAsync(model.SessionId);
        if (session == null)
            return Result.NotFound($"We Can Not Found Session With Id {model.SessionId}");
        if (session.StartDate <= DateTime.Now)
            return Result.Validation($"Session {(session.EndDate > DateTime.Now ? "Started" : "Completed")} Can Not Be Booked");
        var memberHasActiveMembership = await _unitOfWork._membershipRepository.AnyAsync(m => m.Id == model.MemberId && m.EndDate > DateTime.Now);
        if (memberHasActiveMembership)
            return Result.Validation("Member Does Not Have Active Membersip");
        var isAlreadyBooked = await _unitOfWork.SessionMembersRepository.AnyAsync(b => b.Id == model.SessionId && b.MemberId == model.MemberId);
        if (isAlreadyBooked)
            return Result.Validation("Session Already Booked");
        var bookedSessions = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(model.SessionId);
        if (bookedSessions >= session.Capacity)
            return Result.Validation("Session Is Full");
        await _unitOfWork.SessionMembersRepository.AddAsync(_mapper.Map<MemberSession>(model));
        var result = await _unitOfWork.SaveChangesAsync();
        if (!(result > 0))
            return Result.Fail("Internal Server Error");
        return Result.Ok();
    }
    public async Task<Result> CancelSessionBooking(int? memberId, int? sessionId)
    {
        if (memberId == null)
            return Result.Validation("The Member Id Is Null");
        if (sessionId == null)
            return Result.Validation("The Session Id Is Null");
        var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId.Value);
        if (session == null)
            return Result.NotFound($"Can Not Found Session With Id {sessionId}");
        if (session.StartDate <= DateTime.Now)
            return Result.Fail($"You Can Not Canel a Booking For A Session That Already {(session.EndDate > DateTime.Now ? "Started" : "Ended")}");
        var booking = await _unitOfWork.SessionMembersRepository.FristOrDefaultAsync(b => b.SessionId == sessionId && b.MemberId == memberId, tracking: true);
        if (booking == null)
            return Result.NotFound($"Can Not Found Bookin Belongs To Member With Id {memberId} And Session With Id {sessionId}");
        await _unitOfWork.SessionMembersRepository.DeleteAsync(booking);
        var result = await _unitOfWork.SaveChangesAsync() > 0;
        return result ? Result.Ok() : Result.Fail("Internal Server Error");
    }
    public async Task<Result> MarkAttend(int? memberId, int? sessionId)
    {
        if (memberId == null)
            return Result.Validation("The Member Id Is Null");
        if (sessionId == null)
            return Result.Validation("The Session Id Is Null");
        var booking = await _unitOfWork.SessionMembersRepository.FristOrDefaultAsync(s => s.SessionId == sessionId && s.MemberId == memberId, tracking: true);
        if (booking == null)
            return Result.NotFound($"Can Not Found Bookin Belongs To Member With Id {memberId} And Session With Id {sessionId}");
        booking.IsAttned = !booking.IsAttned;
        booking.UpdateAt = DateTime.Now;
        await _unitOfWork.SessionMembersRepository.UpdateAsync(booking);
        if (await _unitOfWork.SaveChangesAsync() > 0)
            return Result.Ok();
        return Result.Fail("Internal Server Error");
    }
    public async Task<Result<IEnumerable<MemberForDropDownListViewModel>>> GetMembersForBookingSession(int? sessionId)
    {
        if (sessionId == null)
            return Result<IEnumerable<MemberForDropDownListViewModel>>.Fail("The Session Id Is Null", ResultKind.ValidationFailed);
        var bookedSessionMembers = await _unitOfWork.SessionMembersRepository.GetAllAsync(s => s.SessionId == sessionId);
        var bookedMemberIds = bookedSessionMembers.Select(s => s.MemberId).ToList();

        var availableMembers = await _unitOfWork.GetRepository<Member>().GetAllAsync(m => !bookedMemberIds.Contains(m.Id));
        return Result<IEnumerable<MemberForDropDownListViewModel>>.Ok(_mapper.Map<IEnumerable<MemberForDropDownListViewModel>>(availableMembers));
    }
}
