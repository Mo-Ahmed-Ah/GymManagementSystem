using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations;

internal class MemberSessionConfigurations : IEntityTypeConfiguration<MemberSession>
{
    public void Configure(EntityTypeBuilder<MemberSession> builder)
    {
        builder.Property(ms => ms.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

    }
}
