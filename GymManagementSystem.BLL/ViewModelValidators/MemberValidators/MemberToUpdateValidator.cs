using FluentValidation;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.ViewModelValidators.MemberValidators;

public class MemberToUpdateValidator : AbstractValidator<MemberToUpdateViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public MemberToUpdateValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(m => m.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email Is Required")
            .EmailAddress()
            .WithMessage("Invalid Email Format")
            .MustAsync(async (model, email, cancellation) =>
            {
                return await _unitOfWork.MemberRepository.FristOrDefaultAsync(m => m.Email == email && m.Id != model.Id, cancellation) != null;
            })
            .WithMessage("Email Is Already Taken");

        RuleFor(m => m.Phone)
           .Cascade(CascadeMode.Stop)
           .NotEmpty()
           .WithMessage("Phone Number Is Required")
           .Matches(@"^(010|011|012|015)\d{8}$")
           .WithMessage("Phone Number Must Be A Valid Egyptian Mobile Number")
           .MustAsync(async (model, phone, cancellation) =>
           {
               return await _unitOfWork.MemberRepository.FristOrDefaultAsync(m => m.Phone == phone && m.Id != model.Id, cancellation) != null;
           })
           .WithMessage("Phone Number Is Already Taken");

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
            .Length(2, 150)
            .WithMessage("Street Must Be Between 2 And 150 Characters");
    }
}
