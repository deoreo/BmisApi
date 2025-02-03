using Microsoft.AspNetCore.Identity;

namespace BmisApi.Identity
{
    public class RoleSeeder
    {
        public async static Task SeedRoles(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "Secretary", "Clerk", "WomanDesk" };

            foreach (var roleName in roleNames)
            {
                var role = await roleManager.RoleExistsAsync(roleName);
                if (!role)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Role '{roleName}' created.");
                }
            }
        }
    }
}
