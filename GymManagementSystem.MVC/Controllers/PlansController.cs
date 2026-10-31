using GymManagementSystem.BLL.Services.PlansServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.MVC.Controllers;

[Authorize]
public class PlansController : Controller
{
    private readonly IPlanServices _planServices;
    public PlansController(IPlanServices planServices)
    {
        _planServices = planServices;
    }
    public async Task<IActionResult> Index()
    {
        var result = await _planServices.GetPLansAsync();
        if (result.Success)
            return View(result.Value);
        return RedirectToAction(nameof(Index), nameof(HomeController));
    }
    public async Task<IActionResult> Details(int? id)
    {
        var result = await _planServices.GetPlanDetailsAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    public IActionResult Create() => View();
    [Authorize(Roles = "SuperAdmin , Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(PlanToCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _planServices.CreatePlanAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Plan Created successfully.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = result.error;
        return View(model);
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        var result = await _planServices.GetPlanToUpdateAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(PlanToUpdateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _planServices.UpdatePlanAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Plan Updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = result.error;
        return View(model);
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int? id)
    {
        var result = await _planServices.UpdatePlanSatus(id);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Plan Status Updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = result.error;
        return RedirectToAction(nameof(Index));
    }

}
