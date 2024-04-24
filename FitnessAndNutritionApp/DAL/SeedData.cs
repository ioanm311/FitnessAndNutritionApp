using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessAndNutritionApp.DAL
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            await EnsureRolesAsync(serviceProvider.GetRequiredService<RoleManager<Role>>());
            await CreateAdminUser(serviceProvider);
        }

        private static async Task EnsureRolesAsync(RoleManager<Role> roleManager)
        {
            // Asigură-te că rolul Admin există
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role { Name = "Admin" });
            }

            // Asigură-te că rolul User există
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new Role { Name = "User" });
            }
        }

        // Metoda pentru crearea utilizatorului Admin
        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            string adminEmail = "admin@example.com";
            string adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new Exception("Admin password not found in environment variables.");
            }

            // Asigură-te că rolul Admin există
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new Role { Name = "Admin" };
                await roleManager.CreateAsync(role);
            }

            // Verifică dacă utilizatorul Admin există
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin", // Asigură-te că acest câmp este setat cu o valoare adecvată
                    LastName = "Administrator" // Dacă și LastName este obligatoriu, setează și acest câmp
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        throw new Exception($"Failed to create the admin user: {error.Description}");
                    }
                }
            }
        }
    }
}
