using GymManagementSystem.BLL.Services.AccountServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.AccountViwModels;
using GymManagementSystem.MVC.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

public class AccountController : Controller
{
    private readonly IAccountServices _accountServices;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountServices accountServices, ILogger<AccountController> logger)
    {
        _accountServices = accountServices;
        _logger = logger;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
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
