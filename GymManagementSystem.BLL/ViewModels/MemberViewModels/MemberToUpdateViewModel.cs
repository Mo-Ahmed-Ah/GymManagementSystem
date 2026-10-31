namespace GymManagementSystem.BLL.ViewModels.MemberViewModels;

public class MemberToUpdateViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; } = default!;
    public string? Photo { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public int BuildingNumber { get; set; }
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string Gender { get; set; } = default!;
}
