using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations;

public class GymUserConfigurations<T> : IEntityTypeConfiguration<T> where T : GymUser, new()
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(gu => gu.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50);

        builder.Property(gu => gu.Email)
               .HasColumnType("varchar")
               .HasMaxLength(100);

        builder.HasIndex(gu => gu.Email).IsUnique();
        builder.HasIndex(gu => gu.Phone).IsUnique();

        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("Email_Check", "Email LIKE '_%@_%._%'");
            tb.HasCheckConstraint("Phone_Check", "Phone LIKE '010%' OR Phone LIKE '011%' OR Phone LIKE '012%' OR Phone LIKE '015%'");
        });
        builder.OwnsOne(gu => gu.Address, address =>
        {
            address.Property(a => a.Street)
                   .HasColumnName("Street")
                   .HasColumnType("varchar")
                   .HasMaxLength(30);
            address.Property(a => a.City)
                   .HasColumnName("City")
                   .HasColumnType("varchar")
                   .HasMaxLength(30);
            address.Property(a => a.BuildingNumber)
                   .HasColumnName("BuildingNumber");
        });
    }
}
