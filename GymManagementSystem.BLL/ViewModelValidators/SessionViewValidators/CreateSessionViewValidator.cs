using FluentValidation;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;

namespace GymManagementSystem.BLL.ViewModelValidators.SessionViewValidators;

public class CreateSessionViewValidator : AbstractValidator<CreateSessionViewModel>
{
    public CreateSessionViewValidator()
    {
        RuleFor(s => s.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(10, 500).WithMessage("Description must be between 10 and 500 characters");

        RuleFor(s => s.Capacity)
            .NotEmpty().WithMessage("Capacity is required")
            .InclusiveBetween(0, 25).WithMessage("Capacity must be between 0 and 25");

        RuleFor(s => s.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.Now).WithMessage("Start date must be in the future")
            .LessThan(x => x.EndDate).WithMessage("Start date must be before end date");

        RuleFor(s => s.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(DateTime.Now).WithMessage("End date must be in the future")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(s => s.TrainerId)
            .NotEmpty().WithMessage("Trainer is required");

        RuleFor(s => s.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}
