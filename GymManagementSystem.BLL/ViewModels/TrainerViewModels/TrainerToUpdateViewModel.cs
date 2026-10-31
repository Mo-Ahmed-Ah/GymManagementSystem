using GymManagementSystem.BLL.CustomAttributes;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.TrainerViewModels;

public class TrainerToUpdateViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    [Required(ErrorMessage = "Email Is Required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [DataType(DataType.EmailAddress)]
    [IsUnique<Trainer>("Email", ErrorMessage = "The email must be Unique")]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "Phone Number Is Required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be a valid Egyptian mobile number")]
    [DataType(DataType.PhoneNumber)]
    [IsUnique<Trainer>("Phone", ErrorMessage = "The phone must be Unique")]
    public string Phone { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    [Required(ErrorMessage = "Building Number Is Required")]
    [Range(1, 9000, ErrorMessage = "Building Number must be greater than 0")]
    public int BuildingNumber { get; set; }

    [Required(ErrorMessage = "City Is Required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces")]
    public string City { get; set; } = default!;
    [Required(ErrorMessage = "Street Is Required")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 150 characters")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Street can only contain letters, numbers, and spaces")]
    public string Street { get; set; } = default!;
    [Required(ErrorMessage = "Specialties Is Required")]
    [EnumDataType(typeof(Specialty), ErrorMessage = "Specialty's Value Is Not True")]
    public Specialty Specialties { get; set; }
}
