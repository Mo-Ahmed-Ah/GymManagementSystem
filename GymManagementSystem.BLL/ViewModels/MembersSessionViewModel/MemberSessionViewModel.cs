namespace GymManagementSystem.BLL.ViewModels.MemberSessionViewModel;

public class MemberSessionViewModel
{
    public int MemberId { get; set; }
    public int SessionId { get; set; }
    public string MemberName { get; set; } = default!;
    public string BookingDate { get; set; } = default!;
    public bool IsAttned { get; set; }
}
