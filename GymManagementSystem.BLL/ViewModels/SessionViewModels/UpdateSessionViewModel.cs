using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.SessionViewModels;

public class UpdateSessionViewModel
{
    public int Id { get; set; }
    public string Description { get; set; } = default!;
    [Display(Name = "Start Date & Time")]
    public DateTime StartDate { get; set; }
    [Display(Name = "End Date & Time")]
    public DateTime EndDate { get; set; }
    [Display(Name = "Trainer")]
    public int TrainerId { get; set; }
}
