using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;
using System.Configuration;
using System.Data;


namespace OnlineBookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Add Entity Framework
            var connstr = builder.Configuration.GetConnectionString("OnlineBookStoreContext");
            builder.Services.AddDbContext<OnlineBookStoreContext>
                (options => options.UseSqlServer(connstr));
            builder.Services.AddIdentity<WebUser, IdentityRole>
                (
                    options =>
                    {
                        options.Password.RequiredLength = 6;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireDigit = false;
                        options.User.RequireUniqueEmail = true;
                    }
                ).AddEntityFrameworkStores<OnlineBookStoreContext>()
                .AddDefaultTokenProviders();

            //A database is generated with some data already inserted
            // -- seeding
            //Password rest and email confimration tokens
            //builder.Services.AddDefaultIdentity<WebUser>
            //    (options =>
            //    {
            //        options.Password.RequiredLength = 6;
            //        options.Password.RequireUppercase = false;
            //        options.Password.RequireDigit = false;
            //        options.User.RequireUniqueEmail = true;
            //        /* add more rules here */
            //    }
            //    ).AddEntityFrameworkStores<OnlineBookStoreContext>();

            
            var app = builder.Build();
            //create a scope for them and then use them

           

            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapGet("Home/Privacy", async context=>
            {
                context.Response.Redirect("/Home/GetTable?num=5");
            }) ;
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var um = services.GetRequiredService<UserManager<WebUser>>();
                var rm = services.GetRequiredService<RoleManager<IdentityRole>>();
                SeedData(um, rm).Wait();//dont run this async
            }
            app.Run();
        }
        //Many teams use that shop
        //Team --> Materials ---> Shop 1 (ServiceProvider)
        //Table - Scope (Area memory)
        //Carboard, Gum, Paper etc ---> Shop (Resolving)
        public static async Task SeedData(UserManager<WebUser> user, RoleManager<IdentityRole> role)
        {
            string[] roleNames = { "Admin", "Customer" };

            foreach (var roleName in roleNames)
            {
                if (await role.RoleExistsAsync(roleName) == false)
                {
                    await role.CreateAsync(new IdentityRole(roleName));
                }
            }


            if (await user.FindByNameAsync("admin") == null)
            {
                WebUser w = new WebUser()
                {
                    UserName = "admin",
                    Email = "admin@mybookstore.com"
                };

                var result = await user.CreateAsync(w, "Admin@123");
                if (result.Succeeded)
                {
                    await user.AddToRoleAsync(w, "Admin");
                }
            }
        }
}
}