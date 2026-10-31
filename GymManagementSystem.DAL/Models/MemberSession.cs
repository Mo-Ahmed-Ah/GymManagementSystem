namespace GymManagementSystem.DAL.Models;

public class MemberSession : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;
    public int SessionId { get; set; }
    public Session Session { get; set; } = default!;
    public int BookingDays { get; set; }
    public bool IsAttned { get; set; }
}
