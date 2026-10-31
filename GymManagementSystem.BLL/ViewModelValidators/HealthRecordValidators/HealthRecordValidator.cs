using FluentValidation;

namespace GymManagementSystem.BLL.ViewModelValidators.HealthRecordValidators;

public class HealthRecordValidator : AbstractValidator<HealthRecordViewModel>
{
    public HealthRecordValidator()
    {
        RuleFor(hr => hr.Height)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Height is required")
            .GreaterThan(0).WithMessage("Height must be greater than 0")
            .LessThanOrEqualTo(300).WithMessage("Height must be less than or equal to 300");

        RuleFor(hr => hr.Weight)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Weight is required")
            .GreaterThan(0).WithMessage("Weight must be greater than 0")
            .LessThanOrEqualTo(500).WithMessage("Weight must be less than or equal to 500");

        RuleFor(hr => hr.BloodType)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Blood Type is required")
            .Length(1, 3).WithMessage("Blood Type must be between 1 and 3 characters");
    }
}
