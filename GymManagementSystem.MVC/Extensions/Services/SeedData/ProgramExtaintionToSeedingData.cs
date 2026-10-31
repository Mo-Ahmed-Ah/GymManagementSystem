using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.DataSeed;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.MVC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.PL.Extensions.Services.SeedData;

public static class ProgramExtaintionToSeedingData
{ 
    public static async Task SeedingData(this WebApplication app)
    {
        var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<GymDbContext>();
        var logger = app.Services.CreateScope().ServiceProvider.GetRequiredService<ILogger<Program>>();
        var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            await dbContext.Database.MigrateAsync();

        var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "SeedData", "PlanModel");

        await PlanDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);
        await IdentityDataSeeding.SeedIdentityDataAsync(roleManager, userManager, logger);
    }
}
