using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.MembershipViewModels;

public class CreateMemberShipViewModel
{
    [Required(ErrorMessage = "Plan Id Is Required")]
    public int PlanId { get; set; }
    [Required(ErrorMessage = "Member Id Is Required")]
    public int MemberId { get; set; }
    public DateTime? StartDate { get; set; }
}
