using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.SessionViewModels;

public class CreateSessionViewModel
{
    public string Description { get; set; } = default!;
    public int Capacity { get; set; }
    [Display(Name = "Start Date & Time")]
    public DateTime StartDate { get; set; }
    [Display(Name = "End Date & Time")]
    public DateTime EndDate { get; set; }
    [Display(Name = "Trainer")]
    public int TrainerId { get; set; }
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
}
