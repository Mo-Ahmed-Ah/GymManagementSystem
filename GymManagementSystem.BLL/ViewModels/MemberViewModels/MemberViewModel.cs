namespace GymManagementSystem.BLL.ViewModels.MemberViewModels;

public class MemberViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Photo { get; set; }
    public int? HealthRecordId { get; set; }
}
