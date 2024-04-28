using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositry
{
    public class EmployeeRepositry : GenericRepositry<Employee>, IEmployeeRepositry
    {
        

        public EmployeeRepositry(MVCAppG0XDbContext dbContext):base(dbContext)
        {
             
        }
        public IQueryable<Employee> GetEmployeesByAddress(string Address)
        {
           throw new NotImplementedException(); 

        }

        public IQueryable<Employee> SerachEmployeeByName(string name)
       
            => _dbContext.Employees.Where(E=>E.Name .ToLower() .Contains(name.ToLower()));

    }
}
