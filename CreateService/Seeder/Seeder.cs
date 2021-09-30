using System;
using System.Threading.Tasks;
using CreateService.Models;
using Microsoft.AspNetCore.Identity;

namespace CreateService.Seeder
{
    public class Seed
    {
        public static void SeedDb(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //context.Database.EnsureCreated();
            SeedRoles(roleManager).Wait();
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Owner", "Admin", "Student" };
            foreach (var role in roles)
            {
                bool roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));

                }
            }
        }
    }
}
