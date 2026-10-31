using GymManagementSystem.BLL.Services.MembersSessionBookingServices.Interfaces;
using GymManagementSystem.BLL.Services.SessionsServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionBookingViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers;

public class BookingController : Controller
{
    private readonly IMembersSessionBookingServices _membersSessionBookingServices;
    private readonly ISessionsServices _sessionsServices;

    public BookingController(IMembersSessionBookingServices membersSessionBookingServices, ISessionsServices sessionsServices)
    {
        _membersSessionBookingServices = membersSessionBookingServices;
        _sessionsServices = sessionsServices;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _sessionsServices.GetSessionsAsync();
        return View(result.Value);
    }

    public async Task<IActionResult> GetMembersForOngoingSessions(int? sessionId)
    {
        var result = await _membersSessionBookingServices.GetAllSessionBookingAsync(sessionId: sessionId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }
    public async Task<IActionResult> GetMembersForUpcomingSession(int? sessionId)
    {
        var result = await _membersSessionBookingServices.GetAllSessionBookingAsync(sessionId: sessionId);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }
    public async Task<IActionResult> Create(int? sessionId)
    {
        var result = await _membersSessionBookingServices.GetMembersForBookingSession(sessionId);
        ViewBag.Members = new SelectList(result.Value, "Id", "Name");
        ViewBag.SessionId = sessionId;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateSessionBookingViewModel model)
    {
        var result = await _membersSessionBookingServices.CreateNewSessionBookingAsync(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return View(model);
        }
        TempData["SuccessMassage"] = "Booking Created Successflly";
        return RedirectToAction(nameof(GetMembersForUpcomingSession), new { sessionId = model.SessionId });
    }
    [HttpPost]
    public async Task<IActionResult> Cancel(int? memberId, int? sessionId)
    {
        var result = await _membersSessionBookingServices.CancelSessionBooking(memberId, sessionId);
        if (!result.Success)
            TempData["ErrorMessage"] = result.error;
        else
            TempData["SuccessMassage"] = "Booking Canceled Successflly";
        return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
    }
    [HttpPost]
    public async Task<IActionResult> Attended(int? memberId, int? sessionId)
    {
        var result = await _membersSessionBookingServices.MarkAttend(memberId, sessionId);
        if (!result.Success)
            TempData["ErrorMessage"] = result.error;
        else
            TempData["SuccessMassage"] = "Attenddance Recorded";
        return RedirectToAction(nameof(GetMembersForOngoingSessions), new { sessionId = sessionId });
    }
}
