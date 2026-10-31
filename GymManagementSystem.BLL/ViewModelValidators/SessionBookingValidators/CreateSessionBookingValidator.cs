using FluentValidation;
using GymManagementSystem.BLL.ViewModels.SessionBookingViewModel;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.ViewModelValidators.SessionBookingValidators;

public class CreateSessionBookingValidator : AbstractValidator<CreateSessionBookingViewModel>
{
    public CreateSessionBookingValidator()
    {
        RuleFor(sb => sb.MemberId)
            .NotEmpty().WithMessage("MemberId is required.");

        RuleFor(sb => sb.SessionId)
            .NotEmpty().WithMessage("SessionId is required.");
    }
}
