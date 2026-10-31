using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations;

internal class MembershipConfigurations : IEntityTypeConfiguration<MemberShip>
{
    public void Configure(EntityTypeBuilder<MemberShip> builder)
    {
        //builder.Ignore(ms => ms.Id);
        builder.Ignore(ms => ms.Status);
        builder.Ignore(ms => ms.IsActive);

        builder.Property(c => c.CreatedAt)
               .HasColumnName("StartDate")
               .HasDefaultValueSql("GETDATE()");

        //builder.HasKey(ms => new
        //{
        //    ms.MemberId,
        //    ms.PlanId
        //});
    }
}
