using FluentValidation;
using GymManagementSystem.BLL.Services.AccountServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.AccountViwModels;
using GymManagementSystem.MVC.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

public class AccountController : Controller
{
    private readonly IValidator<LoginViewModel> _loginValidator;
    private readonly IAccountServices _accountServices;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountServices accountServices, ILogger<AccountController> logger, IValidator<LoginViewModel> loginValidator)
    {
        _accountServices = accountServices;
        _logger = logger;
        _loginValidator = loginValidator;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var validationResult = await _loginValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
        }
        var result = await _accountServices.Login(model);

        if (!result.Success)
        {
            ModelState.AddModelError("InvalidLogin", result.error!.ToString());
            return View(model);
        }
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("The User {UserName} Log Out", User.Identity!.Name);
        await _accountServices.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }
    public IActionResult AccessDenied() => View();
}
