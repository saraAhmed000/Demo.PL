using Demo.BLL.Interfaces;
using Demo.BLL.Repositry;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region ConfigureServices That allow dependency Injection

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MVCAppG0XDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddScoped<IDepartmentRepositry, DepartmentRepositry>();
            //builder.Services.AddScoped<IEmployeeRepositry, EmployeeRepositry>();
            // builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile()));

            //builder.Services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));


            //builder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>>();
            //services.AddScoped<RoleManager<IdentityRole>>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;   //@$%
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MVCAppG0XDbContext>()

            .AddDefaultTokenProviders();




            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "Account/Login";
                   options.AccessDeniedPath = "Home/Error";
               });


            #endregion


            var app = builder.Build();


            #region Configuree httpRequest Pipelines

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });


            //app.UseEndpoints(endpoints =>
            //{
            //    // Route for admin
            //    endpoints.MapControllerRoute(
            //        name: "Admin",
            //        pattern: "{controller=Admin}/{action=AdminHome}/{id?}");

            //    // Default route
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Account}/{action=Login}/{id?}"); // Default route is the login page

            //    // Add a middleware to redirect to the appropriate default route based on the user's role
            //    endpoints.MapGet("/", async context =>
            //    {
            //        var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            //        var user = await userManager.GetUserAsync(context.User);

            //        // Check if the user is logged in
            //        if (user != null)
            //        {
            //            // Check if the user is an admin
            //            if (await userManager.IsInRoleAsync(user, "Admin"))
            //            {
            //                // Redirect to the admin default route
            //                context.Response.Redirect("/Admin/AdminHome");
            //            }
            //            else
            //            {
            //                // Redirect to the user default route (modify this route according to your actual user main page)
            //                context.Response.Redirect("/User/MainPage");
            //            }
            //        }
            //        else
            //        {
            //            // If not logged in, redirect to the default route (login page)
            //            context.Response.Redirect("/Account/Login");
            //        }
            //    });
            //});

            #endregion

            using (var scope = app.Services.CreateScope())
            {
                var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin" };
                foreach (var role in roles)
                {
                    if (!await roleManger.RoleExistsAsync(role))
                        await roleManger.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string email = "admin@gmail.com";
                string password = "Passwords@24";

                if(await userManger.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser()
                    {
                        Email = email,
                        UserName = email
                    };
                    await userManger.CreateAsync(user , password);

                    await userManger.AddToRoleAsync(user, "Admin");
                }
            }

            app.Run();

        }


    }
}
