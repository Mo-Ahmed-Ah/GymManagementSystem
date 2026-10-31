using GymManagementSystem.DAl.Models;
using GymManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagementSystem.DAL.Data.Contexts;

public class GymDbContext : IdentityDbContext<ApplicationUser>
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=GymDb;Trusted_Connection=true;TrustServerCertificate=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<ApplicationUser>(
            au =>
            {
                au.Property(a => a.FristName).HasColumnType("varchar").HasMaxLength(50);
                au.Property(a => a.LastName).HasColumnType("varchar").HasMaxLength(50);
            }
            );
    }

    public DbSet<Plan> Plans { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<HealthRecord> HealthRecords { get; set; }
    public DbSet<MemberSession> MemberSessions { get; set; }
    public DbSet<MemberShip> MemberShips { get; set; }

}
