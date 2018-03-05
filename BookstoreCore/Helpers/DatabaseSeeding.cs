using BookstoreCore.Models;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreCore.Helpers
{
    public static class DatabaseSeeding
    {
        public static void SeedBooks(IApplicationBuilder app)
        {
            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                BookstoreDBContext dBContext = scope.ServiceProvider.GetRequiredService<BookstoreDBContext>();

                if (!dBContext.Database.GetMigrations().Any())
                    return;

                dBContext.Database.Migrate();

                if (dBContext.BookAuthors.Any())
                    return;

                Author schildt = new Author() { Name = "Herbert", Surname = "Schildt" };

                List<BookAuthors> bookAuthor = new List<BookAuthors>()
                {
                    new BookAuthors(){ Book=new Book() { Title = "C++: The Complete Reference" }, Author = schildt},
                    new BookAuthors(){ Book=new Book() { Title = "C#: A Beginner's Guide" }, Author = schildt},
                    new BookAuthors(){ Book=new Book() { Title = "Java: A Beginner's Guide" }, Author = schildt},
                    new BookAuthors(){ Book=new Book() { Title = "Born to Code In C" }, Author = schildt},
                };

                dBContext.BookAuthors.AddRange(bookAuthor);
                dBContext.SaveChanges();
            }
        }

        public static async Task SeedAdminUser(IApplicationBuilder app)
        {
            IServiceScopeFactory scopeFactory =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                BookstoreDBContext dBContext = scope.ServiceProvider.GetRequiredService<BookstoreDBContext>();

                if (!dBContext.Database.GetMigrations().Any())
                    return;

                string adminPassword = "StrongPassword123$";
                UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                ApplicationUser admin = await userManager.FindByIdAsync(UserRoles.Admin.ToString());
                if (admin != null)
                    return;

                admin = new ApplicationUser(UserRoles.Admin.ToString())
                {
                    Email = "Admin@outlook.com"
                };

                RoleManager<IdentityRole> roleManager =
                    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                IdentityRole role = new IdentityRole() { Name = UserRoles.Admin.ToString() };

                if ((await userManager.CreateAsync(admin, adminPassword)).Succeeded &&
                    ((await roleManager.CreateAsync(role)).Succeeded))
                {
                    await userManager.AddToRoleAsync(admin, UserRoles.Admin.ToString());
                }
            }
        }
    }
}