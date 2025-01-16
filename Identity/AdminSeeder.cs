using Microsoft.AspNetCore.Identity;

namespace BmisApi.Identity
{
    public class AdminSeeder
    {
        public async static Task SeedAdmin(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            const string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            const string adminEmail = "admin@gmail.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is not null)
            {
                await userManager.AddToRoleAsync(admin, adminRole);

                Console.WriteLine("Admin permisions granted.");
            }
            else
            {
                admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };

                Console.WriteLine("Enter password for the admin user: ");
                string adminPassword = Console.ReadLine() ?? "Admin@1";

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRole);
                }
                else
                {
                    Console.WriteLine("Error in creating admin user: ");

                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }

                    return;
                }
            }
        }
    }
}
