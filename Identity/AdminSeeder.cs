using Microsoft.AspNetCore.Identity;

namespace BmisApi.Identity
{
    public class AdminSeeder
    {
        public async static Task SeedAdmin(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            const string adminRole = "Admin";
            const string adminUsername = "Kapitan";
            var admin = await userManager.FindByNameAsync(adminUsername);
            if (admin is not null)
            {
                await userManager.AddToRoleAsync(admin, adminRole);

                Console.WriteLine("Admin permisions granted.");
            }
            else
            {
                admin = new ApplicationUser { UserName = adminUsername };

                string adminPassword = "Look1st@kapitan";

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
                }
            }
        }
    }
}
