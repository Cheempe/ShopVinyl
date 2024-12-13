using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VinylShop.Web.Data;
using VinylShop.Web.Data.Entities;

namespace VinylShop.Web.Extensions
{
    public static class AppExtenstions
    {

        public async static Task ConfigureData(this WebApplication app)
        {
            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>()!;
            await context.Database.MigrateAsync();
            RoleManager<RoleEntity> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
            string[] roles = ["Admin"];

            foreach (string? role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new RoleEntity() { Name = role });

            UserManager<UserEntity> userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
            UserEntity? admin = await userManager.FindByNameAsync("vinylshopadmin");
            if (admin is null)
            {
                admin = new()
                {
                    Email = "vinylshopadmin@gmail.com",
                    EmailConfirmed = true,
                    UserName = "vinylshopadmin"
                };

                IdentityResult result = await userManager.CreateAsync(admin, "awsdweA@34RDAW");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
