using FluentValidation;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.ViewModelValidators.TrainerValidators;

public class TrainerToUpdateValidator : AbstractValidator<TrainerToUpdateViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public TrainerToUpdateValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(t => t.Email)
            .NotEmpty().WithMessage("Email Is Required")
            .EmailAddress().WithMessage("Invalid email format")
            .MustAsync(async (email, cancellationToken) => await _unitOfWork.GetRepository<Trainer>().FristOrDefaultAsync(t => t.Email == email, cancellationToken) != null)
            .WithMessage("The email must be unique");

        RuleFor(t => t.Phone)
            .NotEmpty().WithMessage("Phone Number Is Required")
            .Matches(@"^(010|011|012|015)\d{8}$").WithMessage("Phone number must be a valid Egyptian mobile number")
            .MustAsync(async (phone, cancellationToken) => await _unitOfWork.GetRepository<Trainer>().FristOrDefaultAsync(t => t.Phone == phone, cancellationToken) != null)
            .WithMessage("The phone must be unique");

        RuleFor(trainer => trainer.BuildingNumber)
            .NotEmpty().WithMessage("Building Number is required")
            .GreaterThan(0).WithMessage("Building Number must be greater than 0");

        RuleFor(trainer => trainer.City)
            .NotEmpty().WithMessage("City is required")
            .Length(2, 100).WithMessage("City must be between 2 and 100 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("City can only contain letters and spaces");

        RuleFor(trainer => trainer.Street)
            .NotEmpty().WithMessage("Street is required")
            .Length(2, 150).WithMessage("Street must be between 2 and 150 characters")
            .Matches(@"^[a-zA-Z0-9\s]+$").WithMessage("Street can only contain letters, numbers, and spaces");

        RuleFor(trainer => trainer.Specialties)
            .IsInEnum().WithMessage("Specialty's Value Is Not True")
            .Must(s => Enum.IsDefined(typeof(Specialty), s)).WithMessage("Invalid specialty value");

    }
}
