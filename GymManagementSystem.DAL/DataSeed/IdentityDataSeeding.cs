using GymManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem.DAL.DataSeed;

public static class IdentityDataSeeding
{
    public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger)
    {
        var hasUsers = await userManager.Users.AnyAsync();
        var hasRoles = await roleManager.Roles.AnyAsync();

        if (hasUsers && hasRoles)
        {
            logger.LogInformation("We Have Got Some Users And Roles In Our DataBase");
            return;
        }
        if (hasUsers)
            logger.LogInformation("We Have Got Some User In Our DataBase");
        if (hasRoles)
            logger.LogInformation("We Have Got Some Roles In Our DataBase");

        if (!hasRoles)
        {
            logger.LogInformation("We Have Not Got Some Roles In Our DataBase And We Will Try To Add Some Roles Into My Database");
            try
            {
                var roles = new List<IdentityRole>()
                {
                    new IdentityRole("SuperAdmin"),
                    new IdentityRole("Admin"),
                    new IdentityRole("Trainer"),
                    new IdentityRole("Member"),
                };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Create Role : {role.Name}");
                        }
                        else
                        {
                            logger.LogInformation($"Succeeded To Add Role {role.Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed To Create Roles Error Message : {ex.Message}");
            }
        }

        if (!hasUsers)
        {
            logger.LogInformation("We Have Not Got Some User In Our DataBase And We Will Try To Add Some Users Into My Database");
            try
            {
                var MainAdmin = new ApplicationUser()
                {
                    FristName = "Ahmed",
                    LastName = "Mohamed",
                    Email = "Ahmed@gmail.com",
                    UserName = "Ahmed",
                    PhoneNumber = "01033833292"
                };
                var addUserResult = await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                if (addUserResult.Succeeded)
                    logger.LogInformation($"The Usere {MainAdmin.FristName} {MainAdmin.LastName} Is Created Now");
                var addRolesResult = await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                if (addRolesResult.Succeeded)
                    logger.LogInformation($"The Role SuperAdmin Is Seted Into {MainAdmin.FristName} {MainAdmin.LastName} User Now");
                var Admin = new ApplicationUser()
                {
                    FristName = "Mostaf",
                    LastName = "Hany",
                    Email = "MostafHany@gmail.com",
                    UserName = "MostafHany",
                    PhoneNumber = "01234567891"
                };
                addUserResult = await userManager.CreateAsync(Admin, "P@ssw0rd");
                if (addUserResult.Succeeded)
                    logger.LogInformation($"The Usere {Admin.FristName} {Admin.LastName} Is Created Now");
                addRolesResult = await userManager.AddToRoleAsync(Admin, "Admin");
                if (addRolesResult.Succeeded)
                    logger.LogInformation($"The Role Admin Is Seted Into {Admin.FristName} {Admin.LastName} User Now");
                var TrainerUser = new ApplicationUser()
                {
                    FristName = "Ahmed",
                    LastName = "Ali",
                    Email = "ahmed77773330@gmail.com",
                    UserName = "AhmedMohamed",
                    PhoneNumber = "01033883292"
                };
                addUserResult = await userManager.CreateAsync(TrainerUser, "P@ssw0rd");
                if (addUserResult.Succeeded)
                    logger.LogInformation($"The Usere {TrainerUser.FristName} {TrainerUser.LastName} Is Created Now");
                addRolesResult = await userManager.AddToRoleAsync(TrainerUser, "Trainer");
                if (addRolesResult.Succeeded)
                    logger.LogInformation($"The Role Trainer Is Seted Into {TrainerUser.FristName} {TrainerUser.LastName} User Now");

                var MemberUser = new ApplicationUser()
                {
                    FristName = "Mohamed",
                    LastName = "Ahmed",
                    Email = "ahmed78773330@gmail.com",
                    UserName = "MohamedAhmed",
                    PhoneNumber = "01033888292"
                };
                addUserResult = await userManager.CreateAsync(MemberUser, "P@ssw0rd");
                if (addUserResult.Succeeded)
                    logger.LogInformation($"The Usere {MemberUser.FristName} {MemberUser.LastName} Is Created Now");
                addRolesResult = await userManager.AddToRoleAsync(MemberUser, "Member");
                if (addRolesResult.Succeeded)
                    logger.LogInformation($"The Role Member Is Seted Into {MemberUser.FristName} {MemberUser.LastName} User Now");
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed To Create User Error Message : {ex.Message}");
            }
        }
    }
}
