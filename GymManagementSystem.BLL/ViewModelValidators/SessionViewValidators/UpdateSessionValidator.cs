using FluentValidation;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;

namespace GymManagementSystem.BLL.ViewModelValidators.SessionViewValidators;

public class UpdateSessionValidator : AbstractValidator<UpdateSessionViewModel>
{
    public UpdateSessionValidator()
    {
        RuleFor(s => s.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(10, 500).WithMessage("Description must be between 10 and 500 characters");

        RuleFor(s => s.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.Now).WithMessage("Start date must be in the future")
            .LessThan(s => s.EndDate).WithMessage("Start date must be before end date");

        RuleFor(s => s.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(DateTime.Now).WithMessage("End date must be in the future")
            .GreaterThan(s => s.StartDate).WithMessage("End date must be after start date");

        RuleFor(s => s.TrainerId)
            .NotEmpty().WithMessage("Trainer is required");
    }
}
