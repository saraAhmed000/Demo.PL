using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositry
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MVCAppG0XDbContext _dbContext;

        public IEmployeeRepositry EmployeeRepositry { get ; set ; }
        public IDepartmentRepositry DepartmentRepositry { get; set; }

        public UnitOfWork(MVCAppG0XDbContext dbContext)
        {
            EmployeeRepositry=new EmployeeRepositry(dbContext);
            DepartmentRepositry =new DepartmentRepositry(dbContext);
            _dbContext = dbContext;
        }

        public Task <int> Complete()
        =>_dbContext.SaveChangesAsync();

        public   void Dispose()
        
        =>    _dbContext.Dispose();
        
    }
}
