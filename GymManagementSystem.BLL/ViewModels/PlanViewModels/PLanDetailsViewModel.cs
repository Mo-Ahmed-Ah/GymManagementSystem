namespace GymManagementSystem.BLL.ViewModels.PlanViewModels;

public class PLanDetailsViewModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
