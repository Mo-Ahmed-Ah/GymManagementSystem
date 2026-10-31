using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations;

public class TrainerConfigurations : GymUserConfigurations<Trainer>
{
    public new void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.Property(t => t.CreatedAt)
               .HasColumnName("HiringDate")
               .HasDefaultValueSql("GETDATE()");

        base.Configure(builder);
    }
}
