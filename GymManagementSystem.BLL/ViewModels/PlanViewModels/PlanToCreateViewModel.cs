using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.PlanViewModels;

public class PlanToCreateViewModel
{
    [Required(ErrorMessage = "Plan name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Plan name must be between 3 and 50 characters")]
    [Display(Name = "Plan Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    [Display(Name = "Description")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Duration in days is required")]
    [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
    [Display(Name = "Duration (Days)")]
    public int DurationDays { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 99999999.99, ErrorMessage = "Price must be greater than zero")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price can have up to two decimal places")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;
}