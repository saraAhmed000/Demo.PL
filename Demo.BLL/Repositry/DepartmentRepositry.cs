using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositry
{
    public class DepartmentRepositry : GenericRepositry<Department>, IDepartmentRepositry
    {

        public DepartmentRepositry(MVCAppG0XDbContext dbContext) :base(dbContext)
        {

        
        
            
        }
    }
}
