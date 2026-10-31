using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations;

public class SessionConfigurations : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("Capacity_Check", "Capacity BETWEEN 1 AND 25");
            tb.HasCheckConstraint("EndDate_Check", "EndDate > StartDate");
        });
    }
}
