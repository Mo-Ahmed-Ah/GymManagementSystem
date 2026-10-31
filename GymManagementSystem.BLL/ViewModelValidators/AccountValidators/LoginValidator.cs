using FluentValidation;
using GymManagementSystem.BLL.ViewModels.AccountViwModels;

namespace GymManagementSystem.BLL.ViewModelValidators.AccountValidators;

public class LoginValidator : AbstractValidator<LoginViewModel>
{
    public LoginValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty().WithMessage("Email Is Required")
            .EmailAddress().WithMessage("Email Is Valid");

        RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Password Is Required")
            .MinimumLength(6).WithMessage("Password Must Be At Least 6 Characters Long");
    }
}
