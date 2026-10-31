using FluentValidation;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.TrainersServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

[Authorize]
public class TrainersController : Controller
{
    private readonly IValidator<CreateTrainerViewModel> _createTrainerValidator;
    private readonly IValidator<TrainerToUpdateViewModel> _updateTrainerValidator;
    private readonly ITrainersServices _trainerServices;
    public TrainersController(ITrainersServices trainerServices, IValidator<TrainerToUpdateViewModel> updateTrainerValidator, IValidator<CreateTrainerViewModel> createTrainerValidator)
    {
        _trainerServices = trainerServices;
        _updateTrainerValidator = updateTrainerValidator;
        _createTrainerValidator = createTrainerValidator;
    }
    public async Task<IActionResult> Index()
    {
        var result = await _trainerServices.GetTrainersAsync();
        if (!result.Success)
        {
            if (result.Kind != ResultKind.NotFound)
                TempData["ErrorMessage"] = result.error;
            return View();
        }
        return View(result.Value);
    }
    public async Task<IActionResult> Details(int? id)
    {
        var result = await _trainerServices.GetTrainerDetailsAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Value);
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    public IActionResult Create()
    {
        return View();
    }
    [Authorize(Roles = "SuperAdmin , Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTrainerViewModel model)
    {
        var validationResult = await _createTrainerValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
        }
        var result = await _trainerServices.AddTrainerAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Trainer Created Successfully.";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.error;
        return View(model);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Trainer")]
    public async Task<IActionResult> Edit(int? id)
    {
        var result = await _trainerServices.GetTrainerToUpdateAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Trainer")]
    [HttpPost]
    public async Task<IActionResult> Edit(TrainerToUpdateViewModel model)
    {
        var validationResult = await _updateTrainerValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
        }
        var result = await _trainerServices.UpdateTrainerAsync(model);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Trainer Updated Successfully.";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.error;
        return View(model);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Trainer")]
    public IActionResult Delete(int? id)
    {
        return View(id);
    }
    [Authorize(Roles = "SuperAdmin , Admin , Trainer")]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var result = await _trainerServices.DeleteTrainerAsync(id);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Trainer Delete Successfully.";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.error;
        return View(id);
    }
}
