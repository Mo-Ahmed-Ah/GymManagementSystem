using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.CategoriesServices.Interfaces;
using GymManagementSystem.BLL.Services.SessionsServices.Interfaces;
using GymManagementSystem.BLL.Services.TrainersServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers;

[Authorize]
public class SessionsController : Controller
{
    private readonly ISessionsServices _sessionServices;
    private readonly ITrainersServices _trainerServices;
    private readonly ICategoryServices _categoryServices;

    public SessionsController(ISessionsServices sessionServices, ITrainersServices trainerServices, ICategoryServices categoryServices)
    {
        _sessionServices = sessionServices;
        _trainerServices = trainerServices;
        _categoryServices = categoryServices;
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    public async Task<IActionResult> Edit(int? sessionId)
    {
        var result = await _sessionServices.GetSessionToUpdateAsync(sessionId);
        if (result.Success)
        {
            await LodaingTrainersAndCategoriesForSelectedListAsync();
            return View(result.Value);
        }
        TempData["ErrorMessage"] = result.error;
        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(UpdateSessionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _sessionServices.UpdateSessionAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Session Updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        if (result.Kind == ResultKind.Conflict)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.error;
        return View(model);
    }
    public async Task<IActionResult> Index()
    {
        var result = await _sessionServices.GetSessionsAsync();
        return View(result.Value);
    }
    public async Task<IActionResult> Details(int? sessionId)
    {
        var result = await _sessionServices.GetSessionDetailsAsync(sessionId);
        if (result.Success)
        {
            return View(result.Value);
        }
        TempData["ErrorMessage"] = result.error;
        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    public async Task<IActionResult> Create()
    {
        await LodaingTrainersAndCategoriesForSelectedListAsync();
        return View();
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateSessionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _sessionServices.CreateSeassionAsync(model);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Session Created successfully.";
            return RedirectToAction(nameof(Index));
        }
        await LodaingTrainersAndCategoriesForSelectedListAsync();
        TempData["ErrorMessage"] = result.error;
        return View(model);
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    public async Task<IActionResult> Delete(int? sessionId)
    {
        if (sessionId != null)
            return View(sessionId);
        TempData["ErrorMessage"] = "You Can Not Delete Session With Null Id";
        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int? sessionId)
    {
        var result = await _sessionServices.DeleteSessionAsync(sessionId);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Session Created successfully.";
            return RedirectToAction(nameof(Index));
        }
        if (result.Kind == ResultKind.Conflict)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        await LodaingTrainersAndCategoriesForSelectedListAsync();
        TempData["ErrorMessage"] = result.error;
        return RedirectToAction(nameof(Index));
    }
    #region Helper Methods
    [Authorize(Roles = "SuperAdmin , Admin")]
    private async Task LodaingTrainersAndCategoriesForSelectedListAsync()
    {
        ViewBag.Trainers = new SelectList((await _trainerServices.GetTrainersForDropDownListAsync()).Value, "Id", "Name");
        ViewBag.Categories = new SelectList((await _categoryServices.GetCategoriesForDropDownListAsync()).Value, "Id", "CategoryName");
    }
    #endregion
}
