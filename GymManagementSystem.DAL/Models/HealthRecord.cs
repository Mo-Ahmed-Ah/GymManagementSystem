namespace GymManagementSystem.DAL.Models;

public class HealthRecord : BaseEntity
{
    public decimal Height { get; set; } = default!;
    public decimal Weight { get; set; } = default!;
    public string BloodType { get; set; } = default!;
    public string? Note { get; set; }
    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;
}
