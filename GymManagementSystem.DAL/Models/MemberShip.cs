using GymManagementSystem.DAl.Models;

namespace GymManagementSystem.DAL.Models;

public class MemberShip : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;
    public int PlanId { get; set; }
    public Plan Plan { get; set; } = default!;
    // StartDate => CreatedAdd
    public DateTime EndDate { get; set; }
    public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
    public bool IsActive => EndDate > DateTime.Now && CreatedAt <= DateTime.Now;
}
