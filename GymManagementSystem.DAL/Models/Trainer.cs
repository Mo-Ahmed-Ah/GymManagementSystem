using GymManagementSystem.DAL.Models.Enums;

namespace GymManagementSystem.DAL.Models;

public class Trainer : GymUser
{
    // HierDate => CreatedAt
    public Specialty Specialty { get; set; }
    public ICollection<Session> Sessions { get; set; } = default!;
}
