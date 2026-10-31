using GymManagementSystem.BLL.Services.MembershipServices.Interfaces;
using GymManagementSystem.BLL.Services.MembersServices.Interfaces;
using GymManagementSystem.BLL.Services.PlansServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers;

[Authorize(Roles = "SuperAdmin , Admin")]
public class MembershipsController : Controller
{
    private readonly IMembershipServices _membershipServices;
    private readonly IMemberServices _memberServices;
    private readonly IPlanServices _planServices;

    public MembershipsController(IMembershipServices membershipServices, IMemberServices memberServices, IPlanServices planServices)
    {
        _membershipServices = membershipServices;
        _memberServices = memberServices;
        _planServices = planServices;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        return View(await _membershipServices.GetAllAsync(cancellationToken));
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.plans = new SelectList(await _planServices.GetPlansForDropDownListAsync(), "Id", "Name");
        ViewBag.Members = new SelectList(await _memberServices.GetMembersForDropDownListAsync(), "Id", "Name");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberShipViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.plans = new SelectList(await _planServices.GetPlansForDropDownListAsync(), "Id", "Name");
            ViewBag.Members = new SelectList(await _memberServices.GetMembersForDropDownListAsync(), "Id", "Name");
            return View(model);
        }
        var result = await _membershipServices.CreateAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Membership Create Successfully";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.plans = new SelectList(await _planServices.GetPlansForDropDownListAsync(), "Id", "Name");
        ViewBag.Members = new SelectList(await _memberServices.GetMembersForDropDownListAsync(), "Id", "Name");
        TempData["ErrorMessage"] = result.error;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Cancel(int? id)
    {
        var result = await _membershipServices.DeleteActiveMemberShipAsync(id);
        if (result.Success)
            TempData["SuccessMessage"] = "Membership Cancelled Successfully";
        else
            TempData["ErrorMessage"] = result.error;
        return RedirectToAction(nameof(Index));
    }
}
