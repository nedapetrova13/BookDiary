using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace BookDiary.DataAccess
{
    public static class IdentityDataSeeder
    {
        //public static async Task SeedIdentityData(IServiceProvider serviceProvider)
        //{
        //    using (var scope = serviceProvider.CreateScope())
        //    {
        //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        //        // 1️⃣ Ensure roles exist
        //        string[] roles = { "Admin", "User" };
        //        foreach (var role in roles)
        //        {
        //            if (!await roleManager.RoleExistsAsync(role))
        //            {
        //                await roleManager.CreateAsync(new IdentityRole(role));
        //            }
        //        }

        //        // 2️⃣ Ensure an admin user exists
        //        var adminEmail = "admin@example.com";
        //        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        //        if (adminUser == null)
        //        {
        //            var newAdmin = new User { UserName = adminEmail, Email = adminEmail };
        //            await userManager.CreateAsync(newAdmin, "Admin123!");
        //            await userManager.AddToRoleAsync(newAdmin, "Admin");
        //        }
        //    }
        //}
    }

}
