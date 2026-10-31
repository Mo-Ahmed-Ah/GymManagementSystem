namespace GymManagementSystem.DAL.Models;

public abstract class BaseEntity
{
    public int Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
