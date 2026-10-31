using GymManagementSystem.DAL.Models.Enums;

namespace GymManagementSystem.BLL.ViewModels.TrainerViewModels;

public class TrainerToUpdateViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public int BuildingNumber { get; set; }
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
    public Specialty Specialties { get; set; }
}
