using FluentValidation;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;

namespace GymManagementSystem.BLL.ViewModelValidators.PlanValidators;

public class PlanToCreateValidator : AbstractValidator<PlanToCreateViewModel>
{
    public PlanToCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Plan name is required.")
            .Length(3, 50).WithMessage("Plan name must be between 3 and 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(10, 200).WithMessage("Description must be between 10 and 200 characters.");

        RuleFor(x => x.DurationDays)
            .NotEmpty().WithMessage("Duration in days is required.")
            .InclusiveBetween(1, 365).WithMessage("Duration must be between 1 and 365 days.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.")
            .InclusiveBetween(0.01m, 99999999.99m).WithMessage("Price must be between 0.01 and 99999999.99.")
            .PrecisionScale(10, 2, true).WithMessage("Price can have up to two decimal places.");

    }
}
