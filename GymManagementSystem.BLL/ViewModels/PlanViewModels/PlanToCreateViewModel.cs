using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.PlanViewModels;

public class PlanToCreateViewModel
{
    [Display(Name = "Plan Name")]
    public string Name { get; set; } = null!;
    [Display(Name = "Description")]
    public string Description { get; set; } = null!;
    [Display(Name = "Duration (Days)")]
    public int DurationDays { get; set; }
    [Display(Name = "Price")]
    public decimal Price { get; set; }
    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;
}