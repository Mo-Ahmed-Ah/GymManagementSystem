namespace GymManagementSystem.DAL.Models;

public class Member : GymUser
{
    // JoinDate => CreatedAt
    public string? Photo { get; set; }
    #region Relationships
    public HealthRecord HealthRecord { get; set; } = default!;
    public ICollection<MemberShip> MemberShips { get; set; } = default!;
    public ICollection<MemberSession> MemberSessions { get; set; } = default!;
    #endregion
}
