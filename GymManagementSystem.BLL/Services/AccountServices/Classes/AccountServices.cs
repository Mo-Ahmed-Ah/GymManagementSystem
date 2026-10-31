using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.AccountServices.Interfaces;
using GymManagementSystem.BLL.ViewModels.AccountViwModels;
using GymManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem.BLL.Services.AccountServices.Classes;

public class AccountServices : IAccountServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountServices> _logger;

    public AccountServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountServices> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<Result<ApplicationUser>> Login(LoginViewModel model)
    {
        if (model == null)
            return Result<ApplicationUser>.Fail("The Model Is Null", ResultKind.ValidationFailed);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return Result<ApplicationUser>.NotFound($"Can Not Found User With This Email {model.Email}");
        var checkPassword = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

        if (checkPassword.Succeeded)
            return Result<ApplicationUser>.Ok(user);
        else if (checkPassword.IsLockedOut)
            return Result<ApplicationUser>.Fail("Your Account Is Locked Now", ResultKind.LockedAccount);
        else
            return Result<ApplicationUser>.Fail("Invalid Password", ResultKind.ValidationFailed);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
