using FluentValidation;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;

namespace GymManagementSystem.BLL.ViewModelValidators.MemberShipValidators;

public class CreateMemberShipValidator : AbstractValidator<CreateMemberShipViewModel>
{
    public CreateMemberShipValidator()
    {
        RuleFor(x => x.PlanId)
            .NotEmpty().WithMessage("Plan Id Is Required");
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member Id Is Required");
    }
}
