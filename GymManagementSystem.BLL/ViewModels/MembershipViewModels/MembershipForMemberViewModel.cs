namespace GymManagementSystem.BLL.ViewModels.MembershipViewModels;

public class MembershipForMemberViewModel
{
    public string MemberName { get; set; } = default!;
    public string PlanName { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = default!;
    public int? RemainingDays { get => (EndDate - DateTime.Now).Days; }

}
