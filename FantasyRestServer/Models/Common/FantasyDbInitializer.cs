using FantasyRestServer.Models.Data;
using Microsoft.AspNetCore.Identity;


namespace FantasyRestServer.Models.Common
{
    public static class FantasyDbInitializer
    {
        public static void SeedData(
            UserManager<FantasyUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                };

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                };

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<FantasyUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@fantasymanager.com").Result == null)
            {
                FantasyUser admin = new FantasyUser
                {
                    UserName = "admin@fantasymanager.com",
                    Email = "admin@fantasymanager.com"
                };

                IdentityResult result = userManager.CreateAsync(admin, "admin").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }
        }
    }
}
