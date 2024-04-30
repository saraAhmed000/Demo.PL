using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork<T> : IDisposable where T : class
    {
        public IEmployeeRepositry EmployeeRepositry{ get; set; }
        public IDepartmentRepositry DepartmentRepositry { get; set; }
        public IGenericRepositry<T> GenericRepositry { get; set; }
       Task <int> Complete();
    }
}
