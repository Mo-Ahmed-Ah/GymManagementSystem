using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.AccountViwModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email Is Valid")]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "Password Is Valid")]
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
}
