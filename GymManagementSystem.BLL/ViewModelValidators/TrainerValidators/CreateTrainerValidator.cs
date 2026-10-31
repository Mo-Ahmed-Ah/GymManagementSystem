using FluentValidation;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.ViewModelValidators.TrainerValidators;

public class CreateTrainerValidator : AbstractValidator<CreateTrainerViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateTrainerValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(trainer => trainer.Name)
            .NotEmpty().WithMessage("Name Is Required")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces");

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

        RuleFor(t => t.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required")
            .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of Birth must be in the past");

        RuleFor(t => t.Gender)
            .NotEmpty().WithMessage("Gender is required")
            .Must(gender => Enum.IsDefined(typeof(Gender), gender)).WithMessage("Invalid gender value");

        RuleFor(t => t.BuildingNumber)
            .NotEmpty().WithMessage("Building Number Is Required")
            .GreaterThan(0).WithMessage("Building Number must be greater than 0");

        RuleFor(t => t.City)
            .NotEmpty().WithMessage("City Is Required")
            .Length(2, 100).WithMessage("City must be between 2 and 100 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("City can only contain letters and spaces");

        RuleFor(t => t.Street)
            .NotEmpty().WithMessage("Street Is Required")
            .Length(2, 100).WithMessage("Street must be between 2 and 100 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Street can only contain letters and spaces");

        RuleFor(t => t.Specialties)
            .NotEmpty().WithMessage("Specialties Is Required")
            .IsInEnum().WithMessage("Specialty's Value Is Not True")
            .Must(specialty => Enum.IsDefined(typeof(Specialty), specialty)).WithMessage("Invalid specialty value");

    }
}
