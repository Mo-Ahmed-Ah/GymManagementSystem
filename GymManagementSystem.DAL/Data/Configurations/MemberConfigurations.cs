using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations;

public class MemberConfigurations : GymUserConfigurations<Member>
{
    public new void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(m => m.CreatedAt)
               .HasColumnName("JoinDate")
               .HasDefaultValueSql("GETDATE()");

        base.Configure(builder);
    }
}
