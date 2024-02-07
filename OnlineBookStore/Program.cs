using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;
using System.Configuration;


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
            builder.Services.AddDefaultIdentity<WebUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<OnlineBookStoreContext>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}