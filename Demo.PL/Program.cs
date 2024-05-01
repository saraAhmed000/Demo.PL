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
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
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
                .AddEntityFrameworkStores<MVCAppG0XDbContext>()

            .AddDefaultTokenProviders();



            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "Account/Login";
                   options.AccessDeniedPath = "Home/Error";
               });


            #endregion


            var app=builder.Build();


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


            #endregion


            app.Run();  

        }




    }
}
