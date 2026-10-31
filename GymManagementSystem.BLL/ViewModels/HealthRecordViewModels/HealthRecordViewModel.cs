public class HealthRecordViewModel
{
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string BloodType { get; set; } = default!;
    public string? Note { get; set; } = default!;
}