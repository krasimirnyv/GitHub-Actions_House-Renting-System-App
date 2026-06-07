using HouseRentingSystem.Services.Data.Entities;
using Microsoft.AspNetCore.Identity;
using static HouseRentingSystem.Web.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Web.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder SeedAdmin(
        this IApplicationBuilder app)
    {
        using IServiceScope scopedServices = app.ApplicationServices.CreateScope();
        IServiceProvider services = scopedServices.ServiceProvider;

        UserManager<User> userManager = services.GetRequiredService<UserManager<User>>();
        RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        Task
            .Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminRoleName))
                {
                    return;
                }

                IdentityRole role = new IdentityRole { Name = AdminRoleName };

                await roleManager.CreateAsync(role);

                User? admin = await userManager.FindByNameAsync(AdminEmail);

                if (admin != null) await userManager.AddToRoleAsync(admin, role.Name);
            })
            .GetAwaiter()
            .GetResult();

        return app;
    }
}