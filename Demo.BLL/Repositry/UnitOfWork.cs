using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositry
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly MVCAppG0XDbContext _dbContext;

        public IEmployeeRepositry EmployeeRepositry { get ; set ; }
        public IDepartmentRepositry DepartmentRepositry { get; set; }
        

        public UnitOfWork(MVCAppG0XDbContext dbContext)
        {
            EmployeeRepositry=new EmployeeRepositry(dbContext);
            DepartmentRepositry =new DepartmentRepositry(dbContext);
            GenericRepositry = new GenericRepositry<T>(dbContext);
            _dbContext = dbContext;
        }
        public IGenericRepositry<T> GenericRepositry { get; set; }
        public Task <int> Complete()
        =>_dbContext.SaveChangesAsync();

        public   void Dispose()
        
        =>    _dbContext.Dispose();
        
    }
}
