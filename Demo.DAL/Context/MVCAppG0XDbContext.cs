using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Context
{
   public class MVCAppG0XDbContext : IdentityDbContext<ApplicationUser>
    {


        public MVCAppG0XDbContext(DbContextOptions<MVCAppG0XDbContext>options):base(options)
        {
            
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        //    => optionsBuilder.UseSqlServer("server=DESKTOP-U8NHQFM\\SQLEXPRESS;Database=MVCAppG0X;Trusted_Connection=true;");

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Content> contents { get; set; }

        //public DbSet<ApplicationUser>Users { get; set; }

        //public DbSet<ApplicationUser>Roles { get; set; }


    }

}
