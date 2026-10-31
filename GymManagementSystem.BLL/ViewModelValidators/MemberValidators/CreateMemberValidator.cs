using FluentValidation;
using GymManagementSystem.BLL.ViewModelValidators.HealthRecordValidators;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.ViewModelValidators.MemberValidators;

public class CreateMemberValidator : AbstractValidator<CreateMemberViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateMemberValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(m => m.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name Is Required")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Name Can Only Contain Letters And Spaces");

        RuleFor(m => m.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email Is Required")
            .EmailAddress()
            .WithMessage("Invalid Email Format")
            .MustAsync(async (email, cancellation) =>
            {
                return await _unitOfWork.MemberRepository.FristOrDefaultAsync(m => m.Email == email, cancellation) != null;
            })
            .WithMessage("Email Is Already Taken");

        RuleFor(m => m.Phone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Phone Number Is Required")
            .Matches(@"^(010|011|012|015)\d{8}$")
            .WithMessage("Phone Number Must Be A Valid Egyptian Mobile Number")
            .MustAsync(IsPhoneNumberUnique)
            .WithMessage("Phone Number Is Already Taken");

        RuleFor(m => m.DateOfBirth)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Date Of Birth Is Required");

        RuleFor(m => m.Gender)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Gender Is Required")
            .IsInEnum()
            .WithMessage("Invalid Gender");

        RuleFor(m => m.BuildingNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Building Number Is Required")
            .GreaterThan(0)
            .WithMessage("Building Number Must Be A Positive Integer");

        RuleFor(m => m.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("City Is Required")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("City Can Only Contain Letters And Spaces")
            .Length(2, 100)
            .WithMessage("City Must Be Between 2 And 100 Characters");

        RuleFor(m => m.Street)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Street Is Required")
            .Matches(@"^[a-zA-Z0-9\s]+$")
            .WithMessage("Street Can Only Contain Letters, Numbers, And Spaces")
            .Length(2, 100)
            .WithMessage("Street Must Be Between 2 And 100 Characters");

        RuleFor(m => m.HealthRecordViewModel)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Health Record Is Required")
            .SetValidator(new HealthRecordValidator());


        RuleFor(m => m.PhotoFile)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Photo Is Required")
            .WithName("Profile Photo");

    }
    public async Task<bool> IsPhoneNumberUnique(string phoneNumber, CancellationToken cancellationToken)
    {
        var existingMember = await _unitOfWork.MemberRepository.FristOrDefaultAsync(m => m.Phone == phoneNumber, cancellationToken);
        return existingMember == null;
    }
}
